using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard.DTO
{
    /// <summary>
    /// 返回单条记录的结果
    /// </summary>
    public class BaseItemRes<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 消息编码
        /// </summary>
        public string MsgID { get; set; } = "";

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "";

        /// <summary>
        /// 结果
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msgID">消息编码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">结果</param>
        public BaseItemRes(bool isOK, string msgID, string msg, T data)
        {
            this.IsOK = isOK;
            this.MsgID = msgID;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 成功消息
        /// （IsOK=true,MsgID="OK",Msg="成功"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <returns></returns>
        public static BaseItemRes<T> Ok(T data)
        {
            var res = new BaseItemRes<T>(true, "OK", "成功", data);
            return res;
        }

        /// <summary>
        /// 失败消息
        /// （IsOK=false,MsgID="Fail",Msg="失败"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <returns></returns>
        public static BaseItemRes<T> Fail(T data = default(T))
        {
            var res = new BaseItemRes<T>(false, "Fail", "失败", data);
            return res;
        }
    }
}
