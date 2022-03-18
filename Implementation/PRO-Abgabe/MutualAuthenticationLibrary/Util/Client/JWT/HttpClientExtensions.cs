using System;
using System.Net.Http;

namespace MutualAuthenticationLibrary.Util.Client.JWT {
    public class AppendTokenOptions {
        public string Header = "Authorization";
        public TimeSpan? ValidityPeriod = null; // null => Default Period of TokenCreatorService
        public TokenReusePolicy TokenReusePolicy = TokenReusePolicy.Never;
    }

    public static class HttpClientExtensions {
        public static void AppendToken(this HttpClient client, ITokenCreatorService creatorService, string audience) {
            var options = new AppendTokenOptions();

            string token;
            if (options.TokenReusePolicy == TokenReusePolicy.Never) {
                token = creatorService.CreateToken(audience);
            } else {
                token = creatorService.ReuseToken(audience) ?? creatorService.CreateToken(audience);
            }

            client.DefaultRequestHeaders.Remove(options.Header);
            client.DefaultRequestHeaders.Add(options.Header, token);
        }

        public static void AppendToken(this HttpClient client, ITokenCreatorService creatorService, string audience, Action<AppendTokenOptions> configureOptions) {
            var options = new AppendTokenOptions();
            configureOptions(options);

            string token;
            if (options.TokenReusePolicy != TokenReusePolicy.Never) {
                token = creatorService.CreateToken(audience);
            } else {
                token = creatorService.ReuseToken(audience) ?? creatorService.CreateToken(audience);
            }

            client.DefaultRequestHeaders.Add(options.Header, token);
        }
    }
}
