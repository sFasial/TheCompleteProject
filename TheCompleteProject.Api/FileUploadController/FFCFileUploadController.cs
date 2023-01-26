using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Service.Services.User;

namespace TheCompleteProject.Api.FileUploadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FFCFileUploadController : ControllerBase
    {
        private readonly IUserService _userService;

        public FFCFileUploadController(IUserService userService)
        {
            _userService = userService;
        }

    }
}
