using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.Dtos;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;


        public UsersController(ApplicationDbContext context,IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _context = context;
            _appSettings = appSettings.Value;

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
            var testVariable = _appSettings.Secret;

            var users =  await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUsersAsync(Users user)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Exception");
            }
            var users = await _userService.AddUserAsync(user);
            return Ok(users);
        }

    
    }
}
