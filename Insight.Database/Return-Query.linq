<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var connection = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();
	
	var result = connection.QueryResults<Sample, Return>("TestSelectReturn");

	if (result.Outputs.Return_Value == 2)
	{
		Console.WriteLine($"Returned value is: {result.Outputs.Return_Value}");

		Console.WriteLine(result.Set1.FirstOrDefault().Id);
		Console.WriteLine(result.Set1.FirstOrDefault().Name);
	}

	var result2 = connection.QueryResults<Results<Sample>>("TestSelectReturn");
	
	if (result2.Outputs.Return_Value == 2)
	{
		Console.WriteLine($"Returned value is: {result2.Outputs.Return_Value}");
		
		Console.WriteLine(result2.Set1.FirstOrDefault().Id);
		Console.WriteLine(result2.Set1.FirstOrDefault().Name);
	}
	
}

public class Sample
{ 
	public int Id { get; set; }
	public string Name { get; set; }
}

public class Return
{ 
	public int Return_Value { get; set; }
}

/*
The following procedure must exist on the db for this sample to work:

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROC TestSelectReturn 
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	SELECT 1 Id, 'Name' Name

	RETURN 2;
END
GO
*/