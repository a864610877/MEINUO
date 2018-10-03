using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;

namespace Ecard.Services
{
   public class SmsSendHelper
    {
        public static string SmsUrl = @"http://kltx.sms10000.com.cn/sdk/SMS";
        public static string QueryTemplate = "cmd=getnum&uid=7053{0}&psw={1}";
        public static string ModifPswTemplate = "cmd=modifpsw&uid=7053{0}&psw={1}&newpsw={2}";
        public static string TSendTemplate = "cmd=tsend&uid=7053{0}&psw={1}&mobiles={2}&msgid={3}&senddate={4}&sendtime={5}&msg={6}";
        public static string SendTemplate = "cmd=send&uid=7053{0}&psw={1}&mobiles={2}&msgid={3}&msg={4}";
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="purl">发送的URL</param>
        /// <param name="str">参数</param>
        /// <returns></returns>
        public static string SendSms(string purl, string str)
        {
            try
            {
                byte[] data = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
                // 准备请求 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(purl);

                //设置超时
                req.Timeout = 10000;
                req.Method = "Post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                Stream stream = req.GetRequestStream();
                // 发送数据 
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = rep.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder sb = new StringBuilder("");
                while (count > 0)
                {
                    String readstr = new String(read, 0, count);
                    sb.Append(readstr);
                    count = readStream.Read(read, 0, 256);
                }

                rep.Close();
                readStream.Close();

                return sb.ToString();

            }
            catch (Exception ex)
            {
                return "posterror";
                // ForumExceptions.Log(ex);
            }
        }
        /// <summary>
        /// 32位MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd532(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;

        }
        /// <summary>
        /// 16位
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd516(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");

            t2 = t2.ToLower();

            return t2;
        }

        public static CookieContainer cookieContainerSMS = new CookieContainer();


        public string HttpSMSPost(HttpWebRequest hr, string url, string parameters)
        {
            string strRet = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(parameters);

            try
            {
                hr.KeepAlive = false;
                hr.Method = "POST";
                hr.ContentType = "application/x-www-form-urlencoded";
                hr.ContentLength = data.Length;
                Stream newStream = hr.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)hr.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strRet = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();

                return strRet;

            }
            catch (Exception ex)
            {
                strRet = "-200";
            }
            return strRet;
        }

        public string LogOn(string UserID,string UserName,string password)
        {
           // Fag = 1;
            string url = "http://www.lanz.net.cn/LANZGateway/Login.asp";
            string SMSparameters = "UserID=" + UserID + "&Account=" + UserName + "&Password=" + password;

            string targeturl = url.Trim().ToString();
            HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
            hr.CookieContainer = cookieContainerSMS;
            string res = HttpSMSPost(hr, url, SMSparameters);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(res);
            string ActiveID="";
            string errorNum = xml.SelectSingleNode("/LANZ_ROOT/ErrorNum").InnerText;  //获取是否登陆成功
            if (errorNum == "0")
            {
                ActiveID = xml.SelectSingleNode("/LANZ_ROOT/ActiveID").InnerText;  //获取成功后的ActiveID 
            }
            return ActiveID;
        }

        public string Send(string msg, string ActiveID, string Phone)
        {
            string url = "http://www.lanz.net.cn/LANZGateway/SendSMS.asp";
            string content = System.Web.HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("gb2312"));

            string SMSparameters = "ActiveID=" + ActiveID + "&SMSType=1" + "&Phone=" +Phone + "&Content=" + content;
            string targeturl = url.Trim().ToString();
            HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
            hr.CookieContainer = cookieContainerSMS;

            string res = HttpSMSPost(hr, url, SMSparameters);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(res);
            string errorNum = xml.SelectSingleNode("/LANZ_ROOT/ErrorNum").InnerText;  //获取是否登陆成功

            if (errorNum == "0")
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="msg">短信内容</param>
        /// <param name="strPhone">手机号码，多个手机号用半角逗号分开，最多1000个</param>
        /// <param name="strReg">注册号（由华兴软通提供）</param>
        /// <param name="strPwd">密码（由华兴软通提供）</param>
        /// <returns></returns>
        public static string Sends(string msg, string strPhone, string strReg, string strPwd)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");

            //以下参数为所需要的参数，测试时请修改
            //string strReg = "101100-WEB-HUAX-111111";   //注册号（由华兴软通提供）
            //string strPwd = "AAAAAAAA";                 //密码（由华兴软通提供）
            string strSourceAdd = "";                   //子通道号，可为空（预留参数）
            //string strPhone = "13391750223,18701657767";//手机号码，多个手机号用半角逗号分开，最多1000个
            string strContent = HttpUtility.UrlEncode(msg, myEncode);
            //短信内容

            string url = "http://www.stongnet.com/sdkhttp/sendsms.aspx";  //华兴软通发送短信地址

            //要发送的内容
            string strSend = "reg=" + strReg + "&pwd=" + strPwd + "&sourceadd=" + strSourceAdd +
                             "&phone=" + strPhone + "&content=" + strContent;

            //发送
            string strRes = HttpSend.postSend(url, strSend);
            return strRes;
        }
    }
}
