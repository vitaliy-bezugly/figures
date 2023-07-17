using Figures.Domain;

namespace Figures.Infrastructure;

public abstract class FileRepositoryBase : IRepository<Figure>
{
    private readonly string _filePath;

    protected FileRepositoryBase(string fileResolution)
    {
        _filePath = Path.Combine(Environment.CurrentDirectory, $"data/figures.{fileResolution}");
    }

    public async Task<IEnumerable<Figure>> GetAllAsync()
    {
        if(File.Exists(_filePath) == false)
            return Array.Empty<Figure>();

        await using var stream = File.OpenRead(_filePath);
        return await GetFromPersistenceStorageAsync(stream);
    }

    public async Task SaveManyAsync(IEnumerable<Figure> entities)
    {
        await using var stream = File.OpenWrite(_filePath);
        await SaveInPersistenceStorageAsync(stream, entities);
    }
    
    protected abstract Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<Figure> figures);
    protected abstract Task<IEnumerable<Figure>> GetFromPersistenceStorageAsync(FileStream stream);
}