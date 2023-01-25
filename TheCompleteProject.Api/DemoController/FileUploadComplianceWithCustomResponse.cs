using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Api.Controllers;
using TheCompleteProject.ModelsAndDto_s.DemoModels;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Service.Validators;
using TheCompleteProject.Utility.BulkImport;
using TheCompleteProject.Utility.FolderLocations;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Api.DemoController
{
    public class FileUploadComplianceWithCustomResponse : BaseController
    {
        private readonly IUserService _userService;

        public FileUploadComplianceWithCustomResponse(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsersReport")]
        public async Task<Dictionary<string, object>> GenerateUser()
        {
            var users = await _userService.GetUsersAsync();
            var result = users.ToList();
            var fileName = $"GenerateUsers={DateTime.Now:MMddyyyyHHmmss}.xlsx";
            var _workbook = new XSSFWorkbook();
            var genratedUsersSheet = "GeneratedUsersSheet";
            var uploadUsersSheet = _workbook.CreateSheet(genratedUsersSheet);

            ExportImportHelper.WriteData(result, _workbook, uploadUsersSheet, false);
            var memoryStream = new MemoryStream();
            _workbook.Write(memoryStream);
            var base64Data = MultimediaHelper.GetBase64String(memoryStream);
            return ApiResponse("EMP101",
                new {
                    base64String = base64Data
                }); ;
        }

        [HttpPost]
        [Route(("ExcelBulkImportWithCustomReturnType"))]
        public async Task<Dictionary<string, object>> ExcelBulkImportWithCustomReturnType ([FromQuery] UploadUser uploadUser)
        {
            var strFileNameWithPath = uploadUser.file.FileName;
            var strFileType = Path.GetExtension(strFileNameWithPath);
            var strFileName = Path.GetFileName(strFileNameWithPath);
            // var saveLocation = Path.GetFileName(strFileNameWithPath);

            var path = Environment.CurrentDirectory;
            path = path + "\\wwwroot\\Upload";
            path = path.Replace("\\", @"/");

            var filePath = Path.Combine(path, uploadUser.file.FileName);
            filePath = filePath.Replace("\\", @"/");


            if (strFileType == ".xls" || strFileType == ".xlsx" || strFileType == ".XLS")
            {
                //To Save A File on A Path Currently commented It is Working
                //using (Stream fileStream = new FileStream(filePath,FileMode.Create,FileAccess.Write))
                //{
                //    uploadUser.file.CopyTo(fileStream);
                //}

                List<UploadUserErrorDto> uploadUserErrorDto = new List<UploadUserErrorDto>();
                List<UploadUserSuccessDto> uploadUserSuccessDto = new List<UploadUserSuccessDto>();

                var data = ExportImportHelper.GetDataTable(uploadUser.file);
                var userData = (from DataRow dr in data.Rows
                                select new UploadUserErrorDto
                                {
                                    UserName = dr["UserName"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    Password = dr["Password"].ToString()
                                });

                var uploadUserValidator = new UploadUserValidator(userData);
                foreach (var user in userData)
                {
                    ValidationResult result = uploadUserValidator.Validate(user);
                    user.ErrorMessages.AddRange(result.Errors.Select(x => x.ErrorMessage));

                    if (user.UserName == "")
                    {
                        user.ErrorMessages.Add(LanguageContentLoader.ReturnLanguageData("EMP300"));
                    }
                    if (user.Email == "")
                    {
                        user.ErrorMessages.Add(LanguageContentLoader.ReturnLanguageData("EMP301"));
                    }
                    if (user.ErrorMessages.Count > 0)
                    {
                        uploadUserErrorDto.Add(user);
                    }
                    else
                    {
                        var usr = new UploadUserSuccessDto();
                        usr.UserName = user.UserName;
                        usr.Email = user.Email;
                        usr.Password = user.Password;
                        uploadUserSuccessDto.Add(usr);
                    }
                }

                //IF AND ELSE IS RETURNING BASE 64 STRING BUT IN ELSE I HAVE MADE A FUNCTION TO LOOK IT CLEAN
                if (uploadUserErrorDto.SelectMany(c => c.ErrorMessages).Count() > 0)
                {
                    var fileName = $"UploadUsers={DateTime.Now:MMddyyyyHHmmss}.xlsx";
                    var _workbook = new XSSFWorkbook();
                    var genratedUsersSheet = "UploadUsersSheet";
                    var uploadUsersSheet = _workbook.CreateSheet(genratedUsersSheet);
                    ExportImportHelper.WriteData(uploadUserErrorDto.Where(p => p.ErrorMessages.Count != 0).ToList(), _workbook, uploadUsersSheet, false);
                    var memoryStream = new MemoryStream();
                    _workbook.Write(memoryStream);

                    var base64Data = MultimediaHelper.GetBase64String(memoryStream);
                    return ApiResponse("EMP101",
                        new
                        {
                            base64String = base64Data
                        }); ;
                }
                else
                {
                    //success code
                    var stream = MultimediaHelper.GenerateFile(uploadUserSuccessDto, Path.GetFileNameWithoutExtension(uploadUser.file.FileName), "Upload Users");
                    var base64Data = MultimediaHelper.GetBase64String(stream);
                    var toSavePath = MultimediaHelper.GetPath(FolderLocation.TESTING_FILES);
                    var _filePath = MultimediaHelper.SaveMultimedia(base64Data, toSavePath, "xlsx", Path.GetFileNameWithoutExtension(uploadUser.file.FileName), "", "X");

                    return ApiResponse("EMP100",
                       new
                       {
                           base64String = base64Data
                       }); ;
                }
            }

            return FailureResponse("EMP101", null);
        }
    }
}
