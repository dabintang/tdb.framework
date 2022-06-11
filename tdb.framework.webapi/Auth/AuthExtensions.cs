using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.framework.webapi.standard;

namespace tdb.framework.webapi
{
    /// <summary>
    /// 验权
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// 添加身份认证与验权服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="issuerSigningKey">颁发者签名密钥（至少16位）</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddTdbAuthJwtBearer(this IServiceCollection services, string issuerSigningKey)
        {
            return services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = TdbClaimTypes.Name,
                    RoleClaimType = TdbClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey)),
                    //不验Audience
                    ValidateAudience = false,
                    //不验Issuer
                    ValidateIssuer = false,
                    //允许的服务器时间偏移量
                    ClockSkew = TimeSpan.FromSeconds(10),

                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    // ValidateLifetime = true
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        Logger.Ins.Fatal(context.Exception, "认证授权异常");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        context.Response.WriteAsync("权限不足");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("认证未通过");
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
