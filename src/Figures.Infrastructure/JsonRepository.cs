using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Figures.Infrastructure;

public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "figures.json");
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        if (!File.Exists(_path))
            return new List<T>();

        var json = await File.ReadAllTextAsync(_path);
        var figures = DeserializeFigures(json);
        
        return figures;
    }

    public virtual async Task SaveAsync(T entity)
    {
        var json = SerializeFigures(new []{entity});
        await File.WriteAllTextAsync(_path, json);
    }

    public virtual async Task SaveManyAsync(IEnumerable<T> entities)
    {
        var json = SerializeFigures(entities);
        await File.WriteAllTextAsync(_path, json);
    }
    
    private string SerializeFigures(IEnumerable<T> figures)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        
        return JsonConvert.SerializeObject(figures, settings);
    }

    private IEnumerable<T> DeserializeFigures(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        var jArray = JArray.Parse(json);
        var figures = new List<T>();
        foreach (var jToken in jArray)
        {
            var typeName = jToken.Value<string>("$type");
            if(typeName is null)
                throw new InvalidOperationException("Invalid json format");
            
            var type = Type.GetType(typeName);
            var figure = (T)jToken.ToObject(type, JsonSerializer.Create(settings))!;
            figures.Add(figure);
        }
        return figures;
    }
}