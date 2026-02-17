using Godot;
using MuClient.Scenes.WorldObjects;
using System;
namespace MuClient.Data.Object3;


public partial class Object36 : AnimatedWorldObject
{
    public override string ModelName => "Object3/Object36";
    public Object36()
    {
        GD.Print(nameof(Object36));
    }
    public override void _Ready()
    {
        GD.Print(nameof(Object36), " Ready!");
        base._Ready();
    }
}
