<Query Kind="Program">
  <NuGetReference>Azure.Storage.Blobs</NuGetReference>
  <Namespace>Azure.Storage</Namespace>
  <Namespace>Azure.Storage.Blobs</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	const string blobUri = "";
	StorageSharedKeyCredential storagekey = new("", "");
	var blobServiceClient = new BlobServiceClient(new Uri(blobUri), storagekey);

	var poco = new Poco() { Id = Guid.NewGuid(), Name = "Custom"  };

	BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("sample");
	var blobClient = blobContainerClient?.GetBlobClient(string.Concat(poco, ".json"));
	
	await blobClient?.DeleteIfExistsAsync();
}

// You can define other methods, fields, classes and namespaces here
public class Poco
{ 
	public Guid Id { get; set; }
	public string Name { get; set; }
}