using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace MuClient.Models.Terrain;

public class TilePosition(byte x, byte z) : IEqualityComparer
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
    public Vector2I GetVector2I()
    {
        return new Vector2I(X, Z);
    }

    public new bool Equals(object? x, object? y)
    {
        if (x == null || y == null || x is not TilePosition tileA || y is not TilePosition tileB)
        {
            return false;
        }
        return tileA.X == tileB.X && tileA.Z == tileB.Z;
    }

    public int GetHashCode(object obj)
    {
        return X * 1000 + Z;
    }
}