<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(connstring);
	SqlInsightDbProvider.RegisterProvider();

	var response = conn.Query(@"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName
                    SELECT 1 ParentId, 4 ChildId, 'Aidan' ChildName, 'Rojas' SecondChildName", Parameters.Empty, 
		Query.Returns<Parent>().ThenChildren(Some<Child>.Records), CommandType.Text);
		
	response.Dump("Parent Child");
}

// Define other methods and classes here
string connstring = "Server=localhost\\brief;database=master;Trusted_Connection=true;MultipleActiveResultSets=true;";

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