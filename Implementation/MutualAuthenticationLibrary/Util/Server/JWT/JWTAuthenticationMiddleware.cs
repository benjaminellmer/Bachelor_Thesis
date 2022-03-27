using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MutualAuthenticationLibrary.Util.Server.JWT {
    public class JWTAuthenticationMiddlewareOptions {
        public string TokenHeader = "Authorization";
    }

    public class JWTAuthenticationMiddleware {

        private readonly RequestDelegate _next;
        private JWTAuthenticationMiddlewareOptions _options;

        public JWTAuthenticationMiddleware(RequestDelegate next) {
            _next = next;
            _options = new JWTAuthenticationMiddlewareOptions();
        }

        public JWTAuthenticationMiddleware(RequestDelegate next, JWTAuthenticationMiddlewareOptions options) {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context) {
            try {
                var tokenService = context.RequestServices.GetService<ITokenValidationService>();
                var caService = context.RequestServices.GetService<ICertificateAuthorityService>();

                var cert = context.Connection.ClientCertificate;
                string token = context.Request.Headers[_options.TokenHeader];
                bool missingCertificate = false;

                if (token == null) {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Missing Authentication Token.");
                } else {
                    string issuer = null;
                    if (cert != null) {
                        if (caService.ValidateCertificate(cert)) {
                            tokenService.PutIntoTruststore(cert);
                            issuer = cert.GetNameInfo(X509NameType.SimpleName, false);
                        }
                    } else {
                        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                        if (decodedToken != null) {
                            if (tokenService.ContainsIssuerInTruststore(decodedToken.Issuer)) {
                                issuer = decodedToken.Issuer;
                            } else {
                                missingCertificate = true;
                            }
                        }
                    }

                    if (!missingCertificate && issuer != null && tokenService.ValidateTokenWithTruststore(token, issuer)) {
                        await _next(context);
                    } else {
                        if (missingCertificate) {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Could not find a Certificate for the Issuer.");
                        } else {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("Token is invalid.");
                        }
                    }
                }
            } catch {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("An error occured while validating the token.");
            }
        }
    }
    public static class JWTAuthenticationMiddlewareExtensions {
        public static IApplicationBuilder UseJWTAuthentication(
              this IApplicationBuilder app, Action<JWTAuthenticationMiddlewareOptions> configureOptions) {
            var options = new JWTAuthenticationMiddlewareOptions();
            configureOptions(options);

            return app.UseMiddleware<JWTAuthenticationMiddleware>(options);
        }
        public static IApplicationBuilder UseJWTAuthentication(
              this IApplicationBuilder app) {
            return app.UseMiddleware<JWTAuthenticationMiddleware>();
        }
    }
}
