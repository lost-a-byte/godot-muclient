using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object105 : AnimatedWorldObject
{
    public Object105()
    {
        GD.Print(nameof(Object105));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object105), " Ready!");
        base._Ready();
    }
    public override string ModelName => "Object3/Object105";
}
