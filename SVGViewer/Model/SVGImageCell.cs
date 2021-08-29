using Svg;
using SVGViewer.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SVGViewer.Model
{
    public class SVGImageCell
    {
        public SVGImageCell(string svgImageFilePath, Action<object> execute)
        {
            _svgImagePathCopyAction = new CommandBase(execute, (x) => true);

            ImagePath = svgImageFilePath;

            byte[] byteArray = Encoding.ASCII.GetBytes(ImagePath);
            
            SvgDocument document = SvgDocument.Open(ImagePath);

            Bitmap bmp = document.Draw();

            using(var memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                ImageData = new BitmapImage();
                ImageData.BeginInit();
                ImageData.StreamSource = memory;
                ImageData.CacheOption = BitmapCacheOption.OnLoad;
                ImageData.EndInit();
            }
        }


        private ICommand _svgImagePathCopyAction;
        public ICommand SvgImagePathCopyAction
        {
            get
            {
                return _svgImagePathCopyAction;
            }
            set { _svgImagePathCopyAction = value; }
        }


        public string ImagePath { get; set; }
        public BitmapImage ImageData { get; set; }
    }
}
