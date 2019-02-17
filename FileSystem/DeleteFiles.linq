<Query Kind="Program" />

void Main()
{
	//Path to try to delete files.
	string Path = "";
	//Regex wildcard.
	string Extension = "*.xls";
	
	
	try
	{
		if (Directory.Exists(Path))
		{
			var dir = new DirectoryInfo(Path);

			foreach (var f in dir.EnumerateFiles(Extension))
			{
				f.Delete();
			}
		}
	}
	catch (IOException ioExc)
	{
		//File is probably opened.
		ioExc.Dump();
	}
	catch (Exception ex)
	{
		ex.Dump();		
	}
}

// Define other methods and classes here