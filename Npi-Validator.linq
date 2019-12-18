<Query Kind="Program" />

void Main()
{
	//1003347634 - Valid
	//1093791627 - Invalid

	string arr = "1003347634";

	ValidateNpi(arr)
		.Dump();
}

// Define other methods, classes and namespaces here
public bool ValidateNpi(string Npi)
{
	if (!int.TryParse(Npi, out _))
	{
		return false;
	}
	
	return ValidateNpi(Npi.Select(a => a - '0'));
}

public bool ValidateNpi(int Npi)
{
	return ValidateNpi(Npi.ToString());
}

private bool ValidateNpi(IEnumerable<int> Npi)
{
	if (Npi.Count() != 10)
	{
		return false;
	}

	int index = 1, accumulator = 0, checkDigit = Npi.Last();
	foreach (var item in Npi.SkipLast(1))
	{
		if (index % 2 == 1)
		{
			if ((item * 2) >= 10)
			{
				accumulator = accumulator + item * 2 - 10 + 1;
			}
			else
			{
				accumulator = accumulator + item * 2;
			}
		}
		else
		{
			accumulator = accumulator + item;
		}

		index++;
	}
	
	return (Ceil(accumulator + 24) - (accumulator + 24) == checkDigit) ? true : false;

	int Ceil(int a)
	{
		return (int)Math.Ceiling((double)a / 10) * 10;
	}
}