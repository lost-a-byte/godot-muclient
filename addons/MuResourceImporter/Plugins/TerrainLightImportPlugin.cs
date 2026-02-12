#if TOOLS
using Client.Data.OZB;
using Godot;
using Godot.Collections;
using MuClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class TerrainLightImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.terrain-light";
    public override string _GetVisibleName() => "Terrain Light";
    public override string[] _GetRecognizedExtensions() => ["ozb"];
    public override string _GetResourceType() => "ImageTexture";
    public override string _GetSaveExtension() => "res";

    internal OZBReader ozbReader = new();

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


            OZB ozb = Task.Run(async () => await ozbReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;

            byte[] data = [.. ozb.Data.SelectMany(x => new byte[] { x.R, x.G, x.B, x.A })];
            Image image = Image.CreateFromData(Constants.TerrainSize, Constants.TerrainSize, false, Image.Format.Rgba8, data);
            image.FlipY();
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
