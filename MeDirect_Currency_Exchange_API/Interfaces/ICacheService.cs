namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface ICacheService {
        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached item if found; otherwise, default value of T.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="item">The item to cache.</param>
        /// <param name="expiration">The expiration time for the cache item.</param>
        void Set<T>(string key, T item, TimeSpan expiration);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void Remove(string key);
    }
}
