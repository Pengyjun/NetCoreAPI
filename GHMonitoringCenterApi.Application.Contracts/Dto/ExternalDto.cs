using System.ComponentModel.DataAnnotations;

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
        /// 项目类型id
        /// </summary>
        public Guid? ProjectTypeId { get; set; }
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
        /// <summary>
        /// 区域id
        /// </summary>
        public Guid? RegionId { get; set; }
    }
    /// <summary>
    /// 对外接口 项目区域信息
    /// </summary>
    public class ProjectAreaInfos
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
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
    /// <summary>
    /// 项目状态信息
    /// </summary>
    public class ProjectStatusInfos
    {
        /// <summary>
        /// 监控中心项目状态主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目状态Id
        /// </summary>
        public Guid StatusId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
    }
    /// <summary>
    /// 项目类型信息
    /// </summary>
    public class ProjectTypeInfos
    {
        /// <summary>
        /// 项目类型主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联的pomid
        /// </summary>
        public Guid PomId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? Name { get; set; }
    }
    /// <summary>
    /// 对外接口  船舶日报请求dto
    /// </summary>
    public class ShipDayReportsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipPingId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //开始日期结束日期都不是空
            if (!((StartTime == DateTime.MinValue || string.IsNullOrWhiteSpace(StartTime.ToString())) && (EndTime == DateTime.MinValue || string.IsNullOrWhiteSpace(EndTime.ToString()))))
            {
                if (StartTime > EndTime)
                {
                    yield return new ValidationResult("开始日期大于结束日期", new string[] { nameof(StartTime) });
                }
            }
        }
    }
    /// <summary>
    /// 船舶公用下拉列表数据 清单类型/工艺方式/工况级别/吹填分类
    /// </summary>
    public class ShipCommResponseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// pomid
        /// </summary>
        public Guid? PomId { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { set; get; }

    }
    /// <summary>
    /// 船舶日报数据集
    /// </summary>
    public class ShipDayReports
    {
        /// <summary>
        /// 监控中心船舶日报主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 日报日期
        /// </summary>
        public int? DateDay { get; set; }
        /// <summary>
        /// 船舶状态
        /// </summary>
        public int ShipState { get; set; }
        /// <summary>
        /// 船舶状态名称
        /// </summary>
        public string? ShipStateName { get; set; }
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船报产量(方)
        /// </summary>
        public decimal? ShipReportedProduction { get; set; }
        /// <summary>
        ///管线长度 （m）
        /// </summary>
        public decimal? PipelineLength { get; set; }
        /// <summary>
        /// 油耗
        /// </summary>
        public decimal? OilConsumption { get; set; }
        /// <summary>
        /// 估算成本(元)
        /// </summary>
        public decimal? EstimatedCostAmount { get; set; }
        /// <summary>
        /// 估算产值（元）(船报产量*估算单价) 注：产量和单价都是保留小数两位相乘，保证数据完整性保留四位小数
        /// </summary>
        public decimal? EstimatedOutputAmount { get; set; }
        /// <summary>
        /// 施工效率
        /// </summary>
        public decimal? ConstructionEfficiency { get; set; }
        /// <summary>
        /// 生产停歇(h)
        /// </summary>
        public decimal? ProductionStoppage { get; set; }
        /// <summary>
        /// 非生产停歇(h)
        /// </summary>
        public decimal? NonProductionStoppage { get; set; }
        /// <summary>
        /// 时间利用率(%)
        /// </summary>
        public decimal? TimeAvailability { get; set; }
        /// <summary>
        /// 生产运转时间
        /// </summary>
        public decimal? ProductionOperatingTime { get; set; }
        /// <summary>
        ///挖泥
        /// </summary>
        public decimal? Dredge { get; set; }
        /// <summary>
        ///航行
        /// </summary>
        public decimal? Sail { get; set; }
        /// <summary>
        ///吹水
        /// </summary>
        public decimal? BlowingWater { get; set; }
        /// <summary>
        ///抛泥
        /// </summary>
        public decimal? SedimentDisposal { get; set; }
        /// <summary>
        ///吹岸
        /// </summary>
        public decimal? BlowShore { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 对外接口 自有/分包船舶月报请求dto
    /// </summary>
    public class ShipMonthRequestDto : IValidatableObject
    {
        /// <summary>
        /// 船舶Id
        /// </summary>
        public string? ShipId { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 传入开始日期
        /// </summary>
        public DateTime? InStartDate { get; set; }
        /// <summary>
        /// 传入结束日期
        /// </summary>
        public DateTime? InEndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //开始日期结束日期都不是空
            if (!(InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(InStartDate.ToString())) && (InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(InEndDate.ToString())))
            {
                if (InStartDate > InEndDate)
                {
                    yield return new ValidationResult("开始日期大于结束日期", new string[] { nameof(InStartDate) });
                }
            }
        }
    }
    /// <summary>
    /// 对外接口 自有/分包船舶月报数据集
    /// </summary>
    public class ShipMonthReports
    {
        /// <summary>
        /// 月报id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public string? QuitTime { get; set; }
        /// <summary>
        /// 本月施工天数
        /// </summary>
        public decimal MonthWorkDays { get; set; }
        /// <summary>
        /// 本月运转时间
        /// </summary>
        public decimal MonthWorkHours { get; set; }
        /// <summary>
        /// 工况级别
        /// </summary>
        public Guid? GKJBId { get; set; }
        /// <summary>
        /// 工艺方式
        /// </summary>
        public Guid? GYFSId { get; set; }
        /// <summary>
        /// 疏浚吹填分类
        /// </summary>
        public Guid? SJCTId { get; set; }
        /// <summary>
        /// 合同清单类型(字典表typeno=9)
        /// </summary>
        public int QDLXId { get; set; }
        /// <summary>
        /// 合同清单名称
        /// </summary>
        public string? QDLXName { get; set; }
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal MonthQuantity { get; set; }
        /// <summary>
        /// 本月施工产值
        /// </summary>
        public decimal MonthOutputVal { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        ///挖深（m）
        /// </summary>
        public decimal DigDeep { get; set; }
        /// <summary>
        /// 吹距(KM)
        /// </summary>
        public decimal BlowingDistance { get; set; }
        /// <summary>
        /// 运距(KM)
        /// </summary>
        public decimal HaulDistance { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime DateMonth { get; set; }
    }
    /// <summary>
    /// 分包船舶月报数据集
    /// </summary>
    public class SubShipMonthReports
    {
        /// <summary>
        /// 月报id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 分包船舶id
        /// </summary>
        public Guid? SubShipId { get; set; }
        /// <summary>
        /// 船舶动态
        /// </summary>
        public int ShipDynamic { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public string? QuitTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 对外接口自有船舶信息
    /// </summary>
    public class ShipInfos
    {
        /// <summary>
        /// 监控中心主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关联的pomid
        /// </summary>
        public Guid? PomId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型id
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// mmsi
        /// </summary>
        public string? Mmsi { get; set; }
    }
    /// <summary>
    /// 项目日报对外接口请求dto
    /// </summary>
    public class DayReportRequestDto : IValidatableObject
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string[]? ProjectStatusId { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //开始日期结束日期都不是空
            if (!((StartTime == DateTime.MinValue || string.IsNullOrWhiteSpace(StartTime.ToString())) && (EndTime == DateTime.MinValue || string.IsNullOrWhiteSpace(EndTime.ToString()))))
            {
                if (StartTime > EndTime)
                {
                    yield return new ValidationResult("开始日期大于结束日期", new string[] { nameof(StartTime) });
                }
            }
        }
    }
}
