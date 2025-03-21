using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 合同列表
    /// </summary>
    public class ContractSearch
    {
        /// <summary>
        /// 用户业务id
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 处理增改的业务主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
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
        public Guid? Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnBoardName { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? EmploymentTypeName { get; set; }
        /// <summary>
        /// 第一适任
        /// </summary>
        public string? FPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FPositionName { get; set; }
        /// <summary>
        /// 第二适任
        /// </summary>
        public string? SPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SPositionName { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? OnBoardPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnBoardPositionName { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public ContractEnum ContractType { get; set; }
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
        /// 身份证
        /// </summary>
        public string? CardId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? OnStatus { get; set; }
        /// <summary>
        /// 上船日期
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        public DateTime? WorkShipEndTime { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public CrewStatusEnum DeleteResonEnum { get; set; }
        /// <summary>
        /// 到期天数
        /// </summary>
        public int DueDays { get; set; }
    }
    /// <summary>
    /// 提醒统计数
    /// </summary>
    public class RemindCountDto
    {
        /// <summary>
        /// 合同
        /// </summary>
        public long ContractCount { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public long CertificateCount { get; set; }

    }
}
