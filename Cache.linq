public class Cache<TKey, TValue> where TKey : struct
{
	private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new();

	public void Store(TKey key, TValue value, TimeSpan expiresAfter)
	{
		_cache[key] = new CacheItem<TValue>(value, expiresAfter);
	}
	
	public void Store(TKey key, TValue value, int expiresAfterInSeconds)
	{
		_cache[key] = new CacheItem<TValue>(value, TimeSpan.FromSeconds(expiresAfterInSeconds));
	}

	public TValue Get(TKey key)
	{
		if (!_cache.ContainsKey(key)) return default(TValue)!;
		var cached = _cache[key];
		if (DateTimeOffset.UtcNow.Subtract(cached.Created) >= cached.ExpiresAfter)
		{
			_cache.Remove(key);
			return default(TValue)!;
		}
		return cached.Value;
	}
}

public class CacheItem<T>
{
	public CacheItem(T value, TimeSpan expiresAfter)
	{
		Value = value;
		ExpiresAfter = expiresAfter;
	}
	public T Value { get; }
	internal DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;
	internal TimeSpan ExpiresAfter { get; }
}