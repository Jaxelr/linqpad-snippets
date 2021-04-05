<Query Kind="Program">
  <NuGetReference>Insight.Database.Providers.MySql</NuGetReference>
  <Namespace>MySql.Data.MySqlClient</Namespace>
  <Namespace>Insight.Database</Namespace>
  <Namespace>Insight.Database.Providers.MySql</Namespace>
</Query>

void Main()
{
	var connection = new MySqlConnection(MyExtensions.MySqlConnectionString);
	MySqlInsightDbProvider.RegisterProvider();
	connection.Open();
	
	long res = connection.SingleSql<long>("SELECT 1 as p");
	
	res.Dump();
}
