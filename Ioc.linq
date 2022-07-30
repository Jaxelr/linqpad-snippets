<Query Kind="Program" />

/// <summary>
/// This is a thought experiment into the 
/// insides of a simpler IoC Container
/// </summary>
void Main()
{
	string input = "Input Data";
	
	ContainerA.Register<ITest, Test>();
	var test = ContainerA.Resolve<ITest>();

	test.Method(input);
	
	var containerB = new ContainerB();
	containerB.Register<ITest>(() => new Test());
	
	var instance = (Test)containerB.GetInstance<ITest>();
	instance.Method(input);
	
	var containerC = new ContainerC();
	containerC.Register<ITest>(delegate { return new Test(); });
	
	var result = containerC.Create<ITest>();
	result.Method(input);
}

public interface ITest
{ 
	void Method(string Input);
}

public class Test : ITest
{
	public void Method(string Input) => Input.Dump("Test");
}

//Raw IoC: Definitely not for production use
public static class ContainerA
{
	private static readonly Dictionary<Type, Type> types = new();
	private static readonly Dictionary<Type, object> typeInstances = new();
	
	public static void Register<TContract, TImplementation>()
	{
		types[typeof(TContract)] = typeof(TImplementation);
	}
	
	public static void Register<TContract, TImplementation>(TImplementation instance) 
		=> typeInstances[typeof(TContract)] = instance;
	
	public static T Resolve<T>() => (T)Resolve(typeof(T));
	
	public static object Resolve(Type contract)
	{
		if (typeInstances.ContainsKey(contract))
		{
			return typeInstances[contract];
		}

		Type implementation = types[contract];
		ConstructorInfo constructor = implementation.GetConstructors()[0];
		
		ParameterInfo[] constructorParameters = constructor.GetParameters();
		
		if (constructorParameters.Length == 0)
		{
			return Activator.CreateInstance(implementation);
		}
		
		List<object> parameters = new(constructorParameters.Length);
		foreach (ParameterInfo parameterInfo in constructorParameters)
		{
			parameters.Add(Resolve(parameterInfo.ParameterType));
		}
		
		return constructor.Invoke(parameters.ToArray());
	}
}

// Taken from this SO post 
// https://stackoverflow.com/questions/15715978/simple-dependency-resolver/15717047#15717047
public class ContainerB
{
	private readonly Dictionary<Type, Func<object>> regs = new();

	public void Register<TService, TImpl>() where TImpl : TService =>
		regs.Add(typeof(TService), () => this.GetInstance(typeof(TImpl)));

	public void Register<TService>(Func<TService> factory) =>
		regs.Add(typeof(TService), () => factory());

	public void RegisterInstance<TService>(TService instance) =>
		regs.Add(typeof(TService), () => instance);

	public void RegisterSingleton<TService>(Func<TService> factory)
	{
		var lazy = new Lazy<TService>(factory);
		Register(() => lazy.Value);
	}
	
	public object GetInstance<T>() => this.GetInstance(typeof(T));

	public object GetInstance(Type type)
	{
		if (regs.TryGetValue(type, out Func<object> fac)) return fac();
		else if (!type.IsAbstract) return this.CreateInstance(type);
		throw new InvalidOperationException("No registration for " + type);
	}
	
	private object CreateInstance(Type implementationType)
	{
		var ctor = implementationType.GetConstructors().Single();
		var paramTypes = ctor.GetParameters().Select(p => p.ParameterType);
		var dependencies = paramTypes.Select(GetInstance).ToArray();
		return Activator.CreateInstance(implementationType, dependencies);
	}
}

// Taken from this Ayende post
// https://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code
public class ContainerC
{
	public delegate object Creator(ContainerC container);

	private readonly Dictionary<string, object> configuration
				   = new Dictionary<string, object>();
	private readonly Dictionary<Type, Creator> typeToCreator
				   = new Dictionary<Type, Creator>();

	public Dictionary<string, object> Configuration
	{
		get { return configuration; }
	}

	public void Register<T>(Creator creator)
	{
		typeToCreator.Add(typeof(T), creator);
	}

	public T Create<T>()
	{
		return (T)typeToCreator[typeof(T)](this);
	}

	public T GetConfiguration<T>(string name)
	{
		return (T)configuration[name];
	}
}