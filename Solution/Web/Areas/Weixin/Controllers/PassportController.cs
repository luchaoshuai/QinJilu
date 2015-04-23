using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class PassportController : Controller
    {
        //
        // GET: /Weixin/Passport/


        [ActionName("Index")]
        public ActionResult BaseCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");// 用户未授权？这条分支不可能出现。
            }

            if (string.IsNullOrEmpty(state))
            {
                return Content("验证失败！请从正规途径进入！");// 非法访问，没有入口来源引用页。
            }

            //通过，用code换取access_token
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthApi.GetAccessToken(
                QinJilu.Web.Areas.Weixin.Controllers.MpController.AppId,
                QinJilu.Web.Areas.Weixin.Controllers.MpController.AppSecret,
                code);

            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            System.Web.Security.FormsAuthentication.SetAuthCookie(result.openid, false);

            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            //    1,                                                  //  版本号
            //    info.SessionId.ToString(),                          //  与身份验证票关联的用户名
            //    DateTime.Now,                                       //  Cookie 的发出时间
            //    DateTime.Now.AddHours(24),                          //  Cookie 的到期日期
            //    false,                                              //  如果 Cookie 是持久的，为 true；否则为 false
            //    GetRequestUser(),                                   //  将存储在 Cookie 中的用户定义数据
            //    "/"
            //);
            //string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //authCookie.HttpOnly = true;
            //HttpContext.Current.Response.Cookies.Add(authCookie);

            return Redirect(state);
        }

        /// <summary>
        /// 中转页面-初期未认证，无法使用oauth2时使用。开发测试也用这个。
        /// </summary>
        /// <param name="openId">当前登录用户的openid</param>
        /// <param name="state">要跳转的目标url</param>
        /// <returns></returns>
        public ActionResult Transit(string openId, string state)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(openId,false);
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult Test()
        {
            var s = new Core.Services();
            var u = s.Subscribe("oljOKs0tHgi8JsvSrZKEqRjluHCk");




            return Json(u, JsonRequestBehavior.AllowGet);
        }


    }
}
