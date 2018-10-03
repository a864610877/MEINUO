using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Ecard.Services;

namespace Ecard.Mvc.SiteCssBuilders
{
    public class SiteCssBuilder
    {
        readonly StringBuilder _buffer = new StringBuilder();
        public void Add(ISiteCssBuilder s)
        {
            s.ToCssString(_buffer);
        }
        public void Save()
        {
            var filename = HttpContext.Current.Server.MapPath("~/content/site.css");
            //File.WriteAllText(filename, _buffer.ToString(), Encoding.UTF8);
        }
    }
    public interface ISiteCssBuilder
    {
        void ToCssString(StringBuilder builder);
    }
}
