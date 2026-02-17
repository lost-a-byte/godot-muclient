using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object57 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object57";
    public Object57()
    {
        GD.Print(nameof(Object57));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object57), " Ready!");
        base._Ready();
    }
}
