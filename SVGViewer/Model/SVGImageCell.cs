using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SVGViewer.Model
{
    public class SVGImageCell
    {
        public SVGImageCell(string svgImageFilePath)
        {
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

        public string ImagePath { get; set; }
        public BitmapImage ImageData { get; set; }
    }
}
