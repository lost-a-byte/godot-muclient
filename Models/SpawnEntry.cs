using Godot;
using MuClient.Models.Terrain;

namespace MuClient.Models;

[Tool]
public partial class SpawnEntry : Resource
{
    [Export]
    public string Name { get; set; } = "";
    [Export]
    public short Index { get; set; } = 0;
    [Export]
    public byte Type { get; set; } = 0;
    [Export]
    public WorldType World { get; set; } = WorldType.LORENCIA;
    [Export]
    public Vector2I Position { get; set; } = new Vector2I();

}