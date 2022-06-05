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
	string fileName = "Test.pdf";

	string localFilePath = @"C:\Temp\File.pdf";

	try
	{
		var share = new ShareClient(connectionString, shareName);
		var directory = share.GetDirectoryClient(dirName);

		ShareFileClient file = directory.GetFileClient(fileName);

		// Download the file
		ShareFileDownloadInfo download = file.Download();
		using (var stream = File.OpenWrite(localFilePath))
		{
			download.Content.CopyTo(stream);
		}
	}
	catch (Exception ex) 
	{
		ex.Dump();	
	}
}