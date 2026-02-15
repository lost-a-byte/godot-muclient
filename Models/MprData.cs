using System.Text;
using Godot;
using Godot.Collections;

namespace MuClient.Models.Terrain;

[Tool]
public partial class MprData : Resource
{
    [Export]
    public Dictionary<string, byte[]> Data { get; set; } = [];
    [Export]
    public Array<string> Encode { get; set; } = ["UTF8", "EUC-KR", "EUC-JP"];
}