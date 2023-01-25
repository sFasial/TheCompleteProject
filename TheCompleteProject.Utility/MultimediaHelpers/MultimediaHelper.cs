using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
