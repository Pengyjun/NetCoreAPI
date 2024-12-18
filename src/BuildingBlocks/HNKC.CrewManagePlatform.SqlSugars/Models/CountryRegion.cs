using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 国家地区
    /// </summary>
    [SugarTable("t_countryregion", IsDisabledDelete = true, TableDescription = "国家地区")]
    public class CountryRegion : BaseEntity<long>
    {
        /// <summary>
        /// 国家地区代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnDescription = "国家地区代码")]
        public string? Country { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "中文名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "英文名称")]
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 国标三字符代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnDescription = "国标三字符代码")]
        public string? NationalCode { get; set; }
        /// <summary>
        /// 国标数字代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnDescription = "国标数字代码")]
        public string? DigitCode { get; set; }
        /// <summary>
        /// 大洲代码
        /// </summary>  
        [SugarColumn(Length = 3, ColumnDescription = "大洲代码")]
        public string? ContinentCode { get; set; }
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnDescription = "中交区域中心代码")]
        public string? CCCCCenterCode { get; set; }
        /// <summary>
        /// 版本号:数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnDescription = "版本号:数据的版本号。数据每次变更时，版本号自动加1。")]
        public int Version { get; set; }
        /// <summary>
        /// 状态:1是已启用，0是已停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "状态:1是已启用，0是已停用")]
        public string? State { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "一带一路(国资委):0-否，1-是")]
        public string? RoadGuoZiW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "一带一路(海外):0-否，1-是")]
        public string? RoadHaiW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "一带一路(共建):0-否，1-是")]
        public string? RoadGongJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnDescription = "区域代码")]
        public string? AreaCode { get; set; }
    }
}
