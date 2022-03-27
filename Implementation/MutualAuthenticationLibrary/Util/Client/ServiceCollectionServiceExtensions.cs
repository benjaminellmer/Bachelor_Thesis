using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MutualAuthenticationLibrary.Util.Client.MTLS {
    public static class ServiceCollectionServiceExtensions {
        public static IServiceCollection AddCertificateHttpClient(this IServiceCollection services, string identifier, X509Certificate2 certificate) {
            services.AddHttpClient(identifier)
                .ConfigurePrimaryHttpMessageHandler(() => {
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(certificate);
                    return handler;
                });
            return services;
        }
    }
}
