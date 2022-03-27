using System;
using System.Net.Http;

namespace MutualAuthenticationLibrary.Util.Client.JWT {
    public class AppendTokenOptions {
        public string Header = "Authorization";
        public TimeSpan? ValidityPeriod = null; // null => Default Period of TokenCreatorService
    }

    public static class HttpClientExtensions {
        public static void AppendToken(this HttpClient client, ITokenCreatorService creatorService, string audience) {
            var options = new AppendTokenOptions();

            string token = creatorService.CreateToken(audience);
            client.DefaultRequestHeaders.Add(options.Header, token);
        }

        public static void AppendToken(this HttpClient client, ITokenCreatorService creatorService, string audience, Action<AppendTokenOptions> configureOptions) {
            var options = new AppendTokenOptions();
            configureOptions(options);

            string token = creatorService.CreateToken(audience);
            client.DefaultRequestHeaders.Add(options.Header, token);
        }
    }
}
