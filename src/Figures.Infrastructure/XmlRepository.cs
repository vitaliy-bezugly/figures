using System.Xml.Serialization;

namespace Figures.Infrastructure;

public class XmlRepository<T> : IRepository<T> where T : class
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "figures.xml");

    public Task<IEnumerable<T>> GetAllAsync()
    {
        if (!File.Exists(_path))
            return Task.FromResult<IEnumerable<T>>(new List<T>());

        var serializer = new XmlSerializer(typeof(List<T>));
        using var stream = File.OpenRead(_path);
        
        var figures = (List<T>)serializer.Deserialize(stream)!;
        if(figures is null)
            throw new InvalidOperationException("Invalid xml format");
        
        return Task.FromResult<IEnumerable<T>>(figures);
    }

    public Task SaveManyAsync(IEnumerable<T> entities)
    {
        var serializer = new XmlSerializer(typeof(List<T>));

        using var fileStream = new FileStream(_path, FileMode.Create);
        serializer.Serialize(fileStream, entities);

        return Task.CompletedTask;
    }
}