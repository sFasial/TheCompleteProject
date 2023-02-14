using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Utility.MultimediaHelpers;

namespace TheCompleteProject.Utility
{
    public static class ExcelHelper
    {
        public async static Task<DataTable> GetExcelData(IFormFile file, string filePath)
        {
            var ds = new DataSet();
            var saveFilePath = MultimediaHelper.GetPath(filePath) + "/" + Path.GetFileName(file.FileName);
            using (var stream = new FileStream(saveFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }

            var excelConnection = Constants.OLEDB_ConnectionString.Replace("#fullExcelPath#", saveFilePath);
            using (var execlData = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", excelConnection))
            {
                execlData.TableMappings.Add("Table", "Excel");
                execlData.Fill(ds);
            }
            if (ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }
    }
}

