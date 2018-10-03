using Ecard.Mvc;
using Ecard.Services;
using MicroMall.Models;
using Microsoft.Practices.Unity;
using Moonlit;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class UnityController : Controller
    {

        private readonly IUnityContainer _container;
        private readonly IAccountService _accountService;
        private readonly ICityService _cityService;
        private readonly IMembershipService _membershipService;
        private readonly ISetWeChatService _setWeChatService;
        private readonly SecurityHelper _securityHelper;

        public UnityController(IUnityContainer container, ICityService cityService, IAccountService accountService, IMembershipService membershipService, ISetWeChatService setWeChatService, SecurityHelper securityHelper)
        {
            _container = container;
            _cityService = cityService;
            _accountService = accountService;
            _membershipService = membershipService;
            _setWeChatService = setWeChatService;
            _securityHelper = securityHelper;
        }
        //
        // GET: /Unity/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCity(int ProvinceId)
        {
           var query = _cityService.Query(ProvinceId).ToList().Select(x => new SelectModel() { Id = x.CityId, Name = x.Name });
           return Json(query);
        }
        /// <summary>
        /// 验证推荐码是否存在
        /// </summary>
        /// <param name="orangeKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyOrangeKey(string orangeKey)
        {
            var item = _accountService.GetByorangeKey(orangeKey);
            if (item == null)
                return Json(-1);//失败
            else
                return Json(0); //成功
        }
        [HttpPost]
        public ActionResult VerifyMobile(string Mobile)
        {
            var item = _membershipService.GetByMobile(Mobile);
            if (item == null)
                return Json(0); //成功
            else
                return Json(-1);//失败
                
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

        private const string IdentityCode = "__IdentityCode__";
        public static string GetCode(HttpContextBase httpContext, string code)
        {
            var v = (string)httpContext.Session[IdentityCode + code];
            httpContext.Session[IdentityCode + code] = null;
            return v;
        }

      

    }
}
