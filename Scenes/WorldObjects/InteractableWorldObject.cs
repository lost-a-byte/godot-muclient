using Godot;

namespace MuClient.Scenes.WorldObjects;

public partial class InteractableWorldObject : StaticBody3D
{
    public virtual Texture? Cursor => null;
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _MouseEnter()
    {
        Input.SetCustomMouseCursor(Cursor, Input.CursorShape.Arrow, new Vector2(16, 16));
        base._MouseEnter();
    }

    public override void _MouseExit()
    {
        Input.SetCustomMouseCursor(null);
        base._MouseExit();
    }
}
