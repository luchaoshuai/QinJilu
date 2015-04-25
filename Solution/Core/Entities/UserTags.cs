using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class UserTags
    {

        public MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// 标签所属的她Id
        /// </summary>
        public MongoDB.Bson.ObjectId SheId { get; set; }
        /// <summary>
        /// 引用标签
        /// </summary>
        public MongoDB.Bson.ObjectId TagId { get; set; }
        /// <summary>
        /// 冗余标签名
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 个人的引用次数
        /// </summary>
        public int RefTotal { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 排序值 
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }

}
