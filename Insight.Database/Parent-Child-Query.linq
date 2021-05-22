<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var connection = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	var response = connection.Query(@"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName
                    SELECT 1 ParentId, 4 ChildId, 'Aidan' ChildName, 'Rojas' SecondChildName", Parameters.Empty, 
		Query.Returns<Parent>().ThenChildren(Some<Child>.Records), CommandType.Text);
		
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