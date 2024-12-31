using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 年度考核
    /// </summary>
    public class YearCheckSearch : BaseRequest
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
        public string? Country { get; set; }
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
        /// 考核结果
        /// </summary>
        public CheckEnum CheckType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CheckTypeStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CheckTypeName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string? CardId { get; set; }
        /// <summary>
        /// 考核填报时间
        /// </summary>
        public string? CheckFillRepTime { get; set; }
        /// <summary>
        /// 考核年份
        /// </summary>
        public string? CheckYear { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? OnStatus { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public CrewStatusEnum DeleteResonEnum { get; set; }
    }
}
