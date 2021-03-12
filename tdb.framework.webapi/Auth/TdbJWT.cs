using System;
using System.Collections.Generic;
using System.Text;
using tdb.jwt;

namespace tdb.framework.webapi.Auth
{
    /// <summary>
    /// 身份认证
    /// </summary>
    public class TdbJWT : IAuth
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <typeparam name="T">承载信息类型</typeparam>
        /// <param name="payloadInfo">承载信息</param>
        /// <param name="secret">秘钥</param>
        /// <param name="timeoutSeconds">超时时间（单位：秒）</param>
        /// <returns></returns>
        public string Encode<T>(T payloadInfo, string secret, int timeoutSeconds)
        {
            return JWTHelper.Encode(payloadInfo, secret, timeoutSeconds);
        }

        /// <summary>
        /// 解析并验证token
        /// </summary>
        /// <typeparam name="T">承载信息类型</typeparam>
        /// <param name="token">签名</param>
        /// <param name="secret">秘钥</param>
        /// <returns></returns>
        public T Decode<T>(string token, string secret)
        {
            return JWTHelper.Decode<T>(token, secret);
        }
    }
}
