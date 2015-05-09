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
            var ser = new Core.Services();

            var uinfo = Web.Areas.Weixin.Controllers.MpController.GetUserInfo(OpenId);
            ViewBag.UserWeixin = ser.SetWeixinInfo(uinfo);

            return View();
        }

    }
}
