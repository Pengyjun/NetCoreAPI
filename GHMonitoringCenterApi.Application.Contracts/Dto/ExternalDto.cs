using SqlSugar;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// 对外接口dto
    /// </summary>
    public class ExternalDto
    { }
    /// <summary>
    /// 用户信息列表
    /// </summary>
    public class UserInfos
    {
        /// <summary>
        /// 智慧运营监控中心人员主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联的pom系统的主键id
        /// </summary>
        public Guid? PomId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string? IdentityCard { get; set; }
        /// <summary>
        /// 公司Id 关联机构表的pomid
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 部门Id 关联机构表的pomid
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string? LoginName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? LoginAccount { get; set; }
        /// <summary>
        /// 集团账号编码，对应集团empcode字段
        /// </summary>
        public string? GroupCode { get; set; }
        /// <summary>
        /// 用户工号
        /// </summary>
        public string? Number { get; set; }
    }
    /// <summary>
    /// 机构列表
    /// </summary>
    public class InstutionInfos
    {
        /// <summary>
        /// 智慧运营监控中心机构主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联pom系统的主键
        /// </summary>
        public Guid? PomId { get; set; }
        /// <summary>
        /// 机构oid
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 机构父id
        /// </summary>
        public string? Poid { get; set; }
        /// <summary>
        /// 分组id
        /// </summary>
        public string? Gpoid { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string? Ocode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? Shortname { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Sno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Orule { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Grule { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Grade { get; set; }
    }
    /// <summary>
    /// 项目信息列表
    /// </summary>
    public class ProjectInfos
    {
        /// <summary>
        /// 智慧运营监控中心项目主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联pom系统 主键id
        /// </summary>
        public int PomId { get; set; }
        /// <summary>
		/// 名称
		/// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 项目所属项目部 id
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目合同金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例(%)
        /// </summary>
        public decimal ContractMeaPayProp { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        ///对合同工程量说明
        /// </summary>
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string? CompletionDate { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int Tag2 { get; set; }
        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        public bool IsStrength { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        public decimal? CompleteQuantity { get; set; }
        /// <summary>
        ///  完工产值
        /// </summary>
        public decimal? CompleteOutput { get; set; }
        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        public decimal? BudgetInterestRate { get; set; }
        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        public DateTime? CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        public bool SocietySpecEffect { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        public decimal ReclamationArea { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        public int Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        public int Constructor { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportForMertel { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
        /// 是否是重点项目 0 否 1 是
        /// </summary>
        public int? IsMajor { get; set; }
        /// <summary>
        /// 开工时间
        /// </summary>	
        public DateTime? CommencementTime { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        /// <summary>
        /// 项目合同工期(开始)
        /// </summary>
        public DateTime? StartContractDuration { get; set; }
        /// <summary>
        /// 项目合同工期(结束)
        /// </summary>
        public DateTime? EndContractDuration { get; set; }
        /// <summary>
        /// 工程位置
        /// </summary>
        public string? ProjectLocation { get; set; }
        /// <summary>
        /// 中标日期
        /// </summary>
        public DateTime? BidWinningDate { get; set; }
        /// <summary>
        /// 停工日期
        /// </summary>
        public DateTime? ShutdownDate { get; set; }
        /// <summary>
        /// 工期信息
        /// </summary>
        public string? DurationInformation { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        public string? ShutDownReason { get; set; }
        /// <summary>
        /// 合同变更原因
        /// </summary>
        public string? ContractChangeInfo { get; set; }
    }
    /// <summary>
    /// 项目关联的干系人
    /// </summary>
    public class ProjectLeaderInfos
    {
        /// <summary>
        /// 智慧运营监控中心 干系人主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联的pom系统的干系人主键id
        /// </summary>
        public Guid? PomId { get; set; }
        /// <summary>
        /// 项目干系人员职位类型
        /// 1：项目经理； 2：执行总经理； 3：项目书记；
        /// 4：项目总工； 5：安全总监； 6：常务副经理；
        /// 7：项目副经理； 8：现场主要负责人； 9：现场技术负责人
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 关联智慧运营监控系统的项目主键id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 人员 Id
        /// </summary>
        public Guid AssistantManagerId { get; set; }
        /// <summary>
        /// 是否在任 true:在任 false:离任
        /// </summary>
        public bool IsPresent { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
