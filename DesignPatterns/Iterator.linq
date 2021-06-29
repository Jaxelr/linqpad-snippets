<Query Kind="Program" />

void Main()
{
	var aggregate = new Aggregate();
	
	aggregate[0] = "Item A";
	aggregate[1] = "Item B";
	aggregate[2] = "Item C";
	aggregate[3] = "Item D";
	
	var iterator = aggregate.CreateIterator();
	
	iterator.First().Dump("First");
	iterator.IsDone().Dump("Is Done");
	aggregate.Count.Dump("Count");
	
	var item = iterator.First();

	while (item is object)
	{
		iterator.CurrentItem().Dump("Current Item");
		item = iterator.Next();
	}
	
	iterator.IsDone().Dump("Is Done");
}

public interface IAggregate
{ 
	public IIterator CreateIterator();
}

public class Aggregate : IAggregate
{
	Objects items;
	
	public Aggregate() => items = new();
	
	public IIterator CreateIterator() => new Iterator(this);
	
	public int Count => items.Count();

	public object this[int index]
	{
		get => items[index]; 
		set => items.Insert(index, value); 
	}
}

public class Objects : List<object>
{
}

public interface IIterator
{
	public object First();
	public object Next();
	public bool IsDone();
	public object CurrentItem();
}

public class Iterator : IIterator
{
	private int current = 0;
	private Aggregate aggregate;
	
	public Iterator(Aggregate aggregate) => this.aggregate = aggregate;
	
	public object CurrentItem() => aggregate[current];

	public object First() => aggregate[0];
	
	public bool IsDone() => current >= aggregate.Count;

	public object Next()
	{
		object ret = null;
		if (current < aggregate.Count - 1)
		{
			ret = aggregate[++current];
		}
		else
		{
			current = aggregate.Count;
		}
		
		return ret;
	}
}