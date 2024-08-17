<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>

void Main()
{
	var generator = new CertificateGenerator();
	var result = generator.GenerateCertificateChain();
}

public class CertificateGenerator
{
    public X509Certificate2 GenerateCertificate(string subjectName, X509Certificate2? issuer, RSA? issuerKey)
    {
        using (RSA rsa = RSA.Create(2048))
        {
            var request = new CertificateRequest(
                new X500DistinguishedName(subjectName),
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            // Add Basic Constraints extension
            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(issuer == null, issuer != null, 0, true));

            // Add Key Usage extension
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.DigitalSignature, true));

            if (issuer == null)
            {
                // Self-signed certificate
                return request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
            }
            else
            {
                // Certificate signed by issuer
                return request.Create(issuer, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1), Guid.NewGuid().ToByteArray());
            }
        }
    }

    public X509Certificate2Collection GenerateCertificateChain()
    {
        var certificateChain = new X509Certificate2Collection();

        // Generate root certificate (cert1)
        using (RSA rootKey = RSA.Create(2048))
        {
            var rootCert = GenerateCertificate("CN=RootCert", null!, rootKey);
            certificateChain.Add(rootCert);

            // Generate first child certificate (cert2) signed by root certificate
            using (RSA cert2Key = RSA.Create(2048))
            {
                var cert2 = GenerateCertificate("CN=Cert2", rootCert, rootKey);
                certificateChain.Add(cert2);
			}
		}

		return certificateChain;
	}

	private void SaveCertificate(X509Certificate2 cert, string fileName)
	{
		byte[] certData = cert.Export(X509ContentType.Pfx);
		System.IO.File.WriteAllBytes(fileName, certData);
	}
}