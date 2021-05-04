<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>CsvHelper</Namespace>
</Query>

void Main()
{
	/* Make sure the file exists by using CsvWriter.linq first */
	using var reader = new StreamReader("C:\\temp\\output.csv");

	using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

	var records = csv.GetRecords<Example>();

	records.Dump();
}

public class Example
{
	public int Id { get; set; }
	public string Value { get; set; }
	public DateTime Date { get; set; }
}