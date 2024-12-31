using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用户入职时间
    /// </summary>
    [SugarTable("t_userentryinfo", IsDisabledDelete = true, TableDescription = "用户入职时间")]
    public class UserEntryInfo : BaseEntity<long>
    {
        /// <summary>
        /// 入职日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "入职日期")]
        public DateTime EntryTime { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "开始日期")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "截止日期")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 入职材料 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "入职材料")]
        public Guid? EntryScans { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        [SugarColumn(Length = 256, ColumnDescription = "劳务公司")]
        public string? LaborCompany { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "合同主体")]
        public string? ContractMain { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "用工形式")]
        public string? EmploymentId { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "合同类型", DefaultValue = "0")]
        public ContractEnum ContractType { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? UserEntryId { get; set; }
    }
}
