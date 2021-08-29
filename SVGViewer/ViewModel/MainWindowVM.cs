using Microsoft.Win32;
using SVGViewer.Command;
using SVGViewer.Model;
using SVGViewer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace SVGViewer.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {
            this.MainDirectory = AppConfigStuff.GetInstance()[AppConfigStuff.KEY_MAIN_DIRECTORY];
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

                this.DirectoryStructure.Clear();
                this.DirectoryStructure.Add(new UserDirectory(MainDirectory));
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
                return _directoryStructure;
            }
            set
            {
                _directoryStructure = value;
                OnPropertyChanged(nameof(DirectoryStructure));
            }
        }



        private ObservableCollection<SVGImageCell> _svgImages;
        public ObservableCollection<SVGImageCell> SVGImages
        {
            get { return _svgImages; }
            set { 
                _svgImages = value;
                OnPropertyChanged();
            }
        }



        private SVGImageCell _selectedSvgRow;
        public SVGImageCell SelectedSvgRow
        {
            get { return _selectedSvgRow; }
            set 
            { 
                _selectedSvgRow = value; 
            }
        }



        private string _statusBarText = " ";
        public string StatusBarText
        {
            get { return _statusBarText; }
            set 
            { 
                _statusBarText = value;
                OnPropertyChanged();
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



        private ICommand _copyMainDirectoryPathCommand;
        public ICommand CopyMainDirectoryPathCommand
        {
            get
            {
                if(_copyMainDirectoryPathCommand == null)
                {
                    _copyMainDirectoryPathCommand = new CommandBase((x) => CopyMainDirectoryPath(), (x) => true);
                }
                return _copyMainDirectoryPathCommand;
            }
        }



        private ICommand _exitApplicationCommand;
        public ICommand ExitApplicationCommand
        {
            get {
                if(_exitApplicationCommand == null)
                {
                    _exitApplicationCommand = new CommandBase((x) => System.Windows.Application.Current.Shutdown(), (x) => true);
                }
                return _exitApplicationCommand; 
            }
        }



        private ICommand _svgImagePathCopyAction;
        public ICommand SvgImagePathCopyAction
        {
            get 
            {
                if(_svgImagePathCopyAction == null)
                {
                    _svgImagePathCopyAction = new CommandBase((x) => SvgImagePathCopyActionFunction(), (x) => true);
                }
                return _svgImagePathCopyAction; 
            }
            set { _svgImagePathCopyAction = value; }
        }



        private void SelectMainDirectory()
        {
            try
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowNewFolderButton = false;
                    dialog.SelectedPath = MainDirectory;

                    DialogResult result = dialog.ShowDialog();

                    if (!result.Equals(DialogResult.OK))
                    {
                        return;
                    }

                    MainDirectory = dialog.SelectedPath;
                    AppConfigStuff.GetInstance().AddAndSaveAttribute(AppConfigStuff.KEY_MAIN_DIRECTORY, MainDirectory);
                }
            }
            catch(Exception ex)
            {
                InfoLogger.ShowInformationText("Error Opening File", ex.Message);
                MainDirectory = AppConfigStuff.GetInstance()[AppConfigStuff.KEY_MAIN_DIRECTORY];
            }
        }
        private void CopyMainDirectoryPath()
        {
            System.Windows.Forms.Clipboard.SetText(MainDirectory);
            this.StatusBarText = FormatStatusBarText($"Directory {MainDirectory} copied to clipboard.");
        }
        private void SvgImagePathCopyActionFunction()
        {
            this.StatusBarText = FormatStatusBarText($"Directory {SelectedSvgRow.ImagePath} copied to clipboard.");
        }

        public void SelectedTreeItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            dynamic x = sender;
            UserDirectory selectedDir = x.SelectedItem;
            string directoryPath = selectedDir.DirectoryPath;

            string[] svgImageFilePaths = Directory.GetFiles(directoryPath, "*.svg");
            
            ObservableCollection<SVGImageCell> collection = new ObservableCollection<SVGImageCell>();
            
            foreach(string svgImageFilePath in svgImageFilePaths)
            {
                collection.Add(new SVGImageCell(svgImageFilePath, (y) => SvgImagePathCopyActionFunction() ));
            }

            SVGImages = collection;
        }

        public string FormatStatusBarText(string text)
        {
            return $">> {text}";
        }
        #endregion
    }
}
