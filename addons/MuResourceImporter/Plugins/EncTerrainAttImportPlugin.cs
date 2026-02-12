#if TOOLS

using Client.Data.ATT;
using Client.Data.OBJS;
using Godot;
using Godot.Collections;
using MuClient.Models.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class EncTerrainAttImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.enc-terrain-att";
    public override string _GetVisibleName() => "Terrain Tile Attribute";
    public override string[] _GetRecognizedExtensions() => ["att"];
    public override string _GetResourceType() => "TileAttribute";
    public override string _GetSaveExtension() => "tres";

    internal ATTReader objReader = new();
    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        return new Array<Dictionary>();
    }

    public override int _GetPresetCount() => 0;

    public override Error _Import(string sourceFile, string savePath,
        Dictionary options, Array<string> platformVariants,
        Array<string> genFiles)
    {
        try
        {

            var attData = Task.Run(async () => await objReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";

            TileAttribute tileAttribute = new()
            {
                World = (WorldType)attData.Index,
                Version = attData.Version,
                Width = attData.Width,
                Height = attData.Height,
                TileFlags = [.. attData.TerrainWall.Select(t => (TileFlag)t)]
            };

            Error err = ResourceSaver.Save(tileAttribute, saveFilePath);
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
