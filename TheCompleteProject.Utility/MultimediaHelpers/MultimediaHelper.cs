using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s;
using TheCompleteProject.Utility.BulkImport;
using TheCompleteProject.Utility.FolderLocations;

namespace TheCompleteProject.Utility.MultimediaHelpers
{
    public static class MultimediaHelper
    {

        public static IDictionary<string, string> ContentFolders;
        public static IDictionary<string, string> ContentFileExtensions;

        public static void Init()
        {
            ContentFolders = new Dictionary<string, string>();
            ContentFolders.Add("A", "Api/Audio");
            ContentFolders.Add("V", "Api/Video");
            ContentFolders.Add("I", "Api/Images");
            ContentFolders.Add("D", "wwwroot/Documents");
            ContentFolders.Add("X", "wwwroot/companyExcelDocuments");

            ContentFileExtensions = new Dictionary<string, string>();
            ContentFileExtensions.Add("A", "mp3");
            ContentFileExtensions.Add("V", "3gp");
            ContentFileExtensions.Add("I", "jpg");
            ContentFileExtensions.Add("D", "pdf");
            ContentFileExtensions.Add("X", "xls");
        }

        #region Old Working Code
        /*
        [HttpPost]
        [Route("Save")]
        public string SaveMultimedia([FromBody] string data, string multimediaType, string groupIdFolder, string mediaExtension, string local_fileName, string moduleName = "IntercomGroups")
        {
            string fileName = "";
            string WebFilePath = "";

            var path = "wwwroot/images";
            //  Path.GetFullPath(path);

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    if (multimediaType == "YT")
                    {
                        return data;
                    }

                    if (ContentFolders == null)
                        Init();
                    string subPath = ContentFolders[multimediaType] + moduleName + "/" + groupIdFolder;
                    subPath = path + "/" + moduleName + "/" + groupIdFolder;
                    subPath = path + "/" + groupIdFolder;
                    byte[] dataBytes = Convert.FromBase64String(data);
                    //bool exists = Directory.Exists(HostingEnvironment.MapPath(subPath));
                    var exists = Directory.Exists(Path.Combine(subPath));

                    if (!exists)
                    {
                        Directory.CreateDirectory(Path.Combine(subPath));
                    }



                    string fileExtension = (multimediaType == "D" || multimediaType == "A") ? mediaExtension : ContentFileExtensions[multimediaType];

                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    if (!string.IsNullOrEmpty(local_fileName))
                    {
                        fileName += "_" + local_fileName.Replace(" ", "");
                    }

                    fileName += "." + fileExtension;

                    WebFilePath = ContentFolders[multimediaType] + moduleName + "/" + groupIdFolder + "/" + fileName;
                    WebFilePath = WebFilePath.Replace("//", "/");

                    string fullFilePath = Path.Combine(ContentFolders[multimediaType] + moduleName + "/" + groupIdFolder + "/" + fileName);

                    if (multimediaType == "I")
                        SaveImage(data, fullFilePath);
                    else
                        SaveAudioOrVideo(dataBytes, fullFilePath);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //ExceptionLogging.LogExceptionToDB(ex);
                }
            }
            return WebFilePath;
        } 
        */
        #endregion

