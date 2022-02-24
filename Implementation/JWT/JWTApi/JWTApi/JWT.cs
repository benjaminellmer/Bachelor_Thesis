using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace JWTApi {
    public interface IAuthJWTService {
        public bool ValidateJWTWithTruststore(string token, String issuer);
        public string CreateJWT(string audience, int minutes);
        public string CreateJWT(string audience);
        public bool PutIntoTruststore(X509Certificate2 cert);
        public bool ContainsIssuerInTruststore(String issuer);
    }

    public class AuthJWTService : IAuthJWTService {
        const int DEFAULT_VALIDITY_PERIOD = 5;
        private readonly Dictionary<string, X509Certificate2> _trustStore;

        private X509Certificate2 _certificate;

        public AuthJWTService(X509Certificate2 certificate) {
            _certificate = certificate;
            _trustStore = new Dictionary<string, X509Certificate2>();
        }

        public string CreateJWT(string audience, int minutes) {
            var privateKey = new X509SecurityKey(_certificate);

            var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256) {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: _certificate.GetNameInfo(X509NameType.SimpleName, false),
                audience: audience,
                notBefore: now,
                expires: now.AddMinutes(minutes),
                signingCredentials: signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public string CreateJWT(string audience) {
            return CreateJWT(audience, DEFAULT_VALIDITY_PERIOD);
        }

        public bool ValidateJWT(string token, X509Certificate2 clientCertificate) {
            if (!CertificateAuthority.Instance.Validate(clientCertificate)) return false;

            var publicKey = new X509SecurityKey(clientCertificate);

            var validationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = clientCertificate.GetNameInfo(X509NameType.SimpleName, false),
                ValidAudience = _certificate.GetNameInfo(X509NameType.SimpleName, false),
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

        public bool PutIntoTruststore(X509Certificate2 cert) {
            if (CertificateAuthority.Instance.Validate(cert)) {
                _trustStore[cert.GetNameInfo(X509NameType.SimpleName, false)] = cert;
                return true;
            }
            return false;
        }

        public bool ContainsIssuerInTruststore(String issuer) {
            return _trustStore.ContainsKey(issuer);
        }

        public bool ValidateJWTWithTruststore(string token, string issuer) {
            if (_trustStore.ContainsKey(issuer)) {
                return ValidateJWT(token, _trustStore[issuer]);
            }
            return false;
        }
    }
}
