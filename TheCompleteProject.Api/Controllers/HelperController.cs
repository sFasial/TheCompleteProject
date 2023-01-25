using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Api.Controllers
{
    public class HelperController : BaseController
    {

        [HttpPost("GenerateFileFromBase64String")]
        public async Task<IActionResult> GetFile([FromBody] string base64String)
        {
            var bytes =  MultimediaHelper.GenerateFileFromBase64String(base64String);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "File.xlsx");
        }
    }
}
