#if TOOLS

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.OZB;
using Godot;

namespace MuClient.addons.MuResourceImporter.Readers;

public class HeightMapReader : BaseReader<float[,]>
{
    OZBReader ozbReader = new();

    private float heightScale = 4.0f / 255.0f;
    public float HeightScale
    {
        get { return heightScale; }
        set { heightScale = value; }
    }

    public override float[,] Read(byte[] buffer)
    {
        throw new NotImplementedException();
    }

    public override float[,] Read(string path)
    {

        float[,] heightMap = new float[Constants.TerrainSize + 1, Constants.TerrainSize + 1];
        var ozbData = Task.Run(async () =>
        {
            try
            {
                OZB result = await ozbReader.Load(path);
                return result;

            }
            catch (FileLoadException)
            {
                return new OZB()
                {
                    Version = 0,
                    Width = Constants.TerrainSize,
                    Height = Constants.TerrainSize,
                    Data = new System.Drawing.Color[Constants.TerrainSize * Constants.TerrainSize],
                };
            }
        }).Result;
        byte[] heightBuffer = [.. ozbData.Data.Select(color => color.R)];

        Image heightMapImage = Image.CreateFromData(
            width: Constants.TerrainSize,
            height: Constants.TerrainSize,
            useMipmaps: false,
            format: Image.Format.L8,
            data: heightBuffer
        );

        // Do Flip Y
        heightMapImage.FlipY();
        for (int z = 0; z < Constants.TerrainSize; z++)
        {
            for (int x = 0; x < Constants.TerrainSize; x++)
            {
                heightMap[x, z] = heightMapImage.GetPixel(x, z).R8 * heightScale;
            }
        }

        return heightMap;
    }
}
#endif
