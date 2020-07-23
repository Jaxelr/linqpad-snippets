<Query Kind="Program">
  <NuGetReference>RepoDb.SqlServer</NuGetReference>
  <NuGetReference>System.Data.SqlClient</NuGetReference>
  <Namespace>Microsoft.Data.SqlClient</Namespace>
  <Namespace>RepoDb</Namespace>
</Query>

void Main()
{
	using (var connection = new SqlConnection(MyExtensions.SQLConnectionString).EnsureOpen())
	{
	
	    RepoDb.SqlServerBootstrap.Initialize();
		var worked = connection.ExecuteQuery<User>("SELECT 1 Id, 'Jaxel' Name");
	
		worked.Dump("Select");
	
		_ = connection.ExecuteNonQuery(@"DROP TABLE IF EXISTS [User] CREATE TABLE [User] (Id int, Name varchar(256))");
	
		//Insert
		var user = new User() { Id = 2, Name = "Jaxel" };
	
		var insertWorked = connection.Insert(user);
		insertWorked.Dump("Insert");
	
		//Update
		user.Name = "Antonio";
	
		var updateWorked = connection.Update(user);
		updateWorked.Dump("Update");
	
		//Delete
		var deleteWorked = connection.Delete<User>(x => x.Name == user.Name);
		deleteWorked.Dump("Delete");
	
		//Cleanup
		_ = connection.ExecuteNonQuery(@"DROP TABLE IF EXISTS [User] ");
	}	
}

// Define other methods, classes and namespaces here
public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
}