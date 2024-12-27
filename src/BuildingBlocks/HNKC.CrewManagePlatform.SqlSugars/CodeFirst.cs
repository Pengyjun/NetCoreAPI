using HNKC.CrewManagePlatform.SqlSugars.Models;
using SqlSugar;
using System.Reflection;

namespace HNKC.CrewManagePlatform.SqlSugars
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
                IsAutoCloseConnection = false,//不设成true要手动close
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoToUpper = false,//禁用自动转成大写表 5.1.3.41-preview04
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
                        if (c.Name == "Id" && c.PropertyType.Name.IndexOf("T") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 32;
                        }
                        if (c.Name == "CreatedBy" && c.PropertyType.Name.IndexOf("T") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 32;
                        }
                        if (c.Name == "ModifiedBy" && c.PropertyType.Name.IndexOf("T") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 32;
                        }
                        //字段名全小写
                        if (!p.IsIgnore)
                            p.DbColumnName = p.DbColumnName.ToLower();
                    }
                }
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, parames) =>
                {
                    ////创建表的sql语句
                    //Console.WriteLine(sql);
                };
            });
            Type[] types = typeof(BaseEntity<Guid>).Assembly.GetTypes()
            .Where(it => it.FullName.Contains("HNKC.CrewManagePlatform.SqlSugars.Models")
            &&!it.Name .Contains("BaseEntity")
            )
            .ToArray();
            db.CodeFirst.InitTables(types);
        }
    }
}
