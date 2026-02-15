using Godot;
using MuClient.Models.Terrain;

namespace MuClient.Models.MprTables;

[Tool]
public partial class GateItem : Resource
{
    [Export]
    public short Index { get; set; } = 0;
    [Export]
    public byte Type { get; set; } = 0;
    [Export]
    public WorldType World { get; set; } = WorldType.LORENCIA;
    [Export]
    public Vector2I PositionStart { get; set; } = new Vector2I();

    [Export]
    public Vector2I PositionEnd { get; set; } = new Vector2I();

}