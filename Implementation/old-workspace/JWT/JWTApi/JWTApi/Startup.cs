using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JWTApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<IAuthJWTService>(handler => new AuthJWTService(new X509Certificate2(@"service.pfx", "B3njam1n")));

            //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options => {
            //    options.AllowedCertificateTypes = CertificateTypes.Chained;
            //    options.RevocationMode = X509RevocationMode.NoCheck;
            //    options.Events = new CertificateAuthenticationEvents() {
            //        OnCertificateValidated = context => {
            //            //var authService = context.HttpContext.RequestServices.GetService<IAuthJWTService>();
            //            //String token = context.Request.Headers["Authorization"];
            //            //if (token != null && authService.ValidateJWT(token, context.ClientCertificate)) {
            //            //    context.Success();
            //            //} else {
            //            //    context.Fail("JWT ist invalid");
            //            //}
            //            context.Request.Headers["X-SSL-CERT"] = context.ClientCertificate.GetRawCertDataString();
            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context => {
            //            context.Success();
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            //services.AddCertificateForwarding(options => {
            //    options.CertificateHeader = "X-SSL-CERT";

            //    options.HeaderConverter = headerValue => {
            //        X509Certificate2? clientCertificate = null;

            //        if (!string.IsNullOrWhiteSpace(headerValue)) {
            //            clientCertificate = new X509Certificate2(StringToByteArray(headerValue));
            //        }

            //        return clientCertificate!;

            //        static byte[] StringToByteArray(string hex) {
            //            var numberChars = hex.Length;
            //            var bytes = new byte[numberChars / 2];

            //            for (int i = 0; i < numberChars; i += 2) {
            //                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            //            }

            //            return bytes;
            //        }
            //    };
            //});

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<JWTAuthenticationMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
