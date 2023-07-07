<Query Kind="Program" />

void Main()
{
	Either<int, string> either;
	
	either = 42;

	int num = TryMatch(either);
	
	num.Dump("from int");
	
	either = "42";

	num = TryMatch(either);
	
	num.Dump("from string");
}

int TryMatch(Either<int, string> param) => 
	param.Match(
		value1 => value1,
		value2 => int.Parse(value2)
	);

//Taken from this post: https://www.devleader.ca/2023/05/31/implicit-operators-in-c-and-how-to-create-a-multi-type/
public class Either<T1, T2>
{
	private readonly T1? _value1;
	private readonly T2? _value2;
	private readonly bool _isValue1;

	public Either(T1 value) => (_value1, _isValue1) = (value, true);

	public Either(T2 value) => (_value2, _isValue1) = (value, false);

	public static implicit operator Either<T1, T2>(T1 value) => new Either<T1, T2>(value);

	public static implicit operator Either<T1, T2>(T2 value) => new Either<T1, T2>(value);

	public T Match<T>(Func<T1, T> f1, Func<T2, T> f2) => _isValue1 ? f1(_value1!) : f2(_value2!);
}