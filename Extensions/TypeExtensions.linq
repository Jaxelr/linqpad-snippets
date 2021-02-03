<Query Kind="Statements" />

public static class TypeExtension
{ 
	public static bool IsIEnumerable(this Type type) => (type.GetInterface(nameof(IEnumerable)) != null);
	
	public static bool IsCollection(this Type type) => (type.GetInterface(nameof(ICollection)) != null);
	
	public static bool IsArray(this Type type) => (type.BaseType == typeof(Array));
	
	public static Type GetAnyElementType(this Type type)
	{
		if (type.IsArray)
			return type.GetElementType();

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			return type.GetGenericArguments()[0];

		// type implements/extends IEnumerable<T>;
		var enumType = type.GetInterfaces()
								.Where(t => t.IsGenericType &&
									   t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
								.Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
		return enumType ?? type;
	}
}