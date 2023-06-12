using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TheCompleteProject.Api.TestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        [HttpGet("GetDivideBy0Exceptions")]
        public IActionResult GetDivideBy0Exceptions()
        {
            int a = 0;
            int b = 4;
            var c = a / b;

            return Ok(c);
        }
    }
}
