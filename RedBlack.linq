<Query Kind="Program" />

void Main()
{
	
}

public class RedBlack<T>
{ 
	public static RedBlackNode<T> Sentinel;
}

public class RedBlackEnumerator<T>
{ 
	private Stack<RedBlackNode<T>> stack = new Stack<RedBlackNode<T>>();
	private bool keys;
	private bool ascending;
	
	public IComparable Key { get; set; }
	public IComparable ParentKey;
	public string Color;
	public T Value { get; set; }

	public RedBlackEnumerator(RedBlackNode<T> node, bool keys, bool ascending)
	{
		this.keys = keys;
		this.ascending = ascending;
		
		//Missing a while here
		while(node != RedBlack<T>.Sentinel)
		{
			stack.Push(node);
			node = ascending ? node.Left : node.Right;
		}
	}
	
	public bool HasMoreElements() => stack.Count > 0;

	public T NextElement()
	{
		var sentinel = RedBlack<T>.Sentinel;
		
		if (stack.Count == 0)
		{
			throw new RedBlackException("Element not found");
		}
		
		var node = stack.Peek();

		if (ascending)
		{
			if (node.Right == sentinel)
			{
				var tn = stack.Pop();

				while (HasMoreElements() && stack.Peek().Right == tn)
					tn = stack.Pop();
			}
			else
			{
				var tn = node.Right;

				while (tn != sentinel)
				{
					stack.Push(tn);
					tn = tn.Left;
				}
			}
		}
		else
		{
			if (node.Left == sentinel)
			{
				var tn = stack.Pop();
				while (HasMoreElements() && stack.Peek().Left == tn)
					tn = stack.Pop();
			}
			else
			{
				var tn = node.Left;
				while (tn != sentinel)
				{
					stack.Push(tn);
					tn = tn.Right;
				}
			}
		}
		
		Key = node.Key;
		Value = node.Data;

		try
		{
			ParentKey = node.Parent.Key;
		}
		catch (Exception e)
		{
			ParentKey = 0;
		}
		
		Color = node.Color == 0 ? "Red" : "Black";
		
		return node.Data;
	}

	public bool MoveNext()
	{
		if (HasMoreElements())
		{
			NextElement();
			return true;
		}
		
		return false;
	}
}

public class RedBlackNode<T>
{ 
	public const int Red = 0;
	public const int Black = 1;
	
	public IComparable Key { get; set; }
	public T Data { get; set; }
	public int Color { get; set; }
	
	public RedBlackNode<T> Left { get; set; }
	public RedBlackNode<T> Right { get; set; }
	public RedBlackNode<T> Parent { get; set; }
	
	public RedBlackNode()
	{
		Color = Red;
	}
}

class RedBlackException : Exception
{ 
	public RedBlackException()
	{
		
	}
	
	public RedBlackException(string message) : base(message)
	{
		
	}
}