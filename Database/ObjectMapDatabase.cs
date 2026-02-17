using System;
using System.Collections.Generic;
using MuClient.Models.Terrain;

namespace MuClient.Database;

public static class ObjectMapDatabase
{
    static readonly Dictionary<int, string> objects = new()
    {
        {GetKey(WorldType.LORENCIA, 0), "Tree01.tscn"},
        {GetKey(WorldType.LORENCIA, 1), "Tree02.tscn"},
        {GetKey(WorldType.LORENCIA, 2), "Tree03.bmd"},
        {GetKey(WorldType.LORENCIA, 3), "Tree04.bmd"},
        {GetKey(WorldType.LORENCIA, 4), "Tree05.bmd"},
        {GetKey(WorldType.LORENCIA, 5), "Tree06.bmd"},
        {GetKey(WorldType.LORENCIA, 6), "Tree07.bmd"},
        {GetKey(WorldType.LORENCIA, 7), "Tree08.bmd"},
        {GetKey(WorldType.LORENCIA, 8), "Tree09.bmd"},
        {GetKey(WorldType.LORENCIA, 9), "Tree10.bmd"},
        {GetKey(WorldType.LORENCIA, 10), "Tree11.tscn"},
        {GetKey(WorldType.LORENCIA, 11), "Tree12.tscn"},
        {GetKey(WorldType.LORENCIA, 12), "Tree13.tscn"},
        {GetKey(WorldType.LORENCIA, 20), "Grass01.bmd"},
        {GetKey(WorldType.LORENCIA, 21), "Grass02.bmd"},
        {GetKey(WorldType.LORENCIA, 22), "Grass03.bmd"},
        {GetKey(WorldType.LORENCIA, 23), "Grass04.bmd"},
        {GetKey(WorldType.LORENCIA, 24), "Grass05.bmd"},
        {GetKey(WorldType.LORENCIA, 25), "Grass06.bmd"},
        {GetKey(WorldType.LORENCIA, 26), "Grass07.bmd"},
        {GetKey(WorldType.LORENCIA, 27), "Grass08.bmd"},
        {GetKey(WorldType.LORENCIA, 30), "Stone01.bmd"},
        {GetKey(WorldType.LORENCIA, 31), "Stone02.bmd"},
        {GetKey(WorldType.LORENCIA, 32), "Stone03.bmd"},
        {GetKey(WorldType.LORENCIA, 33), "Stone04.bmd"},
        {GetKey(WorldType.LORENCIA, 34), "Stone05.bmd"},
        {GetKey(WorldType.LORENCIA, 40), "StoneStatue01.bmd"},
        {GetKey(WorldType.LORENCIA, 41), "StoneStatue02.bmd"},
        {GetKey(WorldType.LORENCIA, 42), "StoneStatue03.bmd"},
        {GetKey(WorldType.LORENCIA, 43), "SteelStatue01.bmd"},
        {GetKey(WorldType.LORENCIA, 44), "Tomb01.bmd"},
        {GetKey(WorldType.LORENCIA, 45), "Tomb02.bmd"},
        {GetKey(WorldType.LORENCIA, 46), "Tomb03.bmd"},
        {GetKey(WorldType.LORENCIA, 50), "FireLight01.bmd"},
        {GetKey(WorldType.LORENCIA, 51), "FireLight02.bmd"},
        {GetKey(WorldType.LORENCIA, 52), "Bonfire01.bmd"},
        {GetKey(WorldType.LORENCIA, 55), "DoungeonGate01.bmd"},
        {GetKey(WorldType.LORENCIA, 56), "MerchantAnimal01.tscn"},
        {GetKey(WorldType.LORENCIA, 57), "MerchantAnimal02.tscn"},
        {GetKey(WorldType.LORENCIA, 58), "TreasureDrum01.bmd"},
        {GetKey(WorldType.LORENCIA, 59), "TreasureChest01.bmd"},
        {GetKey(WorldType.LORENCIA, 60), "Ship01.tscn"},
        {GetKey(WorldType.LORENCIA, 65), "SteelWall01.bmd"},
        {GetKey(WorldType.LORENCIA, 66), "SteelWall02.bmd"},
        {GetKey(WorldType.LORENCIA, 67), "SteelWall03.bmd"},
        {GetKey(WorldType.LORENCIA, 68), "SteelDoor01.bmd"},
        {GetKey(WorldType.LORENCIA, 69), "StoneWall01.bmd"},
        {GetKey(WorldType.LORENCIA, 70), "StoneWall02.bmd"},
        {GetKey(WorldType.LORENCIA, 72), "StoneWall04.tscn"},
        {GetKey(WorldType.LORENCIA, 74), "StoneWall06.tscn"},
        {GetKey(WorldType.LORENCIA, 75), "StoneMuWall01.bmd"},
        {GetKey(WorldType.LORENCIA, 76), "StoneMuWall02.bmd"},
        {GetKey(WorldType.LORENCIA, 77), "StoneMuWall03.bmd"},
        {GetKey(WorldType.LORENCIA, 78), "StoneMuWall04.bmd"},
        {GetKey(WorldType.LORENCIA, 80), "Bridge01.bmd"},
        {GetKey(WorldType.LORENCIA, 81), "Fence01.bmd"},
        {GetKey(WorldType.LORENCIA, 82), "Fence02.bmd"},
        {GetKey(WorldType.LORENCIA, 83), "Fence03.bmd"},
        {GetKey(WorldType.LORENCIA, 84), "Fence04.bmd"},
        {GetKey(WorldType.LORENCIA, 85), "BridgeStone01.bmd"},
        {GetKey(WorldType.LORENCIA, 90), "StreetLight01.tscn"},
        {GetKey(WorldType.LORENCIA, 91), "Cannon01.bmd"},
        {GetKey(WorldType.LORENCIA, 92), "Cannon02.bmd"},
        {GetKey(WorldType.LORENCIA, 93), "Cannon03.bmd"},
        {GetKey(WorldType.LORENCIA, 95), "Curtain01.tscn"},
        {GetKey(WorldType.LORENCIA, 96), "Sign01.tscn"},
        {GetKey(WorldType.LORENCIA, 97), "Sign02.bmd"},
        {GetKey(WorldType.LORENCIA, 98), "Carriage01.tscn"},
        {GetKey(WorldType.LORENCIA, 99), "Carriage02.bmd"},
        {GetKey(WorldType.LORENCIA, 101), "Carriage04.bmd"},
        {GetKey(WorldType.LORENCIA, 102), "Straw01.bmd"},
        {GetKey(WorldType.LORENCIA, 103), "Straw02.bmd"},
        {GetKey(WorldType.LORENCIA, 105), "Waterspout01.tscn"},
        // {GetKey(WorldType.LORENCIA, 105), "heroNpcSide.tscn"},
        {GetKey(WorldType.LORENCIA, 107), "Well02.bmd"},
        {GetKey(WorldType.LORENCIA, 108), "Well03.bmd"},
        {GetKey(WorldType.LORENCIA, 109), "Well04.bmd"},
        {GetKey(WorldType.LORENCIA, 110), "Hanging01.tscn"},
        {GetKey(WorldType.LORENCIA, 111), "Stair01.bmd"},
        {GetKey(WorldType.LORENCIA, 115), "House01.bmd"},
        {GetKey(WorldType.LORENCIA, 116), "House02.bmd"},
        {GetKey(WorldType.LORENCIA, 117), "House03.bmd"},
        {GetKey(WorldType.LORENCIA, 118), "House04.tscn"},
        {GetKey(WorldType.LORENCIA, 119), "House05.tscn"},
        {GetKey(WorldType.LORENCIA, 120), "Tent01.tscn"},
        {GetKey(WorldType.LORENCIA, 121), "HouseWall01.bmd"},
        {GetKey(WorldType.LORENCIA, 122), "HouseWall02.bmd"},
        {GetKey(WorldType.LORENCIA, 123), "HouseWall03.bmd"},
        {GetKey(WorldType.LORENCIA, 124), "HouseWall04.bmd"},
        {GetKey(WorldType.LORENCIA, 125), "HouseWall05.bmd"},
        {GetKey(WorldType.LORENCIA, 126), "HouseWall06.bmd"},
        {GetKey(WorldType.LORENCIA, 127), "HouseEtc01.bmd"},
        {GetKey(WorldType.LORENCIA, 128), "HouseEtc02.bmd"},
        {GetKey(WorldType.LORENCIA, 129), "HouseEtc03.bmd"},
        {GetKey(WorldType.LORENCIA, 131), "Light02.bmd"},
        {GetKey(WorldType.LORENCIA, 132), "Light03.bmd"},
        {GetKey(WorldType.LORENCIA, 140), "Furniture01.bmd"},
        {GetKey(WorldType.LORENCIA, 141), "Furniture02.bmd"},
        {GetKey(WorldType.LORENCIA, 142), "Furniture03.bmd"},
        {GetKey(WorldType.LORENCIA, 143), "Furniture04.bmd"},
        {GetKey(WorldType.LORENCIA, 144), "Furniture05.bmd"},
        {GetKey(WorldType.LORENCIA, 145), "Furniture06.bmd"},
        {GetKey(WorldType.LORENCIA, 146), "Furniture07.bmd"},
        {GetKey(WorldType.LORENCIA, 150), "Candle01.tscn"},
        {GetKey(WorldType.LORENCIA, 151), "Beer01.bmd"},
        {GetKey(WorldType.LORENCIA, 152), "Beer02.bmd"},
        {GetKey(WorldType.LORENCIA, 153), "Beer03.bmd"},
        {GetKey(WorldType.DUNGEON, 11), "Object12.tscn"},
        {GetKey(WorldType.DUNGEON, 22), "Object23.tscn"},
        {GetKey(WorldType.DUNGEON, 23), "Object24.tscn"},
        {GetKey(WorldType.DUNGEON, 24), "Object25.tscn"},
        {GetKey(WorldType.DUNGEON, 35), "Object36.tscn"},
        {GetKey(WorldType.DUNGEON, 40), "Object41.tscn"},
        {GetKey(WorldType.DUNGEON, 53), "Object54.tscn"},
        {GetKey(WorldType.DUNGEON, 61), "Object62.tscn"},
        {GetKey(WorldType.DEVIAS, 0), "Object01.tscn"},
        {GetKey(WorldType.DEVIAS, 9), "Object10.tscn"},
        {GetKey(WorldType.DEVIAS, 10), "Object11.tscn"},
        {GetKey(WorldType.DEVIAS, 19), "Object20.tscn"},
        {GetKey(WorldType.DEVIAS, 35), "Object36.tscn"},
        {GetKey(WorldType.DEVIAS, 54), "Object55.tscn"},
        {GetKey(WorldType.DEVIAS, 56), "Object57.tscn"},
        {GetKey(WorldType.DEVIAS, 75), "Object76.tscn"},
        {GetKey(WorldType.DEVIAS, 83), "Object84.tscn"},
        {GetKey(WorldType.DEVIAS, 84), "Object85.tscn"},
        {GetKey(WorldType.DEVIAS, 85), "Object86.tscn"},
        {GetKey(WorldType.DEVIAS, 91), "Object92.tscn"},
        {GetKey(WorldType.DEVIAS, 100), "Object101.tscn"},
        {GetKey(WorldType.DEVIAS, 103), "Object104.tscn"},
        {GetKey(WorldType.DEVIAS, 104), "Object105.tscn"},
        {GetKey(WorldType.NORIA, 1 - 1), "Object01.tscn"},
        {GetKey(WorldType.NORIA, 2 - 1), "Object02.tscn"},
        {GetKey(WorldType.NORIA, 10 - 1), "Object10.tscn"},
        {GetKey(WorldType.NORIA, 12 - 1), "Object12.tscn"},
        {GetKey(WorldType.NORIA, 13 - 1), "Object13.tscn"},
        {GetKey(WorldType.NORIA, 14 - 1), "Object14.tscn"},
        {GetKey(WorldType.NORIA, 15 - 1), "Object15.tscn"},
        {GetKey(WorldType.NORIA, 16 - 1), "Object16.tscn"},
        {GetKey(WorldType.NORIA, 17 - 1), "Object17.tscn"},
        {GetKey(WorldType.NORIA, 18 - 1), "Object18.tscn"},
        {GetKey(WorldType.NORIA, 19 - 1), "Object19.tscn"},
        {GetKey(WorldType.NORIA, 21 - 1), "Object21.tscn"},
        {GetKey(WorldType.NORIA, 23 - 1), "Object23.tscn"},
        {GetKey(WorldType.NORIA, 24 - 1), "Object24.tscn"},
        {GetKey(WorldType.NORIA, 27 - 1), "Object27.tscn"},
        {GetKey(WorldType.NORIA, 28 - 1), "Object28.tscn"},
        {GetKey(WorldType.NORIA, 35 - 1), "Object35.tscn"},
        {GetKey(WorldType.NORIA, 36 - 1), "Object36.tscn"},
        {GetKey(WorldType.NORIA, 37 - 1), "Object37.tscn"},
        {GetKey(WorldType.NORIA, 39 - 1), "Object39.tscn"},
        {GetKey(WorldType.NORIA, 40 - 1), "Object40.tscn"},
        {GetKey(WorldType.NORIA, 41 - 1), "Object41.tscn"},
        {GetKey(WorldType.NORIA, 43 - 1), "Object43.tscn"},
    };

    public static string GetPath(WorldType world, short type)
    {

        objects.TryGetValue(GetKey(world, type), out string? matched);
        if (matched == null)
        {
            var modelPath = $"Object{(type + 1).ToString().PadLeft(2, '0')}.bmd";
            return modelPath;
        }
        return matched;
    }
    public static string GetResourcePath(WorldType world, short type)
    {
        string filename = GetPath(world, type);
        string resourceFolder = $"res://Data/Object{(int)world}";
        return resourceFolder + "/" + filename;
    }

    static int GetKey(WorldType world, short type)
    {
        // Cast both to int for bitwise operations and the final return type
        int uintAsInt = (int)world;
        int shortAsInt = type;

        // A simple combination: XOR the uint with the short shifted by 16 bits
        // This is one common pattern but may have more collisions than HashCode.Combine
        int combinedHash = uintAsInt ^ (shortAsInt << 16);
        return combinedHash;
    }
}