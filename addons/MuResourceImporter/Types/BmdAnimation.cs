#if TOOLS

using Godot;
namespace MuClient.addons.MuResourceImporter.Types;

[Tool]
[GlobalClass]
public partial class BmdAnimation : Resource
{
    [Export]
    public Animation.LoopModeEnum LoopMode { get; set; } = Animation.LoopModeEnum.None;
    [Export]
    public bool FixLinear { get; set; } = false;
    [Export]
    public bool FirstKeyIsRestPose { get; set; } = false;
    [Export]
    public bool SkipLastKey { get; set; } = false;

    public bool WillApplyLinearFix => FixLinear && LoopMode == Animation.LoopModeEnum.Linear;
}
#endif