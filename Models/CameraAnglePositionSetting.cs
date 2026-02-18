using Godot;
using MuClient.Models.Terrain;

namespace MuClient.Models;

[Tool]
public partial class CameraAnglePositionSetting : Resource
{
    [Export]
    public string Name { get; set; } = "";
    [Export]
    public Vector3 CameraRotation { get; set; } = new Vector3();
    [Export]
    public float CameraFieldOfView { get; set; } = 0.0f;
    [Export]
    public Vector3 SpringArmPosition { get; set; } = new Vector3();
}