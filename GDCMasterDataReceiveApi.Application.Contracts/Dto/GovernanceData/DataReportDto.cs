using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    /// <summary>
    /// 数据质量报告dto
    /// </summary>
    public class DataReportDto
    {
    }
    /// <summary>
    /// 数据质量报告
    /// </summary>
    public class DataReportRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string? Table { get; set; }
    }
    /// <summary>
    /// ///数据质量响应体
    /// </summary>
    public class DataReportResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public GruleType Type { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public string? Soure { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        public string? Table { get; set; }
        /// <summary>
        /// 检查字段
        /// </summary>
        public string? Column { get; set; }
        /// <summary>
        /// 规则级别
        /// </summary>
        public GruleGradeType Grade { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CreateTime { get; set; }
    }
    /// <summary>
    /// 数据质量导出
    /// </summary>
    public class DataReportReportResponse
    {
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        [DisplayName("人员编码")]
        public string? EmpCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        [DisplayName("所属部门名称")]
        public string? OfficeDepIdName { get; set; }
        /// <summary>
        /// 人员状态信息
        /// </summary>
        [DisplayName("人员状态信息")]
        public string? UserInfoStatus { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [DisplayName("公司名称")]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OfficeDepId { get; set; }
    }
}
