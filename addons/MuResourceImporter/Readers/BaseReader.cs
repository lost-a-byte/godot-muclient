#if TOOLS
using System.IO;
using System.Threading.Tasks;

namespace MuClient.addons.MuResourceImporter.Readers;

public abstract class BaseReader<T>
{
    public async Task<T> Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}", path);

        var buffer = await File.ReadAllBytesAsync(path);

        return Read(buffer);
    }

    public abstract T Read(string path);
    public abstract T Read(byte[] buffer);
}

#endif
