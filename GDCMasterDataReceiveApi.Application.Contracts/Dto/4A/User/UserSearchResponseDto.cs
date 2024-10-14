using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User
{
    /// <summary>
    /// 用户响应dto
    /// </summary>
    public class UserSearchResponseDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [ExcelIgnore]
        public string Id { get; set; }
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        /// 
        public string? EmpCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string? CertNo { get; set; }
        /// <summary>
        /// 主职所在部门 ID
        /// </summary>
        public string? OfficeDepId { get; set; }
        /// <summary>
        /// 人员状态信息
        /// </summary>
        [ExcelColumnName("人员状态信息")]
        public string? UserInfoStatus { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 是否启用   0 禁用  1 启用
        /// </summary>
        public string? Enable { get; set; }
    }
    /// <summary>
    /// 用户详情
    /// </summary>
    public class UserSearchDetailsDto
    {
        /// <summary>
        /// ids（首页下钻）
        /// </summary>
        [ExcelIgnore]
        public List<string>? Ids { get; set; }

        [ExcelIgnore]
        public string Id { get; set; }
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        [ExcelColumnName("人员编码")]
        public string? EmpCode { get; set; }
        /// <summary>
        /// 国家地区
        /// </summary>
        [ExcelColumnName("国家地区")]
        public string? CountryRegion { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [ExcelColumnName("姓名")]
        public string? Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [ExcelColumnName("手机号")]
        public string? Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [ExcelColumnName("电子邮箱")]
        public string? Email { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        [ExcelColumnName("证件编号")]
        public string? CertNo { get; set; }
        /// <summary>
        /// 主职所在部门 ID
        /// </summary>
        [ExcelIgnore]
        public string? OfficeDepId { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        [ExcelColumnName("所属部门名称")]
        public string? OfficeDepIdName { get; set; }
        /// <summary>
        /// 人员状态信息
        /// </summary>
        [ExcelColumnName("人员状态信息")]
        public string? UserInfoStatus { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [ExcelIgnore]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 是否启用   0 禁用  1 启用
        /// </summary>
        [ExcelIgnore]
        public string? Enable { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        [ExcelIgnore]
        public string? NameSpell { get; set; }
        /// <summary>
        ///  英文姓名
        /// </summary>
        [ExcelIgnore]
        public string? EnName { get; set; }
        /// <summary>
        ///有效证件类型
        /// </summary>
        [ExcelIgnore]
        public string? CertType { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [ExcelIgnore]
        public string? Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [ExcelIgnore]
        public string? Birthday { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [ExcelIgnore]
        public string? Nationality { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [ExcelIgnore]
        public string? Nation { get; set; }
        /// <summary>
        ///  职务 ID 串 类型编码(H6.2.3),如有多项 拼串，例：01002,01002见 7.1.5 字典
        /// </summary>
        [ExcelIgnore]
        public string? Positions { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        [ExcelIgnore]
        public string? PositionName { get; set; }
        /// <summary>
        /// 最高职级
        /// </summary>
        [ExcelIgnore]
        public string? PositionGrade { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        [ExcelIgnore]
        public string? JobType { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        [ExcelIgnore]
        public string? JobName { get; set; }
        /// <summary>
        /// 人员主职部门内排序号
        /// </summary>
        [ExcelIgnore]
        public string? Sno { get; set; }
        /// <summary>
        /// 兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        [ExcelIgnore]
        public string? SubDepts { get; set; }
        /// <summary>
        /// 用工类型
        /// </summary>
        [ExcelIgnore]
        public string? EmpSort { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        [ExcelIgnore]
        public string? UserLogin { get; set; }
        /// <summary>
        /// 显示编码
        /// </summary>
        [ExcelIgnore]
        public string? HrEmpCode { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        [ExcelIgnore]
        public string? EntryTime { get; set; }
        /// <summary>
        ///  座机
        /// </summary>
        [ExcelIgnore]
        public string? Tel { get; set; }
        /// <summary>
        ///  传真
        /// </summary>
        [ExcelIgnore]
        public string? Fax { get; set; }
        /// <summary>
        ///  办公室号
        /// </summary>
        [ExcelIgnore]
        public string? OfficeNum { get; set; }
        /// <summary>
        ///  扩展字段 1
        /// </summary>
        [ExcelIgnore]
        public string? Attribute1 { get; set; }
        /// <summary>
        ///  扩展字段 2
        /// </summary>
        [ExcelIgnore]
        public string? Attribute2 { get; set; }
        /// <summary>
        ///  扩展字段3
        /// </summary>
        [ExcelIgnore]
        public string? Attribute3 { get; set; }
        /// <summary>
        ///  职级（新版）
        /// </summary>
        [ExcelIgnore]
        public string? PositionGradeNorm { get; set; }
        /// <summary>
        ///  新版最高职级（新版）
        /// </summary>
        [ExcelIgnore]
        public string? HighEstGrade { get; set; }
        /// <summary>
        ///  统一的最高职级（新版）
        /// </summary>
        [ExcelIgnore]
        public string? SameHighEstGrade { get; set; }
        /// <summary>
        ///  政治面貌（新版）
        /// </summary>
        [ExcelIgnore]
        public string? PoliticsFace { get; set; }
        /// <summary>
        ///  派遣公司全称 (协 议 签 署 单位)（新版）所属往来单位信息，单位名称字典
        /// </summary>
        [ExcelIgnore]
        public string? DispatchunitName { get; set; }
        /// <summary>
        ///  派遣公司简称（新版）
        /// </summary>
        [ExcelIgnore]
        public string? DispatchunitShortName { get; set; }
        /// <summary>
        ///  外部人员标识（新版）
        /// </summary>
        [ExcelIgnore]
        public string? Externaluser { get; set; }
        /// <summary>
        ///  扩展字段 4
        /// </summary>
        [ExcelIgnore]
        public string? Attribute4 { get; set; }
        /// <summary>
        ///  扩展字段 5
        /// </summary>v
        [ExcelIgnore]
        public string? Attribute5 { get; set; }
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 机构dto
    /// </summary>
    public class InstutionRespDto
    {
        /// <summary>
        /// oid
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 上级oid
        /// </summary>
        public string? PoId { get; set; }
        /// <summary>
        /// 规则编码
        /// </summary>
        public string? Grule { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
    /// <summary>
    /// 值域
    /// </summary>
    public class VDomainRespDto
    {
        /// <summary>
        /// 值域编码
        /// </summary>
        public string? ZDOM_CODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>
        public string? ZDOM_DESC { get; set; }
        /// <summary>
        /// 域值
        /// </summary>
        public string? ZDOM_VALUE { get; set; }
        /// <summary>
        /// 域值描述
        /// </summary>
        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? ZDOM_LEVEL { get; set; }
    }
    /// <summary>
    /// 国家地区/行政区划（省、市、区）
    /// </summary>
    public class CountryRegionOrAdminDivisionDto
    {
        /// <summary>
        /// 国家地区代码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 国家地区名称
        /// </summary>
        public string? Name { get; set; }
    }
}
