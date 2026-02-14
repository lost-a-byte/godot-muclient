using Godot;

namespace MuClient.Extensions;

public static class HeightMapShapeExtensions
{
    public static float GetHeightAt(this HeightMapShape3D shape, Vector2I position)
    {
        if (position.X >= 0 && position.X < shape.MapWidth && position.Y >= 0 && position.Y < shape.MapDepth)
        {
            var index = position.Y * shape.MapWidth + position.X;
            float height_value = shape.MapData[index];
            return height_value;
        }
        return 0.0f;
    }
}