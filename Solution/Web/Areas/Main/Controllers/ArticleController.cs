using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Main.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Main/Article/Index

        public ActionResult Index(string id = "", int start = 0, int topN = 10)
        {
            //大姨妈 例假 月经   育儿 产后  怀孕    美肤  美体  美妆  
            string[] colNames = null;// new string[] { "大姨妈", "例假", "月经",       "育儿", "产后", "怀孕",       "美肤", "美体", "美妆" };
            switch (id)
            {
                case "period":
                    colNames = new string[] { "大姨妈", "例假", "月经" };
                    break;
                case "baby":
                    colNames = new string[] { "育儿", "产后", "怀孕" };
                    break;
                case "beauty":
                    colNames = new string[] { "美肤", "美体", "美妆" };
                    break;
                default:
                    break;
            }
            var lst = QinJilu.Web.Areas.Main.Models.EFData.GetNews(colNames, start, topN);
            return View(lst);
        }

        public ActionResult Item(int id)
        {
            var m = QinJilu.Web.Areas.Main.Models.EFData.GetOne(id);
            return View(m);
        }

        public ActionResult Img(string id)
        {
            var img = QinJilu.Web.Areas.Main.Models.EFData.GetImg(id);
            return File(img, "image/jpeg");
        }

    }
}
