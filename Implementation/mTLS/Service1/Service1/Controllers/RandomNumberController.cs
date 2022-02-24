using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Service1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RandomNumberController : ControllerBase {
        Random rnd = new Random();

        //[Authorize]
        [HttpGet]
        public int Get() {
            return rnd.Next();
        }
    }
}
