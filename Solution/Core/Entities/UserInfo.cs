using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class UserInfo
    {
        //// 连续三天登录，就能看到邀请码，若三天未登录了，则会消失。号码未使用，一直存在，不会再新增。有且最多一个.
        //// 默认值为0，有交互时，判断该值是否》2，若没有小于3，则加1，否则不动。
        //// 每天0点，系统自动扫描，当天没有登录的，且连续时间大于0的，则减1。当值为0时，将创建的码也清空。
        //public byte Continuedays { get; set; }  // 使用日志进行统计，不存储

        /// <summary>
        /// 最后活动时间，用户主动联系的时间
        /// （48小时后就不能主动给她发消息了）
        /// </summary>
        public DateTime LastActivitytime { get; set; }


        /// <summary>
        /// 用户当前的上下文环境
        /// </summary>
        public CurrentScope CurrentContext { get; set; }

        /// <summary>
        /// 是否已经检测邀请码
        /// </summary>
        public bool CheckInvitecode { get; set; }


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
        /// 最近（后）一次来月经的时间
        /// </summary>
        public DateTime LasterCycleStart { get; set; }

        /// <summary>
        /// 查看密码,为null是为未启用
        /// </summary>
        public string Passcoed { get; set; }

        /// <summary>
        /// 平均周期天数（28）
        /// </summary>
        public int CycleTypically { get; set; }// 28
        /// <summary>
        /// 平均周期波动天数(±2)
        /// </summary>
        public int CycleVaries { get; set; }// ±2

        /// <summary>
        /// 通常经期持续的时间(4)
        /// </summary>
        public int PeriodTypically { get; set; }// 4
        /// <summary>
        /// 平均经期波动天数(±2)
        /// </summary>
        public int PeriodVaries { get; set; }// ±2

        /// <summary>
        /// 通常经前综合期持续的时间(3)
        /// </summary>
        public int PmsTypically { get; set; }// 3
        /// <summary>
        /// 平均经前踪合期波动天数(±1)
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
