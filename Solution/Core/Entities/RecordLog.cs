using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 流水帐  每一次点击，都会在这里产生一条记录，便于日后分析使用。
    /// </summary>
    public class RecordLog
    {
        public int Id { get; set; }
        /// <summary>
        /// 流水账所属的她Id
        /// </summary>
        public int SheId { get; set; }

        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 操作人Id-大部分情况下为sheId.后期可以分析是别人代记的多还是男友记得多
        /// </summary>
        public int CreaterId { get; set; }


        /// 记录所属的天 从2010-1-1后面加上该天数
        ///                65535
        public UInt16 DateTicks { get; set; }

        /// <summary>
        /// 提交变更的字段属性键值对  filename:value(option|tags_string)
        /// </summary>
        public string Json { get; set; }
    }
}
