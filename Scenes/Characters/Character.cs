using Godot;
using System;

namespace MuClient.Scenes.Characters;

[Tool]
public partial class Character : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }
        float angle = Mathf.DegToRad(45f);

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        // Rotate direction around Y axis
        direction = direction.Rotated(Vector3.Up, angle);
        
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
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
}
