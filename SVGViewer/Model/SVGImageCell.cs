using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGViewer.Model
{
    public class SVGImageCell
    {
        public SVGImageCell()
        {

        }

        public string ImagePath { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
