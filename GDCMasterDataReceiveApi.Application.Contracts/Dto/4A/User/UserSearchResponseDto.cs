using MiniExcelLibs.Attributes;
using SqlSugar;
using System.ComponentModel;

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
        [DisplayName("人员状态信息")]
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

        [Description("Id")]
        [DisplayName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        [Description("EMP_CODE")]
        [DisplayName("人员编码")]
        public string? EmpCode { get; set; }
        /// <summary>
        /// 国家地区
        /// </summary>
        [Description("NATIONALITY")]
        [DisplayName("国家地区")]
        public string? CountryRegion { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Description("NAME")]
        [DisplayName("姓名")]
        public string? Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("PHONE")]
        [DisplayName("手机号")]
        public string? Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Description("EMAIL")]
        [DisplayName("电子邮箱")]
        public string? Email { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        [Description("CERT_NO")]
        [DisplayName("证件编号")]
        public string? CertNo { get; set; }
        /// <summary>
        /// 主职所在部门 ID
        /// </summary>
        [Description("OFFICE_DEPID")]
        [DisplayName("主职所在部门")]
        public string? OfficeDepId { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        [Description("OFFICE_DEPID")]
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
        /// 是否启用   0 禁用  1 启用
        /// </summary>
        [DisplayName("是否启用")]
        public string? Enable { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        [Description("NAME_SPELL")]
        [DisplayName("姓名全拼")]
        public string? NameSpell { get; set; }
        /// <summary>
        ///  英文姓名
        /// </summary>
        [Description("EN_NAME")]
        [DisplayName("英文姓名")]
        public string? EnName { get; set; }
        /// <summary>
        ///有效证件类型
        /// </summary>
        [Description("CERT_TYPE")]
        [DisplayName("有效证件类型")]
        public string? CertType { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Description("SEX")]
        [DisplayName("性别")]
        public string? Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [Description("BIRTHDAY")]
        [DisplayName("出生日期")]
        public string? Birthday { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [Description("NATIONALITY")]
        [DisplayName("国籍")]
        public string? Nationality { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [Description("NATION")]
        [DisplayName("民族")]
        public string? Nation { get; set; }
        /// <summary>
        ///  职务 ID 串 类型编码(H6.2.3),如有多项 拼串，例：01002,01002见 7.1.5 字典
        /// </summary>
        [Description("POSITIONS")]
        [DisplayName("职务 ID 串")]
        public string? Positions { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>v
        [Description("POSITION_NAME")]
        [DisplayName("职务名称串")]
        public string? PositionName { get; set; }
        /// <summary>
        /// 最高职级
        /// </summary>
        [Description("POSITION_GRADE")]
        [DisplayName("最高职级")]
        public string? PositionGrade { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        [Description("JOB_TYPE")]
        [DisplayName("主职岗位类别")]
        public string? JobType { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        [Description("JOB_NAME")]
        [DisplayName("主职岗位名称")]
        public string? JobName { get; set; }
        /// <summary>
        /// 人员主职部门内排序号
        /// </summary>
        [Description("SNO")]
        [DisplayName("人员主职部门内排序号")]
        public string? Sno { get; set; }
        /// <summary>
        /// 兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        [Description("SUB_DEPTS")]
        [DisplayName("兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”")]
        public string? SubDepts { get; set; }
        /// <summary>
        /// 兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        public List<UserSubDepts>? SubDeptsList { get; set; }
        /// <summary>
        /// 用工类型
        /// </summary>
        [Description("EMP_SORT")]
        [DisplayName("用工类型")]
        public string? EmpSort { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        [Description("USER_LOGIN")]
        [DisplayName("登录账户")]
        public string? UserLogin { get; set; }
        /// <summary>
        /// 显示编码
        /// </summary>
        [Description("HR_EMP_CODE")]
        [DisplayName("显示编码")]
        public string? HrEmpCode { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        [Description("ENTRY_TIME")]
        [DisplayName("本企业入职时间")]
        public string? EntryTime { get; set; }
        /// <summary>
        ///  座机
        /// </summary>
        [Description("TEL")]
        [DisplayName("座机")]
        public string? Tel { get; set; }
        /// <summary>
        ///  传真
        /// </summary>
        [Description("FAX")]
        [DisplayName("传真")]
        public string? Fax { get; set; }
        /// <summary>
        ///  办公室号
        /// </summary>
        [Description("OFFICE_NUM")]
        [DisplayName("办公室号")]
        public string? OfficeNum { get; set; }
        /// <summary>
        ///  扩展字段 1
        /// </summary>
        [DisplayName("扩展字段 1")]
        public string? Attribute1 { get; set; }
        /// <summary>
        ///  扩展字段 2
        /// </summary>
        [DisplayName("扩展字段 2")]
        public string? Attribute2 { get; set; }
        /// <summary>
        ///  扩展字段3
        /// </summary>
        [DisplayName("扩展字段3")]
        public string? Attribute3 { get; set; }
        /// <summary>
        ///  职级（新版）
        /// </summary>
        [Description("POSITIONGRADENORM")]
        [DisplayName("（新版）")]
        public string? PositionGradeNorm { get; set; }
        /// <summary>
        ///  新版最高职级（新版）
        /// </summary>
        [Description("HIGHESTGRADE")]
        [DisplayName("新版最高职级（新版）")]
        public string? HighEstGrade { get; set; }
        /// <summary>
        ///  统一的最高职级（新版）
        /// </summary>
        [Description("SAMEHIGHESTGRADE")]
        [DisplayName("统一的最高职级（新版）")]
        public string? SameHighEstGrade { get; set; }
        /// <summary>
        ///  政治面貌（新版）
        /// </summary>
        [Description("POLITICSFACE")]
        [DisplayName("政治面貌（新版）")]
        public string? PoliticsFace { get; set; }
        /// <summary>
        ///  派遣公司全称 (协 议 签 署 单位)（新版）所属往来单位信息，单位名称字典
        /// </summary>
        [Description("DISPATCHUNITNAME")]
        [DisplayName("派遣公司全称 (协 议 签 署 单位)（新版）所属往来单位信息，单位名称字典")]
        public string? DispatchunitName { get; set; }
        /// <summary>
        ///  派遣公司简称（新版）
        /// </summary>
        [Description("DISPATCHUNITSHORTNAME")]
        [DisplayName("派遣公司简称（新版）")]
        public string? DispatchunitShortName { get; set; }
        /// <summary>
        ///  外部人员标识（新版）
        /// </summary>
        [Description("EXTERNALUSER")]
        [DisplayName("外部人员标识（新版）")]
        public string? Externaluser { get; set; }
        /// <summary>
        ///  扩展字段 4
        /// </summary>
        [DisplayName("扩展字段4")]
        public string? Attribute4 { get; set; }
        /// <summary>
        ///  扩展字段 5
        /// </summary>v
        [DisplayName("扩展字段5")]
        public string? Attribute5 { get; set; }
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        [DisplayName("修改时间")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 是否属于本系统自己新增或修改 true 是 默认false
        /// </summary>
        public bool OwnerSystem { get; set; } = false;

        /// <summary>
        /// 域账号
        /// </summary>
        [DisplayName("域账号")]
        public string? DomainAccount { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        [DisplayName("职工号")]
        public string? WorkerAccount { get; set; }
    }
    /// <summary>
    /// 用户所在兼职
    /// </summary>
    public class UserSubDepts
    {
        /// <summary>
        /// 兼职相关键
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string? Value { get; set; }
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
        /// <summary>
        /// 名称
        /// </summary>
        public string? OCode { get; set; }

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
