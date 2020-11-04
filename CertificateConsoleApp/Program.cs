using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeCert("192.168.2.104");
            Console.WriteLine("Hello World!");
        }

        static void MakeCert(string subjectName)
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var certificateRequest = new CertificateRequest($"cn={subjectName}", ecdsa, HashAlgorithmName.SHA256);
            var x509Certificate2 = certificateRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("C:\\Users\\mru\\Austausch\\mycert.pfx", x509Certificate2.Export(X509ContentType.Pfx, "P@55w0rd"));

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText("C:\\Users\\mru\\Austausch\\mycert.cer",
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(x509Certificate2.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }
    }
}
