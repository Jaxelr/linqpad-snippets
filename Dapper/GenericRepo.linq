<Query Kind="Expression">
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
</Query>

// Define other methods and classes here
public class GenericRepo
{
	private readonly IDbConnection Connection;

	public GenericRepo(IDbConnection connection)
	{
		Connection = connection;
	}

	public IEnumerable<T> Select<T>() => Connection.Query<T>($"SELECT * FROM {typeof(T).Name};", commandType: CommandType.Text);

	public IEnumerable<T> SelectWithFilter<T>(T poco, string sql) => Connection.Query<T>(sql, poco, commandType: CommandType.Text);

	public T Store<T>(T poco, string sql) => Connection.QueryFirstOrDefault<T>(sql, poco, commandType: CommandType.Text);

	public IEnumerable<T> SelectWithPaging<T>(int offset, int limit)
	{
		return Connection.Query<T>($"SELECT * FROM {typeof(T).Name} ORDER BY 1 OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;", commandType: CommandType.Text);
	}
}