using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Linq;

/* Sources
 * How to validate: https://stackoverflow.com/questions/60109454/how-to-fully-validate-a-x509-certificate
 * X509Chain: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509chain.build?view=net-5.0
 * Singleton: https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/ 
 */

namespace JWTCreator {
    public class CertificateAuthority {
        private static X509Chain chain = null;
        private static readonly CertificateAuthority instance = new CertificateAuthority();
        public static CertificateAuthority Instance {
            get { return instance; }
        }

        static CertificateAuthority() {
            // Create chain
            chain = new X509Chain();

            // Set options
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        }

        public void AppendCertificate(X509Certificate cert) {
            // Add the certificate to the custom trust store
            chain.ChainPolicy.CustomTrustStore.Add(cert);
        }

        public bool Validate(X509Certificate2 cert) {
            try {
                return chain.Build(cert);
            } catch (ArgumentException ex) {
                return false;
            } catch (CryptographicException ex) {
                return false;
            }
        }
    }
}
