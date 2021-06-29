<Query Kind="Program" />

void Main()
{
	var control = new Control();

	var real = new RealBuilder();
	var realer = new RealerBuilder();

	//Real build
	control.BuildUp(real);
	var product = real.GetProduct();
	product.ShowProduct();

	//Realer build
	control.BuildUp(realer);
	product = realer.GetProduct();
	product.ShowProduct();
}

public class Product
{ 
	private Parts parts = new();
	
	public void Add(string part) => parts.Add(part);
	
	public void ShowProduct() => parts.ForEach(x => x.Dump("Product Parts"));
}

public class Parts : List<string>
{
}

public class Control
{ 
	public void BuildUp(Builder builder)
	{
		builder.BuildOne();
		builder.BuildTwo();
	}
}

public abstract class Builder
{ 
	public abstract void BuildOne();	
	public abstract void BuildTwo();
	public abstract Product GetProduct();
}

public class RealBuilder : Builder
{
	private Product product = new Product();
	
	public override void BuildOne() => product.Add("RealBuilder-BuildOne");
	
	public override void BuildTwo() => product.Add("RealBuilder-BuildTwo");
	
	public override Product GetProduct() => product;

}

public class RealerBuilder : Builder
{
	private Product product = new Product();

	public override void BuildOne() => product.Add("RealerBuilder-BuildOne");

	public override void BuildTwo() => product.Add("RealerBuilder-BuildTwo");

	public override Product GetProduct() => product;

}