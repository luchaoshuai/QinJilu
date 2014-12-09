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
            //TODO:这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (requestMessage.Content == "baidu")
            {
                responseMessage.Content = "<a href=\"http://www.baidu.com/\">点击这里</a>进入baidu ";
            }
            else
            {
                var result = new StringBuilder();
                result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", requestMessage.Content);

                if (CurrentMessageContext.RequestMessages.Count > 1)
                {
                    result.AppendFormat("您之前还发送了如下消息（{0}/{1}）：\r\n", CurrentMessageContext.RequestMessages.Count, CurrentMessageContext.StorageData);
                    for (int i = CurrentMessageContext.RequestMessages.Count - 2; i >= 0; i--)
                    {
                        var historyMessage = CurrentMessageContext.RequestMessages[i];
                        result.AppendFormat("{0} 【{1}】{2}\r\n",
                                            historyMessage.CreateTime.ToShortTimeString(),
                                            historyMessage.MsgType.ToString(),
                                            (historyMessage is RequestMessageText)
                                                ? (historyMessage as RequestMessageText).Content
                                                : "[非文字类型]"
                            );
                    }
                    result.AppendLine("\r\n");
                }

                result.AppendFormat("如果您在{0}分钟内连续发送消息，记录将被自动保留（当前设置：最多记录{1}条）。过期后记录将会自动清除。\r\n", WeixinContext.ExpireMinutes, WeixinContext.MaxRecordCount);
          
                responseMessage.Content = result.ToString();
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
            responseMessage.Content = "刚刚发来的消息为："+requestMessage.Encrypt+" ,  from DefaultResponseMessage。";
            return responseMessage;
        }


    }
}