using System;
using System.Collections.Generic;
using Godot.Collections;
using MuClient.Models;
using MuClient.Models.Terrain;

namespace MuClient.Database;

public static class SpawnEntryDatabase
{
    static readonly Array<SpawnEntry> objects = new()
    {
        new() {
            Name = $"{WorldType.LORENCIA}",
            World = WorldType.LORENCIA,
            Position = new() { X = 133, Y = 137}
        },
        new() {
            Name = $"{WorldType.ARENA}",
            World = WorldType.ARENA,
            Position = new() { X = 72, Y = 115}
        },
        new() {
            Name = $"{WorldType.NORIA}",
            World = WorldType.NORIA,
            Position = new() { X = 174, Y = 140}
        },
        new() {
            Name = $"{WorldType.DEVIAS}",
            World = WorldType.DEVIAS,
            Position = new() { X = 218, Y = 199}
        },
        new() {
            Name = $"{WorldType.DEVIAS} 2",
            World = WorldType.DEVIAS,
            Position = new() { X = 224, Y = 24}
        },
        new() {
            Name = $"{WorldType.DEVIAS} 3",
            World = WorldType.DEVIAS,
            Position = new() { X = 69, Y = 74}
        },
        new() {
            Name = $"{WorldType.DEVIAS} 4",
            World = WorldType.DEVIAS,
            Position = new() { X = 21, Y = 229}
        },
        new() {
            Name = $"{WorldType.ELBELAND}",
            World = WorldType.ELBELAND,
            Position = new() { X = 52, Y = 30}
        },
        new() {
            Name = $"{WorldType.ELBELAND} 2",
            World = WorldType.ELBELAND,
            Position = new() { X = 99, Y = 203}
        },
        new() {
            Name = $"{WorldType.DUNGEON}",
            World = WorldType.DUNGEON,
            Position = new() { X = 108, Y = 7}
        },
        new() {
            Name = $"{WorldType.DUNGEON} 2",
            World = WorldType.DUNGEON,
            Position = new() { X = 233, Y = 130}
        },
        new() {
            Name = $"{WorldType.DUNGEON} 3",
            World = WorldType.DUNGEON,
            Position = new() { X = 6, Y = 170}
        },

        new() {
            Name = $"{WorldType.LOST_TOWER}",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 207, Y = 179}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 2",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 242, Y = 19}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 3",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 88, Y = 88}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 4",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 88, Y = 168}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 5",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 128, Y = 202}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 6",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 52, Y = 202}
        },
        new() {
            Name = $"{WorldType.LOST_TOWER} 7",
            World = WorldType.LOST_TOWER,
            Position = new() { X = 11, Y = 168}
        },
        new() {
            Name = $"{WorldType.ATLANS}",
            World = WorldType.ATLANS,
            Position = new() { X = 23, Y = 236}
        },
        new() {
            Name = $"{WorldType.ATLANS} 2",
            World = WorldType.ATLANS,
            Position = new() { X = 226, Y = 203}
        },
        new() {
            Name = $"{WorldType.TARKAN}",
            World = WorldType.TARKAN,
            Position = new() { X = 197, Y = 191}
        },
        new() {
            Name = $"{WorldType.TARKAN} 2",
            World = WorldType.TARKAN,
            Position = new() { X = 94, Y = 105}
        },
        new() {
            Name = $"{WorldType.AIDA}",
            World = WorldType.AIDA,
            Position = new() { X = 85, Y = 244}
        },
        new() {
            Name = $"{WorldType.AIDA} 2",
            World = WorldType.AIDA,
            Position = new() { X = 160, Y = 140}
        },
        new() {
            Name = $"{WorldType.ICARUS}",
            World = WorldType.ICARUS,
            Position = new() { X = 14, Y = 243}
        },
        new() {
            Name = $"{WorldType.KANTURU}",
            World = WorldType.KANTURU,
            Position = new() { X = 30, Y = 43}
        },
        new() {
            Name = $"{WorldType.KANTURU} 2",
            World = WorldType.KANTURU,
            Position = new() { X = 71, Y = 73}
        },
        new() {
            Name = $"{WorldType.KARUTAN}",
            World = WorldType.KARUTAN,
            Position = new() { X = 126, Y = 132}
        },
    };

    public static Array<SpawnEntry> GetList()
    {
        return objects;
    }
}