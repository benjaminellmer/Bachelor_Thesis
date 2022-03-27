using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Security.Cryptography.X509Certificates;
namespace Service1 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            CertificateAuthority.Instance.AppendCertificate(new X509Certificate2(@"ca.crt"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                // https://blog.kritner.com/2020/07/15/setting-up-mtls-and-kestrel/
                .AddCertificate(options => {
                    options.AllowedCertificateTypes = CertificateTypes.Chained;
                    options.RevocationMode = X509RevocationMode.NoCheck;
                    options.Events = new CertificateAuthenticationEvents() {
                        OnCertificateValidated = context => {
                            if (CertificateAuthority.Instance.Validate(context.ClientCertificate)) {
                                context.Success();
                            } else {
                                context.Fail("Certificate could not be validated");
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context => {
                            context.Fail("Certificate could not be validated");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
