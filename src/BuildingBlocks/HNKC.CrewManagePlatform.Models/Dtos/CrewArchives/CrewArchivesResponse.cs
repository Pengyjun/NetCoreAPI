namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 船员档案相关dto 
    /// </summary>
    public class CrewArchivesResponse
    {
        /// <summary>
        /// 船员总数
        /// </summary>
        public int TatalCount { get; set; }
        /// <summary>
        /// 在岗数量
        /// </summary>
        public int OnDutyCount { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public string? OnDutyProp { get; set; }
        /// <summary>
        /// 待岗数量
        /// </summary>
        public int WaitCount { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public string? WaitProp { get; set; }
        /// <summary>
        /// 休假数量
        /// </summary>
        public int HolidayCount { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public string? HolidayProp { get; set; }
        /// <summary>
        /// 离调退数量
        /// </summary>
        public int OtherCount { get; set; }
        /// <summary>
        /// 占比 
        /// </summary>
        public string? OtherProp { get; set; }
    }
    #region 响应详情Dto
    /// <summary>
    /// 基本信息
    /// </summary>
    public class BaseInfoDto
    {
        #region 基本信息
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string? PoliticalStatus { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string? NativePlace { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string? Nation { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string? HomeAddress { get; set; }
        /// <summary>
        /// 常住地
        /// </summary>
        public string? BuildAddress { get; set; }
        #endregion

        #region 专业信息
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipType { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewType { get; set; }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        public string? ServiceBookType { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? PositionOnBoard { get; set; }
        #endregion

        #region 家庭成员&紧急联系人
        /// <summary>
        /// 家庭成员
        /// </summary>
        public List<UserInfos>? HomeUser { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public List<UserInfos>? EmergencyContacts { get; set; }
        #endregion
    }
    /// <summary>
    /// 家庭成员/紧急联系人
    /// </summary>
    public class UserInfos
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string? RelationShip { get; set; }
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
    public class LaborServicesInfoDto
    {
        #region 入职信息
        /// <summary>
        /// 入职时间
        /// </summary>
        public string? EntryTime { get; set; }
        /// <summary>
        /// 入职材料
        /// </summary>
        public List<FileInfos>? EntryMaterial { get; set; }
        /// <summary>
        /// 状态：进行中...
        /// </summary>
        public string? Staus { get; set; }
        /// <summary>
        /// 用工类型：正式工...
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        public string? LaborServicesUnit { get; set; }

        #endregion
    }
    /// <summary>
    /// 适任及证书
    /// </summary>
    public class CertificateOfCompetencyDto
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
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? FPosition { get; set; }
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
        /// 扫描件 
        /// </summary>
        public List<FileInfos>? FScans { get; set; }
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
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? SPosition { get; set; }
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
        /// 扫描件 
        /// </summary>
        public List<FileInfos>? SScans { get; set; }
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
        /// 扫描件 
        /// </summary>
        public List<FileInfos>? TrainingScans { get; set; }
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
        public List<FileInfos>? HealthScans { get; set; }
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
        public List<FileInfos>? SeamanScans { get; set; }
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
        public List<VisaRecords>? VisaRecords { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public List<FileInfos>? PassportScans { get; set; }
        #endregion

        #region 技能证书
        /// <summary>
        /// 技能证书
        /// </summary>
        public List<SkillCertificatess>? SkillCertificates { get; set; }
        #endregion

        #region 特种设备证书
        /// <summary>
        /// 特种设备证书
        /// </summary>
        public List<SpecialEquipss>? SpecialEquips { get; set; }
        #endregion
    }
    /// <summary>
    /// 技能证书
    /// </summary>
    public class SkillCertificatess
    {
        /// <summary>
        /// 证书类型
        /// </summary>
        public string? SkillCertificateType { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public FileInfos? SkillScans { get; set; }
    }
    /// <summary>
    /// 特种设备证书
    /// </summary>
    public class SpecialEquipss
    {
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
        public FileInfos? SpecialEquipsScans { get; set; }
    }
    /// <summary>
    /// 签证记录
    /// </summary>
    public class VisaRecords
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 签证类型
        /// </summary>
        public string? VisaType { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public string? DueTimeRemarks { get; set; }
        /// <summary>
        /// 是否到期 1未到期 0已到期
        /// </summary>
        public bool IsDue { get; set; }
    }
    /// <summary>
    /// 学历信息
    /// </summary>
    public class EducationalBackgroundDto
    {
        /// <summary>
        /// 学历信息
        /// </summary>
        public List<QualificationInfo>? QualificationInfos { get; set; }
    }
    /// <summary>
    /// 学历信息
    /// </summary>
    public class QualificationInfo
    {
        /// <summary>
        /// 学历类型：全日制...
        /// </summary>
        public string? QualificationType { get; set; }
        /// <summary>
        /// 学校
        /// </summary>
        public string? School { get; set; }
        /// <summary>
        /// 学历：本科...
        /// </summary>
        public string? Qualification { get; set; }
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
        /// 证书
        /// </summary>
        public List<FileInfos>? QualificationScans { get; set; }
    }
    /// <summary>
    /// 职务晋升
    /// </summary>
    public class PromotionDto
    {
        /// <summary>
        /// 职务晋升
        /// </summary>
        public List<Promotions>? PromotionScans { get; set; }
    }
    /// <summary>
    /// 职务晋升
    /// </summary>
    public class Promotions
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Postition { get; set; }
        /// <summary>
        /// 日期：格式2024年10月15日
        /// </summary>
        public DateTime? PromotionTime { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public FileInfos? PromotionScan { get; set; }
    }
    /// <summary>
    /// 任职船舶
    /// </summary>
    public class WorkShipDto
    {
        /// <summary>
        /// 任职船舶
        /// </summary>
        public List<WorkShips>? WorkShips { get; set; }
    }
    /// <summary>
    /// 任职船舶
    /// </summary>
    public class WorkShips
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipType { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Postition { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? WorkShipStartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? WorkShipEndTime { get; set; }
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
    public class TrainingRecordDto
    {
        /// <summary>
        /// 培训记录
        /// </summary>
        public List<TrainingRecords>? TrainingRecords { get; set; }
    }
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecords
    {
        /// <summary>
        /// 培训类型：安全培训...
        /// </summary>
        public string? TrainingType { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 培训文件
        /// </summary>
        public FileInfos? TrainingScan { get; set; }
    }
    /// <summary>
    /// 年度考核
    /// </summary>
    public class YearCheckDto
    {
        /// <summary>
        /// 年度考核
        /// </summary>
        public List<YearChecks>? YearChecks { get; set; }
    }
    /// <summary>
    /// 年度考核
    /// </summary>
    public class YearChecks
    {
        /// <summary>
        /// 考核结果：优秀...
        /// </summary>
        public string? CheckType { get; set; }
        /// <summary>
        /// 考核模式：2024年度考核
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 考核文件
        /// </summary>
        public FileInfos? TrainingScan { get; set; }
    }
    /// <summary>
    /// 备注
    /// </summary>
    public class NotesDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public List<Notes>? Notes { get; set; }
    }
    /// <summary>
    /// 备注
    /// </summary>
    public class Notes
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 备注日期：yyyy-MM-dd HH：mm：ss
        /// </summary>
        public DateTime? NoteTime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
    }

    #endregion
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfos
    {

    }
}
