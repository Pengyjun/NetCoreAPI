using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using UtilsSharp;
using GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 保存船舶动态日报请求
    /// </summary>
    public  class SaveShipDynamicDayReportRequestDto:IValidatableObject, IResetModelProperty
    {

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶动态清单(船舶状态)
        /// </summary>
        public DynamicShipState ShipState { get; set; }

        /// <summary>
        /// 本月实际目的港Id
        /// </summary>
        public Guid? MonthDestinationPortId { get; set; }

        /// <summary>
        /// 本月实际出发港Id
        /// </summary>
        public Guid? MonthDeparturePortId { get; set; }

        /// <summary>
        /// 本月实际航行距离
        /// </summary>
        public decimal? MonthNavigationalDistance { get; set; }

        /// <summary>
        /// 本月实际调遣小时
        /// </summary>
        public decimal? MonthDispatchsHours { get; set; }

        /// <summary>
        /// 本月实际拖轮/半潜船名称
        /// </summary>
        public string? MonthShipName { get; set; }

        /// <summary>
        /// 修理所在港口Id
        /// </summary>
        public Guid? RepairByPortId { get; set; }

        /// <summary>
        /// 待命所在港口Id
        /// </summary>
        public Guid? StandbyPortId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] {  nameof(ShipId) });
            }
            if (!EnumExtension.EnumToList<DynamicShipState>().Any((t=>t.EnumValue==(int)ShipState)))
            {
                yield return new ValidationResult("船舶动态清单不存在", new string[] { nameof(ShipState) });
            }
        }

        /// <summary>
        /// 重置Model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (ShipState!= DynamicShipState.Standby)
            {
                StandbyPortId = null;
            }
            if (ShipState != DynamicShipState.ShopRepair&& ShipState!= DynamicShipState.VoyageRepair)
            {
                RepairByPortId = null;
            }
            if (ShipState != DynamicShipState.Dispatch)
            {
                MonthDestinationPortId = null;
                MonthDeparturePortId = null;
                MonthNavigationalDistance = null;
                MonthDispatchsHours = null;
                MonthShipName = null;
            }
        }
    }
}
