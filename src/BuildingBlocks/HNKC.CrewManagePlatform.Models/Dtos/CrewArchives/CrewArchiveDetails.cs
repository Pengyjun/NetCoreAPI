using HNKC.CrewManagePlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 查看详情
    /// </summary>
    public class CrewArchiveDetails
    { }
    #region 
    /// <summary>
    /// 基本信息
    /// </summary>
    public class BaseInfoDetails
    {
        #region 基本信息
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string? PoliticalStatus { get; set; }
        public string? PoliticalStatusName { get; set; }
        /// <summary>
        /// 当前船舶任职时间
        /// </summary>
        public string? CurrentShipEntryTime { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string? NativePlace { get; set; }
        public string? NativePlaceName { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string? Nation { get; set; }
        public string? NationName { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string? HomeAddress { get; set; }
        /// <summary>
        /// 常住地
        /// </summary>
        public string? BuildAddress { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string? CardId { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 身份证扫描件 
        /// </summary>
        public List<FileInfosForDetails>? IdCardScans { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime? EntryTime { get; set; }
        /// <summary>
        /// 入职开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 入职结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 入职材料 
        /// </summary>
        public string? EntryScans { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentId { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        public string? LaborCompany { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string? ContarctMain { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string? ContarctType { get; set; }
        #endregion

        #region 专业信息
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewType { get; set; }
        public string? CrewTypeName { get; set; }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        public string? ServiceBookTypeName { get; set; }
        public ServiceBookEnum ServiceBookType { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoardName { get; set; }
        public string? OnBoard { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? PositionOnBoard { get; set; }
        public string? PositionOnBoardName { get; set; }
        /// <summary>
        /// 船员照片 
        /// </summary>
        public FileInfosForDetails? PhotoScans { get; set; }
        #endregion

        #region 家庭成员&紧急联系人
        /// <summary>
        /// 家庭成员
        /// </summary>
        public List<UserInfosForDetails>? HomeUser { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public List<UserInfosForDetails>? EmergencyContacts { get; set; }
        #endregion
    }
    /// <summary>
    /// 家庭成员/紧急联系人
    /// </summary>
    public class UserInfosForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public FamilyRelationEnum RelationShip { get; set; }
        public string? RelationShipName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string? WorkUnit { get; set; }
    }
    /// <summary>
    /// 劳务信息
    /// </summary>
    public class LaborServicesInfoDetails
    {
        #region 入职信息
        /// <summary>
        /// 入职时间
        /// </summary>
        public string? EntryTime { get; set; }
        /// <summary>
        /// 入职材料
        /// </summary>
        public List<FileInfosForDetails>? EntryMaterial { get; set; }
        /// <summary>
        /// 入职信息
        /// </summary>
        public List<UserEntryInfosForDetails>? UserEntryInfosForDetails { get; set; }
        #endregion
    }
    /// <summary>
    /// 劳务合同
    /// </summary>
    public class UserEntryInfosForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public string? EntryDate { get; set; }
        ///// <summary>
        ///// 入职材料 ,拼接文件
        ///// </summary>
        //public List<FileInfosForDetails>? EntryScans { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        public string? LaborCompany { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string? ContarctMain { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public ContractEnum ContractType { get; set; }
        public string? ContractTypeName { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentName { get; set; }
        public string? EmploymentId { get; set; }
        /// <summary>
        /// 状态：进行中...
        /// </summary>
        public string? Staus { get; set; }
    }
    /// <summary>
    /// 适任及证书
    /// </summary>
    public class CertificateOfCompetencyDetails
    {
        #region 第一适任证
        /// <summary>
        /// 证书编号
        /// </summary>
        public string? FCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        public string? FNavigationArea { get; set; }
        public string? FNavigationAreaName { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? FPosition { get; set; }
        public string? FPositionName { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? FSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? FEffectiveTime { get; set; }
        /// <summary>
        /// 有效期倒计时 100天
        /// </summary>
        public int FEffectiveCountdown { get; set; }
        /// <summary>
        /// 扫描件 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? FScans { get; set; }
        #endregion

        #region 第二适任证
        /// <summary>
        /// 证书编号
        /// </summary>
        public string? SCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        public string? SNavigationArea { get; set; }
        public string? SNavigationAreaName { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? SPosition { get; set; }
        public string? SPositionName { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? SSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? SEffectiveTime { get; set; }
        /// <summary>
        /// 有效期倒计时 100天
        /// </summary>
        public int SEffectiveCountdown { get; set; }
        /// <summary>
        /// 扫描件 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? SScans { get; set; }
        #endregion

        #region 培训合格证
        /// <summary>
        /// 证书编号
        /// </summary>
        public string? TrainingCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? TrainingSignTime { get; set; }
        /// <summary>
        /// Z01有效日期
        /// </summary>
        public DateTime? Z01EffectiveTime { get; set; }
        /// <summary>
        /// Z07有效日期
        /// </summary>
        public DateTime? Z07EffectiveTime { get; set; }
        /// <summary>
        /// Z08有效日期
        /// </summary>
        public DateTime? Z08EffectiveTime { get; set; }
        /// <summary>
        /// Z04有效日期
        /// </summary>
        public DateTime? Z04EffectiveTime { get; set; }
        /// <summary>
        /// Z05有效日期
        /// </summary>
        public DateTime? Z05EffectiveTime { get; set; }
        /// <summary>
        /// Z02有效日期
        /// </summary>
        public DateTime? Z02EffectiveTime { get; set; }
        /// <summary>
        /// Z06有效日期
        /// </summary>
        public DateTime? Z06EffectiveTime { get; set; }
        /// <summary>
        /// Z09有效日期
        /// </summary>
        public DateTime? Z09EffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? TrainingScans { get; set; }
        #endregion

        #region 健康证
        /// <summary>
        ///  证书编号
        /// </summary>
        public string? HealthCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? HealthSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? HealthEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public List<FileInfosForDetails>? HealthScans { get; set; }
        #endregion

        #region 海员证
        /// <summary>
        ///  证书编号
        /// </summary>
        public string? SeamanCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? SeamanSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? SeamanEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public List<FileInfosForDetails>? SeamanScans { get; set; }
        #endregion

        #region 护照
        /// <summary>
        ///  证书编号
        /// </summary>
        public string? PassportCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? PassportSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? PassportEffectiveTime { get; set; }
        /// <summary>
        /// 签证记录
        /// </summary>
        public List<VisaRecordsForDetails>? VisaRecords { get; set; }
        /// <summary>
        /// 扫描件 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? PassportScans { get; set; }
        #endregion

        #region 技能证书
        /// <summary>
        /// 技能证书
        /// </summary>
        public List<SkillCertificatesForDetails>? SkillCertificates { get; set; }
        #endregion

        #region 特种设备证书
        /// <summary>
        /// 特种设备证书
        /// </summary>
        public List<SpecialEquipsForDetails>? SpecialEquips { get; set; }
        #endregion
    }
    /// <summary>
    /// 技能证书
    /// </summary>
    public class SkillCertificatesForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        public CertificateTypeEnum SkillCertificateType { get; set; }
        public string? SkillCertificateTypeName { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public FileInfosForDetails? SkillScans { get; set; }
    }
    /// <summary>
    /// 特种设备证书
    /// </summary>
    public class SpecialEquipsForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        public string? SpecialEquipsCertificateType { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? SpecialEquipsEffectiveTime { get; set; }
        /// <summary>
        /// 年审日期
        /// </summary>
        public DateTime? AnnualReviewTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public FileInfosForDetails? SpecialEquipsScans { get; set; }
    }
    /// <summary>
    /// 签证记录
    /// </summary>
    public class VisaRecordsForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string? Country { get; set; }
        public string? CountryName { get; set; }
        /// <summary>
        /// 签证类型
        /// </summary>
        public VisaTypeEnum VisaType { get; set; }
        public string? VisaTypeName { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? DueTime { get; set; }
        /// <summary>
        /// 是否到期 true到期 false未到期
        /// </summary>
        public bool IsDue { get; set; }
    }
    /// <summary>
    /// 学历信息
    /// </summary>
    public class EducationalBackgroundDetails
    {
        /// <summary>
        /// 学历信息
        /// </summary>
        public List<QualificationForDetails>? QualificationInfos { get; set; }
    }
    /// <summary>
    /// 学历信息
    /// </summary>
    public class QualificationForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 学历类型：全日制...
        /// </summary>
        public QualificationTypeEnum QualificationType { get; set; }
        public string? QualificationTypeName { get; set; }
        /// <summary>
        /// 学校
        /// </summary>
        public string? School { get; set; }
        /// <summary>
        /// 学历：本科...
        /// </summary>
        public string? QualificationName { get; set; }
        public QualificationEnum Qualification { get; set; }
        /// <summary>
        /// 专业：计算机网络
        /// </summary>
        public string? Major { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 证书 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? QualificationScans { get; set; }
    }
    /// <summary>
    /// 职务晋升
    /// </summary>
    public class PromotionDetails
    {
        /// <summary>
        /// 职务晋升
        /// </summary>
        public List<PromotionsForDetails>? Promotions { get; set; }
    }
    /// <summary>
    /// 职务晋升
    /// </summary>
    public class PromotionsForDetails
    {
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnShip { get; set; }
        public string? OnShipName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Postition { get; set; }
        public string? PostitionName { get; set; }
        /// <summary>
        /// 日期：格式2024年10月15日
        /// </summary>
        public DateTime? PromotionTime { get; set; }
        /// <summary>
        /// 文件 ,拼接
        /// </summary>
        public List<FileInfosForDetails>? PromotionScans { get; set; }
    }
    /// <summary>
    /// 任职船舶
    /// </summary>
    public class WorkShipDetails
    {
        /// <summary>
        /// 任职船舶
        /// </summary>
        public List<WorkShipsForDetails>? WorkShips { get; set; }
    }
    /// <summary>
    /// 任职船舶
    /// </summary>
    public class WorkShipsForDetails
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? OnShip { get; set; }
        public string? OnShipName { get; set; }
        public ShipTypeEnum ShipType { get; set; }
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Postition { get; set; }
        public string? PostitionName { get; set; }
        /// <summary>
        /// 上船日期
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        public DateTime WorkShipEndTime { get; set; }
        /// <summary>
        /// 休假日期
        /// </summary>
        public int HolidayTime { get; set; }
        /// <summary>
        /// 在船时间
        /// </summary>
        public int OnBoardTime { get; set; }
        /// <summary>
        /// 在船天数
        /// </summary>
        public int OnBoardDay { get; set; }
        /// <summary>
        /// 休假天数
        /// </summary>
        public int Holiday { get; set; }
    }
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordDetails
    {
        /// <summary>
        /// 培训记录
        /// </summary>
        public List<TrainingRecordsForDetails>? TrainingRecords { get; set; }
    }
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordsForDetails
    {
        /// <summary>
        /// 培训类型：安全培训...
        /// </summary>
        public string? TrainingType { get; set; }
        public string? TrainingTypeName { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 培训文件
        /// </summary>
        public List<FileInfosForDetails>? TrainingScans { get; set; }
    }
    /// <summary>
    /// 年度考核
    /// </summary>
    public class YearCheckDetails
    {
        /// <summary>
        /// 年度考核
        /// </summary>
        public List<YearChecksForDetails>? YearChecks { get; set; }
    }
    /// <summary>
    /// 年度考核
    /// </summary>
    public class YearChecksForDetails
    {
        /// <summary>
        /// 考核结果：优秀...
        /// </summary>
        public CheckEnum CheckType { get; set; }
        public string? CheckTypeName { get; set; }
        /// <summary>
        /// 考核模式：2024年度考核
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 考核文件
        /// </summary>
        public List<FileInfosForDetails>? TrainingScans { get; set; }
    }
    /// <summary>
    /// 备注
    /// </summary>
    public class NotesDetails
    {
        /// <summary>
        /// 备注
        /// </summary>
        public List<NotesForDetails>? NotesForDetails { get; set; }
    }
    /// <summary>
    /// 备注
    /// </summary>
    public class NotesForDetails
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 备注日期：yyyy-MM-dd HH：mm：ss
        /// </summary>
        public string? NoteTime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
    }

    #endregion
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfosForDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string? OriginName { get; set; }
        /// <summary>
        /// 后缀名称
        /// </summary>
        public string? SuffixName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long? FileSize { get; set; }
    }
}
