using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.Auth
{
    /// <summary>
    /// 因为觉得ClaimTypes里的常量太长了，这里自定义一个短些的
    /// </summary>
    public class TdbClaimTypes
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public const string SID = "tdb_sid";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string Name = "tdb_name";

        /// <summary>
        /// 角色编码
        /// </summary>
        public const string Role = "tdb_role";
    }
}
