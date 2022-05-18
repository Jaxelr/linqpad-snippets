<Query Kind="Program" />

void Main()
{
	int value = 2;
	
	value
	.Pipe(x => x * 3)
	.Pipe(x => x.Dump());
}

/*****
**** Taken from twitter @cwrenhold
******
* F# equivalent:
* 
* let value = 2
* value
* |> (fun x -> x * 3)
* |> printfn "%d"
*******/

public static class GenericExtensions
{
	public static TOut Pipe<TIn, TOut>(this TIn input, Func<TIn, TOut> operation)
	{
		return operation(input);
	}

	public static void Pipe<TIn>(this TIn input, Action<TIn> operation)
	{
		operation(input);
	}
}
