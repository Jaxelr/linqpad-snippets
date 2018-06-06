<Query Kind="Expression" />

public static T Convert<T>(this string me)
{
	try
	{
		var converter = TypeDescriptor.GetConverter(typeof(T));

		if (converter != null)
		{
			return (T)converter.ConvertFromString(me.Trim());
		}

		return default(T);
	}
	catch (NotSupportedException)
	{
		return default(T);
	}
}