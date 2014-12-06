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
        public string OpenID_weixin { get; set; }
        /// <summary>
        /// 创建（关注）时间
        /// </summary>
        public DateTime CreateOn { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 姓别
        /// </summary>
        public Gender gender { get; set; }

        /// <summary>
        /// 若为他登录时，指向一个默认她号，
        /// 若为她登录时，则指向自己本身。
        /// 后续程序，总是使用该id关联数据。
        /// </summary>
        public int SheId { get; set; }

        public DateTime LasterCycleStart { get; set; }

        public string passcoed { get; set; }

        //cycle_length
        public int cycle_typically { get; set; }// 28
        public int cycle_varies { get; set; }// 2

        //period_lasting
        public int period_typically { get; set; }// 4
        public int period_varies { get; set; }// 2

        public int pms_typically { get; set; }// 3
        public int pms_varies { get; set; }// 1

        /// <summary>
        /// 取消后再订阅，会加1
        /// </summary>
        public int SubscribeCount { get; set; }// 1
        /// <summary>
        /// 是否已经取消订阅
        /// </summary>
        public bool Unsubscribe { get; set; }// 1
        /// <summary>
        /// 取消订阅时间
        /// </summary>
        public DateTime UnsubscribeOn { get; set; }// 0

    }


}
