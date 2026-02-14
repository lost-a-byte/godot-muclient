using Godot;
using MuClient.Models.Terrain;

using System;

namespace MuClient.Scenes.Controls;

public partial class MoveCommand : Control
{
	[Export] public WorldType World { get; set; } = WorldType.LORENCIA;

	[Signal]
	public delegate void MoveEventHandler(int world);

	private VBoxContainer container;

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

		foreach (WorldType world in Enum.GetValues<WorldType>())
		{
			Button button = new()
			{
				Text = world.ToString()
			};
			button.Pressed += () => ButtonPressed(world);
			container.AddChild(button);
		}
	}

	void ClearWorldList()
	{
		foreach (var item in container.GetChildren())
		{
			item.QueueFree();
		}
	}

	private void ButtonPressed(WorldType world)
	{
		GD.Print($"World {world} clicked!");
		EmitSignal(SignalName.Move, (int)world);
		GetViewport().SetInputAsHandled();
	}
}
