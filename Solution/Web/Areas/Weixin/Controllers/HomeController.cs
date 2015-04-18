using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class HomeController : Controller
    {
        //
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







    }
}
