#if TOOLS
using Client.Data.OZB;
using Client.Data.Texture;
using Godot;
using Godot.Collections;
using MuClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class TextureImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.texture";
    public override string _GetVisibleName() => "Image Texture";
    public override string[] _GetRecognizedExtensions() => ["ozj", "ozt", "ozp"];
    public override string _GetResourceType() => "ImageTexture";
    public override string _GetSaveExtension() => "res";

    internal OZJReader ozjReader = new();
    internal OZTReader oztReader = new();
    internal OZPReader ozpReader = new();

    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        return [
        ];
    }

    public override int _GetPresetCount() => 0;

    public override Error _Import(string sourceFile, string savePath,
        Dictionary options, Array<string> platformVariants,
        Array<string> genFiles)
    {
        try
        {
            string extension = sourceFile[(sourceFile.Length - 3)..].ToLower();

            TextureData textureData = extension switch
            {
                "ozt" => Task.Run(async () => await oztReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result,
                "ozp" => Task.Run(async () => await ozpReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result,
                "ozj" => Task.Run(async () => await ozjReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result,
                _ => null
            };
            if (textureData == null)
            {
                return Error.Failed;
            }

            Image image = Image.CreateFromData(
                textureData.Width,
                textureData.Height,
                false,
                textureData.Components == 4 ? Image.Format.Rgba8 : Image.Format.Rgb8,
                textureData.Data
            );

            ImageTexture texture = new();
            texture.SetImage(image);

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";
            Error err = ResourceSaver.Save(texture, saveFilePath);

            return err;
        }
        catch (Exception e)
        {
            GD.PushError($"Import failed: {e.Message}");
            return Error.Failed;
        }
    }

}
#endif
