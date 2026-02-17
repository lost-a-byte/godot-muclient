using Godot;
using MuClient.Models.Terrain;
using System;

namespace MuClient.Scenes.Controls;

[Tool]
public partial class NaviMap : Control
{
    private Vector2I characterPositionXZ = new();
    [Export]
    public Vector2I CharacterPostionXZ
    {
        get => characterPositionXZ;
        set
        {
            if (characterPositionXZ == value) return;
            characterPositionXZ = value;
            UpdateCharacterPositionData();
        }
    }

    private WorldType world;
    public WorldType World
    {
        get { return world; }
        set
        {
            if (world == value) return;
            world = value;
            UpdateWorldTexture();
        }
    }


    ColorRect? CharacterCursor;
    Label? MapLabel;
    TextureRect? MinimapTexture;
    ColorRect? MinimapBg;

    public override void _Ready()
    {
        CharacterCursor = GetNode<ColorRect>("MinimapTex/CharacterCursorRec");
        MapLabel = GetNode<Label>("MapLbl");
        MinimapTexture = GetNode<TextureRect>("MinimapTex");
        MinimapBg = GetNode<ColorRect>("BackgroundRec");

        base._Ready();
    }


    void UpdateCharacterPositionData()
    {
        MapLabel?.Text = $"{world} {characterPositionXZ}";
        CharacterCursor?.Position = CharacterPostionXZ;
    }



    void UpdateWorldTexture()
    {
        if (MinimapTexture == null) return;

        string texturePath = $"res://Data/Interface/GFx/NaviMap/Navimap{(int)world:D2}.OZP";
        if (!ResourceLoader.Exists(texturePath))
        {
            MinimapBg?.Visible = false;
            CharacterCursor?.Visible = false;
            MinimapTexture.Texture = null;
            return;
        }

        MinimapBg?.Visible = true;
        CharacterCursor?.Visible = true;
        MinimapTexture.Texture = ResourceLoader.Load<ImageTexture>(texturePath);
    }
}
