﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DoorUI.Areas.Admin.Controllers
{
    public class KindEditorController : Controller
    {
        //
        // GET: /Admin/KindEditor/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadImage()
        {
            string savePath = "/MicroMalls/CommodityImages/";
            string saveUrl = "/MicroMalls/CommodityImages/";
            string fileTypes = "gif,jpg,jpeg,png,bmp,flv,swf";
            int maxSize = 300000000;

            Hashtable hash = new Hashtable();

            HttpPostedFileBase file = Request.Files["imgFile"];
            if (file == null)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传目录不存在";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件大小超过限制";
                return Json(hash, "text/html;charset=UTF-8");
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件扩展名是不允许的扩展名";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            string filePath = dirPath + newFileName;
            file.SaveAs(filePath);
            string fileUrl = saveUrl + newFileName;

            hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;

            return Json(hash, "text/html;charset=UTF-8");
        }

        public ActionResult ProcessRequest()
        {
            //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //根目录路径，相对路径
            String rootPath = "/MicroMalls/CommodityImages/";
            //根目录URL，可以指定绝对路径，
            String rootUrl = "/MicroMalls/CommodityImages/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            //根据path参数，设置各路径和URL
            String path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = Server.MapPath(rootPath);
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = Server.MapPath(rootPath) + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                Response.Write("Access is not allowed.");
                Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                Response.Write("Parameter is not valid.");
                Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                Response.Write("Directory does not exist.");
                Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            //Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            //context.Response.Write(JsonMapper.ToJson(result));
            //context.Response.End();
            return Json(result, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }
    }

    public class NameSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.FullName.CompareTo(yInfo.FullName);
        }
    }
    public class SizeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.Length.CompareTo(yInfo.Length);
        }
    }
    public class TypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            return xInfo.Extension.CompareTo(yInfo.Extension);
        }
    }
}
