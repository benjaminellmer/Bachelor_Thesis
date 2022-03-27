using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace MutualAuthenticationLibrary.Util.Client.JWT {
    public class JWTCreatorServiceOptions {
        public TimeSpan DefaultValidityPeriod = new TimeSpan(0, 5, 0); // 5 Minutes
    }
    public enum TokenReusePolicy {
        ReuseWhenPossible,
        Never
    }

    public class JWTCreatorService : ITokenCreatorService {
        private X509Certificate2 _certificate;
        private JWTCreatorServiceOptions _options = new JWTCreatorServiceOptions();
        private Dictionary<string, string> _tokenCache = new Dictionary<string, string>();

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

            // Add Token to cache and remove it 10 seconds before the validity timespan is over
            _tokenCache.Add(audience, token);
            int tokenCacheLifetime = validityPeriod.Subtract(new TimeSpan(0, 0, 10)).Milliseconds;
            Task.Factory.StartNew(() => Thread.Sleep(tokenCacheLifetime))
                .ContinueWith(t => {
                    _tokenCache.Remove(audience);
                }, TaskScheduler.FromCurrentSynchronizationContext());

            return token;
        }

        /// <summary>
        /// If the cache stores a valid JWT for the given audience, returns the token.
        /// Else Returns null.
        /// </summary>
        /// <param name="audience"></param>
        /// <returns></returns>
        public string ReuseToken(string audience) {
            return _tokenCache.GetValueOrDefault(audience);
        }
    }
}
