using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Models.CommonResult
{
    /// <summary>
    /// 不带数据返回的结果格式
    /// </summary>
    public class Result
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public bool IsSuccess { get => Code == 200; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static Result Success(string message = null)
        {
            var r = new Result
            {
                Code = 200,
                Message = message,
                Data = null
            };
            return r;
        }

        /// <summary>
        /// 返回成功,带数据返回
        /// </summary>
        /// <param name="t">数据</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public static Result Success(object data, string message = null)
        {
            return new Result
            {
                Data = data,
                Code = 200,
                Message = message
            };
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <returns></returns>
        public static Result Fail(string message)
        {
            return new Result
            {
                Code = 500,
                Message = message,
                Data = null
            };
        }

        /// <summary>
        /// 自定义错误码
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Result Fail(string message, int code)
        {
            return new Result
            {
                Code = code,
                Message = message,
                Data = null
            };
        }

        /// <summary>
        /// 未认证
        /// </summary>
        /// <returns></returns>
        public static Result NoAuth(string message = null)
        {
            var r = new Result
            {
                Code = 401,
                Message = message,
                Data = null
            };
            if (string.IsNullOrEmpty(message))
                r.Message = "未通过认证";
            return r;
        }

    }

    /// <summary>
    /// 分页返回结果格式
    /// </summary>
    public class PageResult<T>
    {
        private int _PageIndex;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                if (value <= 0)
                    _PageIndex = 1;
                else
                    _PageIndex = value;
            }
        }

        private int _PageSize;
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                if (value > 100)
                    _PageSize = 100;
                else if (value <= 0)
                    _PageSize = 5;
                else
                    _PageSize = value;
            }
        }
        public long TotalCount { get; set; }
        public IEnumerable<T> List { get; set; }


    }

    public static class ResultExtension
    {
        public static IQueryable<TSource> _Page<TSource>(this IQueryable<TSource> queryable, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize > 100)
                pageSize = 100;

            if (pageSize <= 0)
                pageSize = 5;

            return queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IEnumerable<TSource> _Page<TSource>(this IEnumerable<TSource> enu, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize > 100)
                pageSize = 100;

            if (pageSize <= 0)
                pageSize = 5;

            return enu.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
