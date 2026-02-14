using Godot;
using Godot.Collections;
using MuClient.Models.Terrain;
using System;
using MuClient.Database;
using System.IO;
using MuClient.Scenes;
using MuClient.Extensions;
using MuClient.Scenes.Characters;
using MuClient.Scenes.Controls;

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
    StaticBody3D? StaticBody;
    MeshInstance3D? MeshInstance;
    MoveCommand? moveCommand;

    Adventurer? Character;
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

    TileAttribute tileAttribute = new();

    string WorldFolderResourcePath => $"res://Data/World{(int)World}";
    string TerrainDataResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.obj");
    string MeshResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.map");
    string WorldAttributeResourcePath => Path.Combine(WorldFolderResourcePath, $"EncTerrain{(int)World}.att");
    string CollisionShapeResourcePath => Path.Combine(WorldFolderResourcePath, $"TerrainHeight.OZB");
    PhysicsDirectSpaceState3D? SpaceState;

    // Walkable Start

    private AStarGrid2D AStarGrid = new()
    {
        Region = new Rect2I(0, 0, Constants.TerrainSize, Constants.TerrainSize),
        CellSize = new Vector2(1, 1),
        DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles,
    };


    public override void _Ready()
    {
        WorldObjects = GetNode<Node3D>("WorldObjects");
        CollisionShape = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");
        StaticBody = GetNode<StaticBody3D>("StaticBody3D");
        Character = GetNode<Adventurer>("Adventurer");
        MeshInstance = GetNode<MeshInstance3D>("StaticBody3D/MeshInstance3D");
        SpaceState = GetWorld3D().DirectSpaceState;
        AStarGrid.Update();
        moveCommand = GetNode<MoveCommand>("Controls/MoveCommand");
        moveCommand.Move += OnMoveCommandTriggered;

        CurrentCharacterTile = Character.XZPosition.ToTilePosition();
        Character.XZPositionChanged += OnXZPositionChanged;
        base._Ready();
        UpdateTerrain();
        DrawObjects();
    }

    // Walkable Blocks

    void SetupWalkableBlocks()
    {
        // AStarGrid.SetPointSolid(new Vector2I(10, 10), true);
        // AStarGrid.SetPointSolid(new Vector2I(11, 10), true);

        for (int x = 0; x < Constants.TerrainSize; x++)
        {
            for (int z = 0; z < Constants.TerrainSize; z++)
            {
                int index = z * Constants.TerrainSize + x;
                AStarGrid.SetPointSolid(new Vector2I(x, z), tileAttribute.TileFlags[index].HasFlag(TileFlag.NoMove));
            }
        }
        AStarGrid.Update();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Engine.IsEditorHint() && Input.IsActionJustPressed("toggle_move_command") && moveCommand != null)
        {
            GD.Print("Toggle move command!");
            moveCommand.Visible = !moveCommand.Visible;
        }
        if (Character != null && !Character.IsMoving)
        {

            float angle = Mathf.DegToRad(45f);
            // Get the input direction and handle the movement/deceleration.
            // As good practice, you should replace UI actions with custom gameplay actions.
            Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            // Rotate direction around Y axis
            direction = direction.Rotated(Vector3.Up, angle);
            if (direction != Vector3.Zero)
            {
                TilePosition currentTilePos = CurrentCharacterTile;
                TilePosition nextTilePos = new(
                    (byte)Math.Max(currentTilePos.X + Math.Round(direction.X), 0),
                    (byte)Math.Min(255, currentTilePos.Z + Math.Round(direction.Z))
                );
                var path = AStarGrid.GetIdPath(CurrentCharacterTile.GetVector2I(), nextTilePos.GetVector2I());
                Character.SetMovePath(path);
            }
        }
        base._PhysicsProcess(delta);
    }

    private void OnXZPositionChanged(Vector2 newXZ)
    {
        CurrentCharacterTile = newXZ.ToTilePosition();
    }

    private void OnMoveCommandTriggered(int newWorld)
    {
        WorldType world = (WorldType)newWorld;
        this.World = world;
        moveCommand?.Visible = false;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            RaycastFromMouse(mouseEvent.Position);
        }
    }

    private void RaycastFromMouse(Vector2 mousePosition)
    {
        // Skip this click event if move command 
        // is visible and mouse position inside control
        if (
            moveCommand != null
            && moveCommand.Visible
            && mousePosition.IsInsideControl(moveCommand)
        )
        {
            return;
        }
        var camera = GetViewport().GetCamera3D();
        if (camera == null) return;

        Vector3 rayOrigin = camera.ProjectRayOrigin(mousePosition);
        Vector3 rayEnd = rayOrigin + camera.ProjectRayNormal(mousePosition) * 300f;


        var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
        query.CollisionMask = 1;
        var result = SpaceState?.IntersectRay(query);
        if (result == null)
        {
            return;
        }
        var collider = (GodotObject)result["collider"];

        // Check if the map just clicked
        if (collider == StaticBody)
        {
            Vector3 hitPosition = (Vector3)result["position"];
            var path = AStarGrid.GetIdPath(
                CurrentCharacterTile.GetVector2I(),
                hitPosition.ToTilePosition().GetVector2I()
            );
            Character?.SetMovePath(path);
        }
        else if (collider is Character character)
        {
            GD.Print("Character clicked!");
        }
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
        tileAttribute = ResourceLoader.Load<TileAttribute>(WorldAttributeResourcePath);
        SetupWalkableBlocks();

        Character?.SetWorldShape(shape);
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
                TileObjectMap[tile.X, tile.Z] = [];
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
        moveCommand?.Move -= OnMoveCommandTriggered;
        base._ExitTree();
    }
}
