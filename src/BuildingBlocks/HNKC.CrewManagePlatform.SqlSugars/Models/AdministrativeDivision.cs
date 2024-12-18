using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 境内行政区划
    /// </summary>
    [SugarTable("t_administrativedivision", IsDisabledDelete = true, TableDescription = "境内行政区划")]
    public class AdministrativeDivision : BaseEntity<long>
    {
        /// <summary>
        /// 行政区划代码
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "行政区划代码")]
        public string? RegionalismCode { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [SugarColumn(Length = 64, ColumnDescription = "行政区划名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 上级行政区划代码:第1级行政区划无上级代码。
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "上级行政区划代码:第1级行政区划无上级代码。")]
        public string? SupRegionalismCode { get; set; }
        /// <summary>
        /// 行政区域级别:总共3级，省、直辖市、自治区是第1级。
        /// </summary>
        [SugarColumn(Length = 16, ColumnDescription = "行政区域级别:总共3级，省、直辖市、自治区是第1级。")]
        public string? RegionalismLevel { get; set; }
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        [SugarColumn(Length = 8, ColumnDescription = "中交区域总部代码")]
        public string? CodeOfCCCCRegional { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "版本：数据的版本号。数据每次变更时，版本号自动加1。")]
        public string? Version { get; set; }
        /// <summary>
        /// 状态：1是已启用，0是已停用
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "状态：1是已启用，0是已停用")]
        public string? State { get; set; }
    }
}
