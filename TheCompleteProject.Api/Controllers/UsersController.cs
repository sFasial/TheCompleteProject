using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.Dtos;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Service.Services.User;

namespace TheCompleteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context,IUserService userService)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet("Users")]
        public IActionResult GetUsersAsync()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users =  await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUsersAsync(Users user)
        {
            var users = await _userService.AddUserAsync(user);
            return Ok(users);
        }

    
    }
}
