﻿using CH.Simple.Utils;
using CH.Simple.Web.Models;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace CH.Simple.Web.SqlSugar
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
            //注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
            services.AddHttpContextAccessor();
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
                   //每次上下文都会执行

                   //获取IOC对象不要求在一个上下文
                   //var log=s.GetService<Log>()

                   //获取IOC对象要求在一个上下文
                   //var appServive = s.GetService<IHttpContextAccessor>();
                   //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();

                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {

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
                               if (entityInfo.PropertyName == "Created")//创建时间
                               {
                                   entityInfo.SetValue(DateTime.Now);
                               }
                               if (entityInfo.PropertyName == "Createby")//创建人
                               {
                                   //获取token内容写入操作人
                               }
                               break;
                           case DataFilterType.UpdateByObject:
                               if (entityInfo.PropertyName == "Modified")//修改时间
                               {
                                   entityInfo.SetValue(DateTime.Now);
                               }
                               if (entityInfo.PropertyName == "ModifiedBy")//修改人
                               {
                                   //获取token内容写入操作人
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
