using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace JWTApi {
    public class JWTAuthenticationMiddleware {
        private readonly RequestDelegate _next;

        public JWTAuthenticationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                var cert = context.Connection.ClientCertificate;
                var authService = context.RequestServices.GetService<IAuthJWTService>();
                String token = context.Request.Headers["Authorization"];
                bool missingCertificate = false;

                if (token == null) {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Missing Authentication Token.");
                } else {
                    String issuer = null;
                    if (cert != null) {
                        if (CertificateAuthority.Instance.Validate(cert) && authService.PutIntoTruststore(cert)) {
                            issuer = cert.GetNameInfo(X509NameType.SimpleName, false);
                        }
                    } else {
                        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                        if (decodedToken != null) {
                            if (authService.ContainsIssuerInTruststore(decodedToken.Issuer)) {
                                issuer = decodedToken.Issuer;
                            } else {
                                missingCertificate = true;
                            }
                        }
                    }

                    if (!missingCertificate && issuer != null && authService.ValidateJWTWithTruststore(token, issuer)) {
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
}
