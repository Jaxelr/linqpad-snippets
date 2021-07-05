void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	// Write custom extension methods here. They will be available to all queries.
	public const string SQLConnectionString = "Server=.;database=master;Trusted_Connection=true;MultipleActiveResultSets=true;";
	public const string DB2ConnectionString = "Database=HSCRP;Server=10.31.1.68:60016;UserID=appsuid;Password=Comp1ete4you;CurrentSchema=HS";
	public static string PostgresConnectionString => "Host=localhost;Port=5432;database=postgres;User ID=postgres;Password=postgres";
	public static string MySqlConnectionString => "Server=127.0.0.1;Port=3306;Database=test;User Id=root;";
}

// You can also define non-static classes, enums, etc.