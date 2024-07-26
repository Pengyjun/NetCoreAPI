using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared
{
    /// <summary>
    /// 记录请求信息类
    /// </summary>
    public class  RecordRequestInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        ///// <summary>
        ///// 请求ID 对应请求唯一标识
        ///// </summary>
        //public string RequestId { get; set; }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>

        public string? UserName { get; set; }
       /// <summary>
       /// 请求时间
       /// </summary>
        public string? RequestTime { get; set; }
    
        /// <summary>
        /// sql语句
        /// </summary>
        public List<SqlExecInfo>  SqlExecInfos { get; set; }
        ///// <summary>
        ///// sql执行时间
        ///// </summary>
        //public string? SqlExecutionDuration { get; set; }
        /// <summary>
        /// 接口总耗时时间（包括sql耗时）
        /// </summary>
        public string? ExecutionDuration { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string? ClientIpAddress { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public BrowserInfo  BrowserInfo { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string? HttpMethod { get; set; }

        /// <summary>
        /// Action方法名
        /// </summary>
        public string? ActionMethodName { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public RequestInfo  RequestInfo { get; set; }

        /// <summary>
        /// 请求路由
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exceptions Exceptions { get; set; }
        /// <summary>
        /// 请求状态码
        /// </summary>
        public int HttpStatusCode { get; set; }

    }

    #region sql执行信息
    /// <summary>
    /// sql执行信息
    /// </summary>
    public class SqlExecInfo 
    {
        /// <summary>
        /// sql语句
        /// </summary>
        public string? Sql { get; set; }

        /// <summary>
        /// sql语句总耗时
        /// </summary>
        public string? SqlTotalTime { get; set; }
    }
    #endregion

    #region 请求参数
    /// <summary>
    /// 请求参数
    /// </summary>
    public class RequestInfo 
    {
        /// <summary>
        /// 请求接口入参
        /// </summary>
        public string? Input { get; set; }
    }
    #endregion

    #region 浏览器信息
    /// <summary>
    /// 浏览器信息
    /// </summary>
    public class BrowserInfo
    {
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string? Browser { get; set; }
    }
    #endregion

    #region 异常信息
    /// <summary>
    /// 异常信息
    /// </summary>
    public class Exceptions {
        /// <summary>
        /// 异常详情
        /// </summary>
        public string? ExceptionInfo { get; set; }
    }
    #endregion
}
