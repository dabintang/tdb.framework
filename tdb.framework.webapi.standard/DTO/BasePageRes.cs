using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 分页请求结果基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePageRes<T> : BaseItemRes<List<T>>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msgID">消息编码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">结果</param>
        /// <param name="totalRecord">总数</param>
        public BasePageRes(bool isOK, string msgID, string msg, List<T> data, int totalRecord) : base(isOK, msgID, msg, data)
        {
            this.TotalRecord = totalRecord;
        }

        /// <summary>
        /// 成功消息
        /// （IsOK=true,MsgID="OK",Msg="成功"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <param name="totalRecord">总数</param>
        /// <returns></returns>
        public static BasePageRes<T> Ok(List<T> data, int totalRecord)
        {
            var res = new BasePageRes<T>(true, "OK", "成功", data, totalRecord);
            return res;
        }

        /// <summary>
        /// 失败消息
        /// （IsOK=false,MsgID="Fail",Msg="失败"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <param name="totalRecord">总数</param>
        /// <returns></returns>
        public static BasePageRes<T> Fail(List<T> data = null, int totalRecord = 0)
        {
            var res = new BasePageRes<T>(false, "Fail", "失败", data, totalRecord);
            return res;
        }
    }
}
