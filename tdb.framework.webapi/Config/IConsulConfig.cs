using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// 放在consul的kv上的配置
    /// </summary>
    public interface IConsulConfig
    {
        /// <summary>
        /// 获取consul上的配置信息
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <returns></returns>
        T GetConfig<T>() where T : class, new();

        /// <summary>
        /// 备份配置
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <param name="fullFileName">完整备份文件名(.json文件)</param>
        /// <returns>完整备份文件名</returns>
        string BackupConfig<T>(string fullFileName) where T : class, new();

        /// <summary>
        /// 还原配置
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <param name="fullFileName">完还原份文件名(.json文件)</param>
        /// <param name="msg">还原结果</param>
        /// <returns>还原结果</returns>
        bool RestoreConfig<T>(string fullFileName, out string msg) where T : class, new();
    }
}
