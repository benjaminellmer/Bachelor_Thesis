using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service2 {
    [Route("api/[controller]")]
    [ApiController]
    public class ForwardUsersController : ControllerBase {
        private HttpClient httpClient;

        public ForwardUsersController(IHttpClientFactory factory) {
            httpClient = factory.CreateClient("mtlsclient");
        }


        [HttpGet]
        public async Task<string> Get() {
            // https://www.c-sharpcorner.com/article/creating-a-net-5-client-to-call-an-api-protected-by-certificates/
            using (var response = await httpClient.GetAsync("https://localhost:5001/api/User")) {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }

    public class User {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public User(string firstName, string lastName) {
            this.Firstname = firstName;
            this.Lastname = lastName;
        }
    }
}
