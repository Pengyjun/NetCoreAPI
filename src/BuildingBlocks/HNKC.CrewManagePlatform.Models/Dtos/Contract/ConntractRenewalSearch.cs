using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 合同续签详情
    /// </summary>
    public class ConntractRenewalSearch
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// 所在船舶
        /// </summary>
        public string? OnBoardName { get; set; }
        /// <summary>
        /// 第一适任
        /// </summary>
        public string? FPositionName { get; set; }
        /// <summary>
        /// 第二适任
        /// </summary>
        public string? SPositionName { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? OnBoardPositionName { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentTypeName { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string? ContractTypeName { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string? ContractMain { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        public string? LaborCompany { get; set; }
        /// 开始时间
        /// </summary>
        public string? EntryTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
    }
}
