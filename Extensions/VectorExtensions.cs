namespace MuClient.Extensions;

public static class VectorExtensions
{
    public static Godot.Vector3 ToGodotVector3(this System.Numerics.Vector3 vector)
    {
        return new Godot.Vector3(vector.X, vector.Y, vector.Z);
    }
    public static Godot.Vector2 ToGodotVector2(this System.Numerics.Vector2 vector)
    {
        return new Godot.Vector2(vector.X, vector.Y);
    }
    public static Godot.Quaternion ToGodotQuaternion(this System.Numerics.Quaternion quaternion)
    {
        return new Godot.Quaternion(
            quaternion.X,
            quaternion.Y,
            quaternion.Z,
            quaternion.W
        );
    }
}