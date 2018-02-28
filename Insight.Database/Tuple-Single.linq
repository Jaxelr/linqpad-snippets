<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
</Query>

//Tuple Singleing using Insight.Database.

void Main()
{
	
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();
	
	var (npi, taxid, ssn) = conn.SingleSql<(string, string, string)>("SELECT Item1='123456789', Item2='597074242', Item3='978456123'");
		
	npi.Dump("Npi");
	taxid.Dump("TaxId");
	ssn.Dump("Ssn");
}