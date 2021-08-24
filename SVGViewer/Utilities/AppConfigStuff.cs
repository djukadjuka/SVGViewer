using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGViewer.Utilities
{
    public class AppConfigStuff
    {
        public static string KEY_MAIN_DIRECTORY = "MAIN_DIRECTORY";
        public static string KEY_CONFIGURATION_FILE_NAME = "CONFIG_FILE_NAME";
        public static string KEY_FALLBACK_DIRECTORY = "FALLBACK_DIR";

        public static string VALUE_CONFIGURATION_FILE_NAME = "CONFIG.json";
        public static string VALUE_FALLBACK_DIRECTORY = System.AppDomain.CurrentDomain.BaseDirectory;

        public Dictionary<string, string> AppConfigParameters = new Dictionary<string, string>();

        public void AddAndSaveAttribute(string key, string value)
        {
            AppConfigParameters[key] = value;
            this.SaveConfigurationFile();
        }

        public void ReloadConfigurationFile()
        {
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, VALUE_CONFIGURATION_FILE_NAME);
            if (!File.Exists(filePath))
            {
                ResetConfigurationFile();   
            }

            string jsonText = File.ReadAllText(filePath);
            this.AppConfigParameters = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonText);
        }

        public void ResetConfigurationFile()
        {
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, VALUE_CONFIGURATION_FILE_NAME);
            AppConfigParameters[KEY_MAIN_DIRECTORY] = System.AppDomain.CurrentDomain.BaseDirectory;
            AppConfigParameters[KEY_CONFIGURATION_FILE_NAME] = VALUE_CONFIGURATION_FILE_NAME;
            AppConfigParameters[KEY_FALLBACK_DIRECTORY] = VALUE_FALLBACK_DIRECTORY;

            string json = JsonConvert.SerializeObject(AppConfigParameters);
            File.WriteAllText(filePath, json);
        }

        public void SaveConfigurationFile()
        {
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, VALUE_CONFIGURATION_FILE_NAME);
            string json = JsonConvert.SerializeObject(AppConfigParameters);
            File.WriteAllText(filePath, json);
        }

        public string this[string index]
        {
            get { return this.AppConfigParameters[index]; }
            set { this.AppConfigParameters[index] = value; }
        }

        #region SINGLETON
        public static AppConfigStuff instance;

        private AppConfigStuff()
        {
            // Check base directory for configuration file
            ReloadConfigurationFile();
        }

        public static AppConfigStuff GetInstance()
        {
            if (instance == null)
            {
                instance = new AppConfigStuff();
            }
            return instance;
        }
        #endregion
    }
}
