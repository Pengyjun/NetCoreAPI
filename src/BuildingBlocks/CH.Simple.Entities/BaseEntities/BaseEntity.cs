using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Entities.BaseEntities
{
    public class BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "id")] //设置主键
        public string Id { get; set; }


        [SugarColumn(ColumnName = "created")]//数据库与实体不一样设置列名 
        public DateTime Created { get; set; }


        [SugarColumn(ColumnName = "created_by")]//数据库与实体不一样设置列名 
        public string? CreatedBy { get; set; }


        [SugarColumn(ColumnName = "modified")]//数据库与实体不一样设置列名 
        public DateTime? Modified { get; set; }


        [SugarColumn(ColumnName = "modified_by")]//数据库与实体不一样设置列名 
        public string? ModifiedBy { get; set; }


        [SugarColumn(ColumnName = "is_delete")]//数据库与实体不一样设置列名 
        public bool IsDelete { get; set; }
    }
}
