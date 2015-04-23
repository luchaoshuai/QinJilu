using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class HomeController : Models.WeixinController
    {
        //正而八经用起来后的界面，都在这个控制器里面。
        // GET: /Weixin/Home/


        public ActionResult Index()
        {



            return View();
        }

        public ActionResult Gender()
        {
            return View();
        }

        public ActionResult GenderConfirm(Core.Gender g)
        {
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
        public ActionResult WomanInit(int age, DateTime LasterCycleStart, int CycleTypically, int PeriodTypically)
        {
            return View("Index");
        }



        /// <summary>
        /// 邀请(找)女神
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult InvitationGoddess(string uid)
        {
            return View();
        }









    }
}
