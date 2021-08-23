using Microsoft.Win32;
using SVGViewer.Command;
using System;
using System.Collections.Generic;
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
