using System.Xml.Serialization;
using Figures.Domain;

namespace Figures.Infrastructure;

public class XmlFileRepository : FileRepositoryBase
{
    public XmlFileRepository() : base("xml")
    { }
    
    protected override Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<Figure> figures)
    {
        var serializer = new XmlSerializer(typeof(List<Figure>));
        serializer.Serialize(stream, figures.ToList());
        
        return Task.CompletedTask;
    }

    protected override Task<IEnumerable<Figure>> GetFromPersistenceStorageAsync(FileStream stream)
    {
        var serializer = new XmlSerializer(typeof(List<Figure>));
        
        var figures = (List<Figure>?)serializer.Deserialize(stream);
        if(figures is null)
            return Task.FromResult<IEnumerable<Figure>>(Array.Empty<Figure>());
        
        return Task.FromResult<IEnumerable<Figure>>(figures);
    }
}