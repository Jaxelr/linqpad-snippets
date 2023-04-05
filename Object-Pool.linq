<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.ObjectPool</NuGetReference>
  <Namespace>Microsoft.Extensions.ObjectPool</Namespace>
</Query>

//https://learn.microsoft.com/en-us/aspnet/core/performance/objectpool?view=aspnetcore-7.0

void Main()
{
	var pool = new DefaultObjectPool<List<int>>(new DefaultPooledObjectPolicy<List<int>>());
	
	var list = pool.Get();
	
	list.Add(1);
	list.Add(2);
	list.Add(3);
	
	pool.Return(list);
	
	list.Dump("List-1");
	
	var list2 = pool.Get(); //We retrieve the same object from memory
	
	list2.Dump("List-2");
	
	var list3 = pool.Get(); //This will return a new one

	list3.Add(4);
	list3.Add(5);
	list3.Add(6);

	list3.Dump("List-3");

	pool.Return(list2);	//This will be returned by the amount of returns made
	pool.Return(list3);


	var list4 = pool.Get();
	var list5 = pool.Get();
	
	list4.Dump("List-4");
	list4.Dump("List-5");
	
	var list6 = pool.Get();
	
	list6.Dump("List-6"); //This will return a new one
}
