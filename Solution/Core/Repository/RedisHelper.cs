using System.Collections;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Support;
using System;

namespace QinJilu.Core
{
    public class RedisHelper
    {
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public static void AddObject(string objId, object o)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Set<byte[]>(objId, new ObjectSerializer().Serialize(o));
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="o">到期时间,单位:秒</param>
        public static void AddObject(string objId, object o, int expire)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                //永不过期
                if (expire == 0)
                    Redis.Set<byte[]>(objId, new ObjectSerializer().Serialize(o));
                else
                    Redis.Set<byte[]>(objId, new ObjectSerializer().Serialize(o), DateTime.Now.AddSeconds(expire));
            }
        }

        public static object GetObject(string objId)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                object obj = new ObjectSerializer().Deserialize(Redis.Get<byte[]>(objId));
                return obj;
            }
        }

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public static void RemoveObject(string objId)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Remove(objId);
            }
        }


        /// <summary>
        /// 清空的有缓存数据
        /// </summary>
        public static void FlushAll()
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.FlushAll();
            }
        }
    }

    /// <summary>
    /// MemCache管理操作类
    /// </summary>
    public sealed class RedisManager
    {
        /// <summary>
        /// Redis配置信息类文件
        /// </summary>
        public class RedisConfigInfo
        {

            private static RedisConfigInfo __this = null;
            public static RedisConfigInfo Current
            {
                get
                {
                    if (__this == null)
                    {
                        __this = new RedisConfigInfo();
                    }
                    return __this;
                }
            }

            private bool _applyRedis;
            /// <summary>
            /// 是否应用Redis
            /// </summary>
            public bool ApplyRedis
            {
                get
                {
                    return _applyRedis;
                }
                set
                {
                    _applyRedis = value;
                }
            }

            private string _writeServerList = System.Configuration.ConfigurationManager.ConnectionStrings["qinRedis"].ConnectionString;
            /// <summary>
            /// 可写的Redis链接地址
            /// </summary>
            public string WriteServerList
            {
                get
                {
                    return _writeServerList;
                }
                set
                {
                    _writeServerList = value;
                }
            }

            private string _readServerList = System.Configuration.ConfigurationManager.ConnectionStrings["qinRedis"].ConnectionString;
            /// <summary>
            /// 可读的Redis链接地址
            /// </summary>
            public string ReadServerList
            {
                get
                {
                    return _readServerList;
                }
                set
                {
                    _readServerList = value;
                }
            }

            private int _maxWritePoolSize;
            /// <summary>
            /// 最大写链接数
            /// </summary>
            public int MaxWritePoolSize
            {
                get
                {
                    return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
                }
                set
                {
                    _maxWritePoolSize = value;
                }
            }

            private int _maxReadPoolSize;
            /// <summary>
            /// 最大读链接数
            /// </summary>
            public int MaxReadPoolSize
            {
                get
                {
                    return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
                }
                set
                {
                    _maxReadPoolSize = value;
                }
            }

            private bool _autoStart;
            /// <summary>
            /// 自动重启
            /// </summary>
            public bool AutoStart
            {
                get
                {
                    return _autoStart;
                }
                set
                {
                    _autoStart = value;
                }
            }


        }

        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.Current;

        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }


        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] writeServerList = redisConfigInfo.WriteServerList.Split(',');
            string[] readServerList = redisConfigInfo.ReadServerList.Split(',');

            prcm = new PooledRedisClientManager(readServerList, writeServerList,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                                 MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                                 AutoStart = redisConfigInfo.AutoStart,
                             });
            prcm.Start();
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();

            return prcm.GetClient();
        }
    }

}