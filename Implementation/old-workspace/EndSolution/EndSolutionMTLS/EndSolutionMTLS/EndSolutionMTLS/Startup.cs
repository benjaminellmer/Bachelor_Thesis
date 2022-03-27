using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MutualAuthenticationLibrary.Util.Client.MTLS;
using MutualAuthenticationLibrary.Util.Server;
using MutualAuthenticationLibrary.Util.Server.JWT;
using MutualAuthenticationLibrary.Util.Server.MTLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EndSolutionMTLS {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            services.AddSingleton<ITokenCreatorService>(handler => new JWTCreatorService(new X509Certificate2(@"service.pfx", "B3njam1n")));
            services.AddSingleton<ITokenValidationService>(handler => new JWTValidationService("service1"));

            services.AddMTLSClient("mtlsClient", new X509Certificate2(@"service.pfx", "B3njam1n"));

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options => {
                options.AllowedCertificateTypes = CertificateTypes.Chained;
                options.RevocationMode = X509RevocationMode.NoCheck;
                options.Events = new MTLSAuthenticationEvents();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            //app.UseJWTAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
