using System;
using Godot;
using Godot.Collections;

namespace MuClient.Models.Terrain;

[Tool]
[GlobalClass]
public partial class TileAttribute : Resource
{
    [Export]
    public WorldType World { get; set; }
    [Export]
    public byte Version { get; set; }
    [Export]
    public byte Width { get; set; }
    [Export]
    public byte Height { get; set; }

    [Export]
    public Array<TileFlag> TileFlags { get; set; }

}