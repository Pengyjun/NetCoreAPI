using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 人员主数据
    /// </summary>
    [SugarTable("t_person", IsDisabledDelete = true)]
    public class Person : BaseEntity<long>
    {
        /// <summary>
        /// 人员编码:必填,HR 系统中定义的人员唯 一编码，默认用户名
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Code")]
        public string EMP_CODE { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Name")]
        public string? NAME { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "NameSpell")]
        public string? NAME_SPELL { get; set; }
        /// <summary>
        /// 英文姓名类型编码（hr.certype）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "EnglishTypeCode")]
        public string? EN_NAME { get; set; }
        /// <summary>
        /// 有效证件类型
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "CertType")]
        public string? CERT_TYPE { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "CertNo")]
        public string CERT_NO { get; set; }
        /// <summary>
        /// 性别必填，类型编码（gbt.2261.1）
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Sex")]
        public string? SEX { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Birthday")]
        public string? BIRTHDAY { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Nationality")]
        public string? NATIONALITY { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Nation")]
        public string? NATION { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Phone")]
        public string? PHONE { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Email")]
        public string? EMAIL { get; set; }
        /// <summary>
        /// 职务Id串 类型编码(H6.2.3),如有多项拼串，例：01002,01002
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Positions")]
        public string? POSITIONS { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "PositionName")]
        public string? POSITION_NAME { get; set; }
        /// <summary>
        /// 最高职级 类型编码（hr.postGrade）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "PositionGrade")]
        public string? POSITION_GRADE { get; set; }
        /// <summary>
        /// 主职所在部门Id
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OfficeDep")]
        public string OFFICE_DEPID { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "JobType")]
        public string? JOB_TYPE { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        [SugarColumn(Length = 2400, ColumnName = "JobName")]
        public string? JOB_NAME { get; set; }
        /// <summary>
        /// 人员主职部门内排序
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Sno")]
        public string SNO { get; set; }
        /// <summary>
        /// 兼职所在部门、岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        [SugarColumn(Length = 4000, ColumnName = "SubDeps")]
        public string? SUB_DEPTS { get; set; }
        /// <summary>
        ///  用工类型 类型编码（hr.emptype）
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "EmpSort")]
        public string? EMP_SORT { get; set; }
        /// <summary>
        /// 用户状态 类型编码（hr.empstatus）
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "EmpStatus")]
        public string? EMP_STATUS { get; set; }
        /// <summary>
        /// 用户密码 加密串（不会下发）
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Pwd")]
        public string? PASSWORD { get; set; }
        /// <summary>
        /// 登录账户 
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "LoginNo")]
        public string? USER_LOGIN { get; set; }
        /// <summary>
        /// 显示编码 用于临时员工转正时使用（从 HR 同步过来时，更新这个字段）。各业务系统显示处理时：如果为空则显示 empcode, 否则显示 hrempcode。
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "EmpCode")]
        public string? HR_EMP_CODE { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "EnterTime")]
        public string? ENTRY_TIME { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Tel")]
        public string? TEL { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Fax")]
        public string? FAX { get; set; }
        /// <summary>
        /// 办公室号
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "OfficeNo")]
        public string? OFFICE_NUM { get; set; }
        /// <summary>
        /// 扩展字段 1 4A 兼职串：“兼职所在部门ID||||排序号，”多条兼职循环拼串, 业务系统需要和 subdepts 合并处理业务
        /// </summary>
        [SugarColumn(Length = 4000, ColumnName = "Attr1")]
        public string? ATTRIBUTE1 { get; set; }
        /// <summary>
        /// 扩展字段 2
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Attr2")]
        public string? ATTRIBUTE2 { get; set; }
        /// <summary> 
        /// 扩展字段 3 交建通假离职，应用于跨公司调动
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Attr3")]
        public string? ATTRIBUTE3 { get; set; }
        /// <summary>
        /// 扩展字段 4
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Attr4")]
        public string? ATTRIBUTE4 { get; set; }
        /// <summary>
        /// 扩展字段 5
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Attr5")]
        public string? ATTRIBUTE5 { get; set; }
        /// <summary>
        /// 职级（新版） 类型编码（hr.postGrade.2）
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "NewPosition")]
        public string? POSITIONGRADENORM { get; set; }
        /// <summary>
        /// 新版最高职级
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "NewHighPosition")]
        public string? HIGHESTGRADE { get; set; }
        /// <summary>
        /// 统一的最高职级 类型编码（hr.postGrade.2）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "UnifiedHighPosition")]
        public string? SAMEHIGHESTGRADE { get; set; }
        /// <summary>
        /// 政治面貌 类型编码（GB.4762）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Face")]
        public string? POLITICSFACE { get; set; }
        /// <summary>
        /// 派遣公司全称(协 议 签 署 单位)
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "DispatchName")]
        public string? DISPATCHUNITNAME { get; set; }
        /// <summary>
        /// 派遣公司简称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "DispatchShortName")]
        public string? DISPATCHUNITSHORTNAME { get; set; }
        /// <summary>
        /// 外部人员标识
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "Externaluser")]
        public string? EXTERNALUSER { get; set; }
    }
}
