<Query Kind="Expression" />

//Question taken from this article https://behdadesfahbod.medium.com/on-a-great-interview-question-aea168279942

// Write a function that given a string, determines if it’s a concatenation of two dictionary words
public static bool IsConcatenationOfTwoDictionaryWords(string s)
{
	//This data magically appears from somewhere, thats not the point...
	var words = File.ReadAllLines("/usr/share/dict/words").ToHashSet();
	for (int i = 1; i < s.Length; i++)
	{
		if (words.Contains(s.Substring(0, i)) && words.Contains(s.Substring(i)))
		{
			return true;
		}
	}
	return false;
}

// Write a function that given a string, determines if it’s a concatenation of a number of dictionary words.
public static bool IsConcatenationOfDictionaryWords(string s)
{
	//This data magically appears from somewhere, thats not the point...
	var words = File.ReadAllLines("/usr/share/dict/words").ToHashSet();
	bool[] dp = new bool[s.Length + 1];
	dp[0] = true;
	for (int i = 1; i <= s.Length; i++)
	{
		for (int j = 0; j < i; j++)
		{
			if (dp[j] && words.Contains(s.Substring(j, i - j)))
			{
				dp[i] = true;
				break;
			}
		}
	}
	return dp[s.Length];
}