using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.SqlSugarCore
{
    /// <summary>
    /// dbfist模式
    /// </summary>
    public class DbFirst
    {

        #region DbFist
        /// <summary>
        /// DbFist初始化表
        /// </summary>
        /// <param name="dbCon">连接字符串</param>
        /// <param name="IsCreate">是否生成待特性的类  默认不生成</param>
        public static void InitTable(string dbCon,bool IsCreate=false)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbCon,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            });
            foreach (var item in db.DbMaintenance.GetTableInfoList())
            {
                string entityName = "T" + item.Name.ToUpper().Replace("T_", "").TrimAll().ToPascal();
                db.MappingTables.Add(entityName, item.Name);
                foreach (var col in db.DbMaintenance.GetColumnInfosByTableName(item.Name))
                {
                    db.MappingColumns.Add(col.DbColumnName.ToPascal(), col.DbColumnName, entityName);
                }
            }
            db.DbFirst.IsCreateAttribute(IsCreate).CreateClassFile(Utils.GetRootPath(false)+"\\GHMonitoringCenterApi.Domain\\Models", "GHMonitoringCenterApi.Domain.Models");
        }
        #endregion
    }
}
