using System;

namespace MuClient.Models.Terrain;

public class TilePosition(byte x, byte z)
{
    public byte X = x;
    public byte Z = z;

    public bool IsNearBy(TilePosition other)
    {
        if (other == null)
        {
            return false;
        }
        int x = Math.Abs(other.X - X);
        int z = Math.Abs(other.Z - Z);
        if (x + z < 30)
        {
            return true;
        }
        return false;
    }
    public override string ToString()
    {
        return $"({X},{Z})";
    }
}