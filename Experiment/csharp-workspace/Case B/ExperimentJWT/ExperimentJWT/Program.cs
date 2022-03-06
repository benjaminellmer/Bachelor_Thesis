using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using JWTCreator;
using Microsoft.IdentityModel.Tokens;

namespace ExperimentJWT {
    internal class Program {
        static async Task Main(string[] args) {
            int testIterations = 5;
            int iterations = 1000;
            ArrayList iterationTimes = new ArrayList();

            // Load pfx, which includes the certificate and the privaet key of the service
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            X509Certificate2 certificate = new X509Certificate2(projectDirectory + "/service.pfx", "B3njam1n");

            JWTFactory fac = new JWTFactory("service2.swapindo.com", certificate);
            String temp = fac.CreateJWT("service.swapindo.com");

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            HttpClient clientWithCert = new HttpClient(handler);
            HttpClient clientWithoutCert = new HttpClient();
            HttpClient httpClient = clientWithCert;

            //for (int j = 1; j <= testIterations; j++) {
            DateTime startTotal = DateTime.Now;

            for (int i = 0; i < iterations; i++) {
                DateTime startIteration = DateTime.Now;
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/RandomNumber")) {
                    String jwt = fac.CreateJWT("service.swapindo.com");
                    request.Headers.Add("Authorization", jwt);
                    var response = await httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    Console.WriteLine((i + 1) + " of: " + iterations);
                }
                DateTime endIteration = DateTime.Now;
                TimeSpan durationIteration = (endIteration - startIteration);
                iterationTimes.Add(durationIteration.TotalMilliseconds);

                if (i == 0) {
                    httpClient = clientWithoutCert;
                }
            }
            DateTime endTotal = DateTime.Now;

            TimeSpan durationTotal = (endTotal - startTotal);
            Console.WriteLine("Elapsed Time is {0} s", durationTotal.TotalSeconds);
            Console.WriteLine("Average Request duration is {0} ms", (durationTotal.TotalMilliseconds / (double)iterations));

            //before your loop
            var csv = new StringBuilder();

            foreach (var iteration in iterationTimes) {
                var val = iteration;
                var newLine = string.Format("{0}", val);
                csv.AppendLine(newLine);
            }

            String filename = "results-" + iterations + "-JWT_recreate_0" + testIterations + ".csv";
            //after your loop
            File.WriteAllText(filename, csv.ToString());
            Task.Delay(60000).Wait();   // wait 50ms

            iterationTimes.Clear();

            //int count = 1;
            //foreach (var iteration in iterationTimes) {
            //    Console.WriteLine(count++ + ": " + iteration);
            //}
            //}
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
