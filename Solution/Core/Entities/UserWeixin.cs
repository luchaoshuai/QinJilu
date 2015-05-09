using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class UserWeixin
    {
        /// <summary>
        /// 主键
        /// </summary>
        public MongoDB.Bson.ObjectId Id { get; set; }
        /// <summary>
        /// usrinfo的外键
        /// </summary>
        public MongoDB.Bson.ObjectId UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 存储的头像
        /// </summary>
        public byte[] headimg { get; set; }

        /// <summary>
        /// 个人微信信息 修改版本号，0为新创建，第一次同步,后面每变更一次，该值都会+1
        /// </summary>
        public int Version { get; set; }


        #region 微信返回的资料

        /// <summary>
        /// 关注公号后微信给的OpenID,用户的标识，对当前公众号唯一
        /// </summary>
        public string WeixinOpenID { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int subscribe { get; set; }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long subscribe_time { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }

        #endregion
    }


}
