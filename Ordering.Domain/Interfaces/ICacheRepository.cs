namespace Ordering.Domain.Interfaces
{
    public interface ICacheRepository
    {
        Task<T> GetDataAsync<T>(string cacheKey);
        Task SetDataAsync<T>(string cacheKey, T value);
        Task RemoveAsync(string cacheKey);
    }
}