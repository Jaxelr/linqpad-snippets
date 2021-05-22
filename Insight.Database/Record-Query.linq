<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var connection = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();
	
	var result = connection.SingleSql<Person>("SELECT 2 Age, 'Lana' Name");
	
	result.Dump();
}

// You can define other methods, fields, classes and namespaces here
public record Person
{ 
	public int Age { get; set; }
	public string Name { get; set; }
}