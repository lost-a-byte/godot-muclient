using Godot;
using Godot.Collections;
using MuClient.Extensions;
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
    public delegate void MoveGateEventHandler(GateItem gate);

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

        MprData mprData = ResourceLoader.Load<MprData>("res://Data/Lang.mpr");
        Array<GateItem> gateItems = mprData.GetGateItems();

        foreach (GateItem gate in gateItems)
        {
            if (gate.Type == 2 || gate.Type == 1)
            {
                continue;
            }
            Button button = new()
            {
                Text = $"{gate.World} {gate.PositionStart}",
            };
            button.Pressed += () => GatePressed(gate);
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

    private void GatePressed(GateItem gateItem)
    {
        GetViewport().SetInputAsHandled();
        EmitSignal(SignalName.MoveGate, gateItem);
    }

    private void ButtonPressed(WorldType world)
    {
        GD.Print($"World {world} clicked!");
        GetViewport().SetInputAsHandled();
        EmitSignal(SignalName.Move, (int)world);
    }
}
