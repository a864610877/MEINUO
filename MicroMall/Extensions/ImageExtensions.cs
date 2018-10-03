using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Oxite.Mvc.Extensions
{
    public static class ImageExtensions
    {
        public static Image GetThumbnailImage(this Image image, int width, int height)
        {
            var rate = image.Width / Convert.ToDecimal(image.Height);
            if (width / Convert.ToDecimal(height) > rate)
            {
                image = image.GetThumbnailImage(width, Convert.ToInt32(height * (1 / rate)), () => true, (IntPtr)0);
            }
            else
            {
                image = image.GetThumbnailImage(Convert.ToInt32(rate * height), height, () => true, (IntPtr)0);
            }
            return image;
        }
    }
}
