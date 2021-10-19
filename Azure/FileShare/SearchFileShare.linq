<Query Kind="Program">
  <NuGetReference>Azure.Storage.Files.Shares</NuGetReference>
  <Namespace>Azure.Storage.Files.Shares</Namespace>
</Query>

void Main()
{
	string connectionString = "<<Get Your Connection From Azure>>";
	string shareName = "vital";
	var share = new ShareClient(connectionString, shareName);

	// Track the remaining directories to walk, starting from the root
	var remaining = new Queue<ShareDirectoryClient>();
	remaining.Enqueue(share.GetRootDirectoryClient());
	while (remaining.Count > 0)
	{
		// Get all of the next directory's files and subdirectories
		ShareDirectoryClient dir = remaining.Dequeue();
		foreach (var item in dir.GetFilesAndDirectories())
		{
			// Print the name of the item
			Console.WriteLine(item.Name);

			// Keep walking down directories
			if (item.IsDirectory)
			{
				remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
			}
		}
	}
}

// You can define other methods, fields, classes and namespaces here
