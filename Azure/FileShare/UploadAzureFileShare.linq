<Query Kind="Program">
  <NuGetReference>Azure.Storage.Files.Shares</NuGetReference>
  <Namespace>Azure.Storage.Files.Shares</Namespace>
  <Namespace>Azure</Namespace>
  <Namespace>Azure.Storage.Files.Shares.Models</Namespace>
</Query>

void Main()
{
	string connectionString = "<<Get Your Connection From Azure>>";

	string shareName = "share";
	string dirName = @"/long/path/somewhere";

	string localDirectory = @"C:\Temp";
	string searchPattern = "*.pdf";

	try
	{
		var share = new ShareClient(connectionString, shareName);
		var directory = share.GetDirectoryClient(dirName);

		foreach (string file in Directory.EnumerateFiles(localDirectory, searchPattern))
		{
			var fileInfo = new FileInfo(file);
			var fileClient = directory.GetFileClient(fileInfo.Name);

			using var stream = File.OpenRead(file);
			fileClient.Create(stream.Length);
			fileClient.UploadRange(new HttpRange(0, stream.Length), stream);
		}

	}
	catch (Exception ex) 
	{
		ex.Dump();	
	}
}