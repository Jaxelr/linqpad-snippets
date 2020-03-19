<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var c = new SqlConnection(MyExtensions.SQLConnectionString);

	var Ids = new int[] { 1, 2, 3 };

	var r = c.Query<int>(
	@"select * 
      from (select 1 as Id union all select 2 union all select 3) as X 
      where Id in @Ids",
	new { Ids });
	
	r.Dump();
}

// Define other methods and classes here