using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UserApi;

namespace AdService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase {
        const string userServiceBaseUrl = "https://localhost:5005/";
        private List<Ad> _ads = new List<Ad>();
        private HttpClient _httpClient;

        public AdsController(IHttpClientFactory factory) {
            _ads.Add(new Ad(0, "Bohrmaschine", 150.00, 0));
            _ads.Add(new Ad(1, "Nintendo Switch", 240.00, 0));

            _httpClient = factory.CreateClient("mTLSClient");
        }

        // GET: api/<AdsControllerController>
        [HttpGet]
        // [Authorize] <- commented, so it can be tested using a browser and no certificate
        public IEnumerable<Ad> Get() {
            List<Ad> result = new List<Ad>();

            _ads.ForEach(ad => {
                // The swapindo backend would not create a request for each user, it would use the bulkrequest function
                result.Add(GetAsync(ad.Id).Result);
            });

            return result;
        }

        // GET api/<AdsControllerController>/5
        [HttpGet("{id}")]
        // [Authorize] <- commented, so it can be tested using a browser and no certificate
        public async Task<Ad> GetAsync(int id) {
            Ad elem = _ads.Find(x => x.Id == id);

            UserApiClient userClient = new UserApiClient(userServiceBaseUrl, _httpClient);
            elem.User = await userClient.UsersAsync(elem.UserId);

            return elem;
        }

        public class Ad {
            public Ad(int id, string title, double price, int userId) {
                Id = id;
                Title = title;
                Price = price;
                UserId = userId;
            }

            public int Id { get; set; }
            public string Title { get; set; }
            public double Price { get; set; }
            public int UserId { get; set; }
            public User? User { get; set; }
        }
    }
}
