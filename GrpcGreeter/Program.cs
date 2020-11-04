using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace GrpcGreeter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    // Setup a HTTP/2 endpoint without TLS.
                    //options.ListenLocalhost(5001, o => o.Protocols = HttpProtocols.Http2);
                    options.ListenAnyIP(5001, o => o.UseHttps(CreateCertificate("localhost")));
                });
                webBuilder.UseStartup<Startup>();
            });

        public static X509Certificate2 CreateCertificate(string subjectName)
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var certificateRequest = new CertificateRequest($"cn={subjectName}", ecdsa, HashAlgorithmName.SHA256);
            return certificateRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
        }
    }
}
