<Query Kind="Program">
  <NuGetReference>Microsoft.EntityFrameworkCore</NuGetReference>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var list = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
	
	var paginated = new PaginatedList<int>(list, 10, 2, 2);
	
	paginated.HasPreviousPage.Dump();
	paginated.HasNextPage.Dump();
	paginated.TotalPages.Dump();
}

public class PaginatedList<T> : List<T>
{
	public int PageIndex { get; private set; }
	public int TotalPages { get; private set; }

	public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
	{
		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);

		this.AddRange(items);
	}

	public bool HasPreviousPage
	{
		get
		{
			return (PageIndex > 1);
		}
	}

	public bool HasNextPage
	{
		get
		{
			return (PageIndex < TotalPages);
		}
	}

	public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
	{
		var count = await source.CountAsync();
		var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}
