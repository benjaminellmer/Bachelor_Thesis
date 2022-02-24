using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace JWTCreator {
    internal class Program {
        static void Main(string[] args) {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            X509Certificate2 certificate = new X509Certificate2(projectDirectory + "/service.pfx", "B3njam1n");
            X509Certificate2 ca = new X509Certificate2(projectDirectory + "/ca.crt", "B3njam1n");

            CertificateAuthority.Instance.AppendCertificate(ca);

            JWTFactory fac = new JWTFactory("service1.swapindo.com", certificate);

            String jwt = fac.CreateJWT("service2.swapindo.com");
            Console.WriteLine(jwt);

            var data = fac.DecodeJWT(jwt, certificate);

            if (data == null) {
                Console.WriteLine("Error");
            } else {
                foreach (KeyValuePair<string, object> kvp in data) {
                    Console.WriteLine("Key: " + kvp.Key + " Value: " + kvp.Value);
                }
            }

            //var result = fac.DecodeJWT(jwt, certificate);
            //Console.WriteLine(result);
        }
    }
}

namespace JWTCreator {
    public class JWTFactory {
        const int DEFAULT_VALIDITY_PERIOD = 5;

        String issuer;
        X509Certificate2 certificate;

        public JWTFactory(String issuer, X509Certificate2 certificate) {
            this.issuer = issuer;
            this.certificate = certificate;
        }

        /**
         * https://stackoverflow.com/questions/50998542/trouble-signing-a-jwt-token-with-an-x509-certificate
         * https://vmsdurano.com/-net-core-3-1-signing-jwt-with-rsa/
         */
        public string CreateJWT(String audience, int minutes = DEFAULT_VALIDITY_PERIOD) {
            var privateKey = new X509SecurityKey(certificate);

            var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256) {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: this.issuer,
                audience: audience,
                notBefore: now,
                expires: now.AddMinutes(minutes),
                signingCredentials: signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public Dictionary<string, object> DecodeJWT(string token, X509Certificate2 cert) {
            if (CertificateAuthority.Instance.Validate(cert) && ValidateJWT(token, cert)) {
                var payload = Jose.JWT.Decode(token, cert.GetRSAPublicKey(), Jose.JwsAlgorithm.RS256);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
            }
            return null;
        }

        public bool ValidateJWT(string token, X509Certificate2 cert) {
            var publicKey = new X509SecurityKey(cert);
            var validationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = certificate.GetNameInfo(X509NameType.SimpleName, false),
                ValidAudience = this.issuer,
                IssuerSigningKey = publicKey,
                ClockSkew = TimeSpan.Zero,
                CryptoProviderFactory = new CryptoProviderFactory() {
                    CacheSignatureProviders = false
                }
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
//public class JWTFactory {
//const int DEFAULT_VALIDITY_PERIOD = 5;

//String issuer;
//X509Certificate2 certificate;

//public JWTFactory(String issuer, X509Certificate2 certificate) {
//    this.issuer = issuer;
//    this.certificate = certificate;
//}

//public String CreateJWT(String subject, int minutes = DEFAULT_VALIDITY_PERIOD) {
//    Dictionary<string, object> payload = new Dictionary<string, object>();
//    payload.Add("iss", issuer);
//    payload.Add("sub", subject);
//    payload.Add("iat", GetTimeStamp(0));
//    payload.Add("exp", GetTimeStamp(minutes));

//    return Jose.JWT.Encode(payload, certificate.GetRSAPrivateKey(), Jose.JwsAlgorithm.RS256);
//}

///*
// * Returns null, if the JWT is not valid
// */
//public IDictionary<string, object> DecodeJWT(String jwt, X509Certificate2 cert) {
//    var payload = Jose.JWT.Decode(jwt, cert.GetRSAPublicKey(), Jose.JwsAlgorithm.RS256);

//    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
//    var sub = dict.GetValueOrDefault("sub");
//    var iat = dict.GetValueOrDefault("iat");
//    var exp = dict.GetValueOrDefault("exp");
//    var iss = dict.GetValueOrDefault("iss");

//    if (sub == null || iat == null || exp == null || iss == null) {
//        return null;
//    }
//    if (!sub.Equals(this.issuer)) {
//        return null;
//    }

//    return dict;
//}

//public int GetTimeStamp(int offsetMinutes) {
//    return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + (offsetMinutes * 60);
//}
//}
