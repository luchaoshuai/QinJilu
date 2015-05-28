﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;

namespace QinJilu.Web.Areas.Weixin.Models
{
    public partial class CustomMessageHandler
    {


        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);

            var u = new Core.Services().Subscribe(requestMessage.FromUserName);

            var info = Web.Areas.Weixin.Controllers.MpController.GetUserInfo(u.WeixinOpenID);
            new Core.Services().SetWeixinInfo(info);

            string url = string.Format("http://qinjilu.com/weixin/Passport/Transit?openId={0}&state={1}", requestMessage.FromUserName, "/weixin/more/index");

            if (u.SubscribeCount > 1)
            {
                var ds = (DateTime.Now - u.UnsubscribeOn).TotalDays;
                ds = ds > 1 ? ds : 1;
                responseMessage.Content = "欢迎回来，在您离开的 " + ds.ToString("F0") + " 天里，我们做了很多的升级，更多的精彩，邀您探索！<a href='" + url + "'>点我开始吧！</a>";
            }
            else
            {
                responseMessage.Content = "欢迎关注，更多精彩，邀您探索！<a href='" + url + "'>点我开始吧！</a>";
            }

            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            new Core.Services().UnSubscribe(requestMessage.FromUserName);
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "OnEvent_UnsubscribeRequest";
            return responseMessage;
        }


        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "OnEvent_LocationRequest";
            return null;//这里也可以返回null（需要注意写日志时候null的问题）
        }




        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            var reponseMessage = CreateResponseMessage<ResponseMessageText>();
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                case "yesterday.come":
                    {
                        reponseMessage.Content = "好的，昨天来的，小鹿已经记下来了。";
                        new Core.Services().Begin(requestMessage.FromUserName, DateTime.Now.AddDays(-1) );
                    }
                    break;
                case "yesterday.go":
                    {
                        reponseMessage.Content = "好的，昨天走的，小鹿已经记下来了。";
                        new Core.Services().End(requestMessage.FromUserName, DateTime.Now.AddDays(-1));
                    }
                    break;
                case "today.come":
                    {
                        reponseMessage.Content = "好的，今天刚来，小鹿已经记下来了。";
                        new Core.Services().Begin(WeixinOpenId, DateTime.Now);
                    }
                    break;
                case "today.go":
                    {
                        reponseMessage.Content = "好的，今天刚走，小鹿已经记下来了。";
                        new Core.Services().End(WeixinOpenId, DateTime.Now);
                    }
                    break;
                default:
                    {
                        reponseMessage.Content = "unknow EventKey：" + requestMessage.EventKey;
                    }
                    break;
            }
            return reponseMessage;
        }


        ///// <summary>
        ///// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        //{
        //    var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
        //    return eventResponseMessage;
        //}

        //public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        //{
        //    // 预处理文字或事件类型请求。
        //    // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
        //    // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
        //    // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
        //    // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
        //    // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey
            
        //    return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        //}

        #region 无效（已注释掉的）事件


        //public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        //{
        //    var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //    responseMessage.Content = "您刚才发送了ENTER事件请求。";
        //    return responseMessage;
        //}
        //public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        //{
        //    //通过扫描关注
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "通过扫描关注。";
        //    return responseMessage;
        //}
        //public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        //{
        //    //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
        //    return responseMessage;
        //}

        //public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "接收到了群发完成的信息。";
        //    return responseMessage;
        //}


        ///// <summary>
        ///// 事件之扫码推事件(scancode_push)
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        //{

        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之扫码推事件,requestMessage.ScanCodeInfo.ScanResult: " + requestMessage.ScanCodeInfo.ScanResult;
        //    return responseMessage;
        //}

        ///// <summary>
        ///// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之扫码推事件且弹出“消息接收中”提示框";
        //    return responseMessage;
        //}

        ///// <summary>
        ///// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出拍照或者相册发图";
        //    return responseMessage;
        //}

        ///// <summary>
        ///// 事件之弹出系统拍照发图(pic_sysphoto)
        ///// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出系统拍照发图";
        //    return responseMessage;
        //}

        ///// <summary>
        ///// 事件之弹出微信相册发图器(pic_weixin)
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出微信相册发图器";
        //    return responseMessage;
        //}

        ///// <summary>
        ///// 事件之弹出地理位置选择器（location_select）
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出地理位置选择器";
        //    return responseMessage;
        //}
        #endregion
    }
}