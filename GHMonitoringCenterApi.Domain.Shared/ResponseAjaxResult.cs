using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{
    /// <summary>
    /// 系统用的统一返回结果集
    /// </summary>
    public class ResponseAjaxResult<T>
    {
        /// <summary>
        /// 接口描述
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// 响应消息描述  对应ResponseMessage  这个里面的消息 如果 这个里面不存在就自定义
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 接口响应成功 业务成功
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <param name="businessCodeEnum">业务处理结果状态码</param>
        /// <param name="httpStatusCode">接口请求状态码</param>
        public void Success(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.Success)
        {
            Message = message == null ? ResponseMessage.OPERATION_SUCCESS : message;
            Code = httpStatusCode;
        }


        /// <summary>
        /// 接口响应失败
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <param name="httpStatusCode">接口请求状态码</param>
        /// <param name="businessCodeEnum">业务处理结果状态码</param>
        public void Fail(string? message = ResponseMessage.OPERATION_FAIL, HttpStatusCode httpStatusCode = HttpStatusCode.Fail)
        {
            Message = message == null ? ResponseMessage.OPERATION_FAIL : message;
            Code = httpStatusCode;
        }

        /// <summary>
        /// 接口响应成功 业务成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public ResponseAjaxResult<T> SuccessResult(T data,string? message=null)
        {
            Data = data;
            Success(message, Code = HttpStatusCode.Success);
            return this;
        }

        /// <summary>
        /// 接口响应成功 业务成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="count">数量（条数）</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public ResponseAjaxResult<T> SuccessResult(T data, int count, string? message = null)
        {
            Count = count;
            return SuccessResult(data, message);
        }

        /// <summary>
        ///  接口响应失败
        /// </summary>
        /// <param name="httpStatusCode">状态码</param>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        public ResponseAjaxResult<T> FailResult(HttpStatusCode httpStatusCode , string? message)
        {
            Fail(message, httpStatusCode);
            return this;
        }
    }
}
