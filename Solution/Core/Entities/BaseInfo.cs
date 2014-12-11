using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core.Entities
{
    public class BaseInfo
    {

        /// <summary>
        /// 最后更新记录的时间戳  , 删除记录是没有时间戳的，本系统中也不存在删除记录的功能。
        /// 与sql同步时使用该 时间戳
        /// </summary>
        public UInt64 Timestamp { get; set; }


    }



}
