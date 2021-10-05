<Query Kind="Statements">
  <NuGetReference>System.IO.Compression</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

/*
Taken from this article: https://khalidabuhakmeh.com/compress-strings-with-dotnet-and-csharp
*/

var inputString = "Lorem Ipsum";

var compressions = new[]
{
	await inputString.ToGzipAsync(),
	await inputString.ToBrotliAsync()
};

foreach (var result in compressions)
{
	compressions.Dump();
}

// You can define other methods, fields, classes and namespaces here
public static class Compression
{
	public static async Task<CompressionResult> ToGzipAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
	{
		var bytes = Encoding.Unicode.GetBytes(value);
		await using var input = new MemoryStream(bytes);
		await using var output = new MemoryStream();
		await using var stream = new GZipStream(output, level);

		await input.CopyToAsync(stream);

		var result = output.ToArray();

		return new CompressionResult(
			new CompressionValue(value, bytes.Length),
			new CompressionValue(Convert.ToBase64String(result), result.Length),
			level,
			"Gzip");
	}

	public static async Task<CompressionResult> ToBrotliAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
	{
		var bytes = Encoding.Unicode.GetBytes(value);
		await using var input = new MemoryStream(bytes);
		await using var output = new MemoryStream();
		await using var stream = new BrotliStream(output, level);

		await input.CopyToAsync(stream);
		await stream.FlushAsync();

		var result = output.ToArray();

		return new CompressionResult(
			new CompressionValue(value, bytes.Length),
			new CompressionValue(Convert.ToBase64String(result), result.Length),
			level,
			"Brotli"
		);
	}

	public static async Task<string> FromGzipAsync(this string value)
	{
		var bytes = Convert.FromBase64String(value);
		await using var input = new MemoryStream(bytes);
		await using var output = new MemoryStream();
		await using var stream = new GZipStream(input, CompressionMode.Decompress);

		await stream.CopyToAsync(output);
		await stream.FlushAsync();

		return Encoding.Unicode.GetString(output.ToArray());
	}

	public static async Task<string> FromBrotliAsync(this string value)
	{
		var bytes = Convert.FromBase64String(value);
		await using var input = new MemoryStream(bytes);
		await using var output = new MemoryStream();
		await using var stream = new BrotliStream(input, CompressionMode.Decompress);

		await stream.CopyToAsync(output);

		return Encoding.Unicode.GetString(output.ToArray());
	}
}

public record CompressionResult(
	CompressionValue Original,
	CompressionValue Result,
	CompressionLevel Level,
	string Kind
)
{
	public int Difference =>
		Original.Size - Result.Size;

	public decimal Percent =>
	  Math.Abs(Difference / (decimal)Original.Size);
}

public record CompressionValue(
	string Value,
	int Size
);
