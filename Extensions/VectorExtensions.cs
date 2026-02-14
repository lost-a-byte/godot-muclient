using System;
using Godot;
using MuClient.Models.Terrain;

namespace MuClient.Extensions;

public static class VectorExtensions
{
    public static Vector3 ToGodotVector3(this System.Numerics.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
    public static Vector2 ToGodotVector2(this System.Numerics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    public static Quaternion ToGodotQuaternion(this System.Numerics.Quaternion quaternion)
    {
        return new Quaternion(
            quaternion.X,
            quaternion.Y,
            quaternion.Z,
            quaternion.W
        );
    }
    public static TilePosition ToTilePosition(this Vector2 vector)
    {
        return new TilePosition((byte)Math.Floor(vector.X + Constants.TerrainSize / 2), (byte)Math.Floor(vector.Y + Constants.TerrainSize / 2));
    }
    public static TilePosition ToTilePosition(this Vector3 vector)
    {
        return new TilePosition((byte)Math.Floor(vector.X + Constants.TerrainSize / 2), (byte)Math.Floor(vector.Z + Constants.TerrainSize / 2));
    }
    public static Vector2I GetXZTileVector2I(this Vector3 vector)
    {
        return new((byte)Math.Floor(vector.X + Constants.TerrainSize / 2), (byte)Math.Floor(vector.Z + Constants.TerrainSize / 2));
    }

    public static Vector2I GetXZTileVector2I(this Vector2 vector)
    {
        return new((byte)Math.Floor(vector.X + Constants.TerrainSize / 2), (byte)Math.Floor(vector.Y + Constants.TerrainSize / 2));
    }
    public static bool IsInsideControl(this Vector2 vector, Control control)
    {
        // Check if the mouse coordinates are within the box's boundaries
        return (
            vector.X >= control.Position.Y &&
            vector.X <= control.Size.X &&
            vector.Y >= control.Position.X &&
            vector.Y <= control.Size.Y
        );
    }
    public static bool IsOutsideControl(this Vector2 vector, Control control)
    {
        // Check if the mouse coordinates are outside the box's boundaries
        return !IsInsideControl(vector, control);
    }

}