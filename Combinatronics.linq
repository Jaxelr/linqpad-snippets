<Query Kind="Program" />

/* Permutation lookup 
 * I needed to find from a list of objects the plausible combination that amounts to a specific value 
 * based on the sum of a specific field 
 * In this scenario it is the sum of value, by any amount of POCOs 
 * note that this is inneficient for large amounts of records */
 
void Main()
{
	bool found = false;
	double result = 28;
	
	var list = new List<Example>
	{
		new Example() { Id = 1, Value = 10.0 },
		new Example() { Id = 2, Value = 13.0 },
		new Example() { Id = 3, Value = 17.0 },
		new Example() { Id = 4, Value = 28.0 }
	};

	/* Edge case where the sum of everything is the result */
	if (list.Sum(t => t.Value) == result)
	{
		list.Dump();
		found = true;
	}

	/* Edge cases where the single value is the result */
	list.ForEach(x =>
	{
		if (x.Value == result)
		{
			list.Where(y => y.Id == x.Id).Dump();
			found = true;
		}
	});
	
	int marker = 0;

	/* We will do 2 loops, one with all flags on, the other with all flags off */
	while (!found)
	{
		for (int i = marker; i < list.Count(); i++)
		{
			list[i].Include = false;

			var res = list
				.Where(t => t.Include)
				.Select(t => t.Value)
				.Sum();
				
			if (res == result)
			{
				list.Where(l => l.Include).Dump();
				found = true;
			}
			
			list[i].Include = true;
		}
		
		list.ForEach(x => x.Include = true);

		for (int i = (list.Count() - 1); i >= 0; i--)
		{
			list[i].Include = false;

			var res = list
				.Where(t => t.Include)
				.Select(t => t.Value)
				.Sum();

			if (res == result)
			{
				list.Where(l => l.Include).Dump();
				found = true;
			}

			list[i].Include = true;
		}

		if (marker >= list.Count()) 
			break;

		list[marker].Include = false;
		marker++;
	}
}

public class Example
{ 
	public int Id { get; set; }
	public double Value { get; set; }
	public bool Include { get; set; } = false;
}