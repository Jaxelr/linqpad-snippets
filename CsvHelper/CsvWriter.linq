void Main()
{
	/* Data */
	var examples = new List<Example>()
	{
		new Example(){ Id = 1, Value = "One", Date = DateTime.Now},
		new Example(){ Id = 2, Value = "Two", Date = DateTime.Now}
	};
	
	using var writer = new StreamWriter("C:\\temp\\output.csv");
	
	 var config = new CsvConfiguration(CultureInfo.InvariantCulture) 
	 { 
		 /* This Assures that all strings types are quoted */
		 ShouldQuote = args =>
		 {
		 	if (args.FieldType == typeof(string) && args.Row.Row != 1) /* Exclude the header */
			 {
			 	return true;
			 }
			 
			 return false;
		 }
	 };
	
	using var csv = new CsvWriter(writer, config);

	csv.Context.RegisterClassMap<ExampleMapper>();

	csv.WriteRecords(examples);
}

public class Example
{ 
	public int Id { get; set; }
	public string Value { get; set; }
	public DateTime Date { get; set; }
}

public class ExampleMapper : ClassMap<Example>
{ 
	public ExampleMapper()
	{
		Map(x => x.Id).Index(0);
		Map(x => x.Value).Index(1);
		Map(x => x.Date).TypeConverterOption.Format("yyyy-MM-dd").Index(2);
	}
}