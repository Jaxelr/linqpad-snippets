<Query Kind="Program">
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>

void Main()
{
	//Find root certificate - multiple API surfaces
	string chain = "{base64stringgoeshere}";
	
	var result = GetRootCert(chain);
	
	var resultingCert = new X509Certificate2(result);
	
	resultingCert.Dump("From byte array");
	
	var collection = new X509Certificate2Collection();
	
	collection.Import(Convert.FromBase64String(chain));
	
	var result2 = GetRootCert(collection);
	
	result2.Dump("cert object");
}

public byte[] GetRootCert(string base64InputString) => GetRootCert(Convert.FromBase64String(base64InputString));

public byte[] GetRootCert(byte[] chain)
{
	ArgumentNullException.ThrowIfNull(chain);
	
	var collection = new X509Certificate2Collection();
	
	collection.Import(chain);
	
	var result = GetRootCert(collection);

	return result.Export(X509ContentType.Cert);
}

public X509Certificate2 GetRootCert(X509CertificateCollection collection)
{
	ArgumentNullException.ThrowIfNull(collection);

	foreach (var cert in collection)
	{
		if (string.Equals(cert.Subject, cert.Issuer, StringComparison.OrdinalIgnoreCase))
			return new X509Certificate2(cert);
	}

	throw new Exception("No root certificate found on the chain");
}