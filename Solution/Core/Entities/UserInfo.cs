using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class UserInfo
    {
        /// <summary>
        /// 生日-算年龄用
        /// </summary>
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// 初潮来的年份，可以算出当时年龄
        /// </summary>
        public int FirstYaer { get; set; }

        /// <summary>
        /// 关注公号后微信给的OpenID
        /// </summary>
        public string WeixinOpenID { get; set; }
        /// <summary>
        /// 创建（关注）时间
        /// </summary>
        public DateTime CreateOn { get; set; }
        public MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 姓别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 若为他登录时，指向一个默认她号，
        /// 若为她登录时，则指向自己本身。
        /// 后续程序，总是使用该id关联数据。
        /// </summary>
        public MongoDB.Bson.ObjectId SheId { get; set; }

        /// <summary>
        /// 最近（后）一次来潮的时间
        /// </summary>
        public DateTime LasterCycleStart { get; set; }

        /// <summary>
        /// 查看密码,为null是为未启用
        /// </summary>
        public string Passcoed { get; set; }

        /// <summary>
        /// 通常的经期时间
        /// </summary>
        public int CycleTypically { get; set; }// 28
        /// <summary>
        /// 不同的经期差距
        /// </summary>
        public int CycleVaries { get; set; }// ±2

        /// <summary>
        /// 通常经期持续的时间
        /// </summary>
        public int PeriodTypically { get; set; }// 4
        /// <summary>
        /// 不同经期持续的差距
        /// </summary>
        public int PeriodVaries { get; set; }// ±2

        /// <summary>
        /// 通常经前综合期持续的时间
        /// </summary>
        public int PmsTypically { get; set; }// 3
        /// <summary>
        /// 不同经前综合期持续的差距
        /// </summary>
        public int PmsVaries { get; set; }// ±1

        /// <summary>
        /// 取消后再订阅，会加1
        /// </summary>
        public int SubscribeCount { get; set; }// 1
        /// <summary>
        /// 是否已经取消订阅
        /// </summary>
        public bool Unsubscribe { get; set; }// 0
        /// <summary>
        /// 取消订阅时间
        /// </summary>
        public DateTime UnsubscribeOn { get; set; }// 0

    }


}
