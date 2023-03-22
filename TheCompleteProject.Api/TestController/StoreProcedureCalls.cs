using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Dtos;
using TheCompleteProject.Repository.CommonSpCalls;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.TestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreProcedureCalls : ControllerBase
    {



        [HttpGet("GetUsersWithQueryWithWhereCondition")]
        public async Task<IActionResult> GetUsersWithQueryWithWhereCondition(int Id)
        {
            /*
            "Users", 
            "Id,UserName,Email", 
            "--where condition begins
            Id = '" +Id+ "'
            and UserName = '"+userName+"'
             "--where condition Ends
            ,"");
            */

            var result = await SQLHelper.ExecuteQuery<UserDtos>("Users",
                                                           "Id,UserName,Email",
                                                            "Id = '" +Id+ "'"
                                                            , "");
            return Ok(result);
        }

        [HttpGet("GetUsersWithQueryWithMultipleWhereCondition")]
        public async Task<IActionResult> GetUsersWithQueryWithMultipleWhereCondition(int Id, string userName)
        {
            /*
         "Users", 
         "Id,UserName,Email", 
         "--where condition begins
         Id = '" +Id+ "'
         and UserName = '"+userName+"'
          "--where condition Ends
         ,"");
         */
            var result = await SQLHelper.ExecuteQuery<UserDtos>("Users",
                                                           "Id,UserName,Email",
                                                           "Id = '" + Id + "' and UserName= '" + userName + "'", "");
            return Ok(result);
        }


        [HttpGet("OutputParameterStoreProcedure")]
        public IActionResult DynamicStoreProcedureCallAsync(int from, int to, string search)
        {
            int TotalRecords = 0;
            var result = CommonStoreProcedure.GetUsersBySp(from, to, out TotalRecords, search);
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
