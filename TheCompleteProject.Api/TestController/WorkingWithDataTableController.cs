using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Api.TestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingWithDataTableController : ControllerBase
    {
        private readonly IUserService _userService;

        public WorkingWithDataTableController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route(("DataTable"))]
        public async Task<IActionResult> DataTable()
        {
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
            var dt = DataTableHelper.ConvertListToDataSet(users);
            var table1 = dt.Tables[0];

            var id = table1.Columns["Id"];
            var columnsId = table1.Columns[0];

            var rowId = table1.Rows[0]["Id"];
            var userName = table1.Rows[0]["UserName"];
            var password = table1.Rows[0]["Password"];

             rowId = table1.Rows[1]["Id"];
             userName = table1.Rows[1]["UserName"];
             password = table1.Rows[1]["Password"];
            return Ok(dt);
        }  
        
        [HttpGet]
        [Route(("DataTableExtension"))]
        public async Task<IActionResult> DataTableExtensions()
        {
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
            var dt = DataTableExtension.ToDataTable(users);
            return Ok(dt);
        }

        [HttpGet]
        [Route(("DataSet"))]
        public async Task<IActionResult> DataSet()
        {
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
        }
    }
}
