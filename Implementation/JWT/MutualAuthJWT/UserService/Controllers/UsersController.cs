using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UserService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private List<User> _users = new List<User>();

        public UsersController() {
            _users.Add(new User(0, "Benjamin", "Ellmer"));
            _users.Add(new User(1, "Lukas", "Greinstädter"));
        }

        // GET: api/<AdsControllerController>
        [HttpGet]
        public IEnumerable<User> Get() {
            return _users;
        }

        // GET api/<AdsControllerController>/5
        [HttpGet("{id}")]
        public User Get(int id) {
            return _users.Find(x => x.Id == id);
        }

    }
    public class User {
        public User(int id, string firstname, string lastname) {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
