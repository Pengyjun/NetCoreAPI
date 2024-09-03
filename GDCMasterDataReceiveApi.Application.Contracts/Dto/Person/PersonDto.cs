namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Person
{
    /// <summary>
    /// 人员 反显
    /// </summary>
    public class PersonDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 人员编码:必填,HR 系统中定义的人员唯 一编码，默认用户名
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        public string? NameSpell { get; set; }
        /// <summary>
        /// 英文姓名类型编码（hr.certype）
        /// </summary>
        public string? EnglishTypeCode { get; set; }
        /// <summary>
        /// 有效证件类型
        /// </summary>
        public string? CertType { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string CertNo { get; set; }
        /// <summary>
        /// 性别必填，类型编码（gbt.2261.1）
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
        /// 手机号码
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 职务Id串 类型编码(H6.2.3),如有多项拼串，例：01002,01002
        /// </summary>
        public string? Positions { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        public string? PositionName { get; set; }
        /// <summary>
        /// 最高职级 类型编码（hr.postGrade）
        /// </summary>
        public string? PositionGrade { get; set; }
        /// <summary>
        /// 主职所在部门Id
        /// </summary>
        public string OfficeDep { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        public string? JobType { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        public string? JobName { get; set; }
        /// <summary>
        /// 人员主职部门内排序
        /// </summary>
        public string Sno { get; set; }
        /// <summary>
        /// 兼职所在部门、岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        public string? SubDeps { get; set; }
        /// <summary>
        ///  用工类型 类型编码（hr.emptype）
        /// </summary>
        public string? EmpSort { get; set; }
        /// <summary>
        /// 用户状态 类型编码（hr.empstatus）
        /// </summary>
        public string? EmpStatus { get; set; }
        /// <summary>
        /// 用户密码 加密串（不会下发）
        /// </summary>
        public string? Pwd { get; set; }
        /// <summary>
        /// 登录账户 
        /// </summary>
        public string? LoginNo { get; set; }
        /// <summary>
        /// 显示编码 用于临时员工转正时使用（从 HR 同步过来时，更新这个字段）。各业务系统显示处理时：如果为空则显示 empcode, 否则显示 hrempcode。
        /// </summary>
        public string? EmpCode { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string? Tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string? Fax { get; set; }
        /// <summary>
        /// 办公室号
        /// </summary>
        public string? OfficeNo { get; set; }
        /// <summary>
        /// 扩展字段 1 4A 兼职串：“兼职所在部门ID||||排序号，”多条兼职循环拼串, 业务系统需要和 subdepts 合并处理业务
        /// </summary>
        public string? Attr1 { get; set; }
        /// <summary>
        /// 扩展字段 2
        /// </summary>
        public string? Attr2 { get; set; }
        /// <summary> 
        /// 扩展字段 3 交建通假离职，应用于跨公司调动
        /// </summary>
        public string? Attr3 { get; set; }
        /// <summary>
        /// 扩展字段 4
        /// </summary>
        public string? Attr4 { get; set; }
        /// <summary>
        /// 扩展字段 5
        /// </summary>
        public string? Attr5 { get; set; }
        /// <summary>
        /// 职级（新版） 类型编码（hr.postGrade.2）
        /// </summary>
        public string? NewPosition { get; set; }
        /// <summary>
        /// 新版最高职级
        /// </summary>
        public string? NewHighPosition { get; set; }
        /// <summary>
        /// 统一的最高职级 类型编码（hr.postGrade.2）
        /// </summary>
        public string? UnifiedHighPosition { get; set; }
        /// <summary>
        /// 政治面貌 类型编码（GB.4762）
        /// </summary>
        public string? Face { get; set; }
        /// <summary>
        /// 派遣公司全称(协 议 签 署 单位)
        /// </summary>
        public string? DispatchName { get; set; }
        /// <summary>
        /// 派遣公司简称
        /// </summary>
        public string? DispatchShortName { get; set; }
        /// <summary>
        /// 外部人员标识
        /// </summary>
        public string? Externaluser { get; set; }
    }
    /// <summary>
    /// 人员  接收
    /// </summary>
    public class PersonReceiveDto
    {
        /// <summary>
        /// 人员编码:必填,HR 系统中定义的人员唯 一编码，默认用户名
        /// </summary>
        public string EMP_CODE { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        public string? NAME_SPELL { get; set; }
        /// <summary>
        /// 英文姓名类型编码（hr.certype）
        /// </summary>
        public string? EN_NAME { get; set; }
        /// <summary>
        /// 有效证件类型
        /// </summary>
        public string? CERT_TYPE { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 性别必填，类型编码（gbt.2261.1）
        /// </summary>
        public string? SEX { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string? BIRTHDAY { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string? NATIONALITY { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string? NATION { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? PHONE { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string? EMAIL { get; set; }
        /// <summary>
        /// 职务Id串 类型编码(H6.2.3),如有多项拼串，例：01002,01002
        /// </summary>
        public string? POSITIONS { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        public string? POSITION_NAME { get; set; }
        /// <summary>
        /// 最高职级 类型编码（hr.postGrade）
        /// </summary>
        public string? POSITION_GRADE { get; set; }
        /// <summary>
        /// 主职所在部门Id
        /// </summary>
        public string OFFICE_DEPID { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        public string? JOB_TYPE { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        public string? JOB_NAME { get; set; }
        /// <summary>
        /// 人员主职部门内排序
        /// </summary>
        public string SNO { get; set; }
        /// <summary>
        /// 兼职所在部门、岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        public string? SUB_DEPTS { get; set; }
        /// <summary>
        ///  用工类型 类型编码（hr.emptype）
        /// </summary>
        public string? EMP_SORT { get; set; }
        /// <summary>
        /// 用户状态 类型编码（hr.empstatus）
        /// </summary>
        public string? EMP_STATUS { get; set; }
        /// <summary>
        /// 用户密码 加密串（不会下发）
        /// </summary>
        public string? PASSWORD { get; set; }
        /// <summary>
        /// 登录账户 
        /// </summary>
        public string? USER_LOGIN { get; set; }
        /// <summary>
        /// 显示编码 用于临时员工转正时使用（从 HR 同步过来时，更新这个字段）。各业务系统显示处理时：如果为空则显示 empcode, 否则显示 hrempcode。
        /// </summary>
        public string? HR_EMP_CODE { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        public string? ENTRY_TIME { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string? TEL { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string? FAX { get; set; }
        /// <summary>
        /// 办公室号
        /// </summary>
        public string? OFFICE_NUM { get; set; }
        /// <summary>
        /// 扩展字段 1 4A 兼职串：“兼职所在部门ID||||排序号，”多条兼职循环拼串, 业务系统需要和 subdepts 合并处理业务
        /// </summary>
        public string? ATTRIBUTE1 { get; set; }
        /// <summary>
        /// 扩展字段 2
        /// </summary>
        public string? ATTRIBUTE2 { get; set; }
        /// <summary> 
        /// 扩展字段 3 交建通假离职，应用于跨公司调动
        /// </summary>
        public string? ATTRIBUTE3 { get; set; }
        /// <summary>
        /// 扩展字段 4
        /// </summary>
        public string? ATTRIBUTE4 { get; set; }
        /// <summary>
        /// 扩展字段 5
        /// </summary>
        public string? ATTRIBUTE5 { get; set; }
        /// <summary>
        /// 职级（新版） 类型编码（hr.postGrade.2）
        /// </summary>
        public string? POSITIONGRADENORM { get; set; }
        /// <summary>
        /// 新版最高职级
        /// </summary>
        public string? HIGHESTGRADE { get; set; }
        /// <summary>
        /// 统一的最高职级 类型编码（hr.postGrade.2）
        /// </summary>
        public string? SAMEHIGHESTGRADE { get; set; }
        /// <summary>
        /// 政治面貌 类型编码（GB.4762）
        /// </summary>
        public string? POLITICSFACE { get; set; }
        /// <summary>
        /// 派遣公司全称(协 议 签 署 单位)
        /// </summary>
        public string? DISPATCHUNITNAME { get; set; }
        /// <summary>
        /// 派遣公司简称
        /// </summary>
        public string? DISPATCHUNITSHORTNAME { get; set; }
        /// <summary>
        /// 外部人员标识
        /// </summary>
        public string? EXTERNALUSER { get; set; }
    }
}
