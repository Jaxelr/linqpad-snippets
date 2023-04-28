<Query Kind="Program" />

void Main()
{
	//Order of values is paramount
	
	//Positive percentage gain
	PercentageGain(150, 1000).Dump();
	
	//Negative percentage lossed
	PercentageGain(150, 50).Dump();
}

public decimal PercentageGain(decimal oldValue, decimal newValue) => Math.Round((newValue - oldValue) / oldValue * 100, 2, MidpointRounding.AwayFromZero);