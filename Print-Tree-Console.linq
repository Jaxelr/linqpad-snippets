<Query Kind="Program" />

//Some parts of this code are taken from this github: https://github.com/andrewlock/blog-examples
void Main()
{
	var node = new Node();

	node.Name = "Root";

	var list = new Nodes();
	{
		list.Add(new Node() { Name = "Ride" });
		list.Add(new Node() { Name = "The" });
		list.Add(new Node() { Name = "Lightning" });
	};	
	
	node.Children.Add(new Node() 
	{
		Name = "Master",
		Children = list
	});
	node.Children.Add(new Node()
	{
		Name = "Of",
	});
	node.Children.Add(new Node()
	{
		Name = "Puppets",
	});
	
	node.Dump();
	
	PrintNode(node);
}

// Constants for drawing lines and spaces
private const string _cross = " ├─";
private const string _corner = " └─";
private const string _vertical = " │ ";
private const string _space = "   ";

static void PrintNode(Node node, string indent = "")
{
	Console.WriteLine(node.Name);

	var numberOfChildren = node.Children.Count;
	for (var i = 0; i < numberOfChildren; i++)
	{
		var child = node.Children[i];
		var isLast = (i == (numberOfChildren - 1));
		PrintChildNode(child, indent, isLast);
	}
}

static void PrintChildNode(Node node, string indent, bool isLast)
{
	Console.Write(indent);
	if (isLast)
	{
		Console.Write(_corner);
		indent += _space;
	}
	else
	{
		Console.Write(_cross);
		indent += _vertical;
	}

	PrintNode(node, indent);
}

class Node
{
	public string Name { get; set; }

	public Nodes Children { get; set; } = new Nodes();
}

class Nodes : List<Node>
{ 
}