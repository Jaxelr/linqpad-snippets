<Query Kind="Expression" />

//Given an ienumerable, find any element of a second ienumerable and return true.
public static bool Contains<T>(this IEnumerable<T> ts, IEnumerable<T> ts2)
{
	if (ts is null || ts2 is null)
	{
		return false;
	}

	return (ts.Intersect(ts2).Any());
}