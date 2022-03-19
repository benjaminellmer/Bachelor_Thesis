using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace MutualAuthenticationLibrary.Util.Server.JWT {
    public class JWTValidationService : ITokenValidationService {
        private readonly Dictionary<string, X509Certificate2> _trustStore;
        private string _audience;

        public JWTValidationService(string audience) {
            _trustStore = new Dictionary<string, X509Certificate2>();
            _audience = audience;
        }

        public bool ContainsIssuerInTruststore(string issuer) {
            return _trustStore.ContainsKey(issuer);
        }

        public void PutIntoTruststore(X509Certificate2 certificate) {
            _trustStore[certificate.GetNameInfo(X509NameType.SimpleName, false)] = certificate;
        }

        public bool ValidateTokenWithTruststore(string token, string issuer) {
            if (_trustStore.ContainsKey(issuer)) {
                return false;
            }

            var clientCertificate = _trustStore[issuer];
            var publicKey = new X509SecurityKey(clientCertificate);

            var validationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = clientCertificate.GetNameInfo(X509NameType.SimpleName, false),
                ValidAudience = _audience,
                IssuerSigningKey = publicKey,
                ClockSkew = TimeSpan.Zero,
                CryptoProviderFactory = new CryptoProviderFactory() { CacheSignatureProviders = false }
            };

            try {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, validationParameters, out var payload);
                return payload != null;
            } catch {
                return false;
            }
        }
    }
}