        public static string SaveMultimedia(string data, string path, string mediaExtension, string local_fileName, string moduleName = "", string multimediaType = "I", string groupIdFolder = "")
        {
            string fileName = "";
            string WebFilePath = "";

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    if (ContentFolders == null)
                        Init();

                    string subPath = path + "/" + moduleName;

                    byte[] dataBytes = Convert.FromBase64String(data);
                    var exists = Directory.Exists(Path.Combine(subPath));

                    if (!exists)
                    {
                        Directory.CreateDirectory(Path.Combine(subPath));
                    }


                    string fileExtension = (multimediaType == "D" || multimediaType == "A") ? mediaExtension : ContentFileExtensions[multimediaType];


                    //Creating Unique File Name
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    if (!string.IsNullOrEmpty(local_fileName))
                    {
                        fileName += "_" + local_fileName.Replace(" ", "");
                    }
                    fileName += "." + fileExtension;


                    //If GroupId Folder is not null it will create a subfolder in wwroot/images/GroupIdFolder like this
                    if (!string.IsNullOrEmpty(groupIdFolder))
                    {
                        WebFilePath = subPath + "/" + groupIdFolder + "/" + fileName;
                    }


                    WebFilePath = subPath + "/" + fileName;
                    WebFilePath = WebFilePath.Replace("//", "/");

                    string fullFilePath = Path.Combine(subPath + "/" + fileName);

                    SaveAudioOrVideo(dataBytes, fullFilePath);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //ExceptionLogging.LogExceptionToDB(ex);
                }
            }
            return WebFilePath;
        }


        private static void SaveAudioOrVideo(byte[] dataBytes, string FullFilePath)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(dataBytes, 0, dataBytes.Length))
                {

                    FileInfo imageFile = new FileInfo(FullFilePath);
                    bool fileExists = imageFile.Exists;

                    using (FileStream fs = new FileStream(FullFilePath, FileMode.Create))
                    {
                        if (fileExists == false)
                        {
                            ms.Write(dataBytes, 0, dataBytes.Length);
                            ms.WriteTo(fs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //ExceptionLogging.LogExceptionToDB(ex);
            }
        }


        public static string GetBase64String(MemoryStream stream)
        {
            var fileByte = stream.ToArray();
            return Convert.ToBase64String(fileByte);
        }
        public static string GetPath(string location)
        {
            var path = Environment.CurrentDirectory;
            path = path + location;
            return path.Replace(@"\", @"/");
        }

        public static MemoryStream GenerateFile<T>(List<T> list , string fileName , string sheetName)
        {
            var _fileName = $"{fileName}-{DateTime.Now:MMddyyyyHHMMSS}.xlsx";
            var _workbook = new XSSFWorkbook();
            var _sheetName = sheetName;
            var _sheet = _workbook.CreateSheet(_sheetName);
            ExportImportHelper.WriteData(list, _workbook, _sheet, false);
            var memoryStream = new MemoryStream();
            _workbook.Write(memoryStream);
            return memoryStream;
        }

        public static byte[] GenerateFileFromBase64String(string bytes)
        {
            try
            {
                var _bytes = Convert.FromBase64String(bytes);
                var path = GetPath(FolderLocation.TESTING_FILES);
                var fileName = path + $"/error.xlsx";
                File.WriteAllBytes(fileName, _bytes);
                return File.ReadAllBytes(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   

        //REPORTS CODE REFRENCE FFC 
        public static byte[] GetBytesForExportToExcel_MultipleSheets(List<MultipleExcelSheetsDto> sheets)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage p = new ExcelPackage();


            int workSheetNo = 0;

            foreach (var s in sheets)
            {
                var dt = s.dt;
                p.Workbook.Worksheets.Add(s.WorkSheetName);
                ExcelWorksheet ws = p.Workbook.Worksheets[workSheetNo];
                ws.Name = s.WorkSheetName; //Setting Sheet's name
                ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                ws.Cells[1, 1].Value = s.ReportHeading; // Heading Name
                ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true; //Merge columns start and end range
                ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true; //Font should be bold
                ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet 
                                                                                                                 //ws.Cells[1, 1, 1, dt.Columns.Count].AutoFitColumns();
                int rowIndex = 2;

                CreateHeader(ws, ref rowIndex, dt);
                CreateData(ws, ref rowIndex, dt);
                ws.Cells.AutoFitColumns(10, 30);
                workSheetNo++;
            }

            byte[] filedata = p.GetAsByteArray();
            //File.WriteAllBytes(@"D:\temp\reports\VisitReport.xls", filedata);

            return filedata;
        }
        private static void CreateHeader(ExcelWorksheet ws, ref int rowIndex, DataTable dt)
        {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns) //Creating Headings
            {
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting Top/left,right/bottom borders.
                var border = cell.Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell.Style.Font.Bold = true;

                //Setting Value in cell
                cell.Value = dc.ColumnName;
                colIndex++;
            }
        }

        private static void CreateData(ExcelWorksheet ws, ref int rowIndex, DataTable dt)
        {
            int colIndex = 0;
            foreach (DataRow dr in dt.Rows) // Adding Data into rows
            {
                colIndex = 1;
                rowIndex++;

                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    if (dc.ColumnName == "NotifyManager" || dc.ColumnName == "Admin" || dc.ColumnName == "EntireDayTracking" || dc.ColumnName == "NotifyAdmin")
                    {
                        if (Convert.ToString(dr[dc.ColumnName]) == "Disabled")
                        {
                            cell.Value = (dr[dc.ColumnName]);
                            cell.Style.Font.Color.SetColor(Color.Black);
                        }
                        else
                        {
                            cell.Value = (dr[dc.ColumnName]);
                            cell.Style.Font.Color.SetColor(Color.Green);
                        }
                    }
                    else
                    {
                        //Setting Value in cell
                        cell.Value = (dr[dc.ColumnName]);
                    }

                    //Setting borders of cell
                    var border = cell.Style.Border;
                    border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    colIndex++;
                }
            }
        }
    }
}
