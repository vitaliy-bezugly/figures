namespace Figures.Infrastructure;

public interface IRepository<T> : IDisposable where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task SaveManyAsync(IEnumerable<T> entities);
}