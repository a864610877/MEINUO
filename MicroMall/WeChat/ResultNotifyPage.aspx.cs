using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace MicroMall.WeChat
{
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lock (this)
            {
                //try
                //{
                    Log.Debug(this.GetType().ToString(), "--进入回调--");
                    ResultNotify resultNotify = new ResultNotify(this);
                    resultNotify.ProcessNotify();
                //}
                //catch(Exception ex)
                //{
                //    Log.Error(this.GetType().ToString(),ex.Message);
                //}
            }
        }
    }
}