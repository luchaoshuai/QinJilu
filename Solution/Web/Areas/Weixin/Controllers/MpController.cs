﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP;
using QinJilu.Web.Areas.Weixin.Models;

namespace QinJilu.Web.Areas.Weixin.Controllers
{

    public class MpController : Controller
    {
        // http://localhost:40254/weixin/mp
        // 已经由下面二个方法接管
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public static readonly string Token = "LwZtbfquws773G";
        public static readonly string EncodingAESKey = "Wqn5KLoGVEEihb0iOgnrjgkj5Oo0umdBmcxMGYZML2K";
        //      上面二个所有号都设置成一样即可。

        //      下面二个是微信平台自动生成的。
        public static readonly string AppSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
        public static readonly string AppId = System.Configuration.ConfigurationManager.AppSettings["AppId"];

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://qinjilu.com/weixin/mp
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。 " + DateTime.Now.ToString());
            }
        }
        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);

            try
            {
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                //messageHandler.RequestDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" + messageHandler.RequestMessage.FromUserName + ".txt"));
                //if (messageHandler.UsingEcryptMessage)
                //{
                //    //未解密前的原始响应信息
                //    messageHandler.EcryptRequestDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_Ecrypt_" + messageHandler.RequestMessage.FromUserName + ".txt"));
                //}

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

                //测试时可开启，帮助跟踪数据
                //messageHandler.ResponseDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" + messageHandler.ResponseMessage.ToUserName + ".txt"));
                //if (messageHandler.UsingEcryptMessage)
                //{
                //    //记录加密后的响应信息
                //    messageHandler.FinalResponseDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_Final_" + messageHandler.ResponseMessage.ToUserName + ".txt"));
                //}


                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可

            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }
                    tw.Flush();
                    tw.Close();
                }
                return new EmptyResult();
            }
        }


        /// <summary>
        /// 最简化的处理流程（不加密）
        /// </summary>
        [HttpPost]
        [ActionName("MiniPost")]
        public ActionResult MiniPost(string signature, string timestamp, string nonce, string echostr)
        {
            if (!CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return new WeixinResult("参数错误！");//v0.8+
            }

            var messageHandler = new CustomMessageHandler(Request.InputStream, null);

            messageHandler.Execute();//执行微信处理过程

            return new FixWeixinBugWeixinResult(messageHandler);
        }

        /*
         * v0.3.0之前的原始Post方法见：WeixinController_OldPost.cs
         * 
         * 注意：虽然这里提倡使用CustomerMessageHandler的方法，但是MessageHandler基类最终还是基于OldPost的判断逻辑，
         * 因此如果需要深入了解Senparc.Weixin.MP内部处理消息的机制，可以查看WeixinController_OldPost.cs中的OldPost方法。
         * 目前为止OldPost依然有效，依然可用于生产。
         */

        /// <summary>
        /// 取得用户的微信资料
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static Core.UserWeixin GetUserInfo(string openId)
        {
            var token = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(
                QinJilu.Web.Areas.Weixin.Controllers.MpController.AppId,
                QinJilu.Web.Areas.Weixin.Controllers.MpController.AppSecret);

            var m = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Info(token.access_token, openId);


            byte[] bs = null;
            if (!string.IsNullOrEmpty(m.headimgurl))
            {
                for (int i = 0; i < 3; i++)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    try
                    {
                        bs = wc.DownloadData(m.headimgurl);
                        break;
                    }
                    catch 
                    {
                    }                    
                }
            };

            var newuser = new Core.UserWeixin()
            {
                city = m.city,
                country = m.country,
                headimgurl = m.headimgurl,
                language = m.language,
                nickname = m.nickname,
                province = m.province,
                sex = m.sex,
                subscribe = m.subscribe,
                subscribe_time = m.subscribe_time,
                unionid = m.unionid,

                headimg = bs,

                //CreateOn = DateTime.Now,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                //UserId = new Core.Services().GetUserId(openId),
                //Version = 0,
                WeixinOpenID = openId
            };

            return newuser;
        }



    }
}