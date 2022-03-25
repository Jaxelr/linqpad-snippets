<Query Kind="Program" />

void Main()
{
	var cipher = new RailFenceCipher(3);

	string result = cipher.Encode("Testing");
	
	result.Dump("Ciphered text");
	
	string original = cipher.Decode(result);
	
	original.Dump("Unciphered text");
}

public class RailFenceCipher
{
	internal readonly int _rails;
	public RailFenceCipher(int rails) => _rails = --rails;
	
	internal int GetRow(int i) => _rails - Math.Abs(i % (2 * _rails) - _rails);

	internal IEnumerable<T> ZigZagIndex<T>(IEnumerable<T> seq) => seq
		.Select((c, i) => (c, r: GetRow(i)))
		.GroupBy(v => v.r)
		.SelectMany(v => v.Select(vc => vc.c));

	public string Encode(string input) => string.Join("", ZigZagIndex(input));

	public string Decode(string input) => 
		ZigZagIndex(Enumerable.Range(0, input.Length))
		.Zip(input, (f, s) => (i: f, c: s))
		.OrderBy(v => v.i)
		.Aggregate(new StringBuilder(), (sb, v) => sb.Append(v.c), sb => sb.ToString());
}