using Microsoft.Win32;
using SVGViewer.Command;
using SVGViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SVGViewer.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {

        }

        #region PROPERTIES

        private string _mainDirectory;
        public string MainDirectory
        {
            get
            {
                if (_mainDirectory == null)
                {
                    _mainDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                }
                return _mainDirectory;
            }
            set
            {
                _mainDirectory = value;
                OnPropertyChanged(nameof(MainDirectory));

                ObservableCollection<UserDirectory> directoryStructure = new ObservableCollection<UserDirectory>();
                string [] dirs = Directory.GetDirectories(MainDirectory);
                Console.WriteLine(MainDirectory);
                foreach(string subdir in dirs)
                {
                    Console.WriteLine(subdir);
                }
            }
        }

        public ObservableCollection<UserDirectory> _directoryStructure;
        public ObservableCollection<UserDirectory> DirectoryStructure
        {
            get
            {
                if (_directoryStructure == null)
                {
                    _directoryStructure = new ObservableCollection<UserDirectory>();
                    UserDirectory root_dir = new UserDirectory();
                    root_dir.DirectoryPath = MainDirectory;
                    _directoryStructure.Add(root_dir);
                }
                Console.WriteLine(_directoryStructure);
                return _directoryStructure;
            }
            set
            {
                _directoryStructure = value;
                OnPropertyChanged(nameof(DirectoryStructure));
            }
        }


        #endregion

        #region COMMANDS

        private ICommand _selectMainDirectoryCommand;
        public ICommand SelectMainDirectoryCommand
        {
            get
            {
                if(_selectMainDirectoryCommand == null)
                {
                    _selectMainDirectoryCommand = new CommandBase((x) => SelectMainDirectory(), (x)=>true);
                }
                return _selectMainDirectoryCommand;
            }
        }

        private void SelectMainDirectory()
        {
            using(FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;
                dialog.SelectedPath = MainDirectory;

                DialogResult result = dialog.ShowDialog();

                if (!result.Equals(DialogResult.OK))
                {
                    return;
                }

                MainDirectory = dialog.SelectedPath;
            }
        }

        #endregion
    }
}
