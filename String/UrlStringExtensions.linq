<Query Kind="Program" />

void Main()
{
	string url1 = "https://example.com/posts/42";
	
	url1.NormalizeUrl().Dump();
}

public static class StringExtensions
{
	public static string NormalizeUrl(this string url)
	{
		// If empty return empty string
		if (string.IsNullOrWhiteSpace(url)) return string.Empty;

		// If url not a valid Uri return empty string
		if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri)) return string.Empty;

		// Remove any trailing slash and remove content after any ?
		string result = url.Split(new[] { '?' })[0].TrimEnd(new[] { '/' });

		// Now replace any parts of the URL which is a number or guid with 0
		return string
			.Join("/", result
				.Split('/')
				.Select(part => int.TryParse(part, out _) ? "0" : part)
				.Select(part => Guid.TryParse(part, out _) ? "0" : part));
	}
}