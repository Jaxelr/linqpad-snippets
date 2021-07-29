<Query Kind="Program" />

void Main()
{
	var redblack = new RedBlack<string>();
	
	redblack.Add(new CustomKey(1), "Uno");
	redblack.Add(new CustomKey(2), "Dos");
	redblack.Add(new CustomKey(3), "Tres");

}

public class CustomKey : IComparable
{
	public int Key { get; set; }
	
	public CustomKey(int key)
	{
		Key = key;
	}

	public int CompareTo(object obj)
	{
		if (obj is CustomKey customKey)
		{
			if (customKey.Key < Key)
				return -1;
			else if (customKey.Key > Key)
				return 1;
			else
				return 0;
		}
		
		throw new InvalidCastException("Invalid data type to compare to");
	}

	public override string ToString() => Key.ToString();
}

public class RedBlack<T>
{ 
	private int count;
	private int hashCode;
	private string identifier;
	private Random random = new Random();
	public static RedBlackNode<T> Sentinel;
	private RedBlackNode<T>	tree;
	private RedBlackNode<T>	lastNode;
	
	public RedBlack()
	{
		identifier = string.Concat(base.ToString(), random.Next());
		hashCode = random.Next();
		
		Sentinel = new RedBlackNode<T>();
		Sentinel.Left = new RedBlackNode<T>();
		Sentinel.Right = new RedBlackNode<T>();
		Sentinel.Parent = new RedBlackNode<T>();
		Sentinel.Color = RedBlackNode<T>.Black;
		tree = Sentinel;
		lastNode = Sentinel;
	}
	
	public RedBlack(string identifier)
	{
		hashCode = random.Next();
		this.identifier = identifier;
	}
	
	public void Add(IComparable key, T data)
	{
		if (key is null || data is null)
			throw new RedBlackException("Key and data must not be null");
			
		int result = 0;
		var node = new RedBlackNode<T>();
		var temp = tree;

		while (temp != Sentinel)
		{
			node.Parent = temp;
			result = key.CompareTo(temp.Key);
			
			if (result == 0)
				throw new RedBlackException("A node with the same key exists");
			
			if (result > 0)
				temp = temp.Right;
			else
				temp = temp.Left;
		}
		
		node.Key = key;
		node.Data = data;
		node.Left = Sentinel;
		node.Right = Sentinel;

		if (node.Parent is not null)
		{
			result = node.Key.CompareTo(node.Parent.Key);
			
			if (result > 0)
				node.Parent.Right = node;
			else
				node.Parent.Left = node;
		}
		else
			tree = node;
			
		RestoreAfterInsert(node);
		lastNode = node;
		
		count++;
	}
	
	private void RestoreAfterInsert(RedBlackNode<T> node)
	{
		RedBlackNode<T> t;
		const int Black = RedBlackNode<T>.Black;
		const int Red = RedBlackNode<T>.Red;

		while (node != tree && node.Parent.Color == Red)
		{
			if (node.Parent == node.Parent.Parent.Left)
			{
				t = node.Parent.Parent.Right;

				if (t?.Color == Red)
				{
					node.Parent.Color = Black;
					t.Color = Black;
					
					node.Parent.Parent.Color = Red;
					node = node.Parent.Parent;
				}
				else
				{
					if (node == node.Parent.Right)
					{
						node = node.Parent;
						RotateLeft(node);
					}
					
					node.Parent.Color = Black;
					node.Parent.Parent.Color = Red;
					RotateRight(node);
				}
			}
			else
			{
				t = node.Parent.Parent.Left;

				if (t?.Color == Red)
				{
					node.Parent.Color = Black;
					t.Color = Black;
					node.Parent.Parent.Color = Red;
					node = node.Parent.Parent;
				}
				else
				{
					if (node == node.Parent.Left)
					{
						node = node.Parent;
						RotateRight(node);
					}
					
					node.Parent.Color = Black;
					node.Parent.Parent.Color = Red;
					RotateLeft(node);
				}
			}
		}
		
		tree.Color =  RedBlackNode<T>.Black;
	}
	
	public void RotateLeft(RedBlackNode<T> node)
	{
		var temp = node.Right;
		
		node.Right = temp.Left;
		
		if (temp.Left != Sentinel)
			temp.Left.Parent = node;

		if (node.Parent is not null)
		{
			if (node == node.Parent.Left)
				node.Parent.Left = temp;
			else
				node.Parent.Right = temp;
				
		}
		else
		{
			tree = temp;
		}
		
		temp.Left = node; 
		if (node != Sentinel)
			node.Parent = temp;
	}

	public void RotateRight(RedBlackNode<T> node)
	{
		RedBlackNode<T> temp = node.Left;
		
		node.Left = temp.Right;
		
		if (temp.Right != Sentinel)
			temp.Right.Parent = node;
			
		if(temp != Sentinel)
			temp.Parent = node.Parent;

		if (node.Parent is not null)
		{
			if (node == node.Parent.Right)
				node.Parent.Right = temp;
			else
				node.Parent.Left = temp;
		}
		else
			tree = temp;
			
		temp.Right = node;
		if(node != Sentinel)
			node.Parent = temp;
	}

