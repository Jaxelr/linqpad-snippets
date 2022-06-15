<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var c = new SqlConnection(MyExtensions.SQLConnectionString);

 	var r = c.Query<(int, string)>(
	@"select 1 Id, 'A' [Value] UNION 
	  select 2 Id, 'B' [Value] UNION
	  select 3 Id, 'C' [Value]", null);
	
	int maxValue = r
		.OrderByDescending(x => x.Item1)
		.FirstOrDefault().Item1;
		
	var result = r.Select(x => x.Item2);
	
	maxValue.Dump("Max Previous Value");
	result.Dump("List");
}