<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.Security.KeyVault.Certificates</NuGetReference>
  <Namespace>Azure</Namespace>
  <Namespace>Azure.Core</Namespace>
  <Namespace>Azure.Core.Cryptography</Namespace>
  <Namespace>Azure.Core.Diagnostics</Namespace>
  <Namespace>Azure.Core.Extensions</Namespace>
  <Namespace>Azure.Core.GeoJson</Namespace>
  <Namespace>Azure.Core.Pipeline</Namespace>
  <Namespace>Azure.Core.Serialization</Namespace>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Azure.Messaging</Namespace>
  <Namespace>Azure.Security.KeyVault.Certificates</Namespace>
  <Namespace>Microsoft.Extensions.Azure</Namespace>
  <Namespace>Microsoft.Identity.Client</Namespace>
  <Namespace>Microsoft.Identity.Client.Advanced</Namespace>
  <Namespace>Microsoft.Identity.Client.AppConfig</Namespace>
  <Namespace>Microsoft.Identity.Client.AuthScheme.PoP</Namespace>
  <Namespace>Microsoft.Identity.Client.Cache</Namespace>
  <Namespace>Microsoft.Identity.Client.Extensibility</Namespace>
  <Namespace>Microsoft.Identity.Client.Extensions.Msal</Namespace>
  <Namespace>Microsoft.Identity.Client.Kerberos</Namespace>
  <Namespace>Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos</Namespace>
  <Namespace>Microsoft.Identity.Client.Platforms.Features.WinFormsLegacyWebUi</Namespace>
  <Namespace>Microsoft.Identity.Client.SSHCertificates</Namespace>
  <Namespace>Microsoft.Identity.Client.Utils.Windows</Namespace>
  <Namespace>Microsoft.Web.WebView2.Core</Namespace>
  <Namespace>Microsoft.Web.WebView2.Core.Raw</Namespace>
  <Namespace>Microsoft.Web.WebView2.WinForms</Namespace>
  <Namespace>Microsoft.Web.WebView2.Wpf</Namespace>
</Query>

void Main()
{
	var options = new CertificateClientOptions()
	{
		Retry =
							{
								Delay= TimeSpan.FromSeconds(1),
								MaxDelay = TimeSpan.FromSeconds(8),
								MaxRetries = 5,
								Mode = RetryMode.Exponential
							}
	};

	var client = new CertificateClient(new Uri("https://myvault.vault.azure.net"), new DefaultAzureCredential(), options);
	
	var resp = client.DownloadCertificate("my-cert");

	resp.Dump();
}

// You can define other methods, fields, classes and namespaces here