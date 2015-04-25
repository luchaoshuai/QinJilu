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
        // GET: /Weixin/Init/Guide/

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
        public ActionResult MaleInit(string email,string note)
        {
           bool res = new Core.Services().InvitationGoddess(OpenId,email,note);
           if (res)
           {
               return Content("已经发送请求给女神，请等待女神响应。");
           }
           else
           {
               return Content("女神邮箱有误，请重试。");
           }
        }

        public ActionResult AnyEmail(string email)
        {
            bool res = new Core.Services().AnyEmail(email);
            if (res)
            {
                return Content("true");
            }
            return Content("false");
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
