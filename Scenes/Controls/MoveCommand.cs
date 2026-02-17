using Godot;
using Godot.Collections;
using MuClient.Database;
using MuClient.Extensions;
using MuClient.Models;
using MuClient.Models.MprTables;
using MuClient.Models.Terrain;

using System;

namespace MuClient.Scenes.Controls;

[Tool]
public partial class MoveCommand : Control
{
    [Export]
    public WorldType World { get; set; } = WorldType.LORENCIA;

    [Signal]
    public delegate void MoveEventHandler(int world);
    [Signal]
    public delegate void MoveGateEventHandler(SpawnEntry entry);

    private VBoxContainer? container;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        container = GetNode<VBoxContainer>("Window/ScrollContainer/List");
        BuildWorldList();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    void BuildWorldList()
    {
        ClearWorldList();


        foreach (SpawnEntry entry in SpawnEntryDatabase.GetList())
        {
            Button button = new()
            {
                Text = entry.Name,
            };
            button.Pressed += () => GatePressed(entry);
            container?.AddChild(button);
        }
    }

    void ClearWorldList()
    {
        if (container == null) return;
        foreach (var item in container.GetChildren())
        {
            item.QueueFree();
        }
    }

    private void GatePressed(SpawnEntry entry)
    {
        GetViewport().SetInputAsHandled();
        EmitSignal(SignalName.MoveGate, entry);
    }

    private void ButtonPressed(WorldType world)
    {
        GD.Print($"World {world} clicked!");
        GetViewport().SetInputAsHandled();
        EmitSignal(SignalName.Move, (int)world);
    }
}
