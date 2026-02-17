using Godot;
using MuClient.Scenes.WorldObjects;
using System;

namespace MuClient.Data.Object3;

public partial class Object84 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object84";
    public Object84()
    {
        GD.Print(nameof(Object84));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object84), " Ready!");
        base._Ready();
    }
}
