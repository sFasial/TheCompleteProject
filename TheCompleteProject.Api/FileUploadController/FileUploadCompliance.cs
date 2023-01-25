using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DemoModels;
using TheCompleteProject.Service.Validators;
using TheCompleteProject.Utility.BulkImport;
using NPOI.XSSF.UserModel;

namespace TheCompleteProject.Api.FileUploadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadCompliance : ControllerBase
    {
        [HttpPost]
        [Route(("ExcelBulkImport"))]
        public IActionResult ExcelBulkImport([FromQuery] UploadUser uploadUser)
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

                    if (uploadUserErrorDto.SelectMany(c => c.ErrorMessages).Count() > 0)
                    {
                        var _fileName = $"UploadUserErrorFile-{DateTime.Now:MMddyyyyHHmmss}.xlsx";
                        var workBook = new XSSFWorkbook();
                        var _uploadUserErrorSheet = "Upload Employee Error Sheet";
                        var _uploadUserSheet = workBook.CreateSheet(_uploadUserErrorSheet);
                        ExportImportHelper.WriteData(uploadUserErrorDto.Where(p => p.ErrorMessages.Count != 0).ToList(), workBook, _uploadUserSheet, false);

                        var memoryStream = new MemoryStream();
                        workBook.Write(memoryStream);

                        return File(memoryStream.ToArray(), "application/ynd.openxmlformats-officedocument.spreadsheetml.sheet", _fileName);
                    }
                    else
                    {
                        //success code
                    }
                }
            }

            throw new NotImplementedException();
        }

    }
}
