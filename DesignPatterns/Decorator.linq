<Query Kind="Program" />

void Main()
{
	var component = new ComponentImplementation();
	var decorator = new DecoratorImplementation();
	
	decorator.SetComponent(component);
	
	decorator.Operation();
}

public abstract class Component
{
	public abstract void Operation();
}

public abstract class Decorator : Component
{ 
	protected Component component;

	public void SetComponent(Component component) => 
		this.component = component;
	
	public override void Operation() => component?.Operation();
	
}

public class ComponentImplementation : Component
{
	public override void Operation() =>
		"Ran operation on Component".Dump("ComponentImplementation");
}

public class DecoratorImplementation : Decorator
{ 
	public override void Operation()
	{
		base.Operation();
		"Ran operation on Decorator".Dump("DecoratorImplementation");
	}
}