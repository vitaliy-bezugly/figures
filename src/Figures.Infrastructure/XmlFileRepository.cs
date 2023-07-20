using System.Xml.Serialization;
using Figures.Domain;

namespace Figures.Infrastructure;

public class XmlFileRepository<T> : FileRepositoryBase<T> where T : Figure
{
    public XmlFileRepository(string filePath) : base(filePath)
    { }
    
    protected override Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<T> figures)
    {
        var serializer = new XmlSerializer(typeof(List<Figure>));
        serializer.Serialize(stream, figures.ToList());
        
        return Task.CompletedTask;
    }

    protected override Task<IEnumerable<T>> GetFromPersistenceStorageAsync(FileStream stream)
    {
        var serializer = new XmlSerializer(typeof(List<Figure>));
        
        var figures = (List<T>?)serializer.Deserialize(stream);
        if(figures is null)
            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());
        
        return Task.FromResult<IEnumerable<T>>(figures);
    }
}