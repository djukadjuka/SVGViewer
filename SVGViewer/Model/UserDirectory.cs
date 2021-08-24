using SVGViewer.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGViewer.Model
{
    public class UserDirectory
    {
        public UserDirectory()
        {

        }

        public UserDirectory(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public ObservableCollection<UserDirectory> Directories { get; set; } = new ObservableCollection<UserDirectory>();

        //  Concat demands a non-null argument
        public IEnumerable Items { get { return Directories; } }

        private string _directoryPath;
        public String DirectoryPath {
            get
            {
                return this._directoryPath;
            }
            set
            {
                string path = value;
                // Directory to be set does not exist
                if (!Directory.Exists(value))
                {
                    string fallbackDir = AppConfigStuff.GetInstance()[AppConfigStuff.KEY_FALLBACK_DIRECTORY];
                    path = fallbackDir;
                    // Fallback directory does not exist, reset the config file
                    if (!Directory.Exists(fallbackDir))
                    {
                        AppConfigStuff.GetInstance().ResetConfigurationFile();
                        path = AppConfigStuff.GetInstance()[AppConfigStuff.KEY_FALLBACK_DIRECTORY];
                    }
                }
                this._directoryPath = path;
                this.RecurseDirectoryStructure(this, this._directoryPath);
            }
        }
        public String Name { get { return System.IO.Path.GetFileName(DirectoryPath); } }

        private void RecurseDirectoryStructure(UserDirectory parent, string startDirectory)
        {
            string[] subdirs = Directory.GetDirectories(startDirectory);
            foreach(string subdir in subdirs)
            {
                UserDirectory userSubdir = new UserDirectory(subdir);
                this.Directories.Add(userSubdir);
            }
        }

    }
}
