using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class MeController : Models.QinjiluController
    {
        //
        // GET: /Weixin/Me/

        public ActionResult Index()
        {
            return View();
        }

    }
}
