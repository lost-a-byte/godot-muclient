using System;
using Godot;

namespace MuClient.Scenes.WorldObjects;

[Tool]
public partial class AnimatedWorldObject : Node3D
{
    public AnimationPlayer? animationPlayer;

    public Animation? idleAnimation;

    public virtual string AnimationSelectorName => "AnimationPlayer";
    public virtual string ActionName => "Animations/Action_000";
    public virtual float ActionSpeed => 0.25f;
    public virtual Animation.LoopModeEnum ActionLoopMode => Animation.LoopModeEnum.Linear;
    public virtual string ModelName => "";

    public virtual Aabb ObjectSize => new Aabb(1, 1, 1, 1, 1, 1);
    public VisibleOnScreenNotifier3D? VisibleOnScreenNotifier;
    public override void _Ready()
    {
        SetupOnScreenNotifier();
        SetupAnimationPlayer();
        base._Ready();
        VisibleOnScreenNotifier?.ScreenEntered += OnScreenEntered;
        VisibleOnScreenNotifier?.ScreenExited += OnScreenExited;
        if (VisibleOnScreenNotifier?.IsOnScreen() ?? false)
        {
            PlayAnimation();
        }
    }

    public virtual void SetupOnScreenNotifier()
    {
        VisibleOnScreenNotifier = new()
        {
            Aabb = ObjectSize,
        };
        AddChild(VisibleOnScreenNotifier);
    }

    public virtual void SetupAnimationPlayer()
    {
        animationPlayer = GetNode<AnimationPlayer>(AnimationSelectorName);
        animationPlayer.SpeedScale = ActionSpeed;
        idleAnimation = animationPlayer.GetAnimation(ActionName);
        idleAnimation.LoopMode = ActionLoopMode;
    }

    public virtual void OnScreenEntered()
    {
        PlayAnimation();
    }

    public virtual void OnScreenExited()
    {
        PauseAnimation();
    }



    public virtual void PlayAnimation()
    {
        animationPlayer?.Play(ActionName);
    }
    public virtual void PauseAnimation()
    {
        animationPlayer?.Pause();
    }

    public override void _ExitTree()
    {
        VisibleOnScreenNotifier?.ScreenEntered -= OnScreenEntered;
        VisibleOnScreenNotifier?.ScreenExited -= OnScreenExited;
        base._ExitTree();
    }
}
