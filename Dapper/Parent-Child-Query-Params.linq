<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	int Id = 1;
	
	var lookup = new Dictionary<int, Parent>();
	
	var result = conn.Query<Parent, Child, Parent>(
	@"SELECT 
		p.ParentId, p.Name, p.SecondName, c.ChildId, 
		c.Name ChildName, c.SecondName SecondChildName 
	FROM Parent p INNER JOIN Child c ON p.ParentId = c.ParentId
	WHERE p.ParentId = @Id",
	(parent, child) =>
	{
		if (child is null)
		{
			return parent;
		}
		
		if (!lookup.TryGetValue(parent.ParentId, out Parent found))
		{
			lookup.Add(child.ChildId, found = parent);
		}
		
		found.Childs.Add(child);
		
		return found;
	}, 
	new { Id },
	splitOn: "ChildId").Distinct();

	result.Dump("Parent Child Result");
}

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