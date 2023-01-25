using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Service.Services.User;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace TheCompleteProject.Api.FileUploadController
{

    //https://www.youtube.com/watch?v=j3S3aI8nMeE

    [Route("api/[controller]")]
    [ApiController]
    public class UsersReportControllerTimCorey : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersReportControllerTimCorey(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("UsersExcelExport")]
        public async Task<IActionResult> UsersExcelExport()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            var file = new FileInfo(@"C:\Demos\YouTubeDemo.xlsx");
            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
            await SaveExcelFile(users, file);

            return Ok(users);
        }

        [HttpGet("UsersExcelExportWithFormattedHeader")]
        public async Task<IActionResult> UsersExcelExportWithFormattedHeader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var file = new FileInfo(@"C:\Demos\Demo.xlsx");

            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
            await SaveExcelFileWithFormattedHeaders(users, file);

            return Ok(users);
        }

        [HttpGet("UsersExcelExportWithFormattedHeader2")]
        public async Task<IActionResult> UsersExcelExportWithFormattedHeader2()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var file = new FileInfo(@"C:\Demos\Demo.xlsx");

            var _users = await _userService.GetUsersAsync();
            var users = _users.ToList();
            await SaveExcelFileWithFormattedHeaders2(users, file);

            return Ok(users);
        }

        private static async Task SaveExcelFile(List<Users> people, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);
            var ws = package.Workbook.Worksheets.Add("MainReport");
            var range = ws.Cells["A2"].LoadFromCollection(people, true);
            range.AutoFitColumns();
            await package.SaveAsync();
        }

        private static async Task SaveExcelFileWithFormattedHeaders(List<Users> people, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);
            var ws = package.Workbook.Worksheets.Add("MainReport");
            var range = ws.Cells["A2"].LoadFromCollection(people, true);
            range.AutoFitColumns();             // For Auto Fitting The Columns Below I have override the conventions for the column  ws.Column(3).Width = 20; // Overide to increase width

            //Formats the header

            //Columns Are Vertical |
            //Rows Are  Horizontal ___ 
            ws.Cells["A1"].Value = "Our Cool Report";  //This Indicates This Line Starts In Excel With A1
            ws.Cells["A1:C1"].Merge = true;           //This Indicates Cells Should Be Merged From A1 to C1
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //This Indicates That The Heading Our Cool Report Should be center Aligned This Can Be Done In ANother Way Shown Below
            ws.Row(1).Style.Font.Size = 16;          //This Indicates The Font Size
            ws.Row(1).Style.Font.Color.SetColor(Color.Gold);

            ws.Cells["D1"].Value = "Test Report";
            ws.Cells["D1:J1"].Merge = true;

            ws.Cells["D1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // For ALigning the Text in Center

            ws.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //For ALiging The Text In Row2 in Center
            ws.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Row(2).Style.Font.Bold = true;
            ws.Column(3).Width = 20;        // Cahnging The Column Width 

            await package.SaveAsync();
        }

        private static async Task SaveExcelFileWithFormattedHeaders2(List<Users> people, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);
            var ws = package.Workbook.Worksheets.Add("MainReport");
            var range = ws.Cells["A3"].LoadFromCollection(people, true);
            range.AutoFitColumns();



            ws.Cells["A1"].Value = "Customers Details";  //This Indicates This Line Starts In Excel With A1
            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Formats the header
            ws.Cells["A2"].Value = "Our Cool Report";  //This Indicates This Line Starts In Excel With A1
            ws.Cells["A2:C2"].Merge = true;           //This Indicates Cells Should Be Merged From A1 to C1
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //This Indicates That The Heading Our Cool Report Should be center Aligned This Can Be Done In ANother Way Shown Below
            ws.Row(1).Style.Font.Size = 16;          //This Indicates The Font Size
            ws.Row(1).Style.Font.Color.SetColor(Color.Gold);

            ws.Cells["D2"].Value = "Test Report";
            ws.Cells["D2:J2"].Merge = true;

            ws.Cells["D2:J2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Row(2).Style.Font.Bold = true;
            ws.Column(3).Width = 20;

            await package.SaveAsync();
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

    }
}
