using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 自有船舶月报Dto
    /// </summary>
    public abstract  class OwnerShipMonthReportDto<TReportSoil> where TReportSoil: OwnerShipMonthReportSoilDto
    {
        /// <summary>
        /// 项目id(备注：动态-船舶日报，值为Guid.Empty )
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? EnterTime { get; set; }

        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? QuitTime { get; set; }

        /// <summary>
        /// 施工天数
        /// </summary>
        public decimal? ConstructionDays { get; set; }

        /// <summary>
        /// 合同清单类型(字典表typeno=9)
        /// </summary>
        public int? ContractDetailType { get; set; }

        /// <summary>
        /// 工艺方式Id
        /// </summary>
        public Guid? WorkModeId { get; set; }

        /// <summary>
        /// 疏浚吹填分类Id
        /// </summary>
        public Guid? WorkTypeId { get; set; }

        /// <summary>
        /// 工况级别Id
        /// </summary>
        public Guid? ConditionGradeId { get; set; }

        /// <summary>
        ///挖深（m）
        /// </summary>
        public decimal? DigDeep { get; set; }

        /// <summary>
        /// 吹距(KM)
        /// </summary>
        public decimal? BlowingDistance { get; set; }

        /// <summary>
        /// 运距(KM)
        /// </summary>
        public decimal? HaulDistance { get; set; }

        /// <summary>
        /// 主要施工土质集合
        /// </summary>
        public TReportSoil[] Soils { get; set; }= new TReportSoil[0];
    }

    /// <summary>
    /// 主要施工土质Dto
    /// </summary>
    public abstract  class OwnerShipMonthReportSoilDto
    {
        /// <summary>
        /// 月报施工土质Id
        /// </summary>
        public Guid? MonthReportSoilId { get; set; }

        /// <summary>
        /// 疏浚土分类Id
        /// </summary>
        public Guid SoilId { get; set; }

        /// <summary>
        /// 疏浚土分级Id
        /// </summary>
        public Guid SoilGradeId { get; set; }

        /// <summary>
        /// 所占比重
        /// </summary>
        public decimal Proportion { get; set; }
    }
}
