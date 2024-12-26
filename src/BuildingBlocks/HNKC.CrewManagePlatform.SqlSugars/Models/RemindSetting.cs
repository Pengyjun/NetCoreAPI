using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 到期提醒配置
    /// </summary>
    [SugarTable("t_remindsetting", IsDisabledDelete = true ,TableDescription = "到期提醒配置")]
    public class RemindSetting : BaseEntity<long>
    {
        /// <summary>
        /// 提醒类型  1合同 2 证书
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "提醒类型  1合同 2 证书", DefaultValue = "0")]
        public CertificatesEnum Types { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "类型", DefaultValue = "0")]
        public int RemindType { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "提醒时间", DefaultValue = "0")]
        public int Days { get; set; }
        /// <summary>
        /// 是否启用 启用 1
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "是否启用 1启用", DefaultValue = "0")]
        public int Enable { get; set; }
    }
}
