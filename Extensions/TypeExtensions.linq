<Query Kind="Expression" />

public static class TypeExtension
{ 
	public static bool IsIEnumerable(this Type type) => (type.GetInterface(nameof(IEnumerable)) != null);
	public static bool IsCollection(this Type type) => 	(type.GetInterface(nameof(ICollection)) != null);
}