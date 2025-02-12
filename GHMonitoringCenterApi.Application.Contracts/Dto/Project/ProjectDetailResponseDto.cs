namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 项目详情返回DTO
    /// </summary>
    public class ProjectDetailResponseDto
    {

  
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? MasterCode { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 所属公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目所属项目部 id
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 所属项目部名称
        /// </summary>
        public string? ProjectDeptName { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 施工地点Id
        /// </summary>
        public Guid? AreaId { get; set; }
        /// <summary>
        /// 施工地点名称
        /// </summary>
        public string? AreaName { get; set; }
        /// <summary>
        /// 施工区域Id
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 施工区域名称
        /// </summary>
        public string? RegionName { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 项目类别名称
        /// </summary>
        public string? CategoryName { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public decimal? Rate { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? ExchangeRate { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例(%)
        /// </summary>
        public decimal? ContractMeaPayProp { get; set; }
        /// <summary>
        /// 项目施工资质Id
        /// </summary>
        public Guid? ProjectConstructionQualificationId { get; set; }
        /// <summary>
        /// 项目施工资质
        /// </summary>
        public string? ProjectConstructionQualificationName { get; set; }
        /// <summary>
        /// 货币Id
        /// </summary>
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 货币名称
        /// </summary>
        public string? CurrencyName { get; set; }
        /// <summary>
        /// 项目状态ID
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 项目状态名称
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// 项目规模Id
        /// </summary>
        public Guid? GradeId { get; set; }
        /// <summary>
        /// 项目规模名称
        /// </summary>
        public string? GradeName { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签名称（0：传统；1：新兴）
        /// </summary>
        public string? TagName { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int Tag2 { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签名称（0：现汇；1：投资）
        /// </summary>
        public string? Tag2Name { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        public bool IsStrength { get; set; }
        /// <summary>
        /// 是否完成编制名称
        /// </summary>
        public string? IsStrengthName { get; set; }
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
        /// 行业分类标准数组
        /// </summary>
        public string[]? ClassifyArray { get; set; }
        /// <summary>
        /// 行业分类标准名称
        /// </summary>
        public string? ClassifyArrayName { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        public bool SocietySpecEffect { get; set; }
        /// <summary>
        /// 是否具有特殊社会效应名称
        /// </summary>
        public string? SocietySpecEffectName { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        public decimal ReclamationArea { get; set; }
        /// <summary>
        /// 工况级数id
        /// </summary>
        public string? ConditionGradeId { get; set; }
        /// <summary>
        /// 工况级数名称
        /// </summary>
        public string? ConditionGradeName { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        public int Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        public int ConstructorNum { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportForMertel { get; set; }
        /// <summary>
        /// 主要工程量描述
        /// </summary>
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 主数据项目Id  
        /// </summary>
        public Guid? MasterProjectId { get; set; }
        /// <summary>
        /// 项目干系单位集合
        /// </summary>
        public List<ProjectOrgDto>? projectOrgDtos { get; set; }
        /// <summary>
        /// 项目干系人员信息集合
        /// </summary>
        public List<ProjectDutyDto>? projectDutyDtos { get; set; }
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
        /// <summary>
        /// 工期
        /// </summary>
        public int WorkDay { get; set; }
        /// <summary>
        /// 月报开累产值
        /// </summary>
        public decimal? CompleteOutputValue { get; set; }
        /// <summary>
        /// 合同约定开工日期
        /// </summary>
        public DateTime? ContractStipulationStartDate { get; set; }
        /// <summary>
        /// 合同约定完工日期
        /// </summary>
        public DateTime? ContractStipulationEndDate { get; set; }
        /// <summary>
        /// 合同签约日期
        /// </summary>
        public DateTime? ContractSignDate { get; set; }

        /// <summary>
        /// 是否是分包项目
        /// </summary>
        public int IsSubContractProject { get; set; }

        /// <summary>
        /// 是否是分包项目
        /// </summary>
        public string? PProjectMasterCode { get; set; }

        /// <summary>
        /// 分包项目名称
        /// </summary>
        public string? PProjectName { get; set; }

        public int ManagerType { get; set; }
        public string ManagerTypeName { get; set; }
    }

    /// <summary>
    /// 项目干系 项目单位名称
    /// </summary>
    public class ProjectOrgDto
    {
        /// <summary>
        /// Id 新增时不进行传参
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// PomId  新增时不进行传参
        /// </summary>
        //public Guid? PomId { get; set; }
        /// <summary>
        /// 项目Id  新增时不进行传参
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目干系单位Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 项目干系单位名称
        /// </summary>
        public string? OrganizationName { get; set; }
        /// <summary>
        /// 项目干系单位类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 项目干系单位名称
        /// </summary>
        public string? TypeName { get; set; }
    }

    /// <summary>
    /// 项目干系人员信息
    /// </summary>
    public class ProjectDutyDto
    {
        /// <summary>
        /// Id 新增时不进行传参
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// PomId  新增时不进行传参
        /// </summary>
        //public Guid? PomId { get; set; }
        /// <summary>
        /// 人员职位类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 人员职位名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 项目Id  新增时不进行传参
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 人员 Id
        /// </summary>
        public Guid AssistantManagerId { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string? AssistantManagerName { get; set; }
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

        
    }




}
