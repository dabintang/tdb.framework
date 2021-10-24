using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.APIVersion;
using tdb.framework.webapi.standard.DTO;
using tdb.framework.webapi.standard.Log;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 日志测试
    /// </summary>
    [TdbApiVersion(1)]
    public class TestLogController : BaseController
    {
        #region 接口

        /// <summary>
        /// 写文本日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> WriteFileLog()
        {
            Logger.Ins.Info("info日志");
            Logger.Ins.Error(new Exception("异常"), "error日志");

            return BaseItemRes<bool>.Ok(true);
        }

        #endregion
    }
}
