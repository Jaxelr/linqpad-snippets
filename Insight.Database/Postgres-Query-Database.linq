<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <NuGetReference>Insight.Database.Providers.PostgreSQL</NuGetReference>
  <Namespace>Npgsql</Namespace>
  <Namespace>Insight.Database</Namespace>
  <Namespace>Insight.Database.Providers.PostgreSQL</Namespace>
</Query>

void Main()
{
	var connection = new NpgsqlConnection(MyExtensions.PostgresConnectionString);
	PostgreSQLInsightDbProvider.RegisterProvider();
	
	var result = connection.QuerySql(@"SELECT @param as Id; SELECT 'Welp' stringy;", new { param = 1 },
	Query.Returns<int>().Then(Some<string>.Records));
	
	result.Dump();

}
