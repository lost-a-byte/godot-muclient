using Godot;
using Godot.Collections;
using MuClient.Extensions;
using MuClient.Models.Terrain;
using System;

namespace MuClient.Scenes.Characters;

[Tool]
public partial class Character : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    private Array<Vector2I> CurrentPath = [];
    private int PathIndex = 0;
    private bool isMoving = false;
    public bool IsMoving => isMoving;
    public HeightMapShape3D WorldShape = new()
    {
        MapWidth = Constants.TerrainSize + 1,
        MapDepth = Constants.TerrainSize + 1,
        MapData = new float[(Constants.TerrainSize + 1) * (Constants.TerrainSize + 1)],
    };

    public override void _PhysicsProcess(double delta)
    {

        if (PathIndex < CurrentPath.Count && isMoving)
        {
            Vector2I gridPos = CurrentPath[PathIndex];
            float y = WorldShape.GetHeightAt(gridPos);
            Vector3 targetPosition = new Vector3(gridPos.X - 127.5f, y, gridPos.Y - 127.5f);
            Position = Position.MoveToward(targetPosition, (float)delta * Speed);

            if (GlobalPosition.DistanceTo(targetPosition) <= 0.1f)
            {
                PathIndex++;
                isMoving = PathIndex < CurrentPath.Count;
            }
        }

        // TODO: Align Character into tile;
        if (!isMoving)
        {
            float targetY = WorldShape.GetHeightAt(Position.GetXZTileVector2I());
            if (Math.Abs(targetY - Position.Y) > 0.1f)
            {
                Position = new(Position.X, targetY, Position.Z);
            }
        }
    }

    private Vector2 xzPosition;
    public Vector2 XZPosition
    {
        get { return xzPosition; }
        set
        {
            if (xzPosition == value) return;
            xzPosition = value;

            EmitSignal(SignalName.XZPositionChanged, xzPosition);
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationTransformChanged)
        {
            XZPosition = new Vector2(Position.X, Position.Z);
        }
    }

    [Signal]
    public delegate void XZPositionChangedEventHandler(Vector2 newXZ);

    public override void _Ready()
    {
        xzPosition = new Vector2(Position.X, Position.Z);
        SetNotifyTransform(true);
        base._Ready();
    }

    // Walkable part

    public void SetWorldShape(HeightMapShape3D shape)
    {
        WorldShape.MapData = [.. shape.MapData];
    }

    public void SetMovePath(Array<Vector2I> path)
    {
        if (isMoving)
        {
            return;
        }
        isMoving = true;
        CurrentPath = path;
        PathIndex = 0;
    }

}
