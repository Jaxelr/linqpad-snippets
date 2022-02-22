<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	ValidateIMEINumber(Valid).Dump();
	ValidateIMEINumber(Invalid).Dump();
	
	RunTests();
}

const string Valid = "358981879372208";
const string Invalid = "358981879372999";

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

#region private::Tests

[Fact] void Valid_Imei() => Assert.True(ValidateIMEINumber(Valid));
[Fact] void Invalid_Imei() => Assert.False(ValidateIMEINumber(Invalid));

#endregion