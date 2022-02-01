<Query Kind="Program" />

void Main()
{
	var root = new TreeNode<int>() { Value = 1};

	root.Left = new TreeNode<int>() {Value = 2 };
	root.Right = new TreeNode<int>() {Value = 2  };
	
	IsSymmetric(root).Dump("Is it?");

	root = new TreeNode<int>() { Value = 1 };

	root.Left = new TreeNode<int>() { Value = 4 };
	root.Right = new TreeNode<int>() { Value = 3 };

	IsSymmetric(root).Dump("Is it not?");
}

public class TreeNode<T>
{ 
	public T Value { get; set; }
	public TreeNode<T> Left { get; set; }
	public TreeNode<T> Right { get; set; }
}

public bool IsSymmetric<T>(TreeNode<T> tree)
{
	if (tree is null)
		return false;
		
	return IsMirror(tree.Left, tree.Right);
}

public bool IsMirror<T>(TreeNode<T> left, TreeNode<T> right)
{
	if (left is null && right is null)
		return true;
		
	if (left is null || right is null)
		return false;
		
	return left.Value.Equals(right.Value) && 
	IsMirror(left.Left, right.Right) && 
	IsMirror(left.Right, right.Left);
}
	

