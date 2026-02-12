#if TOOLS
namespace MuClient.addons.MuResourceImporter.Extensions;

public static class BMDTextureBoneExtensions
{
    public static bool IsDummy(this Client.Data.BMD.BMDTextureBone bone)
    {
        return
            bone.Name == Client.Data.BMD.BMDTextureBone.Dummy.Name
            && bone.Parent == 0
            && bone.Matrixes.Length == 0;
    }
    public static Godot.Vector2 ToGodotVector2(this Client.Data.BMD.BMDTexCoord vector)
    {
        return new Godot.Vector2(vector.U, vector.V);
    }
}
#endif
