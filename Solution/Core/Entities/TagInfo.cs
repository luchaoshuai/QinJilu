using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class TagInfo
    {
        public int Id { get; set; }

        /// <summary>
        /// 标签文本内容
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 合计引用次数
        /// </summary>
        public int RefTotal { get; set; }

        /// <summary>
        /// 被合并以后 到的 新的标签Id
        /// </summary>
        public int NewTagId { get; set; }

    }
}
