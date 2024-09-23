using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Tests.CustomMocks;

public class MemoryCacheMock : IMemoryCache
{
    private readonly Dictionary<object, object> _cache = [];

    public bool TryGetValue(object key, out object? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public ICacheEntry CreateEntry(object key)
    {
        var entry = new CacheEntryMock(key);
        _cache[key] = entry.Value;
        return entry;
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }

    public void Dispose() => throw new NotImplementedException();

    private class CacheEntryMock(object key) : ICacheEntry
    {
        public object Key { get; } = key;
        public object Value { get; set; } = new object();
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public IList<IChangeToken> ExpirationTokens { get; } = [];
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = [];
        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }

        public void Dispose() { }
    }
}
