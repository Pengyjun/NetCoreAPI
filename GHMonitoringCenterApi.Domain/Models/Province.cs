using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目所属省份
    /// </summary>
    [SugarTable("t_province", IsDisabledDelete = true)]
    public class Province : BaseEntity<Guid>
    {
        
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Zaddvsup { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Zaddvscode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Zaddvsname { get; set; }
        /// <summary>
        /// 国外
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Overseas { get; set; }
        /// <summary>
        /// 地区Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Zaddvslevel { get; set; }
    }
}
