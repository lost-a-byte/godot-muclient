#if TOOLS
using System.Collections.Generic;
using System.Text.Json;
using Godot;

namespace MuClient.addons.MuResourceImporter.Databases;

public class DefaultTextureDatabaseSingleton
{
    private static DefaultTextureDatabaseSingleton _instance;

    Dictionary<string, string> textureMap = [];
    public virtual string DatabasePath => "addons/MuResourceImporter/Databases/DefaultTextureDatabase.json";
    private DefaultTextureDatabaseSingleton()
    {
        LoadDatabase();
    }

    void LoadDatabase()
    {
        // Read file as string;
        string content = FileAccess.GetFileAsString(DatabasePath);
        // parese dictionary;
        Dictionary<string, string> dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
        // assign
        textureMap = dictionary;
    }

    public static DefaultTextureDatabaseSingleton Instance
    {
        get
        {
            _instance ??= new DefaultTextureDatabaseSingleton();
            return _instance;
        }
    }

    public string FindTextureFile(string bmdDirectory, string textureName)
    {
        string resMask = "res:/";
        string resourceDirectoryPath = bmdDirectory[^(bmdDirectory.Length - resMask.Length)..]; ; // Remove res:/
        string texturePathKey = System.IO.Path.Combine(resourceDirectoryPath, textureName).ToLower();
        if (textureMap.TryGetValue(texturePathKey, out string resolvedTexturePath))
        {
            string textureFilePath = resMask + resolvedTexturePath;
            if (!ResourceLoader.Exists(textureFilePath))
            {
                return "";
            }
            return textureFilePath;
        }
        return "";
    }
}
#endif
