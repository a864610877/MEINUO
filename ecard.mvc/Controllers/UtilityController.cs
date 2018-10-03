
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;

namespace Ecard.Mvc.Controllers
{
    public class UtilityController : Controller
    {
        private readonly Site _site;
        private readonly SmsHelper _smsHelper;
        private readonly RandomCodeHelper _randomCodeHelper;
        private readonly LogHelper Logger;
        private readonly ICityService CityService;

        public UtilityController(Site site, SmsHelper smsHelper, RandomCodeHelper randomCodeHelper, LogHelper logger, ICityService cityService)
        {
            _site = site;
            _smsHelper = smsHelper;
            _randomCodeHelper = randomCodeHelper;
            Logger = logger;
            CityService = cityService;
        }

        static System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (Math.PI * (double)j) / dBaseAxisLen : (Math.PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        public ActionResult Code(string type, int width = 100, int height = 20)
        {
            type = IdentityCode + (type ?? "");
            type = Regex.Replace(type, "\\?", "");
            var text = RandomHelper.GenerateNumber(4);
            Session[type] = text;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                var v = random.Next(0, 5);
                var fontSize = (1 - v / 18f + 0.5) * width / 10.0f;
                var color = random.Next(0, 0xEEEEEE);
                g.DrawString(text[i].ToString(), new Font("宋体", (float)fontSize, FontStyle.Regular), new SolidBrush(Color.Black), 4 + width / 10.0f * i, 4);
            }
            //bitmap = TwistImage(bitmap, false, 7, 7);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return new FileStreamResult(ms, "image");
        }
        [Dependency]
        public RandomCodeHelper CodeHelper { get; set; }
        [Authorize]
        public ActionResult SendSmsCode(string number, string userName)
        {
            return null;
            //var msg = MessageFormator.Format(_site.MessageTemplateOfIdentity ?? "", _site);
            //var randomNumber = RandomHelper.GenerateNumber(6);

            //CodeHelper.CreateObject("sms", randomNumber, TimeSpan.FromMinutes(5));
            //CodeHelper.CreateObject("sms_mobile", number, TimeSpan.FromMinutes(5));
            //try
            //{ 
            //        msg = msg.Replace("#code#", randomNumber);
            //        msg = msg.Replace("#username#", userName);
            //        _smsHelper.Send (number, msg);
            //        return Json("验证短信发送成功." , JsonRequestBehavior.AllowGet);
                 
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(LogTypes.SendSmsCode, ex);
            //    return Json("验证短信发送失败: " + ex.Message, JsonRequestBehavior.AllowGet);
            //}
        }

        [Authorize]
        public ActionResult GetPasswordToken()
        {
            try
            {
                PasswordToken token = new PasswordToken
                                          {
                                              Rsa = (RSACryptoServiceProvider)RSACryptoServiceProvider.Create(),
                                              ChallengeData = Guid.NewGuid().ToString("N").Substring(0, 8)
                                          };
                var rsa = _randomCodeHelper.CreateObject(RandomCodeNames.PasswordToken, token, TimeSpan.FromMinutes(1));
                var p = token.Rsa.ExportParameters(false);
                return Json(new
                {
                    Success = true,
                    RsaModulus = p.Modulus.ToHexString(),
                    RsaExponent = p.Exponent.ToHexString(),
                    ChallengeData = rsa.ChallengeData.GetBytesAscii().ToHexString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new SimpleAjaxResult(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult Print(string content, string printerPage)
        {
            return View(printerPage, (object)content);
        }
         

        private const string IdentityCode = "__IdentityCode__";
        public static string GetCode(HttpContextBase httpContext, string code)
        {
            var v = (string)httpContext.Session[IdentityCode + code];
            httpContext.Session[IdentityCode + code] = null;
            return v;
        }

        [HttpPost]
        public ActionResult GetCity(int ProvinceId)
        {
            var query = CityService.Query(ProvinceId).Select(x => new IdNamePair() { Key = x.CityId, Name = x.Name }).ToList();
            return Json(query);

        }
    }
    public class PasswordToken
    {
        public RSACryptoServiceProvider Rsa { get; set; }
        public string ChallengeData { get; set; }
    }
}
