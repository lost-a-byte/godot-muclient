using Godot;
using MuClient.Scenes.WorldObjects;
using System;


namespace MuClient.Data.Object3;

public partial class Object10 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object10";
    public Object10()
    {
        GD.Print(nameof(Object10));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object10), " Ready!");
        base._Ready();
    }
}
