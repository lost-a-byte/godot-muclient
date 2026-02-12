#if TOOLS

using Client.Data.OBJS;
using Godot;
using Godot.Collections;
using MuClient.Extensions;
using MuClient.Models.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class EncTerrainObjImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.enc-terrain-obj";
    public override string _GetVisibleName() => "Enc Terrain Objects";
    public override string[] _GetRecognizedExtensions() => ["obj"];
    public override string _GetResourceType() => "ObjectMap";
    public override string _GetSaveExtension() => "tres";

    public override float _GetPriority()
    {
        return base._GetPriority() + 5;
    }
    internal OBJReader objReader = new();
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

            OBJ objData = Task.Run(async () => await objReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";

            Array<ObjectAttribute> objects = [];
            HashSet<short> objectTypes = [];

            foreach (IMapObject item in objData.Objects)
            {

                float angleXRadian = (float)item.Angle.X.ToRadians();
                float angleYRadian = (float)item.Angle.Y.ToRadians();
                float angleZRadian = (float)item.Angle.Z.ToRadians();

                Quaternion rotation = Quaternion.FromEuler(
                    new Vector3(angleYRadian, angleZRadian, angleXRadian)
                );

                Vector3 position = new Vector3(
                    item.Position.X * 0.01f,
                    item.Position.Z * 0.01f,
                    -item.Position.Y * 0.01f
                ) - new Vector3(
                    Constants.TerrainSize * 0.5f,
                    0,
                    -Constants.TerrainSize * 0.5f
                );
                ObjectAttribute objectAttribute = new()
                {
                    Type = item.Type,
                    Position = position,
                    Rotation = rotation,
                    Scale = item.Scale,
                };
                if (item is MapObjectV1 mapObjectV1)
                {
                    objectAttribute.UnknownX = mapObjectV1.UnknownX;
                    objectAttribute.UnknownY = mapObjectV1.UnknownY;
                }
                else if (item is MapObjectV2 mapObjectV2)
                {
                    objectAttribute.UnknownX = mapObjectV2.UnknownX;
                    objectAttribute.UnknownY = mapObjectV2.UnknownY;
                    objectAttribute.UnknownZ = mapObjectV2.UnknownZ;
                }
                else if (item is MapObjectV3 mapObjectV3)
                {
                    objectAttribute.UnknownX = mapObjectV3.UnknownX;
                    objectAttribute.UnknownY = mapObjectV3.UnknownY;
                    objectAttribute.UnknownZ = mapObjectV3.UnknownZ;
                    objectAttribute.Lighting = new Vector3(mapObjectV3.Ligthning.X, mapObjectV3.Ligthning.Y, mapObjectV3.Ligthning.Z);
                }
                else if (item is MapObjectV4 mapObjectV4)
                {
                    objectAttribute.UnknownX = mapObjectV4.UnknownX;
                    objectAttribute.UnknownY = mapObjectV4.UnknownY;
                    objectAttribute.UnknownZ = mapObjectV4.UnknownZ;
                    objectAttribute.Lighting = new Vector3(mapObjectV4.Ligthning.X, mapObjectV4.Ligthning.Y, mapObjectV4.Ligthning.Z);
                }
                else if (item is MapObjectV5 mapObjectV5)
                {
                    objectAttribute.UnknownX = mapObjectV5.UnknownX;
                    objectAttribute.UnknownY = mapObjectV5.UnknownY;
                    objectAttribute.UnknownZ = mapObjectV5.UnknownZ;
                    objectAttribute.Lighting = new Vector3(mapObjectV5.Ligthning.X, mapObjectV5.Ligthning.Y, mapObjectV5.Ligthning.Z);
                }
                // obj.Basis = new Basis(finalRotation) * obj.Basis;
                objects.Add(objectAttribute);
                objectTypes.Add(item.Type);
            }

            ObjectMap objectMap = new()
            {
                Version = objData.Version,
                World = (WorldType)objData.MapNumber,
                Objects = objects,
                ObjectTypes = [.. objectTypes.Order()],
            };

            Error err = ResourceSaver.Save(objectMap, saveFilePath);
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
