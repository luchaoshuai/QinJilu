using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Models
{
    /// <summary>
    /// 初始化设置，完整性检查 ok后。可以正常使用本系统的用户。
    /// </summary>
    public class QinjiluController : WeixinController
    {
        protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            IntegrityCheck();
        }



        /// <summary>
        /// 完整性检查
        /// </summary>
        /// <returns>true，完整的，可以往下执行。false,不完整，中间已经跳转，无需再往下执行。</returns>
        protected bool IntegrityCheck()
        {
            Core.Services s = new Core.Services();

            var userInfo = s.GetUser(OpenId);
            

            // 是否启用邀请码？
            bool needInvitecode = Core.Services.NeedInvitationCode();
            if (needInvitecode)
            {
                // 未校验邀请码
                if (!userInfo.CheckInvitecode)
                {
                    ToFix("/Weixin/Welcome/Index");
                    return false;
                }
            }

            //  未定义性别
            if (userInfo.Gender== Core.Gender.Unknow)
            {
                ToFix("/Weixin/Init/Gender");
                return false;
            }


            // 男生
            if (userInfo.Gender== Core.Gender.Male)
            {
                // 未绑定女神
                if (userInfo.SheId== MongoDB.Bson.ObjectId.Empty)
                {
                    ToFix("/Weixin/Init/MaleInit");
                    return false;
                    // 是否已经发送了申请？ 目前未处理。总是再次更新。。。
                }
            }

            // 女生
            if (userInfo.Gender== Core.Gender.Woman)
            {
                // 未初始化经期数据
                if (userInfo.CycleTypically==0)
                {
                    ToFix("/Weixin/Init/WomanInit");
                    return false;
                }
            }
            return true;
        }



        private void ToFix(string target_url)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Redirect(target_url, true);
        }
    }
}
