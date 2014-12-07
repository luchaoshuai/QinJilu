using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class RecordNote
    {
        public int Id { get; set; }
        public int SheId { get; set; }

        public DateTime CreateOn { get; set; }
        public int CreaterId { get; set; }

        /// <summary>
        /// 记录所属的天 从2010-1-1后面加上该天数
        ///                65535
        /// </summary>
        public UInt16 DateTicks { get; set; }

        public string Notes { get; set; }
    }
}
