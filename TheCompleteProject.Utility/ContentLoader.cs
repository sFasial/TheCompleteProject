using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Utility.FolderLocations;

namespace TheCompleteProject.Utility
{
    public static class ContentLoader
    {
        public static Dictionary<string, string> en_US = new Dictionary<string, string>();

        public static void LanguageLoader(string folderPath)
        {
            try
            {
                string languageContent, languageData;
                if (en_US == null || en_US.Count <= 0)
                {
                    languageContent = Path.Combine(folderPath + FolderLocation.EN_US);
                    languageData = File.ReadAllText(languageContent);

                    var _en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);

                    if (_en_US != null)
                        en_US = _en_US;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string ReturnLanguageData(string key, string language)
        {
            var languageContent = "";
            string languageData = null;
            try
            {
                languageContent = Path.Combine(Directory.GetCurrentDirectory() + FolderLocation.EN_US);
                languageData = File.ReadAllText(languageContent);

                en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);
                language = String.IsNullOrEmpty(language) ? "en_US" : language;
                switch (language)
                {
                    case "en_US":
                        return en_US[key];
                    default:
                        return en_US[key];
                }
            }
            catch (Exception)
            {
                return key;
            }
        }
    }
}
