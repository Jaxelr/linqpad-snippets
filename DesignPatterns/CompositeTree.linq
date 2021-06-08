<Query Kind="Program" />

void Main()
{
	// Create a tree structure
	var root = new Compost("root");
	root.Add(new Leaf("Leaf A"));
	root.Add(new Leaf("Leaf B"));

	var comp = new Compost("Compost X");
	comp.Add(new Leaf("Leaf XA"));
	comp.Add(new Leaf("Leaf XB"));

	root.Add(comp);
	root.Add(new Leaf("Leaf C"));

	var leaf = new Leaf("Leaf D");
	root.Add(leaf);
	root.Remove(leaf);

	root.Show(1);
}

abstract class Component
{
	protected string name;

	public Component(string name)
	{
		this.name = name;
	}
	public abstract void Add(Component c);
	public abstract void Remove(Component c);
	public abstract void Show(int depth);
}

class Compost : Component
{
	private List<Component> children = new List<Component>();
		
	public Compost(string name) : base(name)
	{
	}
	
	public override void Add(Component c)
	{
		children.Add(c);
	}

	public override void Remove(Component c)
	{
		children.Remove(c);
	}

	public override void Show(int depth)
	{
		string displayable = string.Concat(new string('-', depth), name);

		displayable.Dump("Compost");

		foreach (var c in children)
		{
			c.Show(depth+2);
		}
	}
}

class Leaf : Component
{
	public Leaf(string name) : base(name)
	{
	}
	
	public override void Add(Component c)
	{
		"Cannot add to a leaf".Dump("Leaf");
	}

	public override void Remove(Component c)
	{
		"Cannot remove from a leaf".Dump("Leaf");
	}

	public override void Show(int depth)
	{
		string displayable = string.Concat(new string('-', depth), name);
		
		displayable.Dump("Leaf");
	}
}

