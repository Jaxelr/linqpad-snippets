<Query Kind="Program" />

void Main(string[] args)
{
	string inputpath = @"C:\\Temp\\";
	string outputpath = @"C:\\Temp\\Output\\";
	string filter = "*.*";

	//Maybe the standard input has something for us?
	if (args.Length > 0 && args[0] is string)
		inputpath = args[0];

	if (args.Length > 1 && args[1] is string)
		outputpath = args[1];

	if (args.Length > 2 && args[2] is string)
		filter = args[2];

	foreach (string file in Directory.GetFiles(inputpath, filter))
	{
		using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
		{
			var sr = new StreamReader(stream);
			var sw = new StreamWriter(string.Concat(file.Replace(inputpath, outputpath)));

			char[] a;

			while (!sr.EndOfStream)
			{
				//This could be a string or whatever you want.
				a = sr.ReadLine().ToCharArray();

				//Manipulation goes here...

				sw.WriteLine(a);
			}

			sw.Close();
		}

		if (Debugger.IsAttached)
			Console.Read();
	}
}
// Define other methods and classes here
