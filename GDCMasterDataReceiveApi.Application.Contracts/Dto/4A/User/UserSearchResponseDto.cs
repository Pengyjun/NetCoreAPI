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
        public string Id { get; set; }
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
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
        /// 所属部门名称
        /// </summary>
        public string? OfficeDepIdName { get; set; }
        /// <summary>
        /// 人员状态信息
        /// </summary>
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
    public class UserSearchOtherColumns
    {
        /// <summary>
        /// 姓名全拼
        /// </summary>
        public string? NameSpell { get; set; }
        /// <summary>
        ///  英文姓名
        /// </summary>
        public string? EnName { get; set; }
        /// <summary>
        ///有效证件类型
        /// </summary>
        public string? CertType { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string? Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string? Birthday { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string? Nationality { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string? Nation { get; set; }
        /// <summary>
        ///  职务 ID 串 类型编码(H6.2.3),如有多项 拼串，例：01002,01002见 7.1.5 字典
        /// </summary>
        public string? Positions { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        public string? PositionName { get; set; }
        /// <summary>
        /// 最高职级
        /// </summary>
        public string? PositionGrade { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        public string? JobType { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        public string? JobName { get; set; }
        /// <summary>
        /// 人员主职部门内排序号
        /// </summary>
        public string? Sno { get; set; }
        /// <summary>
        /// 兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        public string? SubDepts { get; set; }
        /// <summary>
        /// 用工类型
        /// </summary>
        public string? EmpSort { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        public string? UserLogin { get; set; }
        /// <summary>
        /// 显示编码
        /// </summary>
        public string? HrEmpCode { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        public string? EntryTime { get; set; }
        /// <summary>
        ///  座机
        /// </summary>
        public string? Tel { get; set; }
        /// <summary>
        ///  传真
        /// </summary>
        public string? Fax { get; set; }
        /// <summary>
        ///  办公室号
        /// </summary>
        public string? OfficeNum { get; set; }
        /// <summary>
        ///  扩展字段 1
        /// </summary>
        public string? Attribute1 { get; set; }
        /// <summary>
        ///  扩展字段 2
        /// </summary>
        public string? Attribute2 { get; set; }
        /// <summary>
        ///  扩展字段3
        /// </summary>
        public string? Attribute3 { get; set; }
        /// <summary>
        ///  职级（新版）
        /// </summary>
        public string? PositionGradeNorm { get; set; }
        /// <summary>
        ///  新版最高职级（新版）
        /// </summary>
        public string? HighEstGrade { get; set; }
        /// <summary>
        ///  统一的最高职级（新版）
        /// </summary>
        public string? SameHighEstGrade { get; set; }
        /// <summary>
        ///  政治面貌（新版）
        /// </summary>
        public string? PoliticsFace { get; set; }
        /// <summary>
        ///  派遣公司全称 (协 议 签 署 单位)（新版）所属往来单位信息，单位名称字典
        /// </summary>
        public string? DispatchunitName { get; set; }
        /// <summary>
        ///  派遣公司简称（新版）
        /// </summary>
        public string? DispatchunitShortName { get; set; }
        /// <summary>
        ///  外部人员标识（新版）
        /// </summary>
        public string? Externaluser { get; set; }
        /// <summary>
        ///  扩展字段 4
        /// </summary>
        public string? Attribute4 { get; set; }
        /// <summary>
        ///  扩展字段 5
        /// </summary>
        public string? Attribute5 { get; set; }
    }
    /// <summary>
    /// 用户机构dto
    /// </summary>
    public class UInstutionDto
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
}
