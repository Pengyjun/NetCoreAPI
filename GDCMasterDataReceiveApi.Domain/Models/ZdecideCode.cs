using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 决算业务板块
    /// </summary>
    [SugarTable("t_zdecidecode", IsDisabledDelete = true)]
    public class ZdecideCode : BaseEntity<long>
    {
        /// <summary>
        /// 决算业务板块代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZDCID { get; set; }
        /// <summary>
        /// 决算业务板块名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZDCNAME { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? EnName { get; set; }
        /// <summary>
        /// 业务分类代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZBTID { get; set; }
        /// <summary>
        /// 业务分类名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZBTNAME { get; set; }
        /// <summary>
        /// 业务分类英文名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZBTENNAME { get; set; }
    }
}
