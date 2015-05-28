using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;


//  与 CustomMessageHandler_Event 是  partial class

namespace QinJilu.Web.Areas.Weixin.Models
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
        }

        //public override void OnExecuting()
        //{
        //    //测试MessageContext.StorageData
        //    if (CurrentMessageContext.StorageData == null)
        //    {
        //        CurrentMessageContext.StorageData = 0;
        //    }
        //    base.OnExecuting();
        //}

        //public override void OnExecuted()
        //{
        //    base.OnExecuted();
        //    CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        //}

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            string url = string.Format("http://qinjilu.com/weixin/Passport/Transit?openId={0}&state={1}", requestMessage.FromUserName, "/weixin/more/index");

            if (requestMessage.Content == "debug")
            {
                responseMessage.Content = "<a href=\"" + url + "\">点击这里</a>进入记录页面 ";
            }
            else
            {
                new Core.Services().AddNote(requestMessage.FromUserName,null, requestMessage.Content);
                responseMessage.Content = "已将您刚刚发送的消息记录到今日笔记中，<a href=\"" + url + "\">点击这里</a>进入详细页面查看 ";
            }
            return responseMessage;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            // new Core.Services().Today();
            // 跳到另一个网页中？返回图文消息？
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }


        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "刚刚发来的消息为：" + requestMessage.Encrypt + " ,  from DefaultResponseMessage。";
            responseMessage.Content = "暂时仅接收文本类型的消息，其它消息将被忽略！";
            return responseMessage;
        }


    }
}