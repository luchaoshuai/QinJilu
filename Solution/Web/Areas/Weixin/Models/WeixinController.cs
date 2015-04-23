using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Models
{
    /// <summary>
    /// 在微信公众号中打开的界面(微信号登录即可，不一定要在微信里面打开)，都要继承该类，其中会自动取当前微信登录用户的openId值。
    /// </summary>
    public class WeixinController : Controller
    {
        /// <summary>
        /// 当前登录用户的微信id
        /// </summary>
        protected string OpenId
        {
            get
            {
                return User.Identity.Name;
            }
        }

        protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.ServerAndNoCache);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext为空");
            }


            if (!User.Identity.IsAuthenticated)
            {
                ToLogin();
                return;
            }
        }



        private void ToLogin()
        {
            if (Request.Url.DnsSafeHost == "localhost")
            {
                System.Web.HttpContext.Current.Response.Redirect("/Weixin/Passport/Transit?openId=oljOKs0tHgi8JsvSrZKEqRjluHCk&state=" + Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()), true);
                return;
            }

            string sso_url = System.Configuration.ConfigurationManager.AppSettings["SSOPassport"];

            // 微信授权页面地址，目前使用 snsapi_base 模式，不会出现授权界面，只是一个302跳转而已
            string wx_sso_url = Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthApi.GetAuthorizeUrl(
                QinJilu.Web.Areas.Weixin.Controllers.MpController.AppId,
                sso_url,// 统一的回调入口，会在这个控制器里面取回openid,并放在cookie中去,然后再跳转到下面state这个地址中
               Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()),
                Senparc.Weixin.MP.OAuthScope.snsapi_base);


            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Redirect(wx_sso_url, true);
        }


    }
}
