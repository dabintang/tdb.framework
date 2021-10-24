using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace tdb.framework.webapi.standard.Cache
{
    /// <summary>
    /// 本地锁，按字符串锁
    /// </summary>
    public class LocalLock : IDisposable
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        #region 静态方法

        /// <summary>
        /// 对key上锁
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="maxWaitSeconds">最多等待时间（秒）</param>
        /// <returns></returns>
        public static LocalLock Lock(string key, int maxWaitSeconds = 60)
        {
            DateTime startTime = DateTime.Now;
            string lockVal = null;

            //保证lock时间比较短
            lock (memoryCache)
            {
                lockVal = memoryCache.Get<string>(key);
                if (lockVal == null)
                {
                    memoryCache.Set(key, "", TimeSpan.FromSeconds(MaxLockSecond));
                    return new LocalLock(key, false);
                }
            }

            while (lockVal != null)
            {
                //超过等待
                if ((DateTime.Now - startTime).TotalSeconds > maxWaitSeconds)
                {
                    return new LocalLock(key, true);
                }

                Thread.Sleep(30);

                //保证lock时间比较短
                lock (memoryCache)
                {
                    lockVal = memoryCache.Get<string>(key);
                    if (lockVal == null)
                    {
                        memoryCache.Set(key, "", TimeSpan.FromSeconds(MaxLockSecond));
                        return new LocalLock(key, false);
                    }
                }
            }

            //代码应该不会进来到这里
            memoryCache.Set(key, "", TimeSpan.FromSeconds(MaxLockSecond));
            return new LocalLock(key, false);
        }

        #endregion

        #region 变量/属性

        /// <summary>
        /// key
        /// </summary>
        private string Key { get; set; }

        /// <summary>
        /// 是否被他人锁着
        /// </summary>
        public bool IsLockedByOther { get; private set; }

        #endregion

        #region 常量

        /// <summary>
        /// 最大上锁时间（100秒）
        /// </summary>
        public const int MaxLockSecond = 1000;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="isLockedByOther">是否被他人锁着</param>
        private LocalLock(string key, bool isLockedByOther)
        {
            this.Key = key;
            this.IsLockedByOther = isLockedByOther;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            memoryCache.Remove(this.Key);
        }

        #endregion
    }

    ///// <summary>
    ///// 加锁执行的方法返回类型
    ///// </summary>
    //public class LockResult<T>
    //{
    //    /// <summary>
    //    /// 是否已执行方法
    //    /// </summary>
    //    public bool IsDone { get; private set; }

    //    /// <summary>
    //    /// 结果
    //    /// </summary>
    //    public T Result { get; private set; }

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    /// <param name="isDone"></param>
    //    /// <param name="result"></param>
    //    public LockResult(bool isDone, T result)
    //    {
    //        this.IsDone = isDone;
    //        this.Result = result;
    //    }
    //}

}
