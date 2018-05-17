<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <Namespace>Insight.Database</Namespace>
</Query>

void Main()
{
	var conn = new SqlConnection(MyExtensions.SQLConnectionString);
	SqlInsightDbProvider.RegisterProvider();

	var repo = conn.As<HelloRepo>();

	repo.GetType().Dump();

	var res1 = repo.GetParent();
	
	res1.Dump();

	var res2 = repo.GetParentChild();
	
	res2.Dump();
}


public abstract class HelloRepo
{
	public abstract IDbConnection GetConnection();

	[Sql("SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName")]
	public abstract Parent GetParent();

	public IEnumerable<Parent> GetParentChild() =>
		GetConnection().Query(@"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName
                        		SELECT 1 ParentId, 4 ChildId, 'Aidan' ChildName, 'Rojas' SecondChildName", 
								Parameters.Empty, Query.Returns<Parent>()
								.ThenChildren(Some<Child>.Records), CommandType.Text);
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
}