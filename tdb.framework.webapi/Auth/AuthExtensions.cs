using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.Auth
{
    /// <summary>
    /// 身份认证
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// 添加身份认证服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTdbAuth(this IServiceCollection services)
        {
            Verifier.InitAuth();
        }

        /// <summary>
        /// 添加身份认证服务（指定服务）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="getAuth">获取服务</param>
        public static void AddTdbAuth(this IServiceCollection services, Func<IAuth> getAuth)
        {
            Verifier.InitAuth(getAuth());
        }
    }
}
