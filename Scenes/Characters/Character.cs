using Godot;
using System;

namespace MuClient.Scenes;

[Tool]
public partial class Character : Node3D
{

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
}
