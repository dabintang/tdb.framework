using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.DTO;

namespace TestAPI.Controllers
{
    /// <summary>
    /// 测试缓存服务
    /// </summary>
    public class TestCacheController : BaseController
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<bool> Set([FromBody] SetReq req)
        {
            Cacher.Ins.Set(req.Key, req.Value, TimeSpan.FromMinutes(1));
            return BaseItemRes<bool>.Ok(true);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<string> Get([FromQuery] string key)
        {
            var value = Cacher.Ins.Get<string>(key);
            return BaseItemRes<string>.Ok(value);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<bool> Del([FromBody] DelReq req)
        {
            Cacher.Ins.Del(req.Key);
            return BaseItemRes<bool>.Ok(true);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<bool> HSet([FromBody] HSetReq req)
        {
            Cacher.Ins.HSet(req.Key, req.Field, req.Value);
            return BaseItemRes<bool>.Ok(true);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<string> HGet([FromQuery] string key, string field)
        {
            var value = Cacher.Ins.HGet<string>(key, field);
            return BaseItemRes<string>.Ok(value);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<bool> HDel([FromBody] HDelReq req)
        {
            Cacher.Ins.HDel(req.Key, req.Field);
            return BaseItemRes<bool>.Ok(true);
        }

        public class DelReq
        {
            public string Key { get; set; }
        }

        public class SetReq
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class HDelReq
        {
            public string Key { get; set; }
            public string Field { get; set; }
        }

        public class HSetReq
        {
            public string Key { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
        }
    }
}
