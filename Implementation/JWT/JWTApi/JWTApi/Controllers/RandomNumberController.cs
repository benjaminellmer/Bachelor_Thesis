using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JWTApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RandomNumberController : ControllerBase {
        Random rnd = new Random();

        [HttpGet]
        public int Get() {
            return rnd.Next();
        }
    }
}
