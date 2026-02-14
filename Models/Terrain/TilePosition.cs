using System;
using Godot;

namespace MuClient.Models.Terrain;

public class TilePosition(byte x, byte z)
{
    public byte X = x;
    public byte Z = z;

    public bool IsNearBy(TilePosition other)
    {
        float dx = other.X - X;
        float dz = other.Z - Z;

        float distanceSquared = dx * dx + dz * dz;
        float radius = 35f;

        return distanceSquared < radius * radius;

        // Or return 
        // return other.GetVector2().DistanceTo(GetVector2()) < radius;
    }
    public override string ToString()
    {
        return $"({X},{Z})";
    }

    public Vector2 GetVector2()
    {
        return new Vector2(X, Z);
    }
}