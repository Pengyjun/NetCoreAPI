using GDCMasterDataReceiveApi.Domain;
using SqlSugar;
using System.Reflection;

namespace GDCMasterDataReceiveApi.SqlSugarCore
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
                DbType = DbType.Dm,
                IsAutoCloseConnection = true,//不设成true要手动close
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
                        if (c.Name == "id" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "createid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "updateid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "deleteid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "id" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "createid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "updateid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
                        }
                        if (c.Name == "deleteid" && c.PropertyType.FullName.ToLower().IndexOf("guid") >= 0)
                        {
                            p.DataType = "varchar";
                            p.Length = 36;
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
                    //创建表的sql语句
                    Console.WriteLine(sql);
                };
            });
            Type[] types = typeof(BaseEntity<long>).Assembly.GetTypes()
            .Where(it => it.FullName.Contains("GDCMasterDataReceiveApi.Domain.Models"))
            .ToArray();
            db.CodeFirst.InitTables(types);
        }
    }
}
