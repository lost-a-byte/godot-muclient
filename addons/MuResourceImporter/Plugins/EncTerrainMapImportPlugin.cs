#if TOOLS
using Godot;
using Godot.Collections;
using MuClient.addons.MuResourceImporter.Readers;
using MuClient.Models.Terrain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MuClient.addons.MuResourceImporter.Plugins;

[Tool]
public partial class EncTerrainMapImportPlugin : EditorImportPlugin
{
    public override string _GetImporterName() => "dev.dong.mu.enc-terrain-map";
    public override string _GetVisibleName() => "Terrain Map";
    public override string[] _GetRecognizedExtensions() => ["map"];
    public override string _GetResourceType() => "ArrayMesh";
    public override string _GetSaveExtension() => "res";

    public override int _GetImportOrder()
    {
        return 150;
    }

    internal TerrainTileAttributeReader mapReader = new();
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

            float heightScale = (float)options["heightScale"];

            float[,] heightMap = new float[Constants.TerrainSize + 1, Constants.TerrainSize + 1];
            float[,] tileAlphaMap = new float[Constants.TerrainSize + 1, Constants.TerrainSize + 1];
            byte[,] tileLayer1Map = new byte[Constants.TerrainSize + 1, Constants.TerrainSize + 1];
            byte[,] tileLayer2Map = new byte[Constants.TerrainSize + 1, Constants.TerrainSize + 1];

            HashSet<byte> tileIds = [];
            HashSet<byte> baseTileIds = [];
            HashSet<byte> overlayTileIds = [];

            Godot.Collections.Dictionary<string, Texture2D> textures = new();
            System.Collections.Generic.Dictionary<string, int> textureScale = new();

            WorldType world;
            string sourceFolder = sourceFile.GetBaseDir();

            string absoluteSourceFilePath = ProjectSettings.GlobalizePath(sourceFile);

            string worldDirectory = absoluteSourceFilePath.GetBaseDir();
            string terrainHeightFilePath = Path.Combine(worldDirectory, "TerrainHeight.OZB");
            string terrainLightFilePath = Path.Combine(sourceFolder, "TerrainLight.OZB");

            heightMapReader.HeightScale = heightScale;
            heightMap = heightMapReader.Read(terrainHeightFilePath);

            Image terrainTileImage = mapReader.Read(absoluteSourceFilePath);
            world = mapReader.World;

            string saveFilePath = $"{savePath}.{_GetSaveExtension()}";

            if (!Godot.FileAccess.FileExists(terrainLightFilePath))
            {
                return Error.Bug;
            }
            Texture2D lightMapTexture = ResourceLoader.Load<Texture2D>(terrainLightFilePath);


            for (int imageY = 0; imageY < Constants.TerrainSize; imageY++)
            {
                for (int imageX = 0; imageX < Constants.TerrainSize; imageX++)
                {
                    Color color = terrainTileImage.GetPixel(imageX, imageY);
                    baseTileIds.Add((byte)color.R8);
                    overlayTileIds.Add((byte)color.G8);
                    tileAlphaMap[imageX, imageY] = color.B;
                    tileLayer1Map[imageX, imageY] = (byte)color.R8;
                    tileLayer2Map[imageX, imageY] = (byte)color.G8;
                }
            }

            tileIds.UnionWith(baseTileIds);
            tileIds.UnionWith(overlayTileIds);

            foreach (var item in tileIds)
            {
                if (item == 255)
                {
                    continue;
                }
                string textureKey = GenerateTileTextureName(world, item);
                string textureName = GetTileFileName(textureKey);

                if (string.IsNullOrEmpty(textureName))
                {
                    GD.PushWarning($"Missing texture {textureName}, item {item}");
                    continue;
                }
                string resourcePath = sourceFolder + "/" + textureName;
                if (!Godot.FileAccess.FileExists(resourcePath))
                {
                    GD.PushWarning("Texture " + textureName + " At " + resourcePath + " not found!");
                    continue;
                }
                Texture2D texture = ResourceLoader.Load<Texture2D>(resourcePath);
                if (!textureScale.ContainsKey(textureKey))
                {
                    textureScale.Add(textureKey, texture.GetWidth() / 64);
                }
                if (!textures.ContainsKey(textureKey))
                {
                    textures.Add(textureKey, texture);
                }
            }

