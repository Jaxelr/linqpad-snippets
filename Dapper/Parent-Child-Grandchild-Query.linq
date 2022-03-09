<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	
	var lookup = new Dictionary<int, Parent>();
	var lookupChild = new Dictionary<int, Child>();

	var result = conn.Query<Parent, Child, GrandChild, Parent>(
	@"SELECT  1 ParentId, 'Parent1' ParentName, 2 ChildId, 'Child2' ChildName, 3 GrandChildId, 'GrandChild1' GrandChildName
		UNION
		SELECT  1 ParentId, 'Parent1' ParentName, 5 ChildId, 'Child2' ChildName, 4 GrandChildId, 'GrandChild2' GrandChildName",
	(parent, child, grandchild) =>
	{
		if (child is null)
		{
			return parent;
		}
		
		if (!lookup.TryGetValue(parent.ParentId, out Parent found))
		{
			lookup.Add(parent.ParentId, found = parent);
		}

		if (!lookupChild.TryGetValue(child.ChildId, out Child foundChild))
		{
			lookupChild.Add(child.ChildId, foundChild = child);
			found.Childs = found.Childs.Append(child);
		}

		if (grandchild is not null)
		{
			foundChild.GrandChilds = foundChild.GrandChilds.Append(grandchild);
		}

		return found;
		
	}, splitOn: "ChildId,GrandChildId").Distinct();
	
	result.Dump("Parent Child GrandChild Result");
}

class Parent
{ 
	public int ParentId { get; set; }
	public string ParentName { get; set; }
	public IEnumerable<Child> Childs { get; set; }
	
	public Parent()
	{
		Childs = new Child[] {};
	}
}

class Child
{ 
	public int ChildId { get; set; }
	public string ChildName { get; set; }
	public IEnumerable<GrandChild> GrandChilds { get; set; }
	
	public Child()
	{
		GrandChilds = new GrandChild[] {};
	}
}

class GrandChild
{ 
	public int GrandChildId { get; set; }
	public string GrandChildName { get; set; }
}