namespace Figures.Infrastructure;

public abstract class FileRepositoryBase<T> : IRepository<T> where T : class
{
    private readonly string _filePath;

    protected FileRepositoryBase(string filePath)
    {
        _filePath = filePath;
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        if(File.Exists(_filePath) == false)
            return Array.Empty<T>();

        await using FileStream stream = File.OpenRead(_filePath);
        return await GetFromPersistenceStorageAsync(stream);
    }
    
    public async Task SaveManyAsync(IEnumerable<T> entities)
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
    
    protected abstract Task SaveInPersistenceStorageAsync(FileStream stream, IEnumerable<T> figures);
    protected abstract Task<IEnumerable<T>> GetFromPersistenceStorageAsync(FileStream stream);
    
    private void ClearFile(FileStream stream) => stream.SetLength(0);
}