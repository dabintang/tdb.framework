using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.csredis;

namespace tdb.framework.webapi.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class TdbCache : ICache
    {
        /// <summary>
        /// reids缓存
        /// </summary>
        private CSRedisCache rd;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStrings">连接字符串集合</param>
        public TdbCache(string[] connectionStrings)
        {
            this.rd = new CSRedisCache(connectionStrings);
        }

        #region 实现接口

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return this.rd.Get<T>(key);
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public void Set(string key, object value, TimeSpan expire)
        {
            this.rd.Set(key, value, expire);
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        /// <param name="exists"></param>
        public async Task SetAsync(string key, object value, TimeSpan expire)
        {
            await this.rd.SetAsync(key, value, expire);
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        public void Del(params string[] keys)
        {
            this.rd.Del(keys);
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        public async Task DelAsync(params string[] keys)
        {
            await this.rd.DelAsync(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool Exists(string key)
        {
            return this.rd.Exists(key);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public void Expire(string key, TimeSpan expire)
        {
            this.rd.Expire(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public async Task ExpireAsync(string key, TimeSpan expire)
        {
            await this.rd.ExpireAsync(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public void ExpireAt(string key, DateTime expire)
        {
            this.rd.ExpireAt(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public async Task ExpireAtAsync(string key, DateTime expire)
        {
            await this.rd.ExpireAtAsync(key, expire);
        }

        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        public string[] Keys(string pattern)
        {
            return this.rd.Keys(pattern);
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public T HGet<T>(string key, string field)
        {
            return this.rd.HGet<T>(key, field);
        }

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> HGetAll<T>(string key)
        {
            return this.rd.HGetAll<T>(key);
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        public void HSet(string key, string field, object value)
        {
            this.rd.HSet(key, field, value);
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        public async Task HSetAsync(string key, string field, object value)
        {
            await this.rd.HSetAsync(key, field, value);
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        public void HMSet(string key, params object[] keyValues)
        {
            this.rd.HMSet(key, keyValues);
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        public async Task HMSetAsync(string key, params object[] keyValues)
        {
            await this.rd.HMSetAsync(key, keyValues);
        }

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public void HDel(string key, params string[] fields)
        {
            this.rd.HDel(key, fields);
        }

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public async Task HDelAsync(string key, params string[] fields)
        {
            await this.rd.HDelAsync(key, fields);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool HExists(string key, string field)
        {
            return this.rd.HExists(key, field);
        }

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HKeys(string key)
        {
            return this.rd.HKeys(key);
        }

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HLen(string key)
        {
            return this.rd.HLen(key);
        }

        /// <summary>
        /// 缓存壳
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T CacheShell<T>(string key, TimeSpan expire, Func<T> getData)
        {
            return this.rd.CacheShell(key, expire, getData);
        }

        /// <summary>
        /// 缓存壳(哈希表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="expire">过期时间</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T CacheShell<T>(string key, string field, TimeSpan expire, Func<T> getData)
        {
            return this.rd.CacheShell(key, field, expire, getData);
        }

        #endregion

    }
}
