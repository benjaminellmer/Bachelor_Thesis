using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MutualAuthenticationLibrary.Util.Server.MTLS {
    public class MTLSAuthenticationEvents : CertificateAuthenticationEvents {
        override public Task CertificateValidated(CertificateValidatedContext context) {
            var caService = context.Response.HttpContext.RequestServices.GetService<ICertificateAuthorityService>();

            if (caService.ValidateCertificate(context.ClientCertificate)) {
                context.Success();
            } else {
                context.Fail("Certificate could not be validated");
            }
            return Task.CompletedTask;
        }

        override public Task AuthenticationFailed(CertificateAuthenticationFailedContext context) {
            context.Fail("Certificate could not be validated");
            return Task.CompletedTask;
        }
    }
}
