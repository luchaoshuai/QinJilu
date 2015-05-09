using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 各人设置自己的提醒，男号也是一样会收到自定义提醒的。
    /// </summary>
    public class UserRemind
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
        /// 经期来临提醒是否启用
        /// </summary>
        public bool PeriodRemindEnabled { get; set; }
        /// <summary>
        /// 经期来临提醒的提前天数
        /// </summary>
        public int PeriodRemindDay { get; set; }
        /// <summary>
        /// 经期来临提醒的消息发送时间-时
        /// </summary>
        public int PeriodRemindHour { get; set; }
        /// <summary>
        /// 经期来临提醒的消息发送时间-分
        /// </summary>
        public int PeriodRemindMinute { get; set; }
        /// <summary>
        /// 经期来临提醒的消息内容
        /// </summary>
        public int PeriodRemindContent { get; set; }



        /// <summary>
        /// 排卵来临提醒是否启用
        /// </summary>
        public bool OvulationRemindEnabled { get; set; }
        /// <summary>
        /// 排卵来临提醒的提前天数
        /// </summary>
        public int OvulationRemindDay { get; set; }
        /// <summary>
        /// 排卵来临提醒的消息发送时间-时
        /// </summary>
        public int OvulationRemindHour { get; set; }
        /// <summary>
        /// 排卵来临提醒的消息发送时间-分
        /// </summary>
        public int OvulationRemindMinute { get; set; }
        /// <summary>
        /// 排卵来临提醒的消息内容
        /// </summary>
        public int OvulationRemindContent { get; set; }


    }


}
