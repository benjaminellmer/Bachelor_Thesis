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

namespace UserService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            services.AddSingleton<ICertificateAuthorityService>(context => new CertificateAuthorityService(options => {
                options.Add(new X509Certificate2(@"ca.crt", "B3njam1n"));
            }));

            services.AddSingleton<ITokenValidationService>(context => {
                return new JWTValidationService("userservice");
            });

            services.AddSingleton<ITokenCreatorService>(context => {
                return new JWTCreatorService(new X509Certificate2(@"userService.pfx", "B3njam1n"), options => {
                    options.DefaultValidityPeriod = new System.TimeSpan(0, 1, 0);
                });
            });

            services.AddHttpClient("clientNoCertificate");
            services.AddCertificateHttpClient("clientWithCertificate", new X509Certificate2(@"userService.pfx", "B3njam1n"));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseJWTAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
