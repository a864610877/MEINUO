using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace System.Web.Mvc
{
    public static class MvcPage
    {
        
        public static string AjaxPager(int PageIndex, int PageSize, int Total)
        {
            int[] Size = { 10, 50, 100, 200, 300, 400, 500 };

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='pagin'>");
            sb.Append("<div class='message'>共<i class='blue'>" + Total.ToString() + "</i>条记录，当前显示第&nbsp;<i class='blue'>" + PageIndex.ToString() + "&nbsp;</i>页 每页<select class='selectSize' onchange='selectInput(this)' style='opacity:1;'>");
            string op = "";
            foreach (int item in Size)
            {
                if (item == PageSize)
                    op += "<option selected value="+item+">" + item + "</option>";
                else
                    op += "<option value=" + item + ">" + item + "</option>";
            }
            sb.Append(op);
            sb.Append("</select>条</div>");
            sb.Append("<ul class='paginList'>");
            var totalPages = Math.Max((Total + PageSize - 1) / PageSize, 1);//总页数
            if (PageIndex <= 0) PageIndex = 1;
            if (totalPages >= 1)
            {
                int currint = 5;
                if (PageIndex > 1)//上一页
                    sb.Append("<li class='paginItem2'><a value='prev' onclick='submitClicks(this)'><span class='pagepre2'></span></a></li>");
                else
                    sb.Append("<li class='paginItem2'><a><span class='pagepre2'></span></a></li>");
                for (int i = 0; i <= 10; i++)
                {
                    if ((PageIndex + i - currint) >= 1 && (PageIndex + i - currint) <= totalPages)
                        if (currint == i)
                        {
                            sb.Append(string.Format("<li class='paginItem2 current'><a value={0} onclick='submitClicks(this)'>{0}</a></li>", PageIndex));
                        }
                        else
                        {
                            int dict = PageIndex + i - currint;
                            sb.Append(string.Format("<li class='paginItem2'><a value={0} onclick='submitClicks(this)'>{0}</a></li>", dict));
                        }
                }
                if (PageIndex < totalPages)
                {
                    sb.Append("<li class='paginItem2'><a value='next' onclick='submitClicks(this)'><span class='pagenxt2'></span></a></li>");
                }
                else
                {
                    sb.Append("<li class='paginItem2'><a ><span class='pagenxt2'></span></a></li>");
                }
            }
            sb.Append("</ul></div>");
            return sb.ToString();
        }
    }
}