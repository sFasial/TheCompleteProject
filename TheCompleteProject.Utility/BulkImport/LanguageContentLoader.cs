using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Utility.FolderLocations;

namespace TheCompleteProject.Utility.BulkImport
{
    public static class LanguageContentLoader
    {
        public static Dictionary<string, string> en_US = new Dictionary<string, string>();
        public static void LanguageLoader(string folderPath)
        {
            string languageContent = "";
            string languageData = null;
            try
            {
                if (en_US == null)
                {
                    //languageContent = Path.Combine(folderPath + "\\Content\\Commercial\\en-US.json");
                    languageContent = Path.Combine(folderPath + FolderLocation.EN_US);
                    languageData = File.ReadAllText(languageContent);
                    en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);
                }
            }
            catch (Exception ex)
            {
            }
        }

        //public static string ReturnLanguageData(string key, string language)
        //{
        //    try
        //    {
        //        language = String.IsNullOrEmpty(language) ? "en-US" : language;
        //        switch (language)
        //        {
        //            case "en-US": return en_US[key];
        //            default: return en_US[key];
        //        }
        //        //return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception Multilingual Data:" + key.ToString());
        //        return key;
        //    }
        //    finally
        //    {
        //    }
        //}

        public static string ReturnLanguageData(string key, string language = "")
        {
            var languageContent = "";
            string languageData = null;
            try
            {
                //languageContent = Path.Combine(Directory.GetCurrentDirectory(),FolderLocation.EN_US);

                languageContent = Environment.CurrentDirectory;
                languageContent = languageContent + "\\wwwroot\\en_US.json";
                languageContent = languageContent.Replace("\\", @"/");

                languageData = File.ReadAllText(languageContent);

                en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);

                language = String.IsNullOrEmpty(language) ? "en-US" : language;
                switch (language)
                {
                    case "en-US": return en_US[key];
                    default: return en_US[key];
                }
                //return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Multilingual Data:" + key.ToString());
                return key;
            }
            finally
            {
            }
        }
    }
}
