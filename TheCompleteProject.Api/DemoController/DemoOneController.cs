using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Dtos;
using TheCompleteProject.Repository.DatabaseContext;

namespace TheCompleteProject.Api.DemoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoOneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DemoOneController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Users")]
        public IActionResult AllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("UsersWithAutoMapper")]
        public IActionResult UsersWithAutoMapper()
        {
            var users = _context.Users.ToList().FirstOrDefault();
            var userDto = new UserDtos();
            _mapper.Map(users, userDto);
            return Ok(userDto);
        }
    }
}
