using Ecard.Infrastructure;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Articles
{
    public class EditArticles : EcardModelListRequest<Ecard.Models.Articles>
    {
        [NoRender]
        public int articleId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 略缩图
        /// </summary>
        public string imageUrl { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string describe { get; set; }

        [Dependency]
        [NoRender]
        public IArticlesService IArticlesService { get; set; }

        public void Ready(int id)
        {
            var item = IArticlesService.GetById(id);
            if (item != null)
            {
                this.articleId = item.articleId;
                this.title = item.title;
                this.imageUrl = item.imageUrl;
                this.describe = item.describe;
            }
        }
    }
}
