<Query Kind="Program">
  <NuGetReference>Fizzler.Systems.HtmlAgilityPack</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>Fizzler.Systems.HtmlAgilityPack</Namespace>
</Query>

void Main()
{
	var html = new HtmlDocument();
	html.LoadHtml(@"
  <html>
      <head></head>
      <body>
        <div>
          <p class='content'>Fizzler</p>
          <p>CSS Selector Engine</p></div>
      </body>
  </html>");

	var document = html.DocumentNode;

	var content = document.QuerySelectorAll(".content");
	
	content.Select(x => x.InnerText).Dump("Query Selector class content");
	
	content = document.QuerySelectorAll("p");
	
	content.Select(x => x.InnerText).Dump("Query Selector p");

	document.QuerySelectorAll("body>p");

	document.QuerySelectorAll("body p");

	document.QuerySelectorAll("p:first-child");
}
