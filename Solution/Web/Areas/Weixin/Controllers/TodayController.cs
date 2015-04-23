using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class TodayController : Models.QinjiluController
    {
        //
        // GET: /Weixin/Today/

        public ActionResult Index()
        {
            return View();
        }

    }
}
