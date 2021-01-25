using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.common;
using tdb.consul.kv;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// 放在consul的kv上的配置
    /// </summary>
    public class TdbConsulConfig : IConsulConfig
    {
        /// <summary>
        /// consul服务IP
        /// </summary>
        private string _ConsulIP { get; set; }

        /// <summary>
        /// consul服务端口
        /// </summary>
        private int _ConsulPort { get; set; }

        /// <summary>
        /// key前缀，一般用来区分不同服务
        /// </summary>
        private string _PrefixKey { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="consulIP">consul服务IP</param>
        /// <param name="consulPort">consul服务端口</param>
        /// <param name="prefixKey">key前缀，一般用来区分不同服务</param>
        public TdbConsulConfig(string consulIP, int consulPort = 8500, string prefixKey = "TDB")
        {
            this._ConsulIP = consulIP;
            this._ConsulPort = consulPort;
            this._PrefixKey = prefixKey;
        }

        #region 实现接口

        /// <summary>
        /// 获取consul上的配置信息
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <returns></returns>
        public T GetConfig<T>() where T : class, new()
        {
            return ConsulConfigHelper.GetConfig<T>(this._ConsulIP, this._ConsulPort, this._PrefixKey);
        }

        /// <summary>
        /// 备份配置
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <param name="fullFileName">完整备份文件名(.json文件)</param>
        /// <returns>完整备份文件名</returns>
        public string BackupConfig<T>(string fullFileName = "") where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                fullFileName = CommHelper.GetFullFileName($"backup_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_consulConfig.json");
            }

            var path = Path.GetFullPath(fullFileName);
            //如果路径不存在，创建路径
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            //获取consul上的配置信息
            var config = this.GetConfig<T>();

            //转json字符串
            var jsonTxt = JsonConvert.SerializeObject(config);

            //写文件
            File.WriteAllText(fullFileName, jsonTxt, Encoding.Unicode);

            return fullFileName;
        }

        /// <summary>
        /// 还原配置
        /// </summary>
        /// <typeparam name="T">consul配置信息类型</typeparam>
        /// <param name="config">consul配置信息</param>
        /// <param name="fullFileName">完还原份文件名(.json文件)</param>
        /// <param name="msg">还原结果</param>
        /// <returns>还原结果</returns>
        public bool RestoreConfig<T>(string fullFileName, out string msg) where T : class, new()
        {
            if (File.Exists(fullFileName) == false)
            {
                msg = "文件不存在";
                return false;
            }

            //读取文件
            var jsonTxt = File.ReadAllText(fullFileName);

            //转成配置对象
            T config = JsonConvert.DeserializeObject<T>(jsonTxt);

            //还原
            ConsulConfigHelper.PutConfig<T>(this._ConsulIP, this._ConsulPort, config, this._PrefixKey);

            msg = "还原成功";
            return true;
        }

        #endregion
    }
}
