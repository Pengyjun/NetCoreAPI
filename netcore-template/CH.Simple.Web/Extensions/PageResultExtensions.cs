using CH.Simple.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Web.Extensions
{
    public static class PageResultExtensions
    {
        /// <summary>
        /// EFCore分页查询 同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PageResult<T> ToPageResult<T>(this IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            var pageData = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = queryable.LongCount(),
                List = pageData.ToList()
            };
        }

        /// <summary>
        /// EFCore分页查询 异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PageResult<T>> ToPageResultAsync<T>(this IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            var pageData = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = await queryable.LongCountAsync(),
                List = await pageData.ToListAsync()
            };
        }

        /// <summary>
        /// SqlSugar分页查询 同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarQueryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalNumber"></param>
        /// <returns></returns>
        public static PageResult<T> ToPageResult<T>(this ISugarQueryable<T> sugarQueryable, int pageIndex, int pageSize, ref int totalNumber)
        {
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalNumber,
                List = sugarQueryable.ToPageList(pageIndex, pageSize, ref totalNumber)
            };
        }

        /// <summary>
        /// SqlSugar分页查询 异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarQueryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalNumber"></param>
        /// <returns></returns>
        public static async Task<PageResult<T>> ToPageResultAsync<T>(this ISugarQueryable<T> sugarQueryable, int pageIndex, int pageSize, RefAsync<int> totalNumber)
        {
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalNumber,
                List = await sugarQueryable.ToPageListAsync(pageIndex, pageSize, totalNumber)
            };
        }
    }
}
