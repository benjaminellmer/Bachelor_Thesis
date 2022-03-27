using System;

namespace MutualAuthenticationLibrary.Util.Client {
    public interface ITokenCreatorService {
        public string CreateToken(string audience);
        public string CreateToken(string audience, TimeSpan validityPeriod);
        // Experimental, therefore it is not mentioned in the Bachelor Thesis
        public string ReuseToken(string audience);
    }
}
