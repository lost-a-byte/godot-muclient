#if TOOLS
using Client.Data.CAP;
using Client.Data.OZB;
using Godot;
using Godot.Collections;
using MuClient;
using MuClient.Extensions;
using MuClient.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class WorldCapImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.camera-angle-position";
    public override string _GetVisibleName() => "Camera Angle Position";
    public override string[] _GetRecognizedExtensions() => ["bmd"];
    public override string _GetResourceType() => "CameraAnglePositionSetting";
    public override string _GetSaveExtension() => "tres";

    internal CAPReader capReader = new();

    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        return [
            new Dictionary
            {
                { "name", "modelScale" },
                { "default_value", 1.0f/100.0f },
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
            float modelScale = (float)options["modelScale"];

            CameraAnglePosition cap = Task.Run(async () => await capReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;

            System.Numerics.Vector3 cameraPointingVector = (cap.CameraPosition - cap.HeroPosition) * modelScale;
            // Swap Z and Y axis
            Vector3 godotSpringArmPosition = new(cameraPointingVector.X, cameraPointingVector.Z, cameraPointingVector.Y * -1);
            if (cap.CameraAngle.Y != 0)
            {
                GD.PushWarning("Angle Y is not equal 0");
            }
            // Flip X, swap Z with Y and add 90 degree
            Vector3 cameraRotationRadius = new Vector3(-90 - cap.CameraAngle.X, 90 + cap.CameraAngle.Z, 0);
            CameraAnglePositionSetting cameraAnglePositionSetting = new()
            {
                Name = sourceFile.GetFile(),
                CameraFieldOfView = cap.CameraFOV,
                CameraRotation = new Vector3((float)cameraRotationRadius.X.ToRadians(), (float)cameraRotationRadius.Y.ToRadians(), (float)cameraRotationRadius.Z.ToRadians()),
                SpringArmPosition = godotSpringArmPosition,
            };

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";
            Error err = ResourceSaver.Save(cameraAnglePositionSetting, saveFilePath);

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
