using Godot;

namespace MuClient.Models.Terrain;

[Tool]
public partial class ObjectAttribute : Resource
{
    [Export]
    public short Type { get; set; } = 0;
    [Export]
    public Vector3 Position { get; set; } = Vector3.Zero;
    [Export]
    public Vector3 Angle { get; set; } = Vector3.Zero;
    [Export]
    public float Scale { get; set; } = 0;
    [Export]
    public byte UnknownX { get; set; } = 0;
    [Export]
    public byte UnknownY { get; set; } = 0;
    [Export]
    public byte UnknownZ { get; set; } = 0;
    [Export]
    public Vector3 Lighting { get; set; } = Vector3.Zero;
    [Export]
    public byte UnknownByte { get; set; } = 0;
    [Export]
    public float UnknownFloat1 { get; set; } = 0;
    [Export]
    public float UnknownFloat2 { get; set; } = 0;
}