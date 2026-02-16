
using System;
using Godot;
using Godot.Collections;

namespace MuClient.addons.MuResourceImporter.Types;

[Tool]
[GlobalClass]
public partial class BmdAnimationOverride : Resource
{
    [Export]
    public Dictionary<int, BmdAnimation> Override = [];
}