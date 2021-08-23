using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGViewer.Model
{
    public class UserDirectory
    {
        public ObservableCollection<UserDirectory> Directories { get; set; } = new ObservableCollection<UserDirectory>();

        //  Concat demands a non-null argument
        public IEnumerable Items { get { return Directories; } }

        public String DirectoryPath { get; set; }
        public String Name { get { return System.IO.Path.GetFileName(DirectoryPath); } }

    }
}
