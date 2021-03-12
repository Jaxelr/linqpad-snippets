<Query Kind="Program">
  <Namespace>System.Buffers</Namespace>
</Query>

void Main()
{
	var buffer = ArrayPool<byte>.Shared.Rent(1);

	using var _ = Defer(b =>
	{
		"I got invoked after".Dump();
		
		ArrayPool<byte>.Shared.Return(b);
	}, buffer);
	
	"I got invoked prior to mod of buffer".Dump();

	buffer.Dump();
	
	buffer[0] = 3;
	
	"I got the buffer modified".Dump();
	
	buffer.Dump();
}

static DeferDisposable<T> Defer<T>(Action<T> action, T param1) =>
	new DeferDisposable<T>(action, param1);


// struct to avoid allocation
internal readonly struct DeferDisposable<T1> : IDisposable
{
	readonly Action<T1> _action;
	readonly T1 _param1;
	public DeferDisposable(Action<T1> action, T1 param1) => (_action, _param1) = (action, param1);
	public void Dispose() => _action.Invoke(_param1);
}