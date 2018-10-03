using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;


/// <summary>
/// HttpSend 的摘要说明
/// </summary>
public class HttpSend
{
	public HttpSend()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// post方法
    /// </summary>
    /// <param name="url">服务器URL</param>
    /// <param name="param">要发送的参数字符串</param>
    /// <returns>服务器返回字符串</returns>
    public static string postSend(string url, string param)
    {
        System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");
        byte[] postBytes = System.Text.Encoding.ASCII.GetBytes(param);


        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";//;charset=UTF-8
        req.ContentLength = postBytes.Length;

        try
        {
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(postBytes, 0, postBytes.Length);
            }

            using (WebResponse res = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                {
                    string strResult = sr.ReadToEnd();
                    return strResult;
                }
            }

        }
        catch (WebException ex)
        {
            return "无法连接到服务器\r\n错误信息：" + ex.Message;
        }

    }
    /// <summary>
    /// get方法
    /// </summary>
    /// <param name="url"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string getSend(string url, string param)
    {
        string address = url + "?" + param;
        Uri uri = new Uri(address);
        WebRequest webReq = WebRequest.Create(uri);

        try
        {
            using (HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse())
            {
                using (Stream respStream = webResp.GetResponseStream())
                {
                    using (StreamReader objReader = new StreamReader(respStream, System.Text.Encoding.GetEncoding("UTF-8")))
                    {
                        string strRes = objReader.ReadToEnd();
                        return strRes;
                    }
                } 
            }

        }
        catch (WebException ex)
        {
            return "无法连接到服务器/r/n错误信息：" + ex.Message;
        }
    }
}
