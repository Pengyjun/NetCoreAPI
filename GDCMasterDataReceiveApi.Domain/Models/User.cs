using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 人员主数据
    /// </summary>
    [SugarTable("t_user", IsDisabledDelete = true)]
    public class User : BaseEntity<long>
    {
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        public string? EMP_CODE { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        public string? NAME_SPELL { get; set; }
        /// <summary>
        ///  英文姓名
        /// </summary>
        public string? EN_NAME { get; set; }
        /// <summary>
        ///有效证件类型
        /// </summary>
        public string? CERT_TYPE { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string? CERT_NO { get; set; }
        /// <summary>
        /// 性别
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
        ///  职务 ID 串 类型编码(H6.2.3),如有多项 拼串，例：01002,01002见 7.1.5 字典
        /// </summary>
        public string? POSITIONS { get; set; }
        /// <summary>
        /// 职务名称串
        /// </summary>
        public string? POSITION_NAME { get; set; }
        /// <summary>
        /// 最高职级
        /// </summary>
        public string? POSITION_GRADE { get; set; }
        /// <summary>
        /// 主职所在部门 ID
        /// </summary>
        public string? OFFICE_DEPID { get; set; }
        /// <summary>
        /// 主职岗位类别
        /// </summary>
        public string? JOB_TYPE { get; set; }
        /// <summary>
        /// 主职岗位名称
        /// </summary>
        public string? JOB_NAME { get; set; }
        /// <summary>
        /// 人员主职部门内排序号
        /// </summary>
        public string? SNO { get; set; }
        /// <summary>
        /// 兼职所在部门、 岗位类别、职级、岗位名称及排序 HR 兼职，“兼职所在部门 ID| 岗位类别 ID|兼职职级 |岗位名称|排序号，”多条兼职循环拼串
        /// </summary>
        public string? SUB_DEPTS { get; set; }
        /// <summary>
        /// 用工类型
        /// </summary>
        public string? EMP_SORT { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public string? EMP_STATUS { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string? PASSWORD { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        public string? USER_LOGIN { get; set; }
        /// <summary>
        /// 显示编码
        /// </summary>
        public string? HR_EMP_CODE { get; set; }
        /// <summary>
        /// 本企业入职时间
        /// </summary>
        public string? ENTRY_TIME { get; set; }
        /// <summary>
        ///  座机
        /// </summary>
        public string? TEL { get; set; }
        /// <summary>
        ///  传真
        /// </summary>
        public string? FAX { get; set; }
        /// <summary>
        ///  办公室号
        /// </summary>
        public string? OFFICE_NUM { get; set; }
        /// <summary>
        ///  扩展字段 1
        /// </summary>
        [SugarColumn(Length =1024)]
        public string? ATTRIBUTE1 { get; set; }
        /// <summary>
        ///  扩展字段 2
        /// </summary>
        
        public string? ATTRIBUTE2 { get; set; }
        /// <summary>
        ///  扩展字段3
        /// </summary>
        public string? ATTRIBUTE3 { get; set; }
        /// <summary>
        ///  职级（新版）
        /// </summary>
        public string? POSITIONGRADENORM { get; set; }
        /// <summary>
        ///  新版最高职级（新版）
        /// </summary>
        public string? HIGHESTGRADE { get; set; }
        /// <summary>
        ///  统一的最高职级（新版）
        /// </summary>
        public string? SAMEHIGHESTGRADE { get; set; }
        /// <summary>
        ///  政治面貌（新版）
        /// </summary>
        public string? POLITICSFACE { get; set; }
        /// <summary>
        ///  派遣公司全称 (协 议 签 署 单位)（新版）所属往来单位信息，单位名称字典
        /// </summary>
        public string? DISPATCHUNITNAME { get; set; }
        /// <summary>
        ///  派遣公司简称（新版）
        /// </summary>
        public string? DISPATCHUNITSHORTNAME { get; set; }
        /// <summary>
        ///  外部人员标识（新版）
        /// </summary>
        public string? EXTERNALUSER { get; set; }
        /// <summary>
        ///  扩展字段 4
        /// </summary>
        public string? ATTRIBUTE4 { get; set; }
        /// <summary>
        ///  扩展字段 5
        /// </summary>
        public string? ATTRIBUTE5 { get; set; }
        /// <summary>
        /// 是否启用   0 禁用  1 启用
        /// </summary>
        public int Enable { get; set; }
    }
}
