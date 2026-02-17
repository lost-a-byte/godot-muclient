using Godot;
using MuClient.Scenes.WorldObjects;
using System;

namespace MuClient.Data.Object3;


public partial class Object76 : AnimatedWorldObject
{
    
    public override string ModelName => "Object3/Object76";
    public Object76()
    {
        GD.Print(nameof(Object76));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object76), " Ready!");
        base._Ready();
    }
}
