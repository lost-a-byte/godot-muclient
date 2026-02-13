using Godot;
using Godot.Collections;
using MuClient.Models.Terrain;
using System;
using MuClient.Database;
using System.IO;
using MuClient.Scenes;
using MuClient.Extensions;

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
    private bool loadObjectsInEditor = false;
    [ExportGroup("Editor Preview")]
    [Export]
    public bool LoadObjectsInEditor
    {
        get { return loadObjectsInEditor; }
        set
        {
            if (loadObjectsInEditor == value) return;
            loadObjectsInEditor = value;
            if (loadObjectsInEditor)
            {
                DrawObjects();
            }
            else
            {
                ClearObjects();
            }
        }
    }
    private bool showGeneratedNodesInSceneTree = false;
    [Export]
    public bool ShowGeneratedNodesInSceneTree
    {
        get => showGeneratedNodesInSceneTree;
        set
        {
            if (showGeneratedNodesInSceneTree == value)
            {
                return;
            }
            showGeneratedNodesInSceneTree = value;
            if (!showGeneratedNodesInSceneTree)
            {
                return;
            }
            EnsureEditorSceneOwnership();
            if (WorldObjects != null)
            {
                ExposeGeneratedNodesInEditor(WorldObjects);
            }
        }
    }
    private Dictionary<short, PackedScene> GenericScenes = new();
    Node3D? WorldObjects;
    CollisionShape3D? CollisionShape;
    MeshInstance3D? MeshInstance;

    Character? Character;
    private TilePosition currentCharacterTile = new(0, 0);
    public TilePosition CurrentCharacterTile
    {
        get { return currentCharacterTile; }
        set
        {
            if (currentCharacterTile == value)
            {
                return;
            }
            currentCharacterTile = value;
            PlaceNearByTiles();
        }
    }

    Array<ObjectAttribute>[,] TileObjectMap = new Array<ObjectAttribute>[255, 255];
    bool[,] TileObjectMapPlaced = new bool[255, 255];


    string WorldFolderResourcePath => $"res://Data/World{(int)World}";
    string TerrainDataResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.obj");
    string MeshResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.map");
    string CollisionShapeResourcePath => Path.Combine(WorldFolderResourcePath, $"TerrainHeight.OZB");

    public override void _Ready()
    {
        WorldObjects = GetNode<Node3D>("WorldObjects");
        CollisionShape = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");
        Character = GetNode<Character>("Character");
        MeshInstance = GetNode<MeshInstance3D>("StaticBody3D/MeshInstance3D");
        CurrentCharacterTile = Character.XZPosition.ToTilePosition();
        Character.XZPositionChanged += OnXZPositionChanged;
        base._Ready();
        UpdateTerrain();
        DrawObjects();
    }

    private void OnXZPositionChanged(Vector2 newXZ)
    {
        CurrentCharacterTile = newXZ.ToTilePosition();
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

    void ClearObjects()
    {
        TileObjectMap = new Array<ObjectAttribute>[256, 256];
        TileObjectMapPlaced = new bool[256, 256];
        // Clear old items
        if (WorldObjects == null)
        {
            return;
        }
        foreach (var children in WorldObjects.GetChildren())
        {
            children.QueueFree();
        }
    }

    void DrawObjects()
    {
        if (Engine.IsEditorHint() && !LoadObjectsInEditor)
        {
            return;
        }
        ClearObjects();

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
            GenericScenes.Add(item, GD.Load<PackedScene>(ObjectMapDatabase.GetResourcePath(World, item)));
        }

        // Place new items

        foreach (ObjectAttribute item in objectMap.Objects)
        {
            var tile = item.Position.ToTilePosition();
            if (TileObjectMap[tile.X, tile.Z] == null)
            {
                TileObjectMap[tile.X, tile.Z] = new Array<ObjectAttribute>();
            }
            TileObjectMap[tile.X, tile.Z].Add(item);
        }
        PlaceNearByTiles();
    }

    void PlaceNearByTiles()
    {
        if (CurrentCharacterTile == null)
        {
            return;
        }
        for (int x = Math.Max(0, CurrentCharacterTile.X - 10); x < Math.Min(CurrentCharacterTile.X + 10, 255); x++)
        {
            for (int z = Math.Max(0, currentCharacterTile.Z - 10); z < Math.Min(CurrentCharacterTile.Z + 10, 255); z++)
            {
                PlaceObjectAtTile(new TilePosition((byte)x, (byte)z));
            }
        }
    }

    void PlaceObjectAtTile(TilePosition tile)
    {
        var tiles = TileObjectMap[tile.X, tile.Z];
        if (tiles == null)
        {
            return;
        }
        if (TileObjectMapPlaced[tile.X, tile.Z])
        {
            return;
        }
        foreach (var item in tiles)
        {
            PlaceObject(item);
        }
        TileObjectMapPlaced[tile.X, tile.Z] = true;
    }

    void PlaceObject(ObjectAttribute objectAttribute)
    {
        if (WorldObjects == null)
        {
            return;
        }
        GenericScenes.TryGetValue(objectAttribute.Type, out PackedScene? scene);
        if (scene == null)
        {
            return;
        }
        Node3D obj = new();
        if (Engine.IsEditorHint())
        {
            obj.Name = $"Node_{objectAttribute.Type}_{Math.Round(objectAttribute.Position.X)}x{Math.Round(objectAttribute.Position.Z)}";
        }
        Node3D model = scene.Instantiate<Node3D>();
        obj.AddChild(model);

        obj.Position = objectAttribute.Position;
        obj.Basis *= new Basis(objectAttribute.Rotation);

        WorldObjects.AddChild(obj);
    }

    private void EnsureEditorSceneOwnership()
    {
        if (!Engine.IsEditorHint() || !ShowGeneratedNodesInSceneTree)
            return;

        var editedSceneRoot = GetTree()?.EditedSceneRoot;
        if (editedSceneRoot == null)
        {
            return;
        }

        // Keep continuous editor sync lightweight: only patch key roots.
        // Full recursive ownership pass is handled explicitly during world/object build.

        SetOwnerIfNeeded(WorldObjects, editedSceneRoot);
    }

    private static void SetOwnerIfNeeded(Node? node, Node owner)
    {
        if (node == null || !IsInstanceValid(node) || node == owner || node.Owner == owner)
        {
            return;
        }
        node.Owner = owner;
    }

    private void ExposeGeneratedNodesInEditor(Node root)
    {
        if (!Engine.IsEditorHint() || !ShowGeneratedNodesInSceneTree)
            return;

        var editedSceneRoot = GetTree()?.EditedSceneRoot;
        if (editedSceneRoot == null)
            return;

        SetOwnerRecursive(root, editedSceneRoot);
    }

    private static void SetOwnerRecursive(Node node, Node owner)
    {
        if (node != owner)
            node.Owner = owner;

        foreach (Node child in node.GetChildren())
            SetOwnerRecursive(child, owner);
    }

    public override void _ExitTree()
    {
        Character?.XZPositionChanged -= OnXZPositionChanged;
        base._ExitTree();
    }
}
