<Query Kind="Program">
  <NuGetReference>Insight.Database</NuGetReference>
  <NuGetReference>System.Data.SQLite</NuGetReference>
  <Namespace>System.Data.Sql</Namespace>
  <Namespace>System.Data.SQLite</Namespace>
  <Namespace>Insight.Database</Namespace>
</Query>

void Main()
{
	var connection = new SQLiteConnection(MyExtensions.SqliteConnectionString);

	SqlInsightDbProvider.RegisterProvider();
	connection.Open();

	int iterations = 100;

	var cmd = connection.CreateCommand();

	cmd.CommandText = $@"
                    DROP TABLE IF EXISTS Post;
                    DROP TABLE IF EXISTS Comment;	
        
                    CREATE TABLE IF NOT EXISTS [Post] 
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        [Text] VARCHAR NOT NULL,
                        CreationDate DATETIME NOT NULL,
                        LastChangeDate DATETIME NOT NULL
                    );
        
                    CREATE TABLE IF NOT EXISTS [Comment]
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        PostId INTEGER NOT NULL,
                        [CommentText] VARCHAR NOT NULL,
                        CreationDate DATETIME NOT NULL
                    );
        
                    DELETE FROM Comment;
                    DELETE FROM Post;


                    WITH RECURSIVE temp ([Text], [CreationDate], [LastChangeDate])
                    AS 
                    (
                        SELECT 
                            PRINTF('%.' || 2000 ||'c', 'X') [Text],
                            DATETIME('now') [CreationDate], 
                            DATETIME('now') [LastChangeDate]
                        UNION ALL
                        SELECT 
                            [Text], 
                            [CreationDate],
                            [LastChangeDate]
                        FROM temp
                        LIMIT {iterations}
    
                    )

                    INSERT INTO Post([Text], [LastChangeDate], [CreationDate])
                    SELECT [Text], [LastChangeDate], [CreationDate] FROM temp;

                    INSERT INTO Comment([PostId], [CommentText], [CreationDate])
                    SELECT Id PostId, PRINTF('%.' || 2000 ||'c', 'X') [Text], DATETIME('now') [CreationDate]
                    FROM Post
                    UNION ALL 
                    SELECT Id PostId, PRINTF('%.' || 2000 ||'c', 'X') [Text], DATETIME('now') [CreationDate]
                    FROM Post";
					
					
	cmd.Connection = connection;
	cmd.ExecuteNonQuery();


	//var result = connection.SingleSql<dynamic>("SELECT * FROM Post WHERE Id = @param", new { param = 1 });
	
	var result = connection.SingleSql<Post>("SELECT * FROM Post WHERE Id = @param", new { param = 1 });
	
	result.Dump();
}


public class Post
{
	public int Id { get; set; }
	public string Text { get; set; }
	public DateTime CreationDate { get; set; }
	public DateTime LastChangeDate { get; set; }
}
