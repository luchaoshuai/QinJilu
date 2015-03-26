using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class RecordNote
    {
        public int Id { get; set; }
        /// <summary>
        /// 记录所属的她Id
        /// </summary>
        public int SheId { get; set; }

        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 写备注的人
        /// </summary>
        public int CreaterId { get; set; }

        /// <summary>
        /// 记录所属的天 从2010-1-1后面加上该天数
        ///                65535
        /// </summary>
        public UInt16 DateTicks { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// 是否已经删除
        /// </summary>
        public bool Deleted { get; set; }
    }
}
