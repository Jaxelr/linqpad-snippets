<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

//Tuple Singleing using Insight.Database.

void Main()
{
	var connection = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();
	
	var (npi, taxid, ssn) = connection.SingleSql<(string, string, string)>("SELECT Item1='123456789', Item2='597074242', Item3='978456123'");
		
	npi.Dump("Npi");
	taxid.Dump("TaxId");
	ssn.Dump("Ssn");
}