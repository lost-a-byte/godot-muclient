using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object55 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object55";
    public Object55()
    {
        GD.Print(nameof(Object55));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object55), " Ready!");
        base._Ready();
    }
}
