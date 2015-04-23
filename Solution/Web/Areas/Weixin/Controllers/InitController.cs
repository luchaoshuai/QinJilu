using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class InitController : Models.WeixinController
    {
        //  除邀请码外的初始化设置，完整性检查，都在这个控制器中。
        // GET: /Init/Home/

        public ActionResult Guide()
        {
            return View();
        }


        public ActionResult Gender()
        {
            return View();
        }

        public ActionResult GenderConfirm(Core.Gender g)
        {
            new Core.Services().SetGender(OpenId, g);
            return View(g);
        }





        // boy
        [HttpGet]
        public ActionResult MaleInit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MaleInit(string email)
        {
            return View();
        }





        // girl
        [HttpGet]
        public ActionResult WomanInit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WomanInit(int age, DateTime lasterCycleStart, int cycleTypically, int periodTypically)
        {
            new Core.Services().WomanInit(OpenId, age, lasterCycleStart, cycleTypically, periodTypically);
            return Redirect("/Weixin/Today/Index");
        }

    }
}
