using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace MuClient.Database;

public class ObjectMapDataKey(int worldIndex, short type) : EqualityComparer<ObjectMapDataKey>
{
    public int WorldIndex { get; set; } = worldIndex;
    public short Type { get; set; } = type;

    public override bool Equals(ObjectMapDataKey x, ObjectMapDataKey y)
    {
        return x.WorldIndex == y.WorldIndex && x.Type == y.WorldIndex;
    }
    public override bool Equals(object obj)
    {
        if (obj is ObjectMapDataKey key)
        {

            return WorldIndex == key.WorldIndex && Type == key.WorldIndex;
        }
        return false;
    }

    public override int GetHashCode([DisallowNull] ObjectMapDataKey obj)
    {
        // Cast both to int for bitwise operations and the final return type
        int uintAsInt = obj.WorldIndex;
        int shortAsInt = obj.Type;

        // A simple combination: XOR the uint with the short shifted by 16 bits
        // This is one common pattern but may have more collisions than HashCode.Combine
        int combinedHash = uintAsInt ^ (shortAsInt << 16);
        return combinedHash;
    }
    public override int GetHashCode()
    {
        // Cast both to int for bitwise operations and the final return type
        int uintAsInt = WorldIndex;
        int shortAsInt = Type;

        // A simple combination: XOR the uint with the short shifted by 16 bits
        // This is one common pattern but may have more collisions than HashCode.Combine
        int combinedHash = uintAsInt ^ (shortAsInt << 16);
        return combinedHash;
    }
}

