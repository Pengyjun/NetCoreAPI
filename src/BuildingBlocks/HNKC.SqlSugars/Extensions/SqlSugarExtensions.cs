using HNKC.Models.CommonResult;
using HNKC.SqlSugars.Extensions;
using HNKC.SqlSugars.UnitOfTransaction;
using HNKC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HNKC.SqlSugars.Extensions
{
    public static class SqlSugarExtensions
    {
        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="services"></param>
        /// <param name="conn"></param>
        public static void AddSqlSugarContext(this IServiceCollection services, string conn)
        {
            services.AddScoped<IUnitOfWork, SqlSugarUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
            //services.AddHttpContextAccessor();
            //注册SqlSugar用AddScoped
            services.AddScoped<ISqlSugarClient>(s =>
            {
                //Scoped用SqlSugarClient 
                SqlSugarClient sqlSugar = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = DbType.MySql,
                    ConnectionString = conn,
                    IsAutoCloseConnection = true,
                },
               db =>
               {
                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {
                       switch (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                       {
                           case "Development":
                               {
                                   //Console.WriteLine(sql + "\r\n" + JsonSerializer.Serialize(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                                   //Console.WriteLine();
                                   break;
                               }
                           case "Production": break;

                       }
                   };
                   db.Aop.DataExecuting = (value, entityInfo) =>
                   {
                       switch (entityInfo.OperationType)
                       {
                           case DataFilterType.InsertByObject:
                               if (entityInfo.PropertyName == "Id" && value == null)
                               {
                                   entityInfo.SetValue(PKManager.UUID());
                               }
                               else if (entityInfo.PropertyName == "Created")//创建时间
                               {
                                   entityInfo.SetValue(DateTime.Now);
                               }
                               else if (entityInfo.PropertyName == "CreatedBy")//创建人
                               {
                                   var serviceBuilder = services.BuildServiceProvider();
                                   var _context = serviceBuilder.GetService<IHttpContextAccessor>();
                                   entityInfo.SetValue(_context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null);
                               }
                               break;
                           case DataFilterType.UpdateByObject:
                               if (entityInfo.PropertyName == "Modified")//修改时间
                               {
                                   entityInfo.SetValue(DateTime.Now);
                               }
                               else if (entityInfo.PropertyName == "ModifiedBy")//修改人
                               {
                                   var serviceBuilder = services.BuildServiceProvider();
                                   var _context = serviceBuilder.GetService<IHttpContextAccessor>();
                                   entityInfo.SetValue(_context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null);
                               }
                               break;
                       }
                   };
               });
                return sqlSugar;
            });
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
            var data = sugarQueryable.ToPageList(pageIndex, pageSize, ref totalNumber);
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalNumber,
                List = data
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
            var data = await sugarQueryable.ToPageListAsync(pageIndex, pageSize, totalNumber);
            return new PageResult<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalNumber,
                List = data
            };
        }

        /// <summary>
        /// 查询第一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public static async Task<T> FirstAsync<T>(this ISqlSugarClient client, Expression<Func<T, bool>> funcWhere)
        {
            return await client.Queryable<T>().Where(funcWhere).FirstAsync();
        }

    }
}
