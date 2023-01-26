using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Api.FileUploadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FFCReportsController : ControllerBase
    {
        private readonly IUserService _userService;

        public FFCReportsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route(("FFC Excel Import"))]
        public async Task<HttpResponseMessage> ExcelBulkImportAsync()
        {
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();

            var dt = users.Select(x => new
            {
                x.Id,
                x.UserName
            }).ToList().ToDataTable();

            var sheets = new List<MultipleExcelSheetsDto>
            {
                new MultipleExcelSheetsDto
                {
                    dt = dt,
                    ReportHeading = "My Users",
                    WorkSheetName = "Users"
                }
            };

            var dataBytes = MultimediaHelper.GetBytesForExportToExcel_MultipleSheets(sheets);

            string fileName = "UsersTest.XLS";

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new ByteArrayContent(dataBytes);
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        [HttpPost]
        [Route(("FFC Excel Import With DataTableHelper"))]
        public async Task<HttpResponseMessage> ExcelBulkImport()
        {
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();

            var dt = users.Select(x => new
            {
                x.Id,
                x.UserName
            }).ToList().ToDataTable();

            var sheets = new List<MultipleExcelSheetsDto>
            {
                new MultipleExcelSheetsDto
                {
                    dt = dt,
                    ReportHeading = "My Users",
                    WorkSheetName = "Users"
                }
            };

            var dataBytes = MultimediaHelper.GetBytesForExportToExcel_MultipleSheets(sheets);

            string fileName = "UsersTest.XLS";

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new ByteArrayContent(dataBytes);
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

    }
}
