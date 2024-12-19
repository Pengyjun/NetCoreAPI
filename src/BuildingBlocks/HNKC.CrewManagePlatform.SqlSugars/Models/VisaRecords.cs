using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 签证记录
    /// </summary>
    [SugarTable("t_visarecords", IsDisabledDelete = true, TableDescription = "签证记录")]
    public class VisaRecords : BaseEntity<long>
    {
        /// <summary>
        /// 国家
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "国家")]
        public string? Country { get; set; }
        /// <summary>
        /// 签证类型
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "签证类型")]
        public VisaTypeEnum VisaType { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "到期时间")]
        public DateTime? DueTime { get; set; }
    }
}
