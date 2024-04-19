using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using System.Reflection;

namespace GHMonitoringCenterApi.SqlSugarCore
{
    /// <summary>
    /// 代码优先
    /// </summary>
    public class CodeFirst
    {
        public static void InitTable(string dbCon)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbCon,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoToUpper = false,
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {

                    EntityService = (c, p) =>
                    {
                        if (p.IsPrimarykey == false && c.PropertyType.IsGenericType &&
                        c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            p.IsNullable = true;

                        }
                        if (p.IsPrimarykey == false && new NullabilityInfoContext()
                         .Create(c).WriteState is NullabilityState.Nullable)
                        {
                            p.IsNullable = true;

                        }
                        if (c.Name == "Id" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "CreateId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "UpdateId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "DeleteId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "Id" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "CreateId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "UpdateId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "DeleteId" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        //字段名全小写
                        p.DbColumnName = p.DbColumnName.ToLower();
                    }
                }
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, parames) =>
                {
                    //创建表的sql语句
                    //Console.WriteLine(sql);
                };
            });
            Type[] types = typeof(Company).Assembly.GetTypes()
            .Where(it => it.FullName.Contains("GHMonitoringCenterApi.Domain.Models.ShipMovementRecord"))
            .ToArray();
            db.CodeFirst.InitTables(types);
        }
    }
}
