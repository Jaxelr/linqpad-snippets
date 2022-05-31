<Query Kind="Program" />

//Sample taken from this article: https://blog.upperdine.dev/the-operation-result-pattern
void Main()
{
	var action = Implementation(1);
	_ = MapAction(action);

	action = Implementation(-1);
	_ = MapAction(action);

	action = Implementation(9);
	_ = MapAction(action);
}

public BaseAction MapAction(BaseAction action) => action switch
{
	InvalidAction i => Handle(i),
	ActionSuccess a => Handle(a),
	_ => throw new Exception("Unhandled")
};


public BaseAction Handle(InvalidAction a)
{
	string Heading = nameof(InvalidAction);
	a.Reason.Dump(Heading);
	a.Success.Dump(Heading);
	
	return a;
}

public BaseAction Handle(ActionSuccess s)
{
	string Heading = nameof(ActionSuccess);
	s.Order.Dump(Heading);
	s.Success.Dump(Heading);
	
	return s;
}

//Could i move this to pattern matching?
public BaseAction Implementation(int value) 
{
	if (value <= 0)
	{
		return new InvalidAction("Value cant be below 1");
	}
	else if (value % 9 == 0)
	{
		return new InvalidAction("I dont like modulos with 9");
	}
	
	return new ActionSuccess(value);
}

public abstract record BaseAction(bool Success);
public abstract record ActionFailure(string Reason) : BaseAction(false);
public record ActionSuccess(int Order) : BaseAction(true);
public record InvalidAction(string Action) : ActionFailure($"Failure: {Action}");
