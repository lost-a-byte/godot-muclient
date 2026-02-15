using Godot;
using Godot.Collections;

namespace MuClient.Scenes.Characters;

[Tool]
public partial class Adventurer : Character
{

    [ExportToolButton("Mount as DK", Icon = "Button")]
    public Callable MountAsDkButton => Callable.From(MountAsDk);
    private void MountAsDk()
    {
        // TODO: Body of Remount parts
        MountHead("res://Data/Player/HelmClass02.bmd");
        MountArmor("res://Data/Player/ArmorClass02.bmd");
        MountGloves("res://Data/Player/GloveClass02.bmd");
        MountPant("res://Data/Player/PantClass02.bmd");
        MountBoots("res://Data/Player/BootClass02.bmd");
    }

    [ExportToolButton("Mount as ELF", Icon = "Button")]
    public Callable MountAsElfButton => Callable.From(MountAsElf);
    private void MountAsElf()
    {
        // TODO: Body of Mount as ELF
        MountHead("res://Data/Player/HelmClass03.bmd");
        MountArmor("res://Data/Player/ArmorClass03.bmd");
        MountGloves("res://Data/Player/GloveClass03.bmd");
        MountPant("res://Data/Player/PantClass03.bmd");
        MountBoots("res://Data/Player/BootClass03.bmd");
    }

    [ExportToolButton("Mount as DW", Icon = "Button")]
    public Callable MountAsDwButton => Callable.From(MountAsDw);
    private void MountAsDw()
    {
        // TODO: Body of Mount as DW
        MountHead(HeadPath);
        MountArmor(ArmorPath);
        MountGloves(GlovesPath);
        MountPant(PantPath);
        MountBoots(BootsPath);
    }
    Skeleton3D? Skeleton;

    public virtual string HeadPath => "res://Data/Player/HelmClass01.bmd";
    public virtual string ArmorPath => "res://Data/Player/ArmorClass01.bmd";
    public virtual string GlovesPath => "res://Data/Player/GloveClass01.bmd";
    public virtual string PantPath => "res://Data/Player/PantClass01.bmd";
    public virtual string BootsPath => "res://Data/Player/BootClass01.bmd";
    public virtual string WingPath => "res://Data/Item/Wing53.bmd";
    public virtual string WingCore1stPath => "res://Data/Item/Wing53.bmd";
    public virtual string WingCore2ndPath => "res://Data/Item/Wing53.bmd";
    public virtual string Weapon1stPath => "res://Data/Item/Wing53.bmd";
    public virtual string Weapon2ndPath => "res://Data/Item/Wing53.bmd";

    public Node3D? HiddenNode;
    public override void _Ready()
    {
        Node3D SkeletonNode = GetNode<Node3D>("Skeleton");
        Skeleton = SkeletonNode.GetNode<Skeleton3D>("skeleton");
        HiddenNode = GetNode<Node3D>("HiddenNode");
        base._Ready();

        MountHead(HeadPath);
        MountArmor(ArmorPath);
        MountGloves(GlovesPath);
        MountPant(PantPath);
        MountBoots(BootsPath);
        // MountWing(WingPath);
    }


    public virtual void MountArmor(string resourcePath)
    {
        UnmountPart(nameof(MountArmor));
        MountPart(resourcePath, nameof(MountArmor));
    }
    public virtual void MountPant(string resourcePath)
    {
        UnmountPart(nameof(MountPant));
        MountPart(resourcePath, nameof(MountPant));
    }
    public virtual void MountGloves(string resourcePath)
    {
        UnmountPart(nameof(MountGloves));
        MountPart(resourcePath, nameof(MountGloves));
    }
    public virtual void MountBoots(string resourcePath)
    {
        UnmountPart(nameof(MountBoots));
        MountPart(resourcePath, nameof(MountBoots));
    }
    public virtual void MountHelmet(string resourcePath)
    {
        UnmountPart(nameof(MountHelmet));
        MountPart(resourcePath, nameof(MountHelmet));
    }
    public virtual void MountHead(string resourcePath)
    {
        UnmountPart(nameof(MountHead));
        MountPart(resourcePath, nameof(MountHead));
    }
    public virtual void MountPart(string resourcePath, string partName)
    {
        if (Skeleton == null)
        {
            return;
        }


        PackedScene scene = GD.Load<PackedScene>(resourcePath);
        Node3D node = scene.Instantiate<Node3D>();
        // Fix memory leak!
        HiddenNode?.AddChild(node);
        Skeleton3D SubSkeleton = node.GetNode<Skeleton3D>("skeleton");
        int index = 0;
        foreach (var child in SubSkeleton.GetChildren())
        {
            if (child is MeshInstance3D mesh)
            {
                MeshInstance3D newMesh = (MeshInstance3D)mesh.Duplicate();
                // Create Unique name to remove it later
                newMesh.Name = $"{partName}_{index}_{ResourceUid.CreateId()}";
                newMesh.Skin.GetBindCount();
                Dictionary<string, Transform3D> goodBindNames = [];
                for (int i = 0; i < newMesh.Skin.GetBindCount(); i++)
                {
                    string skinBindName = newMesh.Skin.GetBindName(i);
                    if (Skeleton.FindBone(skinBindName) == -1)
                    {
                        continue;
                    }
                    goodBindNames.Add(skinBindName, newMesh.Skin.GetBindPose(i));
                }
                newMesh.Skin.ClearBinds();

                foreach (var item in goodBindNames)
                {
                    newMesh.Skin.AddNamedBind(item.Key, item.Value);
                }

                newMesh.Skeleton = Skeleton.GetPath();

                Skeleton.AddChild(newMesh);
            }
        }
        // Clear memory
        node.QueueFree();
    }

    public virtual void UnmountPart(string partName)
    {
        if (Skeleton == null)
        {
            return;
        }
        foreach (var item in Skeleton.GetChildren())
        {
            if (item is MeshInstance3D mesh)
            {
                if (mesh.Name.ToString().StartsWith(partName + "_"))
                {
                    mesh.QueueFree();
                }
            }
        }
    }
}
