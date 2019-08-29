<Query Kind="Program" />

void Main()
{
	const decimal bytes = 1024.0M;
	var info = new FileInfo(FileName);

	if (info is FileInfo)
	{
		FileSize = decimal.Round(info.Length / bytes, 2, System.MidpointRounding.AwayFromZero);
		RecordCount = File.ReadLines(FileName).Count();

	}
	else
	{
		FileSize = 0.0M;
		RecordCount = 0;
	}
}

// Define other methods and classes here
public string FileName { get; set; }
public decimal FileSize { get; set; }
public int RecordCount { get; set; }