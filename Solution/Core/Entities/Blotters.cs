using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 流水帐
    /// </summary>
    public class Blotters
    {
        public int Id { get; set; }
        public int SheId { get; set; }

        public DateTime CreateOn { get; set; }
        public int CreaterId { get; set; }

        public UInt16 DateTicks { get; set; }

        /// <summary>
        /// 提交变更的字段属性键值对
        /// </summary>
        public string Json { get; set; }
    }
}
