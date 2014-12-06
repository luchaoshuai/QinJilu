using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin
{
    public class WeixinAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Weixin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Weixin_default",
                "Weixin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
