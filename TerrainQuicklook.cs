using Godot;
using Godot.Collections;
using MuClient;
using MuClient.Models.Terrain;
using System;
using MuClient.Database;
using MuClient.Extensions;
using System.IO;
namespace MuClient;

[Tool]
public partial class TerrainQuicklook : Node3D
{
    private WorldType world = WorldType.LORENCIA;
    [Export]
    public WorldType World
    {
        get => world;
        set
        {
            if (world == value) return;
            world = value;
            UpdateTerrain();
            DrawObjects();
        }
    }
    private Dictionary<short, PackedScene> GenericScenes = new();
    Node3D WorldObjects;
    CollisionShape3D CollisionShape;
    MeshInstance3D MeshInstance;

    string WorldFolderResourcePath => $"res://Data/World{(int)World}";
    string TerrainDataResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.obj");
    string MeshResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.map");
    string CollisionShapeResourcePath => Path.Combine(WorldFolderResourcePath, $"TerrainHeight.OZB");

    public override void _Ready()
    {
        WorldObjects = GetNode<Node3D>("WorldObjects");
        CollisionShape = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");
        MeshInstance = GetNode<MeshInstance3D>("StaticBody3D/MeshInstance3D");
        base._Ready();
        UpdateTerrain();
        DrawObjects();

    }

    void UpdateTerrain()
    {
        if (CollisionShape == null || MeshInstance == null)
        {
            // TODO: Null check
            return;
        }
        ArrayMesh mesh = ResourceLoader.Load<ArrayMesh>(MeshResourcePath);
        HeightMapShape3D shape = ResourceLoader.Load<HeightMapShape3D>(CollisionShapeResourcePath);

        CollisionShape.Shape = shape;
        MeshInstance.Mesh = mesh;
    }
    void DrawObjects()
    {

        ObjectMap objectMap = ResourceLoader.Load<ObjectMap>(TerrainDataResourcePath);
        // Clear old cached scene
        GenericScenes.Clear();
        foreach (short item in objectMap.ObjectTypes)
        {
            if (World == WorldType.LORENCIA && item == 133)
            {
                continue;
            }
            if (World == WorldType.ARENA && item == -515)
            {
                continue;
            }
            if (World == WorldType.VALLEY_OF_LOREN && item == 35)
            {
                continue;
            }
            GenericScenes.Add(item, GD.Load<PackedScene>(ObjectMapDatabase.GetResourcePath((int)World, item)));
        }

        // Clear old items
        if (WorldObjects == null)
        {
            return;
        }
        foreach (var children in WorldObjects.GetChildren())
        {
            children.QueueFree();
        }
        // Place new items
        foreach (ObjectAttribute item in objectMap.Objects)
        {
            PlaceObject(item);
        }
    }

    void PlaceObject(ObjectAttribute objectAttribute)
    {
        GenericScenes.TryGetValue(objectAttribute.Type, out PackedScene scene);
        if (scene == null)
        {
            return;
        }
        Node3D obj = new Node3D();
        Node3D model = scene.Instantiate<Node3D>();
        obj.AddChild(model);

        Vector3 position = new Vector3(
            objectAttribute.Position.X * 0.01f,
            objectAttribute.Position.Z * 0.01f,
            -objectAttribute.Position.Y * 0.01f
        );
        position -= new Vector3(
            Constants.TerrainSize * 0.5f,
            0,
            Constants.TerrainSize * 0.5f
        );
        position += new Vector3(
            2.0f,
            0,
            Constants.TerrainSize
        );
        obj.Position = position;


        float angleXRadian = (float)objectAttribute.Angle.X.ToRadians();
        float angleYRadian = (float)objectAttribute.Angle.Y.ToRadians();
        float angleZRadian = (float)objectAttribute.Angle.Z.ToRadians();

        obj.RotateObjectLocal(Vector3.Right, angleYRadian);
        obj.RotateObjectLocal(Vector3.Up, angleZRadian);
        obj.RotateObjectLocal(Vector3.Back, angleXRadian);

        WorldObjects.AddChild(obj);
    }
}
