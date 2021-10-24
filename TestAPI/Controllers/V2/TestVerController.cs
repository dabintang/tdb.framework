using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.APIVersion;

namespace TestAPI.Controllers.V2
{
    /// <summary>
    /// 测试版本控制
    /// </summary>
    [TdbApiVersion(2)]
    public class TestVerController : BaseController
    {
        [HttpGet]
        public string Get()
        {
            return "V2";
        }
    }
}
