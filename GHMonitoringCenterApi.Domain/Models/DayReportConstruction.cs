using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 项目日报施工记录
    /// </summary>
    [SugarTable("t_dayreportconstruction", IsDisabledDelete = true)]
    public  class DayReportConstruction:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 项目日报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid DayReportId { get; set; }

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 施工分类
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性（自有：1，分包：2,分包-自有: 4,）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 自有船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? OwnerShipId { get; set; }

        /// <summary>
        /// 分包船舶Id Or 往来单位Id (PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public  Guid? SubShipId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 外包支出
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 实际日产量(m³)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal ActualDailyProduction { get; set; }

        /// <summary>
        /// 实际日产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal ActualDailyProductionAmount { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public  int? ConstructionNature { get; set; }
        
    }
}
