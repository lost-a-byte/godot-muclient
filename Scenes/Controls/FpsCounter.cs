using Godot;
using System;
namespace MuClient.Scenes.Controls;

[Tool]
public partial class FpsCounter : Control
{
    double FPS = 0.0f;
    // ulong DrawCalls = 0;
    // double FrameTime = 0;
    // float VideoMemory = 0;

    private Label? labelControl;


    private bool enabled = false;
    [Export]
    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            if (!enabled && labelControl != null)
            {
                labelControl.Text = "";
            }
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        labelControl = GetNode<Label>("Label");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!Enabled || labelControl == null)
        {
            return;
        }
        FPS = Engine.GetFramesPerSecond();

        // DrawCalls = RenderingServer.GetRenderingInfo(RenderingServer.RenderingInfo.TotalDrawCallsInFrame);
        // FrameTime = delta;
        // VideoMemory = (float)RenderingServer.GetRenderingInfo(RenderingServer.RenderingInfo.VideoMemUsed) / 1024 / 1024;

        var read_out = $"{(int)FPS:D3} FPS";
        labelControl.Text = read_out;
    }
}
