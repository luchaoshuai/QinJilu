using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class WelcomeController : Controller
    {
        //  http://localhost:40254/Weixin/Welcome/NeedInvitationCode?need=true
        //  http://localhost:40254/Weixin/Welcome/NeedInvitationCode?need=false
        /// <summary>
        /// 设置 邀请码 机制 是否 启用
        /// </summary>
        /// <param name="need"></param>
        /// <returns></returns>
        public ActionResult NeedInvitationCode(bool need)
        {
            Core.Services.NeedInvitationCode(need);
            return Content(" set  to  " + need);
        }


        //  oGXl0uDawdiUTWOvflx0qryzxCyI
        //  548704ead516bd08384dd239


        // 打开的默认首页
        public ActionResult Index(string uid = "548704ead516bd08384dd239")
        {
            var x = new Core.Services().GetUserId("oGXl0uDawdiUTWOvflx0qryzxCyI");

            string toview = "Guide";
            if (Core.Services.NeedInvitationCode())
            {
                toview = "InvitationCode";
            }
            return RedirectToAction(toview, new { uid = "548704ead516bd08384dd239" });// RedirectToAction 用的是302跳转
        }

        //  启用邀请码时。。。
        [HttpGet]
        public ActionResult InvitationCode(string uid)
        {
            ViewBag.uid = uid;
            return View();
        }


        //
        // GET: /Weixin/Welcome/

        // 第一次订阅时的引导页
        [HttpGet]
        public ActionResult Guide(string uid)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Guide(string uid, string nickname, QinJilu.Core.Gender gender)
        {
            string tourl = "Guide";
            switch (gender)
            {
                case QinJilu.Core.Gender.Male:
                    tourl = "InvitationGoddess";
                    break;
                case QinJilu.Core.Gender.Woman:
                    tourl = "IamWoman";
                    break;
                case QinJilu.Core.Gender.Unknow:
                default:
                    break;
            }
            new Core.Services().SetBaseInfo(uid, nickname, gender);
            return View(tourl, new { uid = uid });
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

        // 初始化经期数据
        public ActionResult IamWoman(string uid)
        {
            return View();
        }













    }
}
