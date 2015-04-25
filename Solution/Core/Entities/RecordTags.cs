using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class RecordTags
    {
        public MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// 所属记录
        /// </summary>
        public MongoDB.Bson.ObjectId RecordId { get; set; }
        /// <summary>
        /// 引用标签
        /// </summary>
        public MongoDB.Bson.ObjectId TagId { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

    }

}
