using Godot;
using MuClient.Scenes.WorldObjects;
using System;

namespace MuClient.Data.Object3;

public partial class Object20 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object20";
    public Object20()
    {
        GD.Print(nameof(Object20));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object20), " Ready!");
        base._Ready();
    }
}
