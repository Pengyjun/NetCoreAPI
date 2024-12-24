namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 合同列表
    /// </summary>
    public class ContractSearch : BaseRequest
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CountryNamee { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? EmploymentTypeName { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string? ContractType { get; set; }
        /// <summary>
        /// 
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
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 到期天数
        /// </summary>
        public int DueDays { get; set; }
    }
}
