using System.Runtime.Serialization.Formatters.Binary;
using Figures.Domain;
using Figures.Infrastructure.Utilities;

namespace Figures.Infrastructure;

public class BinaryRepository<T> : IRepository<T> where T : class
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "figures.bin");

    public Task<IEnumerable<T>> GetAllAsync()
    {
        if (!File.Exists(_path))
            return Task.FromResult<IEnumerable<T>>(new List<T>());

        using var stream = File.OpenRead(_path);
        var formatter = new BinaryFormatter
        {
            Binder = new CustomSerializationBinder()
        };

#pragma warning disable SYSLIB0011
        var figures = (IEnumerable<Figure>)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011

        return Task.FromResult(figures.OfType<T>());
    }

    public Task SaveManyAsync(IEnumerable<T> entities)
    {
        var figures = entities.Cast<Figure>().ToList();

        using var stream = File.OpenWrite(_path);
        var formatter = new BinaryFormatter();
        
#pragma warning disable SYSLIB0011
        formatter.Serialize(stream, figures);
#pragma warning restore SYSLIB0011
        
        return Task.CompletedTask;
    }
}