	public T GetData(IComparable key)
	{
		int result;
		
		var treeNode = tree;
		
		while (tree != Sentinel)
		{
			result = key.CompareTo(treeNode.Key);

			if (result == 0)
			{
				lastNode = treeNode;
				return treeNode.Data;
			}
			
			if (result < 0)
				treeNode = treeNode.Left;
			else
				treeNode = treeNode.Right;
		}
		
		throw new RedBlackException("Red black node key was not found.");
	}

	public IComparable GetMinKey()
	{
		var treeNode = tree;
		
		if (treeNode is null || treeNode == Sentinel)
			throw new RedBlackException("Red black tree is empty");
			
		while (treeNode.Left != Sentinel)
			treeNode = treeNode.Left;
			
		lastNode = treeNode;
		
		return treeNode.Key;
	}

	public IComparable GetMaxKey()
	{
		var treeNode = tree;

		if (treeNode is null || treeNode == Sentinel)
			throw new RedBlackException("Red black tree is empty");

		while (treeNode.Right != Sentinel)
			treeNode = treeNode.Right;

		lastNode = treeNode;

		return treeNode.Key;
	}

	public void Remove(IComparable key)
	{
		if (key == null)
			throw new RedBlackException("Red black node key is null");
			
		int result;
		RedBlackNode<T> node;
		
		result = key.CompareTo(lastNode.Key);
		
		if (result == 0 )
			node = lastNode;
		else
		{
			node = tree;
			
			while (node != Sentinel)
			{
				result = key.CompareTo(lastNode.Key);
				
				if (result == 0)
					break;
				if (result < 0)
					node = node.Left;
				else 
					node = node.Right;
			}
			
			if (node == Sentinel)
				return;
		}
		
		Delete(node);
		
		count--;
	}
	
	private void Delete(RedBlackNode<T> toDelete)
	{
		var replacement = new RedBlackNode<T>();
		var work = new RedBlackNode<T>();
		
		if (toDelete.Left == Sentinel || toDelete.Right == Sentinel)
			work = toDelete;
		else
		{
			work = toDelete.Right;
			
			while(work.Left != Sentinel)
				work = work.Left;
		}
		
		if (work.Left != Sentinel)
			replacement = work.Left;
		else
			replacement = work.Right;
		
		replacement.Parent = work.Parent;

		if (work is not null)
			if (work == work.Parent.Left)
				work.Parent.Left = replacement;
			else
				work.Parent.Right = replacement;
		else
			tree = replacement;

		if (work != toDelete)
		{
			toDelete.Key = work.Key;
			toDelete.Data = work.Data;
		}
		
		if (work.Color == RedBlackNode<T>.Black)
			//RestoreAfterDelete
		
		lastNode = Sentinel;
	}
	
	private void RestoreAfterDelete(RedBlackNode<T> node)
	{
		RedBlackNode<T> work;
		const int Red = RedBlackNode<T>.Red;
		const int Black = RedBlackNode<T>.Black;

		while (node != tree && node.Color == Black)
		{
			if (node == node.Parent.Left)
			{
				work = node.Parent.Right;

				if (work.Color == Red)
				{
					work.Color = Black;
					node.Parent.Color = Red;
					RotateLeft(node.Parent);
					work = node.Parent.Right;
				}

				if (work.Left.Color == Black && work.Right.Color == Black)
				{
					work.Color = Red;
					node = node.Parent;
				}
				else
				{
					if (work.Right.Color == Black)
					{
						work.Left.Color = Black;
						work.Color = Red;
						RotateRight(work);
						work = node.Parent.Right;
					}

					work.Color = node.Parent.Color;
					node.Parent.Color = Black;
					work.Right.Color = Black;
					RotateLeft(node.Parent);
					node = tree;
				}
			}
			else
			{
				work = node.Parent.Left;

				if (work.Color == Red)
				{
					work.Color = Black;
					node.Parent.Color = Red;
					RotateRight(node.Parent);
					work = node.Parent.Left;
				}

				if (work.Right.Color == Black && work.Left.Color == Black)
				{
					work.Color = Red;
					node = node.Parent;
				}
				else
				{
					if (work.Left.Color == Black)
					{
						work.Right.Color = Black;
						work.Color = Red;
						RotateLeft(work);
						work = node.Parent.Left;
					}
					
					work.Color = node.Parent.Color;
					node.Parent.Color = Black;
					work.Left.Color = Black;
					RotateRight(node.Parent);
					node = tree;
				}
			}
		}

		node.Color = Black;
	}
	
	public T GetMinValue() => GetData(GetMinKey());
	
	public T GetMaxValue() => GetData(GetMaxKey());
	
	public RedBlackEnumerator<T> Keys() => Keys(true);
	
	public RedBlackEnumerator<T> Keys(bool ascending) => new RedBlackEnumerator<T>(tree, true, true);
	
	public RedBlackEnumerator<T> Values() => Elements(true);
	
	public RedBlackEnumerator<T> Elements() => Elements(true);
	
	public RedBlackEnumerator<T> Elements(bool ascending) => new RedBlackEnumerator<T>(tree, false, ascending);

	public bool IsEmpty() => (tree is null);
	
	public void Clear()
	{
		tree = Sentinel;
		count = 0;
	}
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