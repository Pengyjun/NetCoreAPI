using MiniExcelLibs.Attributes;
using SqlSugar;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 基准计划
    /// </summary>
    public class BaseLinePlanProjectAnnualProductionDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<SearchBaseLinePlanProjectAnnualProductionDto> ExcelImport { get; set; } = new List<SearchBaseLinePlanProjectAnnualProductionDto>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchBaseLinePlanProjectAnnualProduction
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 计划完工时间（合同约定完工日期）
        /// </summary>
        public string? ContractStipulationEndDate { get; set; }

        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal EffectiveAmount { get; set; }

        /// <summary>
        /// 剩余合同额
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// 项目开累产值
        /// </summary>
        public decimal AccumulatedOutputValue { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanVersion { get; set; }

        /// <summary>
        /// 基准或新建计划
        /// </summary>
        public string PlanType { get; set; }

        /// <summary>
        /// 年初状态
        /// </summary>
        public string StartStatus { get; set; }

        /// <summary>
        /// 年初状态名称
        /// </summary>
        public string StartStatusName { get; set; }

        /// <summary>
        /// 是否分包
        /// </summary>
        public int? IsSubPackage { get; set; }


        /// <summary>
        /// 最新完工时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }


        /// <summary>
        /// 审核状态 审核状态，0  待审核 （1：驳回，2：通过  3:撤回）
        /// </summary>
        public int PlanStatus { get; set; }

        /// <summary>
        /// 审核状态文本
        /// </summary>
        public string PlanStatusText { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        public string? Association { get; set; }

        /// <summary>
        /// 关联项目名称
        /// </summary>
        public string? AssociationName { get; set; }

        /// <summary>
        /// 细项
        /// </summary>
        public List<SearchBaseLinePlanProjectAnnualProductionDto> SearchProjectAnnualProductionDto { get; set; } = new List<SearchBaseLinePlanProjectAnnualProductionDto>();
    }
    /// <summary>
    /// 细项
    /// </summary>
    public class SearchBaseLinePlanProjectAnnualProductionDto
    {

        public BaseLinePlanprojectRequestDto baseLinePlanproject { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 1月产量
        /// </summary>
        public decimal JanuaryProductionQuantity { get; set; }

        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }

        /// <summary>
        /// 2月产量
        /// </summary>
        public decimal FebruaryProductionQuantity { get; set; }

        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产量
        /// </summary>
        public decimal MarchProductionQuantity { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }

        /// <summary>
        /// 4月产量
        /// </summary>
        public decimal AprilProductionQuantity { get; set; }

        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }

        /// <summary>
        /// 5月产量
        /// </summary>
        public decimal MayProductionQuantity { get; set; }

        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }

        /// <summary>
        /// 6月产量
        /// </summary>
        public decimal JuneProductionQuantity { get; set; }

        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产量
        /// </summary>
        public decimal JulyProductionQuantity { get; set; }

        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产量
        /// </summary>
        public decimal AugustProductionQuantity { get; set; }

        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产量
        /// </summary>
        public decimal SeptemberProductionQuantity { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产量
        /// </summary>
        public decimal OctoberProductionQuantity { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产量
        /// </summary>
        public decimal NovemberProductionQuantity { get; set; }

        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产量
        /// </summary>
        public decimal DecemberProductionQuantity { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }

        /// <summary>
        /// 船舶
        /// </summary>
        public List<BaseLinePlanAnnualPlanProductionShips> AnnualProductionShips { get; set; } = new List<BaseLinePlanAnnualPlanProductionShips>();

    }


    public class BaseLinePlanprojectRequestDto
    {
        ///// <summary>
        ///// 计划版本
        ///// </summary>
        //public string PlanVersion { get; set; }

        /// <summary>
        /// 基准或新建计划
        /// </summary>
        //public string PlanType { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 年初状态
        /// </summary>
        public string StartStatus { get; set; }

        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 是否分包
        /// </summary>
        public int? IsSubPackage { get; set; }

        /// <summary>
        /// 总有效合同额
        /// </summary>
        public decimal EffectiveAmount { get; set; }

        /// <summary>
        /// 剩余合同额
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// 最新完工时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }


        /// <summary>
        /// 计划状态 审核状态，0  待审核 （1：驳回，2：通过  3:撤回）
        /// </summary>
        public int? PlanStatus { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        public string? Association { get; set; }
    }


    public class SearchBaseLinePlanprojectComparisonRequestDtoRequest
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
    }

    public class BaseLinePlanprojectComparisonRequestDto
    {

        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }


        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }


        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }


        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }


        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }


        //public Guid? BasePlanProjectId { get; set; }

        ///// <summary>
        ///// 计划版本
        ///// </summary>
        //public string PlanVersion { get; set; }

        /// <summary>
        /// 基准或新建计划
        /// </summary>
        public string PlanType { get; set; }

        /// <summary>
        /// 年初状态
        /// </summary>
        public string StartStatus { get; set; }

        public Guid? ProjectId { get; set; }


        //public Guid? CompanyId { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class BaseLinePlanAnnualPlanProductionShips
    {
        /// <summary>
        /// 资源id  船舶id
        /// </summary>
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 1自有 2 分包
        /// </summary>
        public int ShipType { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseLinePlanAnnualProduction
    {
        /// <summary>
        /// 项目
        /// </summary>
        public List<BaseLinePlanProjectInfosForAnnualProduction> ProjectInfosForAnnualProductions { get; set; } = new List<BaseLinePlanProjectInfosForAnnualProduction>();
        /// <summary>
        /// 船
        /// </summary>
        public List<BaseLinePlanShipsForAnnualProduction> OwnShipsForAnnualProductions { get; set; } = new List<BaseLinePlanShipsForAnnualProduction>();
    }
    /// <summary>
    /// 基准计划项目信息
    /// </summary>
    public class BaseLinePlanProjectInfosForAnnualProduction
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 项目所属公司
        /// </summary>
        public Guid? CompanyId { get; set; }

    }
    /// <summary>
    /// 基准计划船舶信息
    /// </summary>
    public class BaseLinePlanShipsForAnnualProduction
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 1自有 2分包
        /// </summary>
        public int SubOrOwn { get; set; }
    }

    /// <summary>
    /// 分子公司
    /// </summary>
    public class SearchSubsidiaryCompaniesProjectProductionDto
    {


        //public Guid BasePlanProjectId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        public Guid? JobId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 基建计划名称
        /// </summary>
        //public string? BasePlanName { get; set; }

        ///// <summary>
        ///// 新建计划名称
        ///// </summary>
        //public string? NewPlanName { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 审核状态，0  待审核 （1：驳回，2：通过  3:撤回）
        /// </summary>
        public int PlanStatus { get; set; }

        /// <summary>
        /// 驳回原因
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        ///  审核状态，0  待审核 （1：驳回，2：通过  3:撤回）
        /// </summary>
        public string PlanStatusStr { get; set; }

        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }


        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }


        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }


        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }


        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }
    }

    /// <summary>
    /// 分子公司
    /// </summary>
    public class SearchCompaniesProjectProductionDto
    {

        //public Guid BasePlanProjectId { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 产值汇总
        /// </summary>
        //public decimal ProductionValueSum { get; set; }

        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }


        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }


        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }


        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }


        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }

        /// <summary>
        /// 年度合计
        /// </summary>
        //public decimal AnnualTotal { get; set; }
    }


    /// <summary>
    /// 计划基准
    /// </summary>
    public class BaseLinePlanAncomparisonResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string ProjectName { get; set; }

        public string Company { get; set; }


        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }


        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }

        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }


        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }


        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }


        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }


        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }


        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }
    }

    public class BaseLinePlanAncomparisonRequsetDto : BaseRequestDto
    {
        public string? Name { get; set; }

        public string? StartStatus { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 是否分包
        /// </summary>
        public int? IsSubPackage { get; set; }
    }

    /// <summary>
    /// 项目基准计划导入
    /// </summary>
    public class BaseLinePlanProjectAnnualProductionImport
    {

        [ExcelColumnName("项目名称")]
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        //public string? ShipName { get; set; }


        [ExcelColumnName("PG码")]
        /// <summary>
        /// PG码
        /// </summary>
        public string? Association { get; set; }


        [ExcelColumnName("年度")]
        /// <summary>
        /// 年份
        /// </summary>
        public string? Year { get; set; }

        /// <summary>
        /// 1月产量
        /// </summary>
        //public decimal JanuaryProductionQuantity { get; set; }

        [ExcelColumnName("一月份产值")]
        /// <summary>
        /// 1月产值
        /// </summary>
        public decimal JanuaryProductionValue { get; set; }

        /// <summary>
        /// 2月产量
        /// </summary>
        //public decimal FebruaryProductionQuantity { get; set; }

        [ExcelColumnName("二月份产值")]
        /// <summary>
        /// 2月产值
        /// </summary>
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产量
        /// </summary>
        //public decimal MarchProductionQuantity { get; set; }


        [ExcelColumnName("三月份产值")]
        /// <summary>
        /// 3月产值
        /// </summary>
        public decimal MarchProductionValue { get; set; }

        /// <summary>
        /// 4月产量
        /// </summary>
        //public decimal AprilProductionQuantity { get; set; }

        [ExcelColumnName("四月份产值")]
        /// <summary>
        /// 4月产值
        /// </summary>
        public decimal AprilProductionValue { get; set; }

        /// <summary>
        /// 5月产量
        /// </summary>
        //public decimal MayProductionQuantity { get; set; }

        [ExcelColumnName("五月份产值")]
        /// <summary>
        /// 5月产值
        /// </summary>
        public decimal MayProductionValue { get; set; }

        /// <summary>
        /// 6月产量
        /// </summary>
        //public decimal JuneProductionQuantity { get; set; }

        [ExcelColumnName("六月份产值")]
        /// <summary>
        /// 6月产值
        /// </summary>
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产量
        /// </summary>
        //public decimal JulyProductionQuantity { get; set; }


        [ExcelColumnName("七月份产值")]

        /// <summary>
        /// 7月产值
        /// </summary>
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产量
        /// </summary>
        //public decimal AugustProductionQuantity { get; set; }


        [ExcelColumnName("八月份产值")]
        /// <summary>
        /// 8月产值
        /// </summary>
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产量
        /// </summary>
        //public decimal SeptemberProductionQuantity { get; set; }


        [ExcelColumnName("九月份产值")]
        /// <summary>
        /// 9月产值
        /// </summary>
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产量
        /// </summary>
        //public decimal OctoberProductionQuantity { get; set; }

        [ExcelColumnName("十月份产值")]
        /// <summary>
        /// 10月产值
        /// </summary>
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产量
        /// </summary>
        //public decimal NovemberProductionQuantity { get; set; }

        [ExcelColumnName("十一月份产值")]
        /// <summary>
        /// 11月产值
        /// </summary>
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产量
        /// </summary>
        //public decimal DecemberProductionQuantity { get; set; }

        [ExcelColumnName("十二月份产值")]
        /// <summary>
        /// 12月产值
        /// </summary>
        public decimal DecemberProductionValue { get; set; }

    }


    public class BaseLinePlanSelectOptiong
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 计划版本
        /// </summary>
        public string PlanVersion { get; set; }
    }

    public class BaseLinePlanprojectImportDto
    {

        /// <summary>
        /// 是否关联项目 1是 0否
        /// </summary>
        public int Association { get; set; }
    }
}
