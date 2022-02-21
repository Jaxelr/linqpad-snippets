<Query Kind="Program" />

void Main()
{
	//Valid "358981879372208"
	//Invalid "358981879372999"
	ValidateIMEINumber("358981879372208").Dump();
	ValidateIMEINumber("358981879372999").Dump();
}

public bool ValidateIMEINumber(string Imei)
{
	if (!long.TryParse(Imei, out _)) /* IMEIs are longer than int.MaxValue */
	{
		return false;
	}
	
	return CheckLuhnNumber(Imei.Select(x => x - '0'));
}

private static bool CheckLuhnNumber(IEnumerable<int> number)
{
	int iDigit = 0;
	int iSum = 0;

	bool bIsOdd = false;
	
	foreach(int item in number)
	{
		iDigit = item;
		if (bIsOdd == true)
			iDigit *= 2;

		iSum += iDigit / 10;
		iSum += iDigit % 10;

		bIsOdd = !bIsOdd;
	}

	return (iSum % 10 == 0);
}