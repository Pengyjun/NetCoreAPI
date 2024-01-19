using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 自有船舶月报-主要施工土质
    /// </summary>
    [SugarTable("t_ownershipmonthreportsoil", IsDisabledDelete = true)]
    public class OwnerShipMonthReportSoil : BaseEntity<Guid>
    {
        /// <summary>
        /// 自有船舶月报Id
        /// </summary>
        public Guid OwnerShipMonthReportId { get; set; }

        /// <summary>
        /// 项目id(备注：动态-船舶日报，值为Guid.Empty )
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 疏浚土分类ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid SoilId { get; set; }

        /// <summary>
        /// 疏浚土分级Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid  SoilGradeId { get; set; }

        /// <summary>
        /// 所占比重
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(8,2)")]
        public decimal Proportion { get; set; }
    }
}