public static class ObjectMapDatabase
{
    static readonly Dictionary<int, string> objects = new Dictionary<int, string>
    {
        {new ObjectMapDataKey(1, 0).GetHashCode(), "Tree01"},
        {new ObjectMapDataKey(1, 1).GetHashCode(), "Tree02"},
        {new ObjectMapDataKey(1, 2).GetHashCode(), "Tree03"},
        {new ObjectMapDataKey(1, 3).GetHashCode(), "Tree04"},
        {new ObjectMapDataKey(1, 4).GetHashCode(), "Tree05"},
        {new ObjectMapDataKey(1, 5).GetHashCode(), "Tree06"},
        {new ObjectMapDataKey(1, 6).GetHashCode(), "Tree07"},
        {new ObjectMapDataKey(1, 7).GetHashCode(), "Tree08"},
        {new ObjectMapDataKey(1, 8).GetHashCode(), "Tree09"},
        {new ObjectMapDataKey(1, 9).GetHashCode(), "Tree10"},
        {new ObjectMapDataKey(1, 10).GetHashCode(), "Tree11"},
        {new ObjectMapDataKey(1, 11).GetHashCode(), "Tree12"},
        {new ObjectMapDataKey(1, 12).GetHashCode(), "Tree13"},
        {new ObjectMapDataKey(1, 20).GetHashCode(), "Grass01"},
        {new ObjectMapDataKey(1, 21).GetHashCode(), "Grass02"},
        {new ObjectMapDataKey(1, 22).GetHashCode(), "Grass03"},
        {new ObjectMapDataKey(1, 23).GetHashCode(), "Grass04"},
        {new ObjectMapDataKey(1, 24).GetHashCode(), "Grass05"},
        {new ObjectMapDataKey(1, 25).GetHashCode(), "Grass06"},
        {new ObjectMapDataKey(1, 26).GetHashCode(), "Grass07"},
        {new ObjectMapDataKey(1, 27).GetHashCode(), "Grass08"},
        {new ObjectMapDataKey(1, 30).GetHashCode(), "Stone01"},
        {new ObjectMapDataKey(1, 31).GetHashCode(), "Stone02"},
        {new ObjectMapDataKey(1, 32).GetHashCode(), "Stone03"},
        {new ObjectMapDataKey(1, 33).GetHashCode(), "Stone04"},
        {new ObjectMapDataKey(1, 34).GetHashCode(), "Stone05"},
        {new ObjectMapDataKey(1, 40).GetHashCode(), "StoneStatue01"},
        {new ObjectMapDataKey(1, 41).GetHashCode(), "StoneStatue02"},
        {new ObjectMapDataKey(1, 42).GetHashCode(), "StoneStatue03"},
        {new ObjectMapDataKey(1, 43).GetHashCode(), "SteelStatue01"},
        {new ObjectMapDataKey(1, 44).GetHashCode(), "Tomb01"},
        {new ObjectMapDataKey(1, 45).GetHashCode(), "Tomb02"},
        {new ObjectMapDataKey(1, 46).GetHashCode(), "Tomb03"},
        {new ObjectMapDataKey(1, 50).GetHashCode(), "FireLight01"},
        {new ObjectMapDataKey(1, 51).GetHashCode(), "FireLight02"},
        {new ObjectMapDataKey(1, 52).GetHashCode(), "Bonfire01"},
        {new ObjectMapDataKey(1, 55).GetHashCode(), "DoungeonGate01"},
        {new ObjectMapDataKey(1, 56).GetHashCode(), "MerchantAnimal01"},
        {new ObjectMapDataKey(1, 57).GetHashCode(), "MerchantAnimal02"},
        {new ObjectMapDataKey(1, 58).GetHashCode(), "TreasureDrum01"},
        {new ObjectMapDataKey(1, 59).GetHashCode(), "TreasureChest01"},
        {new ObjectMapDataKey(1, 60).GetHashCode(), "Ship01"},
        {new ObjectMapDataKey(1, 65).GetHashCode(), "SteelWall01"},
        {new ObjectMapDataKey(1, 66).GetHashCode(), "SteelWall02"},
        {new ObjectMapDataKey(1, 67).GetHashCode(), "SteelWall03"},
        {new ObjectMapDataKey(1, 68).GetHashCode(), "SteelDoor01"},
        {new ObjectMapDataKey(1, 69).GetHashCode(), "StoneWall01"},
        {new ObjectMapDataKey(1, 70).GetHashCode(), "StoneWall02"},
        {new ObjectMapDataKey(1, 72).GetHashCode(), "StoneWall04"},
        {new ObjectMapDataKey(1, 74).GetHashCode(), "StoneWall06"},
        {new ObjectMapDataKey(1, 75).GetHashCode(), "StoneMuWall01"},
        {new ObjectMapDataKey(1, 76).GetHashCode(), "StoneMuWall02"},
        {new ObjectMapDataKey(1, 77).GetHashCode(), "StoneMuWall03"},
        {new ObjectMapDataKey(1, 78).GetHashCode(), "StoneMuWall04"},
        {new ObjectMapDataKey(1, 80).GetHashCode(), "Bridge01"},
        {new ObjectMapDataKey(1, 81).GetHashCode(), "Fence01"},
        {new ObjectMapDataKey(1, 82).GetHashCode(), "Fence02"},
        {new ObjectMapDataKey(1, 83).GetHashCode(), "Fence03"},
        {new ObjectMapDataKey(1, 84).GetHashCode(), "Fence04"},
        {new ObjectMapDataKey(1, 85).GetHashCode(), "BridgeStone01"},
        {new ObjectMapDataKey(1, 90).GetHashCode(), "StreetLight01"},
        {new ObjectMapDataKey(1, 91).GetHashCode(), "Cannon01"},
        {new ObjectMapDataKey(1, 92).GetHashCode(), "Cannon02"},
        {new ObjectMapDataKey(1, 93).GetHashCode(), "Cannon03"},
        {new ObjectMapDataKey(1, 95).GetHashCode(), "Curtain01"},
        {new ObjectMapDataKey(1, 96).GetHashCode(), "Sign01"},
        {new ObjectMapDataKey(1, 97).GetHashCode(), "Sign02"},
        {new ObjectMapDataKey(1, 98).GetHashCode(), "Carriage01"},
        {new ObjectMapDataKey(1, 99).GetHashCode(), "Carriage02"},
        {new ObjectMapDataKey(1, 101).GetHashCode(), "Carriage04"},
        {new ObjectMapDataKey(1, 102).GetHashCode(), "Straw01"},
        {new ObjectMapDataKey(1, 103).GetHashCode(), "Straw02"},
        {new ObjectMapDataKey(1, 105).GetHashCode(), "Waterspout01"},
        {new ObjectMapDataKey(1, 107).GetHashCode(), "Well02"},
        {new ObjectMapDataKey(1, 108).GetHashCode(), "Well03"},
        {new ObjectMapDataKey(1, 109).GetHashCode(), "Well04"},
        {new ObjectMapDataKey(1, 110).GetHashCode(), "Hanging01"},
        {new ObjectMapDataKey(1, 111).GetHashCode(), "Stair01"},
        {new ObjectMapDataKey(1, 115).GetHashCode(), "House01"},
        {new ObjectMapDataKey(1, 116).GetHashCode(), "House02"},
        {new ObjectMapDataKey(1, 117).GetHashCode(), "House03"},
        {new ObjectMapDataKey(1, 118).GetHashCode(), "House04"},
        {new ObjectMapDataKey(1, 119).GetHashCode(), "House05"},
        {new ObjectMapDataKey(1, 120).GetHashCode(), "Tent01"},
        {new ObjectMapDataKey(1, 121).GetHashCode(), "HouseWall01"},
        {new ObjectMapDataKey(1, 122).GetHashCode(), "HouseWall02"},
        {new ObjectMapDataKey(1, 123).GetHashCode(), "HouseWall03"},
        {new ObjectMapDataKey(1, 124).GetHashCode(), "HouseWall04"},
        {new ObjectMapDataKey(1, 125).GetHashCode(), "HouseWall05"},
        {new ObjectMapDataKey(1, 126).GetHashCode(), "HouseWall06"},
        {new ObjectMapDataKey(1, 127).GetHashCode(), "HouseEtc01"},
        {new ObjectMapDataKey(1, 128).GetHashCode(), "HouseEtc02"},
        {new ObjectMapDataKey(1, 129).GetHashCode(), "HouseEtc03"},
        {new ObjectMapDataKey(1, 131).GetHashCode(), "Light02"},
        {new ObjectMapDataKey(1, 132).GetHashCode(), "Light03"},
        {new ObjectMapDataKey(1, 140).GetHashCode(), "Furniture01"},
        {new ObjectMapDataKey(1, 141).GetHashCode(), "Furniture02"},
        {new ObjectMapDataKey(1, 142).GetHashCode(), "Furniture03"},
        {new ObjectMapDataKey(1, 143).GetHashCode(), "Furniture04"},
        {new ObjectMapDataKey(1, 144).GetHashCode(), "Furniture05"},
        {new ObjectMapDataKey(1, 145).GetHashCode(), "Furniture06"},
        {new ObjectMapDataKey(1, 146).GetHashCode(), "Furniture07"},
        {new ObjectMapDataKey(1, 150).GetHashCode(), "Candle01"},
        {new ObjectMapDataKey(1, 151).GetHashCode(), "Beer01"},
        {new ObjectMapDataKey(1, 152).GetHashCode(), "Beer02"},
        {new ObjectMapDataKey(1, 153).GetHashCode(), "Beer03"}
    };

    public static string GetPath(int worldIndex, short type)
    {
        string matched;
        ObjectMapDataKey pathToFind = new ObjectMapDataKey(worldIndex, type);
        objects.TryGetValue(pathToFind.GetHashCode(), out matched);
        if (matched == null)
        {
            var modelPath = $"Object{(type + 1).ToString().PadLeft(2, '0')}.bmd";
            return modelPath;
        }
        return matched + ".bmd";
    }
    public static string GetResourcePath(int worldIndex, short type)
    {
        string filename = GetPath(worldIndex, type);
        string resourceFolder = $"res://Data/Object{worldIndex}";
        return resourceFolder + "/" + filename;
    }
}