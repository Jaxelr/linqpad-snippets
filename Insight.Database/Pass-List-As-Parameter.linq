<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

    conn.ExecuteSql("IF OBJECT_ID(N'dbo.Sample') IS NULL CREATE TABLE dbo.Sample (sampleid int, description nvarchar(32));");
    conn.ExecuteSql("IF TYPE_ID(N'dbo.SampleType') IS NULL CREATE TYPE dbo.SampleType AS TABLE ( sampleid int, [description] nvarchar(50) );");

    var samples = new List<Sample>
    {
        new Sample { SampleId = 1, Description = "This is a description" },
        new Sample { SampleId = 2, Description = "This is a second description" }
    };

    var res = conn.QuerySql<Sample>("INSERT INTO Sample output inserted.* SELECT * FROM @SampleType", new { SampleType = samples });

	res.Dump();

	conn.ExecuteSql("if OBJECT_ID(N'dbo.Sample') > 0 DROP TABLE dbo.Sample;");
	conn.ExecuteSql("IF TYPE_ID(N'dbo.SampleType') > 0 DROP TYPE dbo.SampleType;");
}

// Define other methods and classes here

class Sample
{
	public int SampleId { get; set; }
	public string Description { get; set; }
}