using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tdb.common;
using tdb.framework.webapi.standard.Exceptions;

namespace tdb.framework.webapi.standard.Cache
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class TdbMemoryCache : ICache
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        private IMemoryCache cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">内存缓存配置</param>
        public TdbMemoryCache(MemoryCacheOptions option)
        {
            if (option == null)
            {
                option = new MemoryCacheOptions();
            }

            this.cache = new MemoryCache(option);
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
            return this.cache.Get<T>(key);
        }

        /// <summary>
        /// 设置指定 key 的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public void Set(string key, object value, TimeSpan expire)
        {
            this.cache.Set(key, value, expire);
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
            this.Set(key, value, expire);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        public void Del(params string[] keys)
        {
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    this.cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        public async Task DelAsync(params string[] keys)
        {
            this.Del(keys);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool Exists(string key)
        {
            object objVal;
            return this.cache.TryGetValue(key, out objVal);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public void Expire(string key, TimeSpan expire)
        {
            object objVal;
            if (this.cache.TryGetValue(key, out objVal))
            {
                this.Set(key, objVal, expire);
            }
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        public async Task ExpireAsync(string key, TimeSpan expire)
        {
            this.Expire(key, expire);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireAt">过期时间</param>
        public void ExpireAt(string key, DateTime expireAt)
        {
            var expire = expireAt - DateTime.Now;
            if (expire.TotalSeconds > 0)
            {
                this.Expire(key, expire);
            }
            else
            {
                this.Del(key);
            }
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireAt">过期时间</param>
        public async Task ExpireAtAsync(string key, DateTime expireAt)
        {
            this.ExpireAt(key, expireAt);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        public string[] Keys(string pattern)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = cache.GetType().GetField("_entries", flags).GetValue(cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys.ToArray();
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                var key = cacheItem.Key.ToString();
                if (pattern == "*" || string.IsNullOrEmpty(pattern))
                {
                    keys.Add(key);
                }
                else if (pattern.StartsWith("*") && key.StartsWith(pattern.Substring(1)))
                {
                    keys.Add(key);
                }
                else if (pattern.EndsWith("*") && key.EndsWith(pattern.Substring(0, pattern.Length -1)))
                {
                    keys.Add(key);
                }
                else if (key == pattern)
                {
                    keys.Add(key);
                }
            }
            return keys.ToArray();
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
            var dic = this.HGetAll<T>(key);
            if (dic == null)
            {
                return default(T);
            }

            if (dic.ContainsKey(field))
            {
                return dic[field];
            }

            return default(T);
        }

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> HGetAll<T>(string key)
        {
            object objVal;
            if (this.cache.TryGetValue(key, out objVal))
            {
                var objDicVal = objVal as Dictionary<string, object>;
                if (objDicVal == null)
                {
                    return null;
                }

                if (typeof(T) == typeof(object))
                {
                    return objDicVal as Dictionary<string, T>;
                }

                var dicVal = new Dictionary<string, T>();
                foreach(var objItem in objDicVal)
                {
                    if (objItem.Value is T)
                    {
                        dicVal[objItem.Key] = (T)objItem.Value;
                    }
                }
                
                return dicVal;
            }
            else
            {
                return null;
            }    
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        public void HSet(string key, string field, object value)
        {
            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
                this.Set(key, dic, TimeSpan.FromDays(100));
            }

            dic[field] = value;
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        public async Task HSetAsync(string key, string field, object value)
        {
            this.HSet(key, field, value);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        public void HMSet(string key, params object[] keyValues)
        {
            if (keyValues == null)
            {
                return;
            }

            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
                this.Set(key, dic, TimeSpan.FromDays(100));
            }

            for (int i = 0; i < keyValues.Length; i++)
            {
                var field = keyValues[i] as string;
                var value = keyValues[++i];

                dic[field] = value;
            }
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        public async Task HMSetAsync(string key, params object[] keyValues)
        {
            this.HMSet(key, keyValues);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public void HDel(string key, params string[] fields)
        {
            if (fields == null)
            {
                return;
            }

            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                return;
            }

            foreach(var field in fields)
            {
                if (dic.ContainsKey(field))
                {
                    dic.Remove(field);
                }
            }
        }

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public async Task HDelAsync(string key, params string[] fields)
        {
            this.HDel(key, fields);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool HExists(string key, string field)
        {
            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                return false;
            }

            return dic.ContainsKey(field);
        }

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HKeys(string key)
        {
            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                return new string[0];
            }

            return dic.Keys.ToArray();
        }

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HLen(string key)
        {
            var dic = this.HGetAll<object>(key);
            if (dic == null)
            {
                return 0;
            }

            return dic.Count;
        }

        /// <summary>
        /// 缓存壳
        /// </summary>
        /// <typeparam name="T">必须是可空类型</typeparam>
        /// <param name="key"></param>
        /// <param name="expire">过期时间</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T CacheShell<T>(string key, TimeSpan expire, Func<T> getData)
        {
            //必须是可空类型才能用这个缓存方法，否则无法判断是否已有缓存
            if (CheckHelper.IsNullableType(typeof(T)) == false)
            {
                throw new TdbException("必须是可空类型才能用[TdbMemoryCache.CacheShell]缓存方法");
            }

            //先从缓存获取看是否已有缓存
            var value = this.Get<T>(key);
            if (value != null)
            {
                return value;
            }

            //上锁
            using (var lockRet = LocalLock.Lock(key))
            {
                //再次尝试从缓存获取值
                var valAgain = this.Get<T>(key);
                if (valAgain != null)
                {
                    return valAgain;
                }

                //获取源数据
                value = getData();
            }

            //缓存
            if (value != null)
            {
                this.Set(key, value, expire);
            }

            return value;
        }

        /// <summary>
        /// 缓存壳(哈希表)
        /// </summary>
        /// <typeparam name="T">必须是可空类型</typeparam>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="expireAt">过期时间</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T HCacheShell<T>(string key, string field, DateTime expireAt, Func<T> getData)
        {
            //必须是可空类型才能用这个缓存方法，否则无法判断是否已有缓存
            if (CheckHelper.IsNullableType(typeof(T)) == false)
            {
                throw new TdbException("必须是可空类型才能用[TdbMemoryCache.CacheShell]缓存方法");
            }

            //先从缓存获取看是否已有缓存
            var value = this.HGet<T>(key, field);
            if (value != null)
            {
                return value;
            }

            //上锁
            using (var lockRet = LocalLock.Lock(key))
            {
                //再次尝试从缓存获取值
                var valAgain = this.HGet<T>(key, field);
                if (valAgain != null)
                {
                    return valAgain;
                }

                //获取源数据
                value = getData();

                //缓存
                if (value != null)
                {
                    this.HSet(key, field, value);

                    //设置过期时间
                    this.ExpireAt(key, expireAt);
                }
            }

            return value;
        }

        #endregion
    }
}
