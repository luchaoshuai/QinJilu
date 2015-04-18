using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 当主人的文章发表以后，就往 朋友号里面插入文章，以便于他们马上就能收到该看的文章。
    /// </summary>
    public class Friends
    {
        /// <summary>
        /// 所属主人的号 (主人号只能为 sheId  )
        /// </summary>
        public MongoDB.Bson.ObjectId UserId { get; set; }

        /// <summary>
        /// 被邀请的朋友号 (朋友号任意 userId  )
        /// </summary>
        public MongoDB.Bson.ObjectId FriendId { get; set; }
        /// <summary>
        /// 朋友号的备注名
        /// </summary>
        public string Notename { get; set; }
        /// <summary>
        /// 授予朋友的操作权限项
        /// </summary>
        public Operation Operations { get; set; }


    }
}
