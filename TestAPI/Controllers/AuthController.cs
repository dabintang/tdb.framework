using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tdb.framework.webapi.APILog;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Log;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 身份验证与授权
    /// </summary>
    [APILog(Level = EnumLogLevel.Debug)]
    [Authorize]
    public class AuthController : BaseController
    {
        private const string Secret = "tangdabin20210419";//大于等于16位

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> Login([FromBody] LoginReq req)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(TdbClaimTypes.Name, "张三"),
                    new Claim(TdbClaimTypes.Role, req.Role)
                }),
                //Issuer = "tdb",
                //Audience = "api",
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var ttt = tokenHandler.ReadJwtToken(tokenString);

            return BaseItemRes<string>.Ok(tokenString);
        }

        /// <summary>
        /// 需要身份认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BaseItemRes<string> NeedLogin()
        {
            //HttpContext.User

            return BaseItemRes<string>.Ok(HttpContext.User.FindFirst(TdbClaimTypes.Name).Value);
        }

        /// <summary>
        /// 需要角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public BaseItemRes<string> NeedRoleAdmin()
        {
            //HttpContext.User

            return BaseItemRes<string>.Ok(HttpContext.User.FindFirst(TdbClaimTypes.Name).Value);
        }
    }

    /// <summary>
    /// 登录模拟
    /// </summary>
    public class LoginReq
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
    }
}
