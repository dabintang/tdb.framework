using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi
{
    /// <summary>
    /// api版本
    /// </summary>
    public class TdbApiVersionAttribute : ApiVersionAttribute
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="version">版本号</param>
        public TdbApiVersionAttribute(double version) : base(version.ToString())
        {
            this.Version = version;
        }
    }
}

/// API版本控制使用：
/// 一、在api的Controller中添加版本
/// [ApiVersion("1.0", Deprecated = false)] //Deprecated是否弃用该版本 默认为false不弃用。即使标记了弃用此接口还是会显示，只是做个提示此版本将会被弃用了。
/// [ApiExplorerSettings(IgnoreApi = true)] 隐藏该接口Controller
/// public class ValuesController : Controller
/// 二、在api的Action中添加版本
/// [Obsolete] //弃用该Action方法
/// [ApiExplorerSettings(IgnoreApi = true)] //隐藏该Action方法
/// public IEnumerable<string> Get()
