<Query Kind="Program">
  <Connection>
    <ID>b8f1c7ca-2805-4049-940e-3663c608c694</ID>
    <Persist>true</Persist>
    <Server>localhost\brief</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
</Query>

//Tuple Singleing using Insight.Database.

void Main()
{
	var conn = new SqlConnection(connstring);
	SqlInsightDbProvider.RegisterProvider();
	
	var (npi, taxid, ssn) = conn.SingleSql<(string, string, string)>("SELECT Item1='123456789', Item2='597074242', Item3='978456123'");
		
	npi.Dump("Npi");
	taxid.Dump("TaxId");
	ssn.Dump("Ssn");
}

string connstring = "Server=localhost\\brief;database=master;Trusted_Connection=true;MultipleActiveResultSets=true;";