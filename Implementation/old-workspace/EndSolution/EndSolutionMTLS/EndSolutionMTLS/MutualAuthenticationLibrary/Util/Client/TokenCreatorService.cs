using System;

namespace MutualAuthenticationLibrary.Util.Client {
    public interface ITokenCreatorService {
        public string CreateToken(string audience);
        public string CreateToken(string audience, TimeSpan validityPeriod);
    }
}
