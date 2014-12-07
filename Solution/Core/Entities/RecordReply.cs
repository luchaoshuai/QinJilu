using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class RecordReply
    {
        public int Id { get; set; }

        /// <summary>
        /// 所属记录Id
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// 创建(回复)时间
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 创建者Id  本人可以删除
        /// </summary>
        public int CreaterId { get; set; }
        public string CreateBy { get; set; }

        /// <summary>
        /// @之前人ID  发消息给这个人+she，有新回复了。
        /// </summary>
        public int RefUserId { get; set; }
        public string RefBy { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

    }
}
