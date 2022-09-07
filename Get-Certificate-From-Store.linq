<Query Kind="Expression">
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>

public X509Certificate2? GetCertificateFromStore(string subjectName)
{
	using X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
	
	store.Open(OpenFlags.ReadOnly);
	var certCollection = store.Certificates;
	var currentCerts = certCollection.Find(X509FindType.FindBySubjectDistinguishedName, subjectName, true);
	return GetLatestCert(currentCerts);
	
}

private X509Certificate2? GetLatestCert(X509Certificate2Collection certCollection)
{
	X509Certificate2? latestCert = null; //return null if theres nothing on the collection

	foreach (var cert in certCollection)
	{
		if (cert.HasPrivateKey && cert.NotAfter > DateTime.UtcNow)
		{
			if (latestCert is null || latestCert.NotAfter < cert.NotAfter)
			{
				latestCert = cert;
			}
		}
	}
	
	return latestCert;
}