#if TOOLS

using System;
using System.Linq;
using Godot;
using Godot.Collections;
using MuClient.Models.Terrain;

namespace MuClient.addons.MuResourceImporter.InspectorPlugins;

[Tool]
public partial class TileFlagInspectorPlugin : EditorProperty
{
    Sprite2D imageTexture;

    Control nodeControl;
    TileFlag currentPreviewFlag = TileFlag.SafeZone;

    OptionButton flagSelectorControl;

    public TileFlag CurrentPreviewFlag
    {
        get => currentPreviewFlag;
        set
        {
            if (currentPreviewFlag == value) return;
            currentPreviewFlag = value;
        }
    }

    internal Dictionary<long, TileFlag> TileFlagMap = new();

    public override void _Ready()
    {
        flagSelectorControl = new()
        {
            Visible = true,
        };
        int index = 0;
        foreach (var flag in Enum.GetValues<TileFlag>())
        {
            TileFlagMap.Add(index, flag);

            flagSelectorControl.AddItem(flag.ToString(), index);
            if (flag == CurrentPreviewFlag)
            {
                flagSelectorControl.Selected = index;
            }
            index++;
        }

        flagSelectorControl.ItemSelected += OnItemChanged;

        nodeControl = new()
        {
            CustomMinimumSize = new() { X = 256, Y = 256 },
            ClipContents = false,
        };

        AddChild(flagSelectorControl);


        imageTexture = new()
        {
            Centered = false,
            Position = new Vector2(0, 2),
        };

        nodeControl.AddChild(imageTexture);

        AddChild(nodeControl, true, InternalMode.Disabled);
        SetBottomEditor(nodeControl);
        ClipContents = true;
        DrawBackground = false;
        Label = "Preview";
        base._Ready();
    }

    private void OnItemChanged(long index)
    {

        TileFlag tileFlag = TileFlagMap[index];
        CurrentPreviewFlag = tileFlag;
        UpdateTexture();
    }

    public override void _UpdateProperty()
    {
        UpdateTexture();
    }

    internal void UpdateTexture()
    {
        Array<TileFlag> tileFlags = GetEditedObject().Get(GetEditedProperty()).As<Array<TileFlag>>();
        byte[] imageBuffer = [.. tileFlags.Select(tile => tile.HasFlag(currentPreviewFlag) ? (byte)0 : (byte)255)];
        Image image = Image.CreateFromData(Constants.TerrainSize, Constants.TerrainSize, false, Image.Format.L8, imageBuffer);
        ImageTexture someTexture = ImageTexture.CreateFromImage(image);
        imageTexture.Texture = someTexture;
    }

    public override void _ExitTree()
    {
        // flagSelectorControl?.ItemSelected -= OnItemChanged;
        base._ExitTree();
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
#endif
