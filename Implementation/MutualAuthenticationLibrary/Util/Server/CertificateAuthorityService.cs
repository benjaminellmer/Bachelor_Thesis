using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MutualAuthenticationLibrary.Util.Server {
    public interface ICertificateAuthorityService {
        public void AppendCertificate(X509Certificate2 certificate);
        public bool ValidateCertificate(X509Certificate2 certificate);
    }

    public class CertificateAuthorityService : ICertificateAuthorityService {
        private X509Chain _chain = null;

        public CertificateAuthorityService() {
            _chain = new X509Chain();

            _chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            _chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        }
        public CertificateAuthorityService(Action<X509Certificate2Collection> configureTrustedCAs) {
            _chain = new X509Chain();

            _chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            _chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

            configureTrustedCAs(_chain.ChainPolicy.CustomTrustStore);
        }

        public void AppendCertificate(X509Certificate2 certificate) {
            _chain.ChainPolicy.CustomTrustStore.Add(certificate);
        }

        public bool ValidateCertificate(X509Certificate2 certificate) {
            try {
                return _chain.Build(certificate);
            } catch {
                return false;
            }
        }
    }
}
