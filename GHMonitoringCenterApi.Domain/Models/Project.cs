using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目表
    /// </summary>
    [SugarTable("t_project", IsDisabledDelete = true)]
    public class Project : BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int PomId { get; set; }
        ///// <summary>
        ///// 机构ID
        ///// </summary>
        //[SugarColumn(Length = 36)]
        //public Guid? InstitutionId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ShortName { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(5,2)", DefaultValue = "0")]
        public decimal? Score { get; set; }
        /// <summary>
        /// 主数据编码编码
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? MasterCode { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? Code { get; set; }
        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 项目所属项目部 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目状态 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 项目类型 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 工况级数 id
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ConditionGradeId { get; set; }
        /// <summary>
        /// 项目规模Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? GradeId { get; set; }
        /// <summary>
        /// 币种 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 施工地点 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? AreaId { get; set; }

        /// <summary>
        /// 施工区域 id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 项目合同金额
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例(%)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ContractMeaPayProp { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)")]
        public decimal? Rate { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal? ExchangeRate { get; set; }
        /// <summary>
        /// 项目施工资质Id
        /// </summary>
        [SugarColumn(Length = 50)]
        public Guid? ProjectConstructionQualificationId { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? Quantity { get; set; }

        /// <summary>
        ///对合同工程量说明
        /// </summary>
        [SugarColumn(Length = 3000)]
        public string? QuantityRemarks { get; set; }

        /// <summary>
        /// 竣工日期
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? CompletionDate { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)")]
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)")]
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Tag { get; set; }

        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Tag2 { get; set; }

        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Category { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsStrength { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? CompleteQuantity { get; set; }
        /// <summary>
        ///  完工产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? CompleteOutput { get; set; }

        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)")]
        public decimal? BudgetInterestRate { get; set; }

        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CompilationTime { get; set; }

        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool SocietySpecEffect { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ReclamationArea { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Constructor { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ReportForMertel { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Remarks { get; set; }
        /// <summary>
        /// 主数据项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? MasterProjectId { get; set; }

        /// <summary>
        /// 是否是重点项目 0 否 1 是
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? IsMajor { get; set; }

        /// <summary>
        /// 开工时间
        /// </summary>	
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CommencementTime { get; set; }

        /// <summary>
        /// 完工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 项目合同工期(开始)
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartContractDuration { get; set; }

        /// <summary>
        /// 项目合同工期(结束)
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndContractDuration { get; set; }
        /// <summary>
        /// 工程位置
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ProjectLocation { get; set; }
        /// <summary>
        /// 中标日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? BidWinningDate { get; set; }
        /// <summary>
        /// 停工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ShutdownDate { get; set; }
        /// <summary>
        /// 工期信息
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? DurationInformation { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ShutDownReason { get; set; }
        /// <summary>
        /// 合同变更原因
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ContractChangeInfo { get; set; }
        /// <summary>
        /// 工期默认给1
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int WorkDay { get; set; }

        /// <summary>
        /// 合同约定开工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractStipulationStartDate { get; set; }
        /// <summary>
        /// 合同约定完工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractStipulationEndDate { get; set; }
        /// <summary>
        /// 合同签约日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractSignDate { get; set; }

        /// <summary>
        /// 分包项目得上级得主数据编码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? PProjectMasterCode { get; set; }
        /// <summary>
        /// 是否时分包项目
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int IsSubContractProject { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ManagerType { get; set; }
    }
}
