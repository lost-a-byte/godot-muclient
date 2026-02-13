
using Godot;

namespace MuClient.addons.MuResourceImporter.Types;

[Tool]
public partial class BmdMeshBlend : Resource
{
    [Export]
    public BmdMeshBlendType Mesh0 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh1 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh2 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh3 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh4 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh5 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh6 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh7 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh8 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh9 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh10 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh11 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh12 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh13 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh14 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh15 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh16 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh17 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh18 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh19 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh20 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh21 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh22 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh23 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh24 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh25 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh26 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh27 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh28 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh29 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh30 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh31 = BmdMeshBlendType.None;
    [Export]
    public BmdMeshBlendType Mesh32 = BmdMeshBlendType.None;


    public BmdMeshBlendType? GetBlendByIndex(int index)
    {
        return index switch
        {
            0 => Mesh0,
            1 => Mesh1,
            2 => Mesh2,
            3 => Mesh3,
            4 => Mesh4,
            5 => Mesh5,
            6 => Mesh6,
            7 => Mesh7,
            8 => Mesh8,
            9 => Mesh9,
            10 => Mesh10,
            11 => Mesh11,
            12 => Mesh12,
            13 => Mesh13,
            14 => Mesh14,
            15 => Mesh15,
            16 => Mesh16,
            17 => Mesh17,
            18 => Mesh18,
            19 => Mesh19,
            20 => Mesh20,
            21 => Mesh21,
            22 => Mesh22,
            23 => Mesh23,
            24 => Mesh24,
            25 => Mesh25,
            26 => Mesh26,
            27 => Mesh27,
            28 => Mesh28,
            29 => Mesh29,
            30 => Mesh30,
            31 => Mesh31,
            32 => Mesh32,
            _ => null
        };
    }
}