<Query Kind="Expression" />

public static bool ApplyRule<T>(this T poco, Func<T, bool> func)
{
	return func(poco);
}

public static bool ApplyRules<T>(this T poco, IEnumerable<Func<T, bool>> funcs)
{
	foreach (var func in funcs)
	{
		if (func(poco))
		{
			continue;
		}
		
		return false;
	}
	
	return true;
}