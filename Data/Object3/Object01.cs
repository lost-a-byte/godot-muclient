using Godot;
using MuClient.Scenes.WorldObjects;
using System;

namespace MuClient.Data.Object3;

public partial class Object01 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object01";
    public Object01()
    {
        GD.Print(nameof(Object01));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object01), " Ready!");
        base._Ready();
    }
}
