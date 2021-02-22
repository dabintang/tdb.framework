using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Exceptions;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestController : BaseController
    {
        /// <summary>
        /// 时间较长
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<string> LongTime(string param)
        {
            Thread.Sleep(10 * 1000);
            return BaseItemRes<string>.Ok(param);
        }

        /// <summary>
        /// 时间较短
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<string> ShortTime(string param)
        {
            return BaseItemRes<string>.Ok(param);
        }

        /// <summary>
        /// 抛业务内异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> ThrowTdbException()
        {
            throw new TdbException("业务内异常");
        }

        /// <summary>
        /// 抛未知异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> ThrowException()
        {
            throw new Exception("未知异常");
        }
    }
}
