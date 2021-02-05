using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.consul.kv;
using tdb.framework.webapi.Config;
using System.Text;
using tdb.framework.webapi.DTO;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 放在consul的kv上的配置
    /// </summary>
    public class TestConsulConfigController : BaseController
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns>结果</returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<ConsulConfig> GetConfig()
        {
            var data = DistributedConfigurator.Ins.GetConfig<ConsulConfig>();
            return BaseItemRes<ConsulConfig>.Ok(data);
        }

        /// <summary>
        /// 还原配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        /// <returns>还原结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> RestoreConfig(IFormFile file)
        {
            using (var ms = file.OpenReadStream())
            {
                var buf = new byte[ms.Length];
                ms.Read(buf, 0, (int)ms.Length);

                var jsonTxt = Encoding.Default.GetString(buf);
                var config = JsonConvert.DeserializeObject<ConsulConfig>(jsonTxt);

                DistributedConfigurator.Ins.RestoreConfig<ConsulConfig>(config);
                return BaseItemRes<string>.Ok("还原完成");
            }
        }

        /// <summary>
        /// 备份配置
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>备份配置完整文件名</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> BackupConfig()
        {
            var data = DistributedConfigurator.Ins.BackupConfig<ConsulConfig>();
            return BaseItemRes<string>.Ok(data);
        }
    }

    #region class

    /// <summary>
    /// 还原配置条件
    /// </summary>
    public class RestoreConfigReq
    {
        /// <summary>
        /// 完整还原份文件名(.json文件)
        /// </summary>
        public string FullFileName { get; set; }
    }

    /// <summary>
    /// consul配置
    /// </summary>
    public class ConsulConfig
    {
        /// <summary>
        /// mysql日志数据库连接字符串
        /// </summary>
        [ConsulConfig("MySqlLogConnStr")]
        public string MySqlLogConnStr { get; set; }

        /// <summary>
        /// redis配置
        /// </summary>
        [ConsulConfig("Redis")]
        public RedisConfig Redis { get; set; }

        #region 内部类

        /// <summary>
        /// redis配置
        /// </summary>
        public class RedisConfig
        {
            /// <summary>
            /// 连接字符串
            /// </summary>
            public List<string> ConnectString { get; set; }
        }

        #endregion
    }

    #endregion
}
