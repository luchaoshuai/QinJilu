using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 朋友邀请 只能由本人发起邀请别人参与进来，不能由他人发起添加的请求。而且是单向的，非双向公开。
    /// 计划是由女人向男人发起邀请的，也可变形为女主人向闺蜜发起邀请。
    ///                                 你想给我看，不代表我也想给你看。尤其是xx记录。
    /// </summary>
    public class FriendInvitation
    {
        public  MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// 申请人Id
        /// </summary>
        public MongoDB.Bson.ObjectId UserId { get; set; }

        /// <summary>
        /// 被邀请人的Id
        /// </summary>
        public MongoDB.Bson.ObjectId FriendId { get; set; }
        /// <summary>
        /// FriendId is Goddess
        /// </summary>
        public bool SheIsGoddess { get; set; }


        /// <summary>
        /// 创建时间（申请时间）
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 邀请的备注说明
        /// </summary>
        public string InvitationNote { get; set; }
        /// <summary>
        /// 邀请时标注的朋友 备注明
        /// </summary>
        public string Notename { get; set; }

        /// <summary>
        /// 申请前希望授予的操作权限项，实际授权以 friend 表中的为准
        /// </summary>
        public Operation Operations { get; set; }



        /// <summary>
        /// 是否已经同意
        /// </summary>
        public Opinion Opinions { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime OpinionOn { get; set; }

    }
}
