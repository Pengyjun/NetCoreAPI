﻿using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 任职船舶
    /// </summary>
    [SugarTable("t_workship", IsDisabledDelete = true, TableDescription = "任职船舶")]
    public class WorkShip : BaseEntity<long>
    {
        /// <summary>
        /// 所在船舶
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "所在船舶")]
        public string? OnShip { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "职务")]
        public string? Postition { get; set; }
        /// <summary>
        /// 上船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "上船日期")]
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "下船日期")]
        public DateTime WorkShipEndTime { get; set; }
        /// <summary>
        /// 在船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "在船日期", DefaultValue = "0")]
        public int OnBoardTime { get; set; }
        /// <summary>
        /// 休假日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "休假日期", DefaultValue = "0")]
        public int HolidayTime { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? WorkShipId { get; set; }
    }
}
