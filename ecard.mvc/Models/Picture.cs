using System.Web;

namespace Ecard.Mvc.Models
{
    public class Picture
    {
        public Picture()
        {

        }
        public Picture(string url, string photo, int? width = null, int? height = null)
        {
            Url = url;
            Name = photo;
            Width = width;
            Height = height;
        }

        public HttpPostedFileBase File { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}