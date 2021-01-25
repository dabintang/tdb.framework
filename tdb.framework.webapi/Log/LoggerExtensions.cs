﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 添加日志服务（文本）
        /// </summary>
        /// <param name="services"></param>
        public static void AddTdbLogger(this IServiceCollection services)
        {
            Logger.InitLog();
        }

        /// <summary>
        /// 添加日志服务（MySql）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">MySQL数据库连接字符串</param>
        /// <param name="servicesCode">服务编码</param>
        public static void AddTdbMySqlLogger(this IServiceCollection services, string connectionString, string servicesCode = "TDB")
        {
            Logger.InitMySqlLog(connectionString, servicesCode);
        }

        /// <summary>
        /// 添加日志服务（指定服务）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="getLog">获取服务</param>
        public static void AddTdbLogger(this IServiceCollection services, Func<ILog> getLog)
        {
            Logger.InitLog(getLog());
        }
    }
}
