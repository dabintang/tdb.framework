using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.Auth
{
    /// <summary>
    /// 身份认证者
    /// </summary>
    public class Verifier
    {
        /// <summary>
        /// 身份认证
        /// </summary>
        private static IAuth _auth = null;

        /// <summary>
        /// 身份认证实例
        /// </summary>
        public static IAuth Ins
        {
            get
            {
                if (_auth == null)
                {
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddTdbAuth]");
                }

                return _auth;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal static void InitAuth()
        {
            _auth = new TdbJWT();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="auth">服务</param>
        internal static void InitAuth(IAuth auth)
        {
            _auth = auth;
        }
    }
}
