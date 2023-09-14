<Query Kind="Program">
  <NuGetReference>System.Security.Cryptography.Pkcs</NuGetReference>
  <Namespace>System.Security.Cryptography.Pkcs</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	string body = "MEQCAQEwMTANBglghkgBZQMEAgEFAAQgTcoP1fQkoxsDq4B8uud+syvy0Inu0c7hVLOv7UWN4NwCCQDN6eIoEO5w1wEB/w==";

	if (Rfc3161TimestampRequest.TryDecode(Convert.FromBase64String(body), out var request, out int bytesConsumed))
	{
		if (request.HashAlgorithmId.Value is null)
		{
			throw new Exception("Invalid algorithm");
		}
		
		var digest = request.GetMessageHash();
		var hash = HashAlgorithmName.FromOid(request?.HashAlgorithmId?.Value!);
		
		digest.Dump();
		hash.Dump();
	}
}

