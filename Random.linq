<Query Kind="Program" />

void Main()
{
	(int x, int y) = (0, 10);
	var random = new Random();
	
	//Get Random number within range
	random.Next(x, y)
	.Dump();
	
	//Range of max - min int values (2147483647)
	random.Next()
	.Dump();
}
