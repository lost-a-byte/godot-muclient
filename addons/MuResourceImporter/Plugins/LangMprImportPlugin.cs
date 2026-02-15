#if TOOLS

using Client.Data.ATT;
using Client.Data.LANG;
using Client.Data.OBJS;
using Godot;
using Godot.Collections;
using ICSharpCode.SharpZipLib.Zip;
using MuClient.Models.Terrain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class LangMprImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.lang-mpr";
    public override string _GetVisibleName() => "Lang.mpr Import plugin";
    public override string[] _GetRecognizedExtensions() => ["mpr"];
    public override string _GetResourceType() => "MprData";
    public override string _GetSaveExtension() => "tres";

    internal LangMPRReader mprReader = new();
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

            var mprData = Task.Run(async () => await mprReader.Load(ProjectSettings.GlobalizePath(sourceFile))).Result;

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";

            Godot.Collections.Dictionary<string, byte[]> dictionary = [];
            foreach (ZipEntry entry in mprData)
            {
                if (entry.IsFile)
                {
                    using MemoryStream memoryStream = new();
                    mprData.GetInputStream(entry).CopyTo(memoryStream);
                    byte[] content = memoryStream.ToArray();
                    dictionary[entry.Name] = content;
                }
            }

            var data = new MprData()
            {
                Data = dictionary,
            };
            Error err = ResourceSaver.Save(data, saveFilePath);
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
