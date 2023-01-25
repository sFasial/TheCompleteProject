using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Api.DemoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FFCSaveAttachments : ControllerBase
    {


        [HttpPost("SaveAttachment")]
        public IActionResult SaveAttachment([FromBody] string data, string fileName)
        {
            var path = "wwwroot/images";
            Path.GetFullPath(path);
            path = path.Replace("\\", @"/");

            var currentEnviormentPath = Environment.CurrentDirectory;
            currentEnviormentPath = currentEnviormentPath + "\\wwwroot";
            currentEnviormentPath = currentEnviormentPath.Replace("\\", @"/");

            var result = MultimediaHelper.SaveMultimedia(data, path, "pdf", fileName, "", "D", "");
            return Ok(result);

        }

        [HttpPost("SaveAttachmentWithGroupIdFolder")]
        public IActionResult SaveAttachmentWithGroupIdFolder([FromBody] string data, string fileName)
        {

            var currentEnviormentPath = Environment.CurrentDirectory;
            currentEnviormentPath = currentEnviormentPath + "\\wwwroot\\images";
            currentEnviormentPath = currentEnviormentPath.Replace("\\", @"/");

            var result = MultimediaHelper.SaveMultimedia(data, currentEnviormentPath, "pdf", fileName, "CompanyDocuments", "D", "");
            return Ok(result);

        }

        [HttpPost("SaveAttachmentWithIformFile")]
        public IActionResult SaveAttachmentWithIformFile([FromQuery] IFormFile file)
        {
            var path = "wwwroot/images";
            var length = file.Length;
            var fileName = file.FileName;
            var ContentDisposition = file.ContentDisposition;
            var contentType = file.ContentType;

            throw new NotImplementedException();
        }

    }
}
