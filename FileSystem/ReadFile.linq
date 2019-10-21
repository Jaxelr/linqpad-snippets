<Query Kind="Program" />

void Main()
{
	List<string> list = new List<string>();


	using (var fileStream = File.OpenRead("{Path Goes here}"))
	using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
	{
		string line;
		while ((line = streamReader.ReadLine()) != null)
	  	{
			string look = line.Substring(0, line.IndexOf("*"));
		
			if (!list.Contains(look))
				list.Add(look);
		}
	}
	
	list.Dump();
}

// Define other methods, classes and namespaces here