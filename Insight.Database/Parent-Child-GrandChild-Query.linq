<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

//Parent-Child-GrandChild-Query
void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	var repo = conn.As<HelloRepo>();

	var res1 = repo.GetParent();
	res1.Dump();

	var res2 = repo.GetParentChild();
	res2.Dump();
}


public abstract class HelloRepo
{
	public abstract IDbConnection GetConnection();

	[Sql("SELECT 1 ParentId, 'Parent' Name, 'Last Parent' SecondName")]
	public abstract Parent GetParent();

	public IEnumerable<Parent> GetParentChild() =>
		GetConnection().Query(@"
		    SELECT 1 ParentId, 'Parent' Name, 'Last Parent' SecondName
    		SELECT 1 ParentId, 4 ChildId, 'Child' ChildName, 'Last Child' SecondChildName
			SELECT 4 ChildId, 6 GrandChildId, 'Grand Child' GranChildName, 'Grand Child Last' SecondGrandChildName",
			Parameters.Empty, Query.Returns<Parent>()
			.ThenChildren(Some<Child>.Records)
			.ThenChildren(Some<GrandChild>.Records, parents: p => p.Childs, into: (p, c) => p.GrandChild = c), 
			CommandType.Text);
}

public class Parent
{
	public int ParentId { get; set; }
	public string Name { get; set; }
	public string SecondName { get; set; }
	public IEnumerable<Child> Childs { get; set; }
}

public class Child
{
	public int ChildId { get; set; }
	public string ChildName { get; set; }
	public string SecondChildName { get; set; }
	public IEnumerable<GrandChild> GrandChild { get; set; }
}

public class GrandChild
{
	public int GrandChildId { get; set; }
	public string GranChildName { get; set; }
	public string SecondGrandChildName { get; set; }
}