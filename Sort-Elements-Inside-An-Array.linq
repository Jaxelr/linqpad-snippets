<Query Kind="Program" />

/*I had an interview question with this issue */

void Main()
{
	int?[] myArray = new int?[] { null, null, 0, 0, 7};
	
	var result = Minimize(myArray);
	
	result.Dump();
}

//Swap empty elements, do not create another array
public static int?[] Minimize(int?[] input)
{
	int index = 0, counter = 0, ZeroBasedSize = (input.Length-1);

	while (index <= (ZeroBasedSize))
	{
		if (input[index] is null)
		{
			int nextEmpty = index + counter;
			
			while (nextEmpty <= ZeroBasedSize)
			{
				if (input[nextEmpty] is not null)
				{
					counter++;
					input[index] = input[nextEmpty];
					input[nextEmpty] = null;
					break;
				}
				
				nextEmpty++;
			}
		}

		index++;
	}
	
	Array.Resize(ref input, counter);
	return input;
}