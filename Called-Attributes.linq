<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

/* Taken from this article https://khalidabuhakmeh.com/get-csharp-metadata-from-a-callsite */
void Main()
{
	/* Get the filepath of the calling code */
	FilePath();

	/* Get the caller of the method/function */
	WhoThere();

	/* Get the called line number */
	WhereYouAt();

	/* Get the called expression */
	var math = 2 * 2;
	ExpressYourself(math);

	ExpressYourself(2 + 2);

}

static void FilePath([CallerFilePath] string filepath = "")
{
	filepath.Dump("Caller filepath");
}

static void WhoThere([CallerMemberName] string name = "")
{
	name.Dump("Caller name");
}

static void WhereYouAt([CallerLineNumber] int lineNumber = 0)
{
	lineNumber.Dump("Line Number");
}

static void ExpressYourself(
	int result,
	[CallerArgumentExpression("result")] string expression = "")
{
	$"{expression} is {result}".Dump("Expression Result");
}