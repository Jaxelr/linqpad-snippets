<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	
	var lookup = new Dictionary<int, Parent>();

	var result = conn.Query<Parent, Child, Parent>(
	@"SELECT 
		p.ParentId, p.Name, p.SecondName, c.ChildId, 
		c.Name ChildName, c.SecondName SecondChildName 
	FROM Parent p INNER JOIN Child c ON p.ParentId = c.ParentId",
	(parent, child) =>
	{
		if (!lookup.TryGetValue(parent.ParentId, out Parent found))
		{
			lookup.Add(child.ChildId, found = parent);
		}
		found.Childs.Add(child);
		return found;
	}, splitOn: "ChildId").Distinct();
	
	result.Dump("Parent Child Result");
}

//conn string
string cnstring = "Data Source=localhost\\brief;Initial Catalog=ParentChild;Integrated Security=True";

class Parent
{ 
	public int ParentId { get; set; }
	public string Name { get; set; }
	public string SecondName { get; set; }
	public IList<Child> Childs { get; set; }
	
	public Parent()
	{
		Childs = new List<Child>();
	}
}

class Child
{ 
	public int ChildId { get; set; }
	public string ChildName { get; set; }
	public string SecondChildName { get; set; }
}