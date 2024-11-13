namespace Lab3.Services
{
    public interface ICachedService<T>
    {
        public IEnumerable<T> GetAll();

        public void AddIntoCache(string cacheKey, IEnumerable<T> values);

        public bool TryGetFromCache(string cacheKey, out IEnumerable<T> values);

        public IEnumerable<T> GetByCount(int countRows = 20);
    }
}
