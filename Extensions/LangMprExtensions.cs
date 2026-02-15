using System;
using System.Linq;
using System.Text;
using Godot;
using Godot.Collections;
using MuClient.Models.MprTables;
using MuClient.Models.Terrain;

namespace MuClient.Extensions;

public static class LangMprExtensions
{
    public static Array<GateItem> GetGateItems(this MprData mprData)
    {
        var buffer = mprData.Data["\\Gate.txt"];
        if (buffer == null)
        {
            return [];
        }
        string rawString = Encoding.UTF8.GetString(buffer);
        return [
            ..rawString
            .Split(['\n'])
            .Select(l => l.Trim())
            .Where(l => l.Length > 0 && l.Count('\t') > 7)
            .ToList()
            .Select(line =>
            {
                var valueArray = line.Split('\t');

                return new GateItem()
                {
                    Index = short.Parse(valueArray[0]),
                    Type = byte.Parse(valueArray[1]),
                    World = (WorldType)(byte.Parse(valueArray[2])+ 1),
                    PositionStart = new(int.Parse(valueArray[3]), Math.Max(0, Constants.TerrainSize - 1 - int.Parse(valueArray[4]))),
                    PositionEnd = new(int.Parse(valueArray[5]), int.Parse(valueArray[6])),

                };
            })
            .ToArray()
        ];
    }
}