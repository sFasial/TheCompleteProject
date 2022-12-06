using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Repository.DatabaseContext;

namespace TheCompleteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Users")]
        public IActionResult AllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}
