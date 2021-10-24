using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard.Config
{
    /// <summary>
    /// 分布式配置服务
    /// </summary>
    public interface IDistributedConfig
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <typeparam name="T">配置信息类型</typeparam>
        /// <returns></returns>
        T GetConfig<T>() where T : class, new();

        /// <summary>
        /// 备份配置
        /// </summary>
        /// <typeparam name="T">配置信息类型</typeparam>
        /// <param name="fullFileName">完整备份文件名(.json文件)</param>
        /// <returns>完整备份文件名</returns>
        string BackupConfig<T>(string fullFileName = "") where T : class, new();

        /// <summary>
        /// 还原配置
        /// </summary>
        /// <typeparam name="T">配置信息类型</typeparam>
        /// <param name="config">配置信息</param>
        /// <returns>还原结果</returns>
        bool RestoreConfig<T>(T config) where T : class, new();
    }
}
