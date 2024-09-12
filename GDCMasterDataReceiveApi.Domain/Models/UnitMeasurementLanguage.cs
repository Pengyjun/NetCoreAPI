using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 计量单位语种
    /// </summary>
    [SugarTable("t_unitmeasurementlanguage", IsDisabledDelete = true)]
    public class UnitMeasurementLanguage : BaseEntity<long>
    {
        /// <summary>
        /// 计量单位代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZUNITCODE { get; set; }
        /// <summary>
        /// 语言代码：EN、ES	
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZSPRAS { get; set; }
        /// <summary>
        /// 语言描述:
        /// 语言描述是对语言代码的说明：
        /// 英语: EN
        /// 西班牙语：ES
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ZSPTXT { get; set; }
        /// <summary>
        /// 单位描述:计量单位的名称或说明。
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ZUNITDESCR { get; set; }
    }
}
