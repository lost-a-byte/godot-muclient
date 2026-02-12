using Godot;
using Godot.Collections;

namespace MuClient.Models.Terrain;

[Tool]
[GlobalClass]
public partial class ObjectMap : Resource
{
    [Export]
    public WorldType World { get; set; }
    [Export]
    public byte Version { get; set; }
    [Export]
    public Array<ObjectAttribute> Objects { get; set; }
    [Export]
    public Array<short> ObjectTypes { get; set; }
}
