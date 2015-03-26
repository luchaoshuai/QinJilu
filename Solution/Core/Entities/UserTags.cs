using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class UserTags
    {

        public int Id { get; set; }

        /// <summary>
        /// 标签所属的她Id
        /// </summary>
        public int SheId { get; set; }
        /// <summary>
        /// 引用标签
        /// </summary>
        public int TagId { get; set; }
        public TagInfo Tag { get; set; }

        /// <summary>
        /// 个人的引用次数
        /// </summary>
        public int RefTotal { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        public bool Hidden { get; set; }

    }

}
