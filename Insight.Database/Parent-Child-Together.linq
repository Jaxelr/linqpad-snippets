<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>Insight.Database.Structure</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	var response = conn.QuerySql(@"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName, 3 ChildId, 'Seba' ChildName, 'Rojas' SecondChildName UNION
                    SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName, 4 ChildId, 'Aidan' ChildName, 'Rojas' SecondChildName", null, 
		Query.Returns(Together<Parent, Child>.Records));
		
	response.Dump("Parent Child");
}

class Parent
{
	public int ParentId { get; set; }
	public string Name { get; set; }
	public string SecondName { get; set; }
	public IEnumerable<Child> Childs { get; set; }
}

class Child
{
	public int ChildId { get; set; }
	public string ChildName { get; set; }
	public string SecondChildName { get; set; }
}