            var st = new SurfaceTool();
            st.Begin(Mesh.PrimitiveType.Triangles);

            st.SetCustomFormat(0, SurfaceTool.CustomFormat.RgbaFloat);
            st.SetCustomFormat(1, SurfaceTool.CustomFormat.RgbFloat);

            for (int z = 0; z < Constants.TerrainSize; z++)
            {
                for (int x = 0; x < Constants.TerrainSize; x++)
                {
                    Vector3 p00 = new(x, heightMap[x, z], z);
                    Vector3 p10 = new(x + 1, heightMap[x + 1, z], z);
                    Vector3 p01 = new(x, heightMap[x, z + 1], z + 1);
                    Vector3 p11 = new(x + 1, heightMap[x + 1, z + 1], z + 1);

                    Vector2 uv00 = new((float)x / Constants.TerrainSize, (float)z / Constants.TerrainSize);
                    Vector2 uv10 = new((float)(x + 1) / Constants.TerrainSize, (float)z / Constants.TerrainSize);
                    Vector2 uv01 = new((float)x / Constants.TerrainSize, (float)(z + 1) / Constants.TerrainSize);
                    Vector2 uv11 = new((float)(x + 1) / Constants.TerrainSize, (float)(z + 1) / Constants.TerrainSize);

                    float a1 = tileAlphaMap[x, z]; // Self
                    float a2 = tileAlphaMap[x + 1, z]; // Down
                    float a3 = tileAlphaMap[x + 1, z + 1]; // Down And Right
                    float a4 = tileAlphaMap[x, z + 1]; // Right

                    byte alpha = (byte)(tileAlphaMap[x, z] * 255); // Self

                    byte layer1 = tileLayer1Map[x, z];
                    byte layer2 = tileLayer2Map[x, z];


                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv00); st.AddVertex(p00);

                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv10); st.AddVertex(p10);

                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv11); st.AddVertex(p11);

                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv00); st.AddVertex(p00);

                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv11); st.AddVertex(p11);

                    st.SetCustom(0, new Color(a1, a2, a3, a4));
                    st.SetCustom(1, Color.Color8(layer1, layer2, alpha));
                    st.SetUV(uv01); st.AddVertex(p01);
                }
            }

            st.GenerateNormals();
            st.Index();

            var mesh = st.Commit();

            var material = new ShaderMaterial
            {
                Shader = GenerateShader(
                    GenerateTextureHeaders(world, [.. tileIds.Order()]),
                    GenerateIfStatements(world, [.. tileIds.Order()])
                )
            };
            mesh.SurfaceSetMaterial(0, material);

            // Apply Shader Parameters
            foreach (var textureData in textures)
            {
                material.SetShaderParameter(textureData.Key, textureData.Value);
            }

            // Add Light Map             
            material.SetShaderParameter("Light_Map", lightMapTexture);

            Error err = ResourceSaver.Save(mesh, saveFilePath);
            return err;
        }
        catch (Exception e)
        {
            GD.PushError($"Import failed: {e.Message}");
            return Error.Failed;
        }
    }


    internal static string GenerateTileTextureName(WorldType world, byte tileId)
    {
        string name = tileId switch
        {
            0 => "Tile_Grass_01",
            1 => world switch
            {

                WorldType.NORIA => "Tile_Grass_01", // Noria
                _ => "Tile_Grass_02"
            },
            2 => "Tile_Ground_01",
            3 => world switch
            {

                WorldType.NORIA or WorldType.ATLANS => "Tile_Ground_01", // Noria or Atlans
                _ => "Tile_Ground_02"
            },
            4 => "Tile_Ground_03",
            5 => world switch
            {
                WorldType.ATLANS => "Tile_Grass_01", // Atlans
                _ => "Tile_Water_01"
            },
            6 => "Tile_Wood_01",
            7 => "Tile_Rock_01",
            8 => "Tile_Rock_02",
            9 => "Tile_Rock_03",
            10 => "Tile_Rock_04",
            11 => "Tile_Rock_05",
            12 => "Tile_Rock_06",
            13 => "Tile_Rock_07",
            15 => world switch
            {
                WorldType.TARKAN => "Ext_Tile_01",
                _ => "",
            },
            16 => world switch
            {
                WorldType.TARKAN => "Ext_Tile_02",
                _ => "",
            },
            _ => "",
        };
        return name;
    }
    internal static string GetTileFileName(string tileTextureName)
    {
        return tileTextureName switch
        {
            "Tile_Grass_01" => "TileGrass01.OZJ",
            "Tile_Grass_02" => "TileGrass02.OZJ",
            "Tile_Ground_01" => "TileGround01.OZJ",
            "Tile_Ground_02" => "TileGround02.OZJ",
            "Tile_Ground_03" => "TileGround03.OZJ",
            "Tile_Water_01" => "TileWater01.OZJ",
            "Tile_Wood_01" => "TileWood01.OZJ",
            "Tile_Rock_01" => "TileRock01.OZJ",
            "Tile_Rock_02" => "TileRock02.OZJ",
            "Tile_Rock_03" => "TileRock03.OZJ",
            "Tile_Rock_04" => "TileRock04.OZJ",
            "Tile_Rock_05" => "TileRock05.OZJ",
            "Tile_Rock_06" => "TileRock06.OZJ",
            "Tile_Rock_07" => "TileRock07.OZJ",
            "Ext_Tile_01" => "ExtTile01.OZJ",
            "Ext_Tile_02" => "ExtTile02.OZJ",
            _ => ""
        };
    }

    internal static string GenerateIfStatement(WorldType world, byte tileId)
    {
        string tileTextureName = GenerateTileTextureName(world, tileId);
        if (tileTextureName.Length == 0)
        {
            return "";
        }
        return @"if (tile_id == [tile_id]) return texture([tile_texture_name], uv).rgb;"
            .Replace("[tile_id]", tileId.ToString())
            .Replace("[tile_texture_name]", tileTextureName);
    }


    internal static string GenerateTextureHeader(WorldType world, byte tileId)
    {
        string tileTextureName = GenerateTileTextureName(world, tileId);
        if (tileTextureName.Length == 0)
        {
            return "";
        }
        return $"uniform sampler2D {tileTextureName};";
    }

    internal static string GenerateTextureHeaders(WorldType world, byte[] tileIds)
    {
        HashSet<string> headers = [];
        foreach (var item in tileIds)
        {
            string header = GenerateTextureHeader(world, item);
            if (header.Length == 0)
            {
                continue;
            }
            headers.Add(header);
        }
        return headers.ToArray().Join($"{System.Environment.NewLine}");
    }

    internal static string GenerateIfStatements(WorldType world, byte[] tileIds)
    {
        List<string> statements = [];
        foreach (var item in tileIds)
        {
            string statement = GenerateIfStatement(world, item);
            if (string.IsNullOrEmpty(statement))
            {
                continue;
            }
            statements.Add(statement);
        }
        return statements.ToArray().Join($"{System.Environment.NewLine}\t");
    }
    internal static Shader GenerateShader(
        string textureHeaders,
        string tileColors
    )
    {
        Shader shader = ResourceLoader.Load<Shader>("res://Shaders/TerrainTemplate.gdshader");
        string code = shader.Code
            .Replace("//[texture-headers]", textureHeaders)
            .Replace("//[tile_colors]", tileColors);
        return new Shader
        {
            Code = code,
        };
    }
}
#endif
