<Query Kind="Program">
  <NuGetReference>xunit</NuGetReference>
  <NuGetReference>xunit.runner.utility</NuGetReference>
  <Namespace>Xunit</Namespace>
</Query>

// This query can be #load-ed into other queries for xunit test support. *V=1.5*
// You can modify the code to customize the xunit test runner behavior.

void Main()
{
	RunTests();
	return;
}

#region private::Demo Tests

// Note: Regions prefixed with 'private::' are ignored when the query is #loaded into another query.

[Fact] void Test_ShouldSucceed() => Assert.True (1 + 1 == 2);
[Fact] void Test_ShouldFail()    => Assert.True (1 + 1 == 3);

public class TestClass   // You can also define tests in a (public) class.
{
	[Theory]
	[InlineData (3)]
	[InlineData (5)]
	[InlineData (6)]
	void MyFirstTheory (int value) => Assert.True (IsOdd (value));
}

public static bool IsOdd (int value) => value % 2 == 1;

#endregion

/// <summary>Runs an xunit test with the specified method name and displays the results. This method is editable in the 'xunit' query.</summary>
static TestResultSummary[] RunTest (string methodName, bool quietly = false, bool reportFailuresOnly = false)
	=> RunTests (quietly, reportFailuresOnly, c => c.TestMethod.Method.Name == methodName);

/// <summary>Runs all xunit tests and displays the results. To run a single test, call RunTest() instead. This method is editable in the 'xunit' query.</summary>
static TestResultSummary[] RunTests (bool quietly = false, bool reportFailuresOnly = false, Func<Xunit.Abstractions.ITestCase, bool> filter = null)
{
	using var runner = Xunit.Runners.AssemblyRunner.WithoutAppDomain (typeof (UserQuery).Assembly.Location);
	if (filter != null) runner.TestCaseFilter = filter;


	int totalTests = 0, completedTests = 0, failures = 0;
	runner.OnDiscoveryComplete = info => totalTests = info.TestCasesToRun;

	var tests = new TestResultSummary [0];
	var dc = new DumpContainer (Util.WithHeading (tests, "Test Results"));
	if (!quietly) dc.Dump ("", collapseTo: 1, repeatHeadersAt:0);

	runner.OnTestFailed = info => AddTestResult (info);
	runner.OnTestPassed = info => AddTestResult (info);

	using var done = new ManualResetEventSlim();
	runner.OnExecutionComplete = info =>
	{
		if (!quietly) $"Completed {info.TotalTests} tests in {Math.Round (info.ExecutionTime, 3)}s ({info.TestsFailed} failed)".Dump();
		done.Set();
	};
	runner.Start();
	done.Wait();
	return tests;

	void AddTestResult (Xunit.Runners.TestInfo testInfo)
	{
		var summary = new TestResultSummary (testInfo);
		lock (dc)
		{
			completedTests++;
			if (summary.Failed()) failures++;
			
			if (!reportFailuresOnly || summary.Failed())
				tests = tests
					.Append (summary)
					.OrderBy (t => t.Succeeded())
					.ThenBy (t => t.TypeName)
					.ThenBy (t => t.MethodName)
					.ThenBy (t => t.Case)
					.ToArray();

			dc.Content = Util.WithHeading (tests, $"Test Results - {completedTests} of {totalTests} ({failures} failures)");
		}
	}
}

class TestResultSummary
{
	Xunit.Runners.TestInfo _testInfo;
	public TestResultSummary (Xunit.Runners.TestInfo testInfo) => _testInfo = testInfo;

	public bool Succeeded() => _testInfo is Xunit.Runners.TestPassedInfo;
	public bool Failed() => _testInfo is Xunit.Runners.TestFailedInfo;

	public string TypeName => _testInfo.TypeName;
	public string MethodName => _testInfo.MethodName;
	
	public string Case => _testInfo.TestDisplayName.Substring (
		_testInfo.TestDisplayName.StartsWith (TypeName + "." + MethodName) ? TypeName.Length + 1 + MethodName.Length : 0);
		
	public decimal? Seconds => (_testInfo as Xunit.Runners.TestExecutedInfo)?.ExecutionTime;

	public object Status =>
		_testInfo is Xunit.Runners.TestPassedInfo ? Util.WithStyle ("Succeeded", "color:green") :
		_testInfo is Xunit.Runners.TestFailedInfo ? Util.WithStyle ("Failed", "color:red") :
		"";

	public Xunit.Runners.TestFailedInfo FailureInfo => _testInfo as Xunit.Runners.TestFailedInfo;

	public object Location => Util.VerticalRun (
		from match in Regex.Matches (FailureInfo?.ExceptionStackTrace ?? "", @"(at .+?)\s+in\s+.+?LINQPadQuery:line\s+(\d+)")
		let line = int.Parse (match.Groups [2].Value)
		select Util.HorizontalRun (true, match.Groups [1].Value, new Hyperlinq (line - 1, 0, $"line {line}")));
}