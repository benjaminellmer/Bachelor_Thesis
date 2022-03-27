using System;
using System.Security.Cryptography.X509Certificates;

namespace MutualAuthenticationLibrary.Util.Server {
    public interface ITokenValidationService {
        public void PutIntoTruststore(X509Certificate2 certificate);
        public bool ContainsIssuerInTruststore(string issuer);
        public bool ValidateTokenWithTruststore(string token, string issuer);
    }
}
