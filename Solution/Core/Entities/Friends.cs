using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core.Entities
{
    public class Friends
    {
        /// <summary>
        /// 所属主人的号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 被邀请的朋友号
        /// </summary>
        public int FriendId { get; set; }
        /// <summary>
        /// 朋友号的备注名
        /// </summary>
        public string Notename { get; set; }
        /// <summary>
        /// 操作权限项
        /// </summary>
        public Operation Operations { get; set; }


    }
}
