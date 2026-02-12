#if TOOLS
using Client.Data.OZB;
using Godot;
using Godot.Collections;
using MuClient.addons.MuResourceImporter.Readers;
using System;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class TerrainHeightImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.terrain-height";
    public override string _GetVisibleName() => "Terrain Height";
    public override string[] _GetRecognizedExtensions() => ["ozb"];
    public override string _GetResourceType() => "HeightMapShape3D";
    public override string _GetSaveExtension() => "tres";

    internal OZBReader ozbReader = new();

    internal HeightMapReader heightMapReader = new();

    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        return [
            new Dictionary
            {
                { "name", "heightScale" },
                { "default_value", 4.0f/255.0f },
            },
        ];
    }

    public override int _GetPresetCount() => 0;

    public override Error _Import(string sourceFile, string savePath,
        Dictionary options, Array<string> platformVariants,
        Array<string> genFiles)
    {
        try
        {
            var shape = new HeightMapShape3D
            {
                MapWidth = Constants.TerrainSize + 1,
                MapDepth = Constants.TerrainSize + 1
            };
            string filePath = ProjectSettings.GlobalizePath(sourceFile);
            heightMapReader.HeightScale = (float)options["heightScale"];

            float[,] heightMap = heightMapReader.Read(filePath);
            float[] mapData = new float[shape.MapWidth * shape.MapDepth];

            for (int z = 0; z < shape.MapDepth; z++)
            {
                for (int x = 0; x < shape.MapWidth; x++)
                {
                    mapData[z * shape.MapDepth + x] = heightMap[x, z];
                }
            }

            shape.MapData = mapData;

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";
            Error err = ResourceSaver.Save(shape, saveFilePath);

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
