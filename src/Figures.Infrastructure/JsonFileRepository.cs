using Figures.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Figures.Infrastructure;

public class JsonFileRepository<T> : FileRepositoryBase<T> where T : Figure
{
    public JsonFileRepository(string filePath) : base(filePath)
    { }
    
    protected override async Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<T> figures)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        
        string json = JsonConvert.SerializeObject(figures, settings);
        
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
    }

    protected override async Task<IEnumerable<T>> GetFromPersistenceStorageAsync(FileStream stream)
    {
        using var reader = new StreamReader(stream);
        string json = await reader.ReadToEndAsync();

        return DeserializeFigures(json);
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