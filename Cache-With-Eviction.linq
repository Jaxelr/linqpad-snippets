<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Caching.Abstractions</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Caching.Memory</NuGetReference>
  <Namespace>Microsoft.Extensions.Internal</Namespace>
  <Namespace>Microsoft.Extensions.Caching.Memory</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//POC with handle eviction
	var testClock = new TestSystemClock();
	var memoryCache = new MemoryCache(new MemoryCacheOptions
	{
		Clock = testClock
	});
	var item = new Item() { Id = 1, Value = "Test" };
	
	var cacheTest = new CacheTest(memoryCache);
	
	var response = cacheTest.GetNextValue(item, "oldKey");
	response.Dump();
	
	Task.Delay(2000);

	item = new Item() { Id = 2, Value = "Test-2" };
	response = cacheTest.GetNextValue(item, "oldKey");
	response.Dump();

	item = new Item() { Id = 3, Value = "Test-3" };
	response = cacheTest.GetNextValue(item, "oldKey");
	response.Dump();
}

public class CacheTest
{
	private readonly MemoryCache memoryCache;
	
	public CacheTest(MemoryCache memoryCache)
	{
		this.memoryCache = memoryCache;
	}
	
	public async Task<Item> GetNextValue(Item item, string key)
	{
		var temp = await memoryCache.GetOrCreateAsync(key, async newCacheEntry =>
		{
			newCacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2);
			newCacheEntry.RegisterPostEvictionCallback(HandleCacheEviction);
			
			await Task.Delay(1000);
			
			return item;
		});
		
		return temp;
	}
	
	internal virtual void HandleCacheEviction(object key, object? value, EvictionReason reason, object? state)
	{
		if (reason == EvictionReason.Expired && value is Item existing)
		{
			try
			{
				var newOptions = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1)
				};
				newOptions.RegisterPostEvictionCallback(HandleCacheEviction);

				memoryCache.Set("OldKey", existing, newOptions);
			}
			catch (Exception ex)
			{
				// Existing value remains in cache, will retry on next expiration
				ex.Dump();
			}
		}
	}
}



// You can define other methods, fields, classes and namespaces here
public class Item
{ 
	public int Id { get; set; }
	public string Value { get; set; }
}

internal sealed class TestSystemClock : ISystemClock
{
	private DateTimeOffset now;

	public TestSystemClock(DateTimeOffset? start = null)
	{
		now = start ?? DateTimeOffset.UtcNow;
	}

	public DateTimeOffset UtcNow => now;

	public void Advance(TimeSpan delta) => now = now.Add(delta);
}

