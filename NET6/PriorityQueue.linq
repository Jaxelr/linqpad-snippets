<Query Kind="Program" />

/* New dotnet PriorityQueue */
void Main()
{
	var queue = new PriorityQueue<Person, int>();
	
	queue.Enqueue(new Person("Test 1", 10), 1);
	queue.Enqueue(new Person("Test 2", 12), 3);
	queue.Enqueue(new Person("Test 3", 11), 2);


	while (queue.Count > 0)
	{
		var item = queue.Dequeue();

		$"Name: {item.Name} Age: {item.Age}".Dump();
	}
}

// You can define other methods, fields, classes and namespaces here
public record Person(string Name, int Age);
