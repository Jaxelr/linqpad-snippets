<Query Kind="Program">
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>

void Main()
{
	using X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
	
	string subject = "CN=test.example.com";

	store.Open(OpenFlags.ReadOnly);
	var cert = store.Certificates.Where
	(x => 
		x.HasPrivateKey &&
		x.NotAfter >= DateTime.UtcNow && 
		x.Subject == subject
	);

	cert.Dump();
}

// You can define other methods, fields, classes and namespaces here