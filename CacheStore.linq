<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Caching.Memory</NuGetReference>
  <Namespace>Microsoft.Extensions.Caching.Memory</Namespace>
</Query>

void Main()
{
	
}

//Store: this is where the interaction with the cache is perform.
public class Store
{
	private readonly IMemoryCache cache;
	private readonly CacheConfig props;

	public Store(IMemoryCache cache, CacheConfig props)
	{
		this.cache = cache;
		this.props = props;
	}

	public T GetOrSetCache<T>(string key, Func<T> fn)
	{
		var (res, cacheHit) = GetCache(key, fn);

		if (!cacheHit)
			SetCache(key, res);

		return res;
	}

	public void SetCache<T>(string key, T res)
	{
		if (props.CacheEnabled)
		{
			string realKey = Key.Create<T>(key);

			var options = new MemoryCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(props.CacheTimespan))
				.SetSize(props.CacheMaxSize);

			cache.Set(realKey, res, options);
		}
	}

	public (T, bool) GetCache<T>(string key, Func<T> fn)
	{
		string realKey = Key.Create<T>(key);

		if (props.CacheEnabled && cache.TryGetValue(realKey, out T cachedRes))
		{
			return (cachedRes, true);
		}

		return (fn(), false);
	}
}

//This creates a unique key for the cache generator using very simple concatenations and the datatype.
public class Key
{
	public static string FieldSeparator = ":";

	public static string Create<T>(string field) => Create(typeof(T), field);

	public static string Create(Type type, string field)
	{
		if (type == null)
		{
			throw new ArgumentNullException($"Argument {nameof(type)} cannot be null");
		}

		if (field == null)
		{
			throw new ArgumentNullException($"Argument {nameof(field)} cannot be null");
		}

		return $"{type.Name}{FieldSeparator}{field}";
	}
}

//Simple configurations needed for the Cache
public class CacheConfig
{
	public bool CacheEnabled { get; set; } //Enable or Disable
	public int CacheTimespan { get; set; } //Timespan of persistence
	public int CacheMaxSize { get; set; } //Mas size in bytes of the object to persist
}