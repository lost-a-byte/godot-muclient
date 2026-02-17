using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object104 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object104";
    public Object104()
    {
        GD.Print(nameof(Object104));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object104), " Ready!");
        base._Ready();
    }
}
