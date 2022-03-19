using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MutualAuthenticationLibrary.Util.Client;
using MutualAuthenticationLibrary.Util.Client.JWT;
using MutualAuthenticationLibrary.Util.Client.MTLS;
using MutualAuthenticationLibrary.Util.Server;
using MutualAuthenticationLibrary.Util.Server.JWT;
using System.Security.Cryptography.X509Certificates;

namespace AdService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            services.AddSingleton<ICertificateAuthorityService>(context => new CertificateAuthorityService(options => {
                options.Add(new X509Certificate2(@"ca.crt", "B3njam1n"));
            }));

            services.AddSingleton<ITokenValidationService>(context => {
                return new JWTValidationService("adservice");
            });

            services.AddSingleton<ITokenCreatorService>(context => {
                return new JWTCreatorService(new X509Certificate2(@"adService.pfx", "B3njam1n"), options => {
                    options.DefaultValidityPeriod = new System.TimeSpan(0, 1, 0);
                });
            });

            services.AddHttpClient("clientNoCertificate");
            services.AddCertificateHttpClient("clientWithCertificate", new X509Certificate2(@"adService.pfx", "B3njam1n"));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // app.UseJWTAuthentication(); <- commented, so it can be tested using a browser and no JWT

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
