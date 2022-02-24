using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Service1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private User[] _users = {
            new User("Benjamin", "Ellmer"),
            new User("Philipp", "Kräutl"),
        };

        [Authorize]
        [HttpGet]
        public IEnumerable<User> Get() {
            return _users;
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
