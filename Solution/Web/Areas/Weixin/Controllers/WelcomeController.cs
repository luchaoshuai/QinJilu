using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    //  邀请码 相关界面
    public class WelcomeController : Models.WeixinController
    {
        //  openId      卢朝帅
        //  oljOKs0tHgi8JsvSrZKEqRjluHCk
        //  var x = new Core.Services().GetUserId("oljOKs0tHgi8JsvSrZKEqRjluHCk");


        //
        // GET: /Weixin/Welcome/

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

        [ActionName("Index")]
        public ActionResult InvitationCode()
        {
            return View("InvitationCode");
        }

        public ActionResult CheckInvitationcode(string code)
        {
            string msg = string.Empty;
            var res = new Core.Services().CheckInvitecode(code, ref msg);
            return Json(new { res=res, msg=msg });
        }

        public ActionResult UseInvitecode(string code)
        {
            string msg = string.Empty;
            var res = new Core.Services().UseInvitecode(OpenId, code, ref msg);
            return Json(new { res = res, msg = msg });
        }



        public ActionResult Guide()
        {
            return View();
        }




    }
}
