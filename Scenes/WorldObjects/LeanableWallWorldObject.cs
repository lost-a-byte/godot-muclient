using Godot;

namespace MuClient.Scenes.WorldObjects;

public partial class LeanableWallWorldObject : InteractableWorldObject
{
    private Texture cursor = ResourceLoader.Load<ImageTexture>("res://Data/Interface/CursorLeanAgainst.ozt");
    public override Texture? Cursor => cursor;
}