using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace MutualAuthenticationLibrary.Util.Server.JWT {
    public class JWTCreatorServiceOptions {
        public TimeSpan DefaultValidityPeriod = new TimeSpan(0, 5, 0); // 5 Minutes
    }

    public class JWTCreatorService : ITokenCreatorService {
        private X509Certificate2 _certificate;
        private JWTCreatorServiceOptions _options = new JWTCreatorServiceOptions();

        public JWTCreatorService(X509Certificate2 certificate) {
            _certificate = certificate;
        }

        public JWTCreatorService(X509Certificate2 certificate, Action<JWTCreatorServiceOptions> configureOptions) {
            _certificate = certificate;
            configureOptions(_options);
        }

        /// <summary>
        /// Creates a signed JWT, targeted for the given audience.
        /// </summary>
        /// <param name="audience">Audience, the JWT is targeted for.</param>
        /// <returns></returns>
        public string CreateToken(string audience) {
            return CreateToken(audience, _options.DefaultValidityPeriod);
        }

        public string CreateToken(string audience, TimeSpan validityPeriod) {
            var privateKey = new X509SecurityKey(_certificate);

            var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256) {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: _certificate.GetNameInfo(X509NameType.SimpleName, false),
                audience: audience,
                notBefore: now,
                expires: now.Add(validityPeriod),
                signingCredentials: signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

    }
}
