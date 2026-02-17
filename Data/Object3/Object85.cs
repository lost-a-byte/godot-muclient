using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object85 : AnimatedWorldObject
{
    public Object85()
    {
        GD.Print(nameof(Object85));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object85), " Ready!");
        base._Ready();
    }
}
