using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Classes;
using UserApi.Models;

namespace UserApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly SwapindoDbContext _context;
        private const int _standardPageSize = 5;

        public UserController(SwapindoDbContext context) {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(int page = 1, int pagesize = _standardPageSize) {
            DbSet<User> users = _context.Users;

            return await PaginatedList<User>.CreateAsync(users.AsNoTracking(), page, pagesize);
        }


        private bool UserExists(long id) {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
