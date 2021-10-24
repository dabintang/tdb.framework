using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.standard.Auth
{
    /// <summary>
    /// 因为觉得ClaimTypes里的常量太长了，这里自定义一个短些的
    /// </summary>
    public class TdbClaimTypes
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public const string UID = "tdb_uid";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string Name = "tdb_name";

        /// <summary>
        /// 角色编码
        /// </summary>
        public const string Role = "tdb_role";

        /// <summary>
        /// 客户端IP
        /// </summary>
        public const string ClientIP = "tdb_client_ip";
    }
}
