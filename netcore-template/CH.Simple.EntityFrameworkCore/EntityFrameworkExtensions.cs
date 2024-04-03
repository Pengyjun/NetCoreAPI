using CH.Simple.Models.CommonResult;
using Microsoft.EntityFrameworkCore;
using SqlSugar;

namespace CH.Simple.EntityFrameworkCore
{
    public static class EntityFrameworkExtensions
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

    }
}
