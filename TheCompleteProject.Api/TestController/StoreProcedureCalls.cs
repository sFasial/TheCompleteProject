using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheCompleteProject.Repository.CommonSpCalls;

namespace TheCompleteProject.Api.TestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreProcedureCalls : ControllerBase
    {

        [HttpGet("OutputParameterStoreProcedure")]
        public IActionResult DynamicStoreProcedureCallAsync(int from , int to , string search)
        {
            int TotalRecords = 0;
            var result =  CommonStoreProcedure.GetUsersBySp(from,to,out TotalRecords,search);
            return Ok(result);
        }


        [HttpGet("DynamicStoreProcedureGet")]
        public async Task<IActionResult> DynamicStoreProcedureCallAsync(int id)
        {
            var result = await CommonStoreProcedure.GetUserNameByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("VoidStoreProcedure")]
        public IActionResult AddVoid(string UserName, string Email, string Password, int RoleId)
        {
            CommonStoreProcedure.AddUsersBySp(UserName, Email, Password, RoleId);
            return Ok("Record Added");
        }
    }
}
