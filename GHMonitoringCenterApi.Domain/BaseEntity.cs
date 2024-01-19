using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain
{

    /// <summary>
    /// CodeFirst基本实体全部都要继承此类
    /// </summary>
    /// <typeparam name="T">主键数据类型</typeparam>
    public class BaseEntity<T> where T : struct
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public T Id { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public T? CreateId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime",IsOnlyIgnoreUpdate =true)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新人Id
        /// </summary>
        public T? UpdateId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public T? DeleteId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 是否删除  0删除  1有效
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int IsDelete { get; set; } = 1;
    }
}
