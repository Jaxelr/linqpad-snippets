<Query Kind="Program" />

void Main()
{
	var poco = new Poco() { Id = 1, Value = "Test"};
	var enforcer = new RuleEnforcer<Poco>(poco);

	enforcer.local.Dump();
	
	var result = enforcer.ApplyRule(x => x.Id == 1);
	
	result.Dump();
		
	result = enforcer
		.Define(x => x.Id == 1)
		.Define(x => x.Value == "Testy")
		.Resolve();

	result.Dump();
}

public record Poco
{
	public int Id { get; set; }
	public string Value { get; set; }
}

class RuleEnforcer<T>
{
	public RuleEnforcer(T poco)
	{
		local = poco;
		rules = new List<System.Func<T, bool>>();
	}

	public T local;

	IList<Func<T, bool>> rules;

	public RuleEnforcer<T> Define(Func<T, bool> rule)
	{
		rules.Add(rule);

		return this;
	}

	public bool ApplyRule(Func<T, bool> func)
	{
		return func(local);
	}

	public bool Resolve()
	{
		foreach (var rule in rules)
		{
			if (!rule(local))
			{
				return false;
			}
		}

		return true;
	}
}