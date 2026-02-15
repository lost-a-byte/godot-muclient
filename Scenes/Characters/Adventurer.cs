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
    #region Inspector
    [ExportGroup("Equipment")]
    private bool haveWing = false;
    [Export]
    public bool HaveWing
    {
        get { return haveWing; }
        set
        {
            if (haveWing == value) return;
            haveWing = value;
            if (haveWing)
            {
                MountWing(WingPath);
            }
            else
            {
                UnmountPart(nameof(MountWing));
            }
        }
    }

    private bool haveWingCore1st = false;
    [Export]
    public bool HaveWingCore1st
    {
        get { return haveWingCore1st; }
        set
        {
            if (haveWingCore1st == value) return;
            haveWingCore1st = value;
            if (haveWingCore1st)
            {
                MountWingCore1st(WingCore1stPath);
            }
            else
            {
                UnmountPart(nameof(MountWingCore1st));
            }
        }
    }

    private bool haveWingCore2nd = false;
    [Export]
    public bool HaveWingCore2nd
    {
        get { return haveWingCore2nd; }
        set
        {
            if (haveWingCore2nd == value) return;
            haveWingCore2nd = value;
            if (haveWingCore2nd)
            {
                MountWingCore2nd(WingCore2ndPath);
            }
            else
            {
                UnmountPart(nameof(MountWingCore2nd));
            }
        }
    }

    private bool haveWeapon1st = false;
    [Export]
    public bool HaveWeapon1st
    {
        get { return haveWeapon1st; }
        set
        {
            if (haveWeapon1st == value) return;
            haveWeapon1st = value;
            if (haveWeapon1st)
            {
                MountWeapon1st(Weapon1stPath);
            }
            else
            {
                UnmountPart(nameof(MountWeapon1st));
            }
        }
    }

    private bool haveWeapon2nd = false;
    [Export]
    public bool HaveWeapon2nd
    {
        get { return haveWeapon2nd; }
        set
        {
            if (haveWeapon2nd == value) return;
            haveWeapon2nd = value;
            if (haveWeapon2nd)
            {
                MountWeapon2nd(Weapon2ndPath);
            }
            else
            {
                UnmountPart(nameof(MountWeapon2nd));
            }
        }
    }
    #endregion

    public virtual string HeadPath => "res://Data/Player/HelmClass01.bmd";
    public virtual string ArmorPath => "res://Data/Player/ArmorClass01.bmd";
    public virtual string GlovesPath => "res://Data/Player/GloveClass01.bmd";
    public virtual string PantPath => "res://Data/Player/PantClass01.bmd";
    public virtual string BootsPath => "res://Data/Player/BootClass01.bmd";
    public virtual string WingPath => "res://Data/Item/Wing504.bmd";
    public virtual string WingCore1stPath => "res://Data/Item/Wing504_core1.bmd";
    public virtual string WingCore2ndPath => "res://Data/Item/Wing504_core2.bmd";
    public virtual string Weapon1stPath => "res://Data/Item/absolute02_staff.bmd";
    public virtual string Weapon2ndPath => "res://Data/Item/absolute02_staff.bmd";

    public Dictionary<string, int> BoneNames = new();
    AnimationPlayer? AnimationPlayer;
    public Node3D? HiddenNode;
    public override void _Ready()
    {
        Node3D SkeletonNode = GetNode<Node3D>("Skeleton");
        Skeleton = SkeletonNode.GetNode<Skeleton3D>("skeleton");
        HiddenNode = GetNode<Node3D>("HiddenNode");
        AnimationPlayer = SkeletonNode.GetNode<AnimationPlayer>("AnimationPlayer");
        Animation flyAnimation = AnimationPlayer.GetAnimation("Animations/Action_019");
        flyAnimation.LoopMode = Animation.LoopModeEnum.Linear;
        AnimationPlayer.Play("Animations/Action_019");
        InitBoneName();
        base._Ready();

        MountHead(HeadPath);
        MountArmor(ArmorPath);
        MountGloves(GlovesPath);
        MountPant(PantPath);
        MountBoots(BootsPath);

        MountWeapon1st(Weapon1stPath);
        MountWeapon2nd(Weapon2ndPath);
        MountWing(WingPath);
        MountWingCore1st(WingCore1stPath);
        MountWingCore2nd(WingCore2ndPath);
    }

    #region Mount Armor

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
    #endregion

    #region Mount Weapons
    public virtual void MountWing(string resourcePath)
    {
        UnmountPart(nameof(MountWing));
        MountPartToBoneId(resourcePath, nameof(MountWing), GetBoneId("Bone05"));
    }

    public virtual void MountWingCore1st(string resourcePath)
    {
        UnmountPart(nameof(MountWingCore1st));
        MountPartToBoneId(resourcePath, nameof(MountWingCore1st), GetBoneId("Bone05"));
    }
    public virtual void MountWingCore2nd(string resourcePath)
    {
        UnmountPart(nameof(MountWingCore2nd));
        MountPartToBoneId(resourcePath, nameof(MountWingCore2nd), GetBoneId("Bone05"));
    }
    public virtual void MountWeapon1st(string resourcePath)
    {
        UnmountPart(nameof(MountWeapon1st));
        MountPartToBoneId(resourcePath, nameof(MountWeapon1st), GetBoneId("knife_gdf"));
    }
    public virtual void MountWeapon2nd(string resourcePath)
    {
        UnmountPart(nameof(MountWeapon2nd));
        MountPartToBoneId(resourcePath, nameof(MountWeapon2nd), GetBoneId("hand_bofdgne01"));
    }



    public virtual NodePath? MountPartToBoneId(string resourcePath, string partName, int boneId)
    {
        if (Skeleton == null || Skeleton.GetBoneCount() < boneId) return null;

        PackedScene scene = GD.Load<PackedScene>(resourcePath);
        Node3D node = scene.Instantiate<Node3D>();

        // Fix memory leak!
        HiddenNode?.AddChild(node);

        Skeleton3D SubSkeleton = node.GetNode<Skeleton3D>("skeleton");
        Skeleton3D attachingSkeleton = (Skeleton3D)SubSkeleton.Duplicate();

        var boneAttachment = new BoneAttachment3D()
        {
            Name = $"{partName}_{ResourceUid.CreateId()}",
            BoneName = Skeleton.GetBoneName(boneId),
        };

        AnimationPlayer? animationPlayer =
            node.HasNode("AnimationPlayer")
                ? (AnimationPlayer)node.GetNode<AnimationPlayer>("AnimationPlayer")
                    .Duplicate()
                : null;
        if (animationPlayer != null)
        {
            animationPlayer.Active = true;
            // TODO: Check if that part does have animation player and execute play first animation;
            animationPlayer.Play("Animations/Action_000");
            boneAttachment.AddChild(animationPlayer);
        }

        boneAttachment.AddChild(attachingSkeleton);
        Skeleton.AddChild(boneAttachment);

        node.QueueFree();

        return boneAttachment.GetPath();
    }
    #endregion

    #region Bone Utils

    private void InitBoneName()
    {
        if (Skeleton == null)
        {
            return;
        }
        for (int i = 0; i < Skeleton.GetBoneCount(); i++)
        {
            var boneName = Skeleton.GetBoneName(i);
            BoneNames[boneName] = i;
        }
    }

    public virtual int GetBoneId(string boneName)
    {
        if (Skeleton == null)
        {
            return -1;
        }
        if (BoneNames.TryGetValue(boneName, out int boneId))
        {
            return boneId;
        }
        return -1;
    }
    #endregion


    #region Unmount

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
            else if (item is BoneAttachment3D boneAttachment)
            {
                if (boneAttachment.Name.ToString().StartsWith(partName + "_"))
                {
                    boneAttachment.QueueFree();
                }
            }
        }
    }
    #endregion
}
