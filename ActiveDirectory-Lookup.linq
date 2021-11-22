<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.AccountManagement.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <NuGetReference>System.DirectoryServices.AccountManagement</NuGetReference>
  <Namespace>System.DirectoryServices</Namespace>
  <Namespace>System.DirectoryServices.ActiveDirectory</Namespace>
  <Namespace>System.DirectoryServices.AccountManagement</Namespace>
</Query>

void Main()
{
	var result = ActiveDirectoryHelper.GetCurrentDomains();
	
	var df = ActiveDirectoryHelper.GetUserGroups("user1", result);
	var df2 = ActiveDirectoryHelper.GetUserGroups("user2", result);
	
	df.Dump();
	df2.Dump();
}

// Define other methods and classes here
public static class ActiveDirectoryHelper
{
	public static IEnumerable<string> GetCurrentDomains()
	{
		var domains = new List<string>();

		using (var forest = Forest.GetCurrentForest())
		{
			foreach (Domain domain in forest.Domains)
			{
				domains.Add(domain.Name);
				domain.Dispose();
			}
		}

		return domains;
	}

	public static IEnumerable<string> GetUserGroups(string username, IEnumerable<string> domains)
	{
		return domains.AsParallel().Select(x => GetUserGroups(username, x)).SelectMany(y => y);
	}

	public static IEnumerable<string> GetUserGroups(string userName, string domain)
	{
		var groups = new List<string>();

		PrincipalContext cbx = new PrincipalContext(ContextType.Domain, domain);
		UserPrincipal user = UserPrincipal.FindByIdentity(cbx, userName);

		var groupResults = user?.GetGroups() as PrincipalSearchResult<Principal>;

		if (groupResults is PrincipalSearchResult<Principal>)
		{
			foreach (Principal p in groupResults)
			{
				groups.Add(p.Name);
			}
		}

		return groups;
	}

	public static IEnumerable<string> GetDomainGroups(string domainName)
	{
		var groups = new List<string>();

		DirectoryEntry ADAM = default(DirectoryEntry);
		DirectoryEntry GroupEntry = default(DirectoryEntry);
		DirectorySearcher SearchAdam = default(DirectorySearcher);
		SearchResultCollection SearchResults = default(SearchResultCollection);
		var result = new List<string>();

		try
		{
			ADAM = new DirectoryEntry($"LDAP://{domainName}");
			ADAM.RefreshCache();
		}
		catch (Exception e)
		{
			throw e;
		}

		try
		{
			SearchAdam = new DirectorySearcher(ADAM)
			{
				Filter = "(&(objectClass=group))",
				SearchScope = SearchScope.Subtree
			};
			SearchResults = SearchAdam.FindAll();
		}
		catch (Exception e)
		{
			throw e;
		}

		try
		{
			if (SearchResults.Count != 0)
			{
				foreach (SearchResult objResult in SearchResults)
				{
					GroupEntry = objResult.GetDirectoryEntry();
					int a = GroupEntry.Name.Trim().IndexOf("=".ToString());

					groups.Add(GroupEntry.Name.Trim().Substring(a + 1));
				}
			}
			else
			{
			}
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}

		groups.Sort();

		return groups;
	}
}