using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//  Cache 缓存，Asynchronous 异步，Concurrent 并行
namespace QinJilu.Core
{
    public class Services
    {
        #region subscribe
        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe(string openId)
        {

        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        public void UnSubscribe(string openId)
        {

        }
        #endregion


        #region record

        public void Begin(string openId,int dateticks)
        {
            // befor

            // ing
            // 队列缓存 Redis
            // 所有的数据库操作，先暂存在缓存队列中，集中存储。
            // 减少数据库交互次数,多个字段都更新完成以后一起提交。
            // 


            // sql  无索引的操作，总是通过主键
            // 写总是使用sql，成功后再到mongodb
            // 读总是使用mongodb
            // 定期做二者之前的比较同步。


            //  after

        }
        public void End(string openId, int dateticks)
        {

        }

        public void Set(string openId, FieldName fName, Options opt, int dateticks)
        {
            switch (fName)
            {
                case FieldName.Period:
                    break;
                case FieldName.Pain:
                    break;
                case FieldName.Sex:
                    break;
                case FieldName.Mood:
                    break;
                case FieldName.Fluid:
                    break;
                case FieldName.Pill:
                    break;
                case FieldName.Temperature:
                    break;
                case FieldName.Tags:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Temperature(string openId, Int16 temperature,bool unreliable, int dateticks)
        {

        }

        #region tag

        /// <summary>
        /// 输入一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tag"></param>
        public void Add(string openId, string tag, int dateticks)
        {

        }
        /// <summary>
        /// 选择一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Add(string openId, int tagId, int dateticks)
        {

        }
        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Del(string openId, int tagId, int dateticks)
        {

        }

        #endregion

        #endregion

        /// <summary>
        /// 收到 位置 请求后的响应
        /// </summary>
        public void Today()
        {

        }
    }
}
