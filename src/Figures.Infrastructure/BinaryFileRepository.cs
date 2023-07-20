using System.Runtime.Serialization.Formatters.Binary;
using Figures.Domain;
using Figures.Infrastructure.Utilities;

namespace Figures.Infrastructure;

public class BinaryFileRepository<T> : FileRepositoryBase<T> where T : Figure
{
    public BinaryFileRepository(string filePath) : base(filePath)
    { }

    protected override Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<T> figures)
    {
        var formatter = new BinaryFormatter();
        
        #pragma warning disable SYSLIB0011
        formatter.Serialize(stream, figures);
        #pragma warning restore SYSLIB0011
        
        return Task.CompletedTask;
    }

    protected override Task<IEnumerable<T>> GetFromPersistenceStorageAsync(FileStream stream)
    {
        var formatter = new BinaryFormatter
        {
            Binder = new CustomSerializationBinder()
        };

        #pragma warning disable SYSLIB0011
        var figures = (IEnumerable<Figure>)formatter.Deserialize(stream);
        #pragma warning restore SYSLIB0011

        // ReSharper disable once RedundantEnumerableCastCall
        return Task.FromResult(figures.OfType<T>());
    }
}