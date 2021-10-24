using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.APIVersion;
using tdb.framework.webapi.standard.DTO;

namespace TestAPI.Controllers
{
    /// <summary>
    /// Autofac
    /// </summary>
    [TdbApiVersion(1)]
    public class TestAutofacController : BaseController
    {
        /// <summary>
        /// Autofac上下文
        /// </summary>
        private IComponentContext componentContext;

        public TestAutofacController(IComponentContext _componentContext)
        {
            this.componentContext = _componentContext;
        }

        #region 接口


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> Test0()
        {
            //var c = this.componentContext.Resolve<IShape>();
            var c = this.componentContext.ResolveNamed<IShape>(typeof(Circle0).Name);
            c.Area(10);

            return BaseItemRes<bool>.Ok(true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<double> Test1()
        {
            //var c = this.componentContext.Resolve<IShape>();
            var c = this.componentContext.ResolveNamed<IShape>(typeof(Circle).Name);
            var value = c.Area(10);

            return BaseItemRes<double>.Ok(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> Test2()
        {
            //var c = this.componentContext.Resolve<IShape>();
            var c = this.componentContext.ResolveNamed<IShape>(typeof(Circle).Name);
            c.Area2(new Area2Req() { R = 20 });

            return BaseItemRes<bool>.Ok(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public BaseItemRes<bool> Test3()
        {
            //var c = this.componentContext.Resolve<IShape>();
            var c = this.componentContext.ResolveNamed<IShape>(typeof(Circle).Name);
            c.Area3();

            return BaseItemRes<bool>.Ok(true);
        }

        #endregion
    }

    /// <summary>
    /// 拦截器 需要实现 IInterceptor接口 Intercept方法
    /// </summary>
    public class CallLogger : IInterceptor
    {
        TextWriter _output;

        public CallLogger(TextWriter output)
        {
            _output = output;
        }

        /// <summary>
        /// 拦截方法 打印被拦截的方法执行前的名称、参数和方法执行后的 返回结果
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {

            //_output.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
            //  invocation.Method.Name,
            //  string.Join(", ", invocation.Arguments.Select(a => JsonConvert.SerializeObject(a).ToString()).ToArray()));

            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();

            //_output.WriteLine("方法执行完毕，返回结果：{0}", JsonConvert.SerializeObject(invocation.ReturnValue));

            invocation.ReturnValue = 100D;
        }
    }

    public interface IShape
    {
        /// <summary>
        /// 形状的面积
        /// </summary>
        double Area(double r);

        /// <summary>
        /// 形状的面积
        /// </summary>
        Area2Res Area2(Area2Req req);

        /// <summary>
        /// 形状的面积
        /// </summary>
        void Area3();
    }

    [Intercept(typeof(CallLogger))]
    public class Circle : IShape
    {
        //重写父类抽象方法
        public double Area(double r)
        {
            //Console.WriteLine("你正在调用Area");
            return r * Math.PI;
        }

        public Area2Res Area2(Area2Req req)
        {
            Console.WriteLine("你正在调用Area2");
            var res = new Area2Res();
            res.Area = req.R * Math.PI;
            return res;
        }

        public void Area3()
        {
            Console.WriteLine("你正在调用Area3");
        }
    }

    public class Circle0 : IShape
    {
        //重写父类抽象方法
        public double Area(double r)
        {
            //Console.WriteLine("你正在调用Area");
            return r * Math.PI;
        }

        public Area2Res Area2(Area2Req req)
        {
            Console.WriteLine("你正在调用Area2");
            var res = new Area2Res();
            res.Area = req.R * Math.PI;
            return res;
        }

        public void Area3()
        {
            Console.WriteLine("你正在调用Area3");
        }
    }

    public class Area2Req
    { 
        public double R { get; set; }
    }

    public class Area2Res
    {
        public double Area { get; set; }
    }
}
