namespace Figures.Infrastructure;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task SaveAsync(T entity);
    public Task SaveManyAsync(IEnumerable<T> entities);
}