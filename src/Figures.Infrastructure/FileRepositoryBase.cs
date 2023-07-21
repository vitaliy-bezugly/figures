namespace Figures.Infrastructure;

public abstract class FileRepositoryBase<T> : IRepository<T> where T : class
{
    private readonly FileStream _stream;

    protected FileRepositoryBase(string filePath)
    {
        _stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await GetFromPersistenceStorageAsync(_stream);
    }
    
    public async Task SaveManyAsync(IEnumerable<T> entities)
    {
        await ExecuteBeforeSaveAsync(_stream);
        await SaveInPersistenceStorageAsync(_stream, entities);
    }
    
    public void Dispose()
    {
        _stream.Dispose();
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