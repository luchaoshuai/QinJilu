using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    /// <summary>
    /// 总是会存储该数据。
    /// 历史每次的点击，有一个流水表。
    /// </summary>
    public class RecordInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 记录所属的她Id
        /// </summary>
        public int SheId { get; set; }

        public DateTime CreateOn { get; set; }
        public int CreaterId { get; set; }

        public DateTime EditedOn { get; set; }
        public int EditorId { get; set; }


        /// <summary>
        /// 记录所属的天 从2010-1-1后面加上该天数
        ///                65535
        /// </summary>
        public UInt16 DateTicks { get; set; }

        //  start|stop

        /// <summary>
        /// 明确“来了”标识
        /// </summary>
        public bool BeginMark { get; set; }
        /// <summary>
        /// 明确“走了”标识
        /// </summary>
        public bool EndMark { get; set; }

        /// <summary>
        /// 经量
        /// </summary>
        public Options Period { get; set; }
       
        /// <summary>
        /// 疼痛
        /// </summary>
        public Options Pain { get; set; }
        
        /// <summary>
        /// 房事
        /// </summary>
        public Options Sex { get; set; }

        /// <summary>
        /// 情绪
        /// </summary>
        public Options Mood { get; set; }
        /// <summary>
        /// 白带
        /// </summary>
        public Options Fluid { get; set; }
        
        /// <summary>
        /// 药丸
        /// </summary>
        public Options Pill { get; set; }

        /// <summary>
        /// 体温
        /// </summary>
        public Int16 Temperature { get; set; }
        /// <summary>
        /// 是否为 可靠的体温(使用设备精确测量的true, 粗略输入的为false)
        /// </summary>
        public bool Reliable { get; set; }
        /// <summary>
        /// 记录中的标签值，每个标签用“#”分割
        /// </summary>
        public string Tags { get; set; }
    }


}




