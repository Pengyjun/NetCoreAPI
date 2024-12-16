using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 字典数据表
    /// </summary>
    [SugarTable("t_dictionarytable", IsDisabledDelete = true, TableDescription = "字典表")]
    public class DictionaryTable : BaseEntity<long>
    {
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 接入的id
        /// </summary>
        [SugarColumn(Length = 19)]
        public long VDId { get; set; }
        /// <summary>
        /// 域值层级 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_LEVEL { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ZREMARKS { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [SugarColumn(Length = 16)]
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(Length = 6)]
        public string? ZDELETE { get; set; }
    }
}
