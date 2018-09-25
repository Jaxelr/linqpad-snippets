<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>Insight.Database.Structure</Namespace>
</Query>

void Main()
{
	//Together Implementation
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	const string query = @"SELECT 1 ParentId, 'Jaxel' ParentName, 2 ChildId, 'Aidan' ChildName UNION
                           SELECT 1 ParentId, 'Jaxel' ParentName, 3 ChildId, 'Sebastian' ChildName;";

	var result = conn.QuerySql(query, Parameters.Empty, Query.Returns(Together<Parent, Child>.Records));

	result.Dump();
}

// Define other methods and classes here

public class Parent
{
	public int ParentId { get; set; }
	public string ParentName { get; set; }
	public IEnumerable<Child> Children { get; set; }
}

public class Child
{
	public int ChildId { get; set; }
	public int ParentId { get; set; }
	public string ChildName { get; set; }
}