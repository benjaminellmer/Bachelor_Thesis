using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace JWTApi {
    public class Program {
        public static void Main(string[] args) {
            CertificateAuthority.Instance.AppendCertificate(new X509Certificate2(@"ca.crt"));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options => {
                        options.ConfigureHttpsDefaults(configureOptions => {
                            configureOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                        });
                    });
                });
    }
}
