#if TOOLS

using System.Threading.Tasks;
using Client.Data.MAP;
using Godot;
using MuClient;
using MuClient.addons.MuResourceImporter.Readers;
using MuClient.Models.Terrain;

public partial class TerrainTileAttributeReader : BaseReader<Image>
{
    internal MapReader mapReader = new();
    public WorldType World = WorldType.LORENCIA;
    public override Image Read(string path)
    {
        var terrainTileMapData = Task.Run(async () => await mapReader.Load(path)).Result;

        World = (WorldType)terrainTileMapData.MapNumber;

        byte[] terrainTileImageBuffer = new byte[Constants.TerrainSize * Constants.TerrainSize * 3];

        for (int i = 0; i < Constants.TerrainSize * Constants.TerrainSize; i++)
        {
            terrainTileImageBuffer[i * 3] = terrainTileMapData.Layer1[i];
            terrainTileImageBuffer[i * 3 + 1] = terrainTileMapData.Layer2[i];
            terrainTileImageBuffer[i * 3 + 2] = terrainTileMapData.Alpha[i];
        }

        Image terrainTileImage = Image.CreateFromData(
            width: Constants.TerrainSize,
            height: Constants.TerrainSize,
            useMipmaps: false,
            format: Image.Format.Rgb8,
            data: terrainTileImageBuffer
        );

        // Do Flip Y
        terrainTileImage.FlipY();

        return terrainTileImage;
    }

    public override Image Read(byte[] buffer)
    {
        throw new System.NotImplementedException();
    }
}
#endif
