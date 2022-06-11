using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.standard;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Route("/v{api-version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [APILogActionFilter]
    public class BaseController : ControllerBase
    {
    }
}
