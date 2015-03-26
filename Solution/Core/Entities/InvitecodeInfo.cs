using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core.Entities
{
    public class InvitecodeInfo
    {
        public MongoDB.Bson.ObjectId Id { get; set; }

        // 邀请码..船票。。。 暂定为 8 位数字，后期可以扩展为任意字符。使用二维码扫描输入。
        public string Invitecode { get; set; }

        /// <summary>
        /// 主人的用户Id
        /// </summary>
        public MongoDB.Bson.ObjectId FromUserId { get; set; }
        /// <summary>
        /// 使用者的用户Id
        /// </summary>
        public MongoDB.Bson.ObjectId UsedUserId { get; set; }

        public InvitecodeStatus State { get; set; }

        /// <summary>
        /// 创建时间（主人创建出来的时间,第一次绑定到fromuserId的时间）
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 使用时间（别人用他登船的时间）
        /// </summary>
        public DateTime UsedOn { get; set; }

        /// <summary>
        /// 重生时间（失效以后，重复生效的时间,持续更新）
        /// </summary>
        public DateTime RebirthOn { get; set; }

    }
}
