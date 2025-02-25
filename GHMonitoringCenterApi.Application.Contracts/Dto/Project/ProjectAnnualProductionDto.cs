namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 项目年初计划
    /// </summary>
    public class ProjectAnnualProductionDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<SearchProjectAnnualProductionDto> ExcelImport { get; set; } = new List<SearchProjectAnnualProductionDto>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchProjectAnnualProduction
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
        /// 细项
        /// </summary>
        public List<SearchProjectAnnualProductionDto> SearchProjectAnnualProductionDto { get; set; } = new List<SearchProjectAnnualProductionDto>();
    }
    /// <summary>
    /// 细项
    /// </summary>
    public class SearchProjectAnnualProductionDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

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
        /// 资源Id(船舶)
        /// </summary>
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 船舶 1自有 2分包
        /// </summary>
        public int ShipType { get; set; }

        /// <summary>
        /// 资源(船舶)
        /// </summary>
        public string? ShipName { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>
        //public int Sequence { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseAnnualProduction
    {
        /// <summary>
        /// 项目
        /// </summary>
        public List<ProjectInfosForAnnualProduction> ProjectInfosForAnnualProductions { get; set; } = new List<ProjectInfosForAnnualProduction>();
        /// <summary>
        /// 船
        /// </summary>
        public List<OwnShipsForAnnualProduction> OwnShipsForAnnualProductions { get; set; } = new List<OwnShipsForAnnualProduction>();
    }
    /// <summary>
    /// 年度计划项目信息
    /// </summary>
    public class ProjectInfosForAnnualProduction
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
    /// 年度计划船舶信息
    /// </summary>
    public class OwnShipsForAnnualProduction
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
}
