using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 船员值班表
    /// </summary>
    [SugarTable("t_crewrota", IsDisabledDelete = true, TableDescription = "船员值班")]
    public class CrewRota : BaseEntity<long>
    {
        /// <summary>
        /// 部门类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "部门类型", DefaultValue = "0")]
        public RotaEnum RotaType { get; set; }
        /// <summary>
        /// 固定/非固定时间
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "固定/非固定时间", DefaultValue = "0")]
        public TimeEnum TimeType { get; set; }
        /// <summary>
        /// 固定时间区间类型 1：0-4 2：4-8 3：8-12
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "固定时间区间类型 1：0-4 2：4-8 3：8-12", DefaultValue = "0")]
        public TimeSoltEnum TimeSlotType { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "船舶id")]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 班组名称  班组1  班组2  类推
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "班组名称 班组1  班组2  类推")]
        public string? TeamGroup { get; set; }
        /// <summary>
        /// 班组排序  如果是班组1 传1 班组2 传2  类推
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "版本", DefaultValue = "0")]
        public int TeamGroupDesc { get; set; }
        /// <summary>
        /// 领导1
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "领导1")]
        public Guid? FLeaderUserId { get; set; }
        /// <summary>
        /// 领导2
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "领导2")]
        public Guid? SLeaderUserId { get; set; }
        /// <summary>
        /// 其他人员 ,拼接
        /// </summary>
        [SugarColumn(Length = 1000, ColumnDescription = "其他人员 ,拼接")]
        public string? OhterUserId { get; set; }
        /// <summary>
        /// 排班时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "排班时间")]
        public DateTime? SchedulingTime { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "版本", DefaultValue = "0")]
        public int Version { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? RotaId { get; set; }
    }
}
