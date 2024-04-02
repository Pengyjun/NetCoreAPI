using CH.Simple.Entities.BaseEntities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Entities
{
    [SugarTable("user")]
    public class User: BaseEntity
    {
        [SugarColumn(ColumnName = "name")]//数据库与实体不一样设置列名 
        public string Name { get; set; }

        [SugarColumn(ColumnName = "mobile")]//数据库与实体不一样设置列名 
        public string Mobile { get; set; }
    }
}
