
#if TOOLS

using Godot;
using MuClient.Models.Terrain;

namespace MuClient.addons.MuResourceImporter.InspectorPlugins;

[Tool]
public partial class EncTerrainAttInspectorPlugin : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return @object is TileAttribute;
    }
    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if (name == "TileFlags")
        {
            var editor = new TileFlagInspectorPlugin();
            AddPropertyEditor("TileFlags", editor);
        }
        return base._ParseProperty(@object, type, name, hintType, hintString, usageFlags, wide);
    }
}
#endif
