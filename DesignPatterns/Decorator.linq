<Query Kind="Program" />

void Main()
{
	var component = new ComponentImplementation();
	var decorator = new DecoratorImplementation();
	
	decorator.SetComponent(component);
	
	decorator.Operation();
}

abstract class Component
{
	public abstract void Operation();
}

abstract class Decorator : Component
{ 
	protected Component component;

	public void SetComponent(Component component)
	{
		this.component = component;
	}
	
	public override void Operation()
	{
		component?.Operation();
	}
}

class ComponentImplementation : Component
{
	public override void Operation()
	{
		"Ran operation on Component".Dump("ComponentImplementation");
	}
}

class DecoratorImplementation : Decorator
{ 
	public override void Operation()
	{
		base.Operation();
		"Ran operation on Decorator".Dump("DecoratorImplementation");
	}
}