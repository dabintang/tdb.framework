using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Validation.Attributes;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 测试参数验证
    /// </summary>
    public class TestParamValidateController : BaseController
    {
        #region 接口

        /// <summary>
        /// 写文本日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<TestParamValidateReq> TestParamValidate([FromQuery]TestParamValidateReq req)
        {
            return BaseItemRes<TestParamValidateReq>.Ok(req);
        }

        /// <summary>
        /// 写文本日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<TestParamValidateReq> TestParamValidate2([FromBody] TestParamValidateReq req)
        {
            return BaseItemRes<TestParamValidateReq>.Ok(req);
        }

        #endregion

        /// <summary>
        /// 条件
        /// </summary>
        public class TestParamValidateReq
        {
            /// <summary>
            /// 姓名
            /// </summary>
            [TdbRequired(ParamName = "姓名")]
            [TdbStringLength(10, ParamName = "姓名")]
            public string Name { get; set; }

            /// <summary>
            /// 身高
            /// </summary>
            [TdbNumRange(ParamName = "身高", MinValue = 5, MaxValue =400)]
            public decimal Hight { get; set; }

            /// <summary>
            /// 年龄
            /// </summary>
            [TdbNumRange(ParamName = "年龄", MinValue = 1, MaxValue = 200)]
            public int Age { get; set; }
        }
    }
}
