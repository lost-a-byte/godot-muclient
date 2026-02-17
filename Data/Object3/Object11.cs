using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object11 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object11";
    public Object11()
    {
        GD.Print(nameof(Object11));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object11), " Ready!");
        base._Ready();
    }
}
