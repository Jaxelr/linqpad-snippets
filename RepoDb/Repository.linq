<Query Kind="Program">
  <NuGetReference>RepoDb.SqlServer</NuGetReference>
  <Namespace>RepoDb</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

void Main()
{
	using (var repository = new SampleRepository(MyExtensions.SQLConnectionString))
	{
		repository.CreateTable();

		var user = new User() { Id = 1, Name = "Jaxel" };

		var resp = repository.Create(user);

		resp.Dump("Insert");

		user.Name = "Antonio";

		var resp2 = repository.Update(user);

		resp2.Dump("Update");

		user.Name = "Rojas";

		var resp4 = repository.Merge(user);

		resp4.Dump("Merge");

		var resp3 = repository.Remove(user);

		resp3.Dump("Delete");

		repository.DropTable();
	}
}

// Define other methods, classes and namespaces here
public class SampleRepository : BaseRepository<User, SqlConnection>
{
	public SampleRepository(string connectionString) : base(connectionString)
	{
		RepoDb.SqlServerBootstrap.Initialize();
	}

	public void CreateTable() => ExecuteNonQuery("DROP TABLE IF EXISTS [User] CREATE TABLE [User] (Id int, Name varchar(256))");

	public object Create(User user) => Insert(user);

	public int Remove(User user) => Delete(user);

	public int Update(User user) => base.Update(user);

	public object Merge(User user) => base.Merge(user);

	public void DropTable() => ExecuteNonQuery("DROP TABLE IF EXISTS [User]");
}

public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
}
