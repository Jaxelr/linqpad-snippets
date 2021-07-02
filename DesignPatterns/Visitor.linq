<Query Kind="Program" />

void Main()
{
	var structure = new Structure();
	
	structure.Attach(new ElementA());
	structure.Attach(new ElementB());
	
	var visitorA = new VisitorImplementationA();
	var visitorB = new VisitorImplementationB();
	
	structure.Accept(visitorA);
	structure.Accept(visitorB);
}

public abstract class Visitor
{ 
	public abstract void VisitImplementationA(ElementA element);
	public abstract void VisitImplementationB(ElementB element);
}

public class VisitorImplementationA : Visitor
{
	public override void VisitImplementationA(ElementA element) =>
		$"{element.GetType().Name} visited by {this.GetType().Name}".Dump("Visit Impl A");
	

	public override void VisitImplementationB(ElementB element) =>
		$"{element.GetType().Name} visited by {this.GetType().Name}".Dump("Visit Impl A");
}

public class VisitorImplementationB : Visitor
{
	public override void VisitImplementationA(ElementA element) =>
		$"{element.GetType().Name} visited by {this.GetType().Name}".Dump("Visit Impl B");


	public override void VisitImplementationB(ElementB element) =>
		$"{element.GetType().Name} visited by {this.GetType().Name}".Dump("Visit Impl B");
}

public interface IElement
{ 
	public void Accept(Visitor visitor);
}

public class ElementA : IElement
{
	public void Accept(Visitor visitor) => visitor.VisitImplementationA(this); 
}

public class ElementB : IElement
{ 
	public void Accept(Visitor visitor) => visitor.VisitImplementationB(this);
}

public class Structure
{ 
	List<IElement> elements = new();
	
	public void Attach(IElement element) => elements.Add(element);
	
	public void Detach(IElement element) => elements.Remove(element);
	
	public void Accept(Visitor visitor) => elements.ForEach(e => e.Accept(visitor));
}