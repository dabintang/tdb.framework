using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.standard.IocAutofac.CacheAOP
{
    /// <summary>
    /// 读取key-value类型缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TdbReadCacheStringAttribute : Attribute
    {
        /// <summary>
        /// key前缀
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// 缓存超时时间（单位：秒）
        /// </summary>
        public int TimeoutSeconds { get; set; } = 3600;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="keyPrefix">key前缀</param>
        public TdbReadCacheStringAttribute(string keyPrefix)
        {
            this.KeyPrefix = keyPrefix;
        }
    }
}
