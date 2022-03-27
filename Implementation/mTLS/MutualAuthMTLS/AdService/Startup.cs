using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MutualAuthenticationLibrary.Util.Client.MTLS;
using MutualAuthenticationLibrary.Util.Server;
using MutualAuthenticationLibrary.Util.Server.MTLS;
using System.Security.Cryptography.X509Certificates;

namespace AdService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            // Inject Certificate Authority Service with the certificate of the CA
            services.AddSingleton<ICertificateAuthorityService>(context => new CertificateAuthorityService(options => {
                options.Add(new X509Certificate2(@"ca.crt", "B3njam1n"));
            }));

            // Inject HttpClient, that stores a certificate in the ClientCertificate field
            services.AddCertificateHttpClient("mTLSClient", new X509Certificate2(@"adService.pfx", "B3njam1n"));

            // Setup MTLS Authentication
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options => {
                options.AllowedCertificateTypes = CertificateTypes.Chained;
                options.RevocationMode = X509RevocationMode.NoCheck;
                options.Events = new MTLSAuthenticationEvents();
            });
            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
