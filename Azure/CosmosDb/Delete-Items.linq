<Query Kind="Program">
  <NuGetReference>Microsoft.Azure.Cosmos</NuGetReference>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Azure.Cosmos</Namespace>
  <Namespace>Azure.Core.Serialization</Namespace>
</Query>

async Task Main()
{
	var Options = new JsonSerializerOptions()
	{
		PropertyNameCaseInsensitive = true
	};

	var cosmosSerializer = new CosmosSystemTextJsonSerializer(Options);

	var client = new CosmosClient(MyExtensions.CosmosDbUrl, new CosmosClientOptions()
	{
		Serializer = cosmosSerializer
	});

	var db = client.GetDatabase("DefenderDb");
	var container = db.GetContainer("VirusTotal");

	// Change your filter here
	var iterator = container.GetItemQueryIterator<Poco>("SELECT * FROM c WHERE 1=1");

	while (iterator.HasMoreResults)
	{
		foreach (var item in await iterator.ReadNextAsync())
		{
			item.id.Dump();
			var result = await container.DeleteItemAsync<Poco>(item.id, new PartitionKey(item.PartitionKey));

			result.Dump();
		}
	}
}


public class CosmosSystemTextJsonSerializer : CosmosSerializer
{
	private readonly JsonObjectSerializer systemTextJsonSerializer;

	public CosmosSystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
	{
		this.systemTextJsonSerializer = new JsonObjectSerializer(jsonSerializerOptions);
	}

	public override T FromStream<T>(Stream stream)
	{
		using (stream);
		
		if (stream.CanSeek && stream.Length == 0)
		{
			return default;
		}

		if (typeof(Stream).IsAssignableFrom(typeof(T)))
		{
			return (T)(object)stream;
		}

		return (T)this.systemTextJsonSerializer.Deserialize(stream, typeof(T), default);
	
	}

	public override Stream ToStream<T>(T input)
	{
		MemoryStream streamPayload = new MemoryStream();
		this.systemTextJsonSerializer.Serialize(streamPayload, input, typeof(T), default);
		streamPayload.Position = 0;
		return streamPayload;
	}
}

public class Poco
{ 
	public string id { get; set; }
	public string PartitionKey { get; set; }
}