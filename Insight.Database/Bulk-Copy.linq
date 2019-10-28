<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	conn.ExecuteSql("IF OBJECT_ID('dbo.Sample') IS NULL CREATE TABLE dbo.Sample (sampleid int, description nvarchar(32));");
	
	var samples = new List<Sample>();

	samples.Add(new Sample { SampleId = 1, Description = "This is a description" });
	samples.Add(new Sample { SampleId = 2, Description = "This is a second description" });

	conn.BulkCopy("Sample", samples);
	
	var response = conn.QuerySql<Sample>("SELECT * FROM dbo.Sample;");
	
	response.Dump();

	conn.ExecuteSql("if OBJECT_ID('dbo.Sample') > 0 DROP TABLE dbo.Sample;");
}

// Define other methods and classes here

class Sample
{ 
	public int SampleId { get; set; }
	public string Description { get; set; }
}