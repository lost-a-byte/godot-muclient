using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object86 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object86";
    public Object86()
    {
        GD.Print(nameof(Object86));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object86), " Ready!");
        base._Ready();
    }
}
