using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodyCRM.Utility.ImageCompound
{
    public class ImageCompound
    {
        
        /// <summary>
        /// 创建海报
        /// </summary>
        /// <param name="backGroundUrl"></param>
        /// <param name="qrUrl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static System.IO.Stream CreatePoster(string backGroundUrl, string qrUrl, int x, int y, int width, int height,SizeProperty name,SizeProperty mobile)
        {
            //Bitmap destBmp = new Bitmap(backGroundUrl);
            //Bitmap destBmp1 = new Bitmap(qrUrl);
            Image imgBack = System.Drawing.Image.FromFile(backGroundUrl);     //相框图片  
            Image img = System.Drawing.Image.FromFile(qrUrl);

            Bitmap map = new Bitmap(imgBack, new Size(imgBack.Width, imgBack.Height));//照片图片
            Graphics g = Graphics.FromImage(map);
            //g.DrawImage(imgBack, 0, 0, 148, 124);
            g.DrawImage(img, x, y, width, height);
            g.DrawString(name.value, name.font, name.brushes, name.x, name.y);
            g.DrawString(mobile.value, mobile.font, mobile.brushes, mobile.x, mobile.y);
            g.Flush();
            GC.Collect();
            var stream = new System.IO.MemoryStream();
            map.Save(stream,System.Drawing.Imaging.ImageFormat.Jpeg);
            return stream;

        }
    }

    public class SizeProperty
    {
        public string value { get; set; }
        public Font font { get; set; }
        public Brush brushes { get; set; }

        public float x { get; set; }

        public float y { get; set; }

    }
}
