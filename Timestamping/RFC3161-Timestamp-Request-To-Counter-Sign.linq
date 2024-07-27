<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Security.Cryptography.Pkcs</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var httpClient = new HttpClient() { BaseAddress = new("http://timestamp=-endpoint.com") };
	var digest = SHA256.HashData(GetRandomByteArray(32)); //sha 256 bits = 32 bytes, subs your digest here.

	var nonce = GetRandomByteArray(20); //Make sure to send a random Nonce to avoid replay attacks

	var req = Rfc3161TimestampRequest.CreateFromData(digest, HashAlgorithmName.SHA256, null, nonce, true);

	var content = new ByteArrayContent(req.Encode());

	content.Headers.Add("Content-Type", "application/timestamp-query");

	var response = await httpClient.PostAsync("/", content);

	response.Dump();
}


private byte[] GetRandomByteArray(int length)
{
	var _rng = RandomNumberGenerator.Create();
	var nonce = new byte[length];
	_rng.GetBytes(nonce);
	return nonce;
}