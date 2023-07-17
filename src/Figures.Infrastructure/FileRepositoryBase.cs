using Figures.Domain;

namespace Figures.Infrastructure;

public abstract class FileRepositoryBase : IRepository<Figure>
{
    private readonly string _filePath;

    protected FileRepositoryBase(string filePath)
    {
        _filePath = filePath;
    }
    
    public async Task<IEnumerable<Figure>> GetAllAsync()
    {
        if(File.Exists(_filePath) == false)
            return Array.Empty<Figure>();

        await using FileStream stream = File.OpenRead(_filePath);
        return await GetFromPersistenceStorageAsync(stream);
    }

    public async Task SaveManyAsync(IEnumerable<Figure> entities)
    {
        await using FileStream stream = File.OpenWrite(_filePath);
        
        await ExecuteBeforeSaveAsync(stream);
        await SaveInPersistenceStorageAsync(stream, entities);
    }
    
    protected virtual Task ExecuteBeforeSaveAsync(FileStream stream)
    {
        ClearFile(stream);
        return Task.CompletedTask;
    }
    
    protected abstract Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<Figure> figures);
    protected abstract Task<IEnumerable<Figure>> GetFromPersistenceStorageAsync(FileStream stream);
    
    private void ClearFile(FileStream stream) => stream.SetLength(0);
}