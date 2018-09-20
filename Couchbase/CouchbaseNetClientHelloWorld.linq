<Query Kind="Program">
  <NuGetReference>CouchbaseNetClient</NuGetReference>
  <Namespace>Couchbase</Namespace>
  <Namespace>Couchbase.Configuration.Client</Namespace>
  <Namespace>Couchbase.Authentication</Namespace>
</Query>

void Main()
{
	var cluster = new Cluster(new ClientConfiguration
	{
	    Servers = new List<Uri> { new Uri("http://127.0.0.1") }
	});
	
	var authenticator = new PasswordAuthenticator("user1", "password");
	cluster.Authenticate(authenticator);
	
	using (var bucket = cluster.OpenBucket("travel-sample"))
	{
		var document = new Document<dynamic>
		{
			Id = "Hello",
			Content = new
			{
				name = "Couchbase"
			}
		};

		var upsert = bucket.Upsert(document);
		if (upsert.Success)
		{
			var get = bucket.GetDocument<dynamic>(document.Id);
			document = get.Document;
			var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
			Console.Write(msg);
		}
		
	}
}

// Define other methods and classes here
