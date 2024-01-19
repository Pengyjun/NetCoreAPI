using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Const;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 新增/修改船舶日报
    /// </summary>
    public class SaveShipDayReportRequestDto : ShipDayReportDto, IValidatableObject, IResetModelProperty
    {


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }
            if (ShipDayReportType != ShipDayReportType.ProjectShip && ShipDayReportType != ShipDayReportType.DynamicShip)
            {
                yield return new ValidationResult("船舶日报类型不存在", new string[] { nameof(ShipDayReportType) });
            }

            if (ShipState==null||!EnumExtension.EnumToList<ProjectShipState>().Any(t=>t.EnumValue==(int)ShipState))
            {
                yield return new ValidationResult("船舶动态清单不存在", new string[] { nameof(ShipState) });
            }
            if (ShipDayReportType == ShipDayReportType.ProjectShip)
            {
                if (ProjectId == null || ProjectId == Guid.Empty)
                {
                    yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
                }
                if (string.IsNullOrWhiteSpace(ConstructionArea))
                {
                    yield return new ValidationResult("施工区域不能为空", new string[] { nameof(ConstructionArea) });
                }
                if (string.IsNullOrWhiteSpace(SoilQuality))
                {
                    yield return new ValidationResult("土质不能为空", new string[] { nameof(SoilQuality) });
                }
            }
            if (ForwardNumber < 0)
            {
                yield return new ValidationResult("前进距/舱/驳数不能小于0", new string[] { nameof(ForwardNumber) });
            }
            if (OilConsumption < 0)
            {
                yield return new ValidationResult("油耗不能小于0", new string[] { nameof(OilConsumption) });
            }
            if (EstimatedUnitPrice < 0)
            {
                yield return new ValidationResult("估算单价小于0", new string[] { nameof(EstimatedUnitPrice) });
            }
            if (AverageExcavationDepth != null && AverageExcavationDepth < 0)
            {
                yield return new ValidationResult("平均挖深不能小于0", new string[] { nameof(AverageExcavationDepth) });
            }
            if (AverageExcavationWidth != null && AverageExcavationWidth < 0)
            {
                yield return new ValidationResult("平均挖宽不能小于0", new string[] { nameof(AverageExcavationWidth) });
            }
            if (ShipReportedProduction < 0)
            {
                yield return new ValidationResult("船报产量不能小于0", new string[] { nameof(ShipReportedProduction) });
            }
            if (PipelineLength != null && PipelineLength < 0)
            {
                yield return new ValidationResult("管线长度不能小于0", new string[] { nameof(PipelineLength) });
            }
            if (FloatingTubeLength != null && FloatingTubeLength < 0)
            {
                yield return new ValidationResult("浮管长度不能小于0", new string[] { nameof(PipelineLength) });
            }
            if (ShorePipeLength != null && ShorePipeLength < 0)
            {
                yield return new ValidationResult("岸管长度不能小于0", new string[] { nameof(ShorePipeLength) });
            }
            if (ImmersedTubeLength != null && ImmersedTubeLength < 0)
            {
                yield return new ValidationResult("沉管长度不能小于0", new string[] { nameof(ImmersedTubeLength) });
            }
            if (FuelUnitPrice != null && FuelUnitPrice < 0)
            {
                yield return new ValidationResult("燃油单价不能小于0", new string[] { nameof(FuelUnitPrice) });
            }
            if (EstimatedCostAmount != null && EstimatedCostAmount < 0)
            {
                yield return new ValidationResult("估算成本不能小于0", new string[] { nameof(EstimatedCostAmount) });
            }
            if (Dredge != null && Dredge < 0)
            {
                yield return new ValidationResult("挖泥时长不能小于0", new string[] { nameof(EstimatedCostAmount) });
            }
            if (Sail != null && Sail < 0)
            {
                yield return new ValidationResult("航行时长不能小于0", new string[] { nameof(Sail) });
            }
            if (BlowingWater != null && BlowingWater < 0)
            {
                yield return new ValidationResult("吹水时长不能小于0", new string[] { nameof(BlowingWater) });
            }
            if (SedimentDisposal != null && SedimentDisposal < 0)
            {
                yield return new ValidationResult("抛泥时长不能小于0", new string[] { nameof(SedimentDisposal) });
            }
            if (BlowShore != null && BlowShore < 0)
            {
                yield return new ValidationResult("吹岸时长不能小于0", new string[] { nameof(BlowingWater) });
            }
            if (ConstructionLayout != null && ConstructionLayout < 0)
            {
                yield return new ValidationResult("开工展布时长不能小于0", new string[] { nameof(ConstructionLayout) });
            }
            if (StandbyMachine != null && StandbyMachine < 0)
            {
                yield return new ValidationResult("备机时长不能小于0", new string[] { nameof(StandbyMachine) });
            }
            if (DownAnchor != null && DownAnchor < 0)
            {
                yield return new ValidationResult("下(移)锚览时长不能小于0", new string[] { nameof(DownAnchor) });
            }
            if (MovingShip != null && MovingShip < 0)
            {
                yield return new ValidationResult("移船时长不能小于0", new string[] { nameof(MovingShip) });
            }
            if (WeldingCutter != null && WeldingCutter < 0)
            {
                yield return new ValidationResult("焊绞刀/齿座时长不能小于0", new string[] { nameof(WeldingCutter) });
            }
            if (ChangeGear != null && ChangeGear < 0)
            {
                yield return new ValidationResult("换齿时长不能小于0", new string[] { nameof(ChangeGear) });
            }
            if (Supply != null && Supply < 0)
            {
                yield return new ValidationResult("补给时长不能小于0", new string[] { nameof(Supply) });
            }
            if (MeasurementImpact != null && MeasurementImpact < 0)
            {
                yield return new ValidationResult("测量影响时长不能小于0", new string[] { nameof(MeasurementImpact) });
            }
            if (LayingPipelines != null && LayingPipelines < 0)
            {
                yield return new ValidationResult("敷设管线时长不能小于0", new string[] { nameof(LayingPipelines) });
            }
            if (CleaningPump != null && CleaningPump < 0)
            {
                yield return new ValidationResult("清泥泵时长不能小于0", new string[] { nameof(CleaningPump) });
            }
            if (CleaningCRS != null && CleaningCRS < 0)
            {
                yield return new ValidationResult("清绞刀/耙头/吸口时长不能小于0", new string[] { nameof(CleaningCRS) });
            }
            if (GrabRefueling != null && GrabRefueling < 0)
            {
                yield return new ValidationResult("抓斗加油时长不能小于0", new string[] { nameof(GrabRefueling) });
            }
            if (WaitingReplaceRefueling != null && WaitingReplaceRefueling < 0)
            {
                yield return new ValidationResult("等换驳时长不能小于0", new string[] { nameof(WaitingReplaceRefueling) });
            }
            if (ReplaceWire != null && ReplaceWire < 0)
            {
                yield return new ValidationResult("换钢丝时长不能小于0", new string[] { nameof(ReplaceWire) });
            }
            if (AvoidingShip != null && AvoidingShip < 0)
            {
                yield return new ValidationResult("避让船舶时长不能小于0", new string[] { nameof(AvoidingShip) });
            }
            if (WeatherImpact != null && WeatherImpact < 0)
            {
                yield return new ValidationResult("天气影响时长不能小于0", new string[] { nameof(WeatherImpact) });
            }
            if (TideImpact != null && TideImpact < 0)
            {
                yield return new ValidationResult("潮流影响时长不能小于0", new string[] { nameof(TideImpact) });
            }
            if (SuddenFailure != null && SuddenFailure < 0)
            {
                yield return new ValidationResult("突发故障时长不能小于0", new string[] { nameof(SuddenFailure) });
            }
            if (WaitingSparePartRepair != null && WaitingSparePartRepair < 0)
            {
                yield return new ValidationResult("等备件待修时长不能小于0", new string[] { nameof(WaitingSparePartRepair) });
            }
            if (WaitingOilWaterMaterial != null && WaitingOilWaterMaterial < 0)
            {
                yield return new ValidationResult("等油水料时长不能小于0", new string[] { nameof(WaitingOilWaterMaterial) });
            }
            if (WaitingBargeAndTowing != null && WaitingBargeAndTowing < 0)
            {
                yield return new ValidationResult("等驳等拖时长不能小于0", new string[] { nameof(WaitingBargeAndTowing) });
            }
            if (NotifyShutdown != null && NotifyShutdown < 0)
            {
                yield return new ValidationResult("通知停工时长不能小于0", new string[] { nameof(WaitingBargeAndTowing) });
            }
            if (EquipmentModificationMaintenance != null && EquipmentModificationMaintenance < 0)
            {
                yield return new ValidationResult("设备改装及维护时长不能小于0", new string[] { nameof(EquipmentModificationMaintenance) });
            }
            if (Standby != null && Standby < 0)
            {
                yield return new ValidationResult("待命时长不能小于0", new string[] { nameof(Standby) });
            }
            if (FSPipeFailure != null && FSPipeFailure < 0)
            {
                yield return new ValidationResult("浮管/岸管故障时长不能小于0", new string[] { nameof(FSPipeFailure) });
            }
            if (SunkenTubeFailure != null && SunkenTubeFailure < 0)
            {
                yield return new ValidationResult("沉管故障时长不能小于0", new string[] { nameof(SunkenTubeFailure) });
            }
            if (SocialInterference != null && SocialInterference < 0)
            {
                yield return new ValidationResult("社会干扰时长不能小于0", new string[] { nameof(SocialInterference) });
            }
            if (CofferdamDrainageIssues != null && CofferdamDrainageIssues < 0)
            {
                yield return new ValidationResult("围堰/排水问题时长不能小于0", new string[] { nameof(CofferdamDrainageIssues) });
            }
            if (Other != null && Other < 0)
            {
                yield return new ValidationResult("其它时长不能小于0", new string[] { nameof(Other) });
            }
            if (ExecuteDispatch != null && ExecuteDispatch < 0)
            {
                yield return new ValidationResult("执行调遣时长不能小于0", new string[] { nameof(ExecuteDispatch) });
            }
            if (PrepareDispatch != null && PrepareDispatch < 0)
            {
                yield return new ValidationResult("准备调遣时长不能小于0", new string[] { nameof(PrepareDispatch) });
            }
            if (ShipConversion != null && ShipConversion < 0)
            {
                yield return new ValidationResult("船舶改装时长不能小于0", new string[] { nameof(ShipConversion) });
            }
            if (SealedForGoSea != null && SealedForGoSea < 0)
            {
                yield return new ValidationResult("出海封仓时长不能小于0", new string[] { nameof(SealedForGoSea) });
            }
            if (ApplyForInspection != null && ApplyForInspection < 0)
            {
                yield return new ValidationResult("申请检验时长不能小于0", new string[] { nameof(ApplyForInspection) });
            }
            if (ShipyardRepair != null && ShipyardRepair < 0)
            {
                yield return new ValidationResult("船厂/基地修理时长不能小于0", new string[] { nameof(ShipyardRepair) });
            }
            if (MonthlyPreventiveMaintenance != null && MonthlyPreventiveMaintenance < 0)
            {
                yield return new ValidationResult("月度预防检修时长不能小于0", new string[] { nameof(MonthlyPreventiveMaintenance) });
            }
            if (OffSiteStandby != null && OffSiteStandby < 0)
            {
                yield return new ValidationResult("非工地待命时长不能小于0", new string[] { nameof(OffSiteStandby) });
            }
            if (CustomsClearanceAndDeclaration != null && CustomsClearanceAndDeclaration < 0)
            {
                yield return new ValidationResult("清关报关时长不能小于0", new string[] { nameof(CustomsClearanceAndDeclaration) });
            }
            if(BlowTorque!=null&& BlowTorque<0)
            {
                yield return new ValidationResult("吹矩(m)不能小于0", new string[] { nameof(BlowTorque) });
            }
            if (ShipboardFuel != null && ShipboardFuel < 0)
            {
                yield return new ValidationResult("船存燃油（t）不能小于0", new string[] { nameof(ShipboardFuel) });
            }
            if(FuelSupply!=null&& FuelSupply<0)
            {
                yield return new ValidationResult("燃油补给（t）不能小于0", new string[] { nameof(FuelSupply) });
            }
            //if (IsHaveProductionWarning)
            //{
            //    if (string.IsNullOrWhiteSpace(ProductionWarningContent))
            //    {
            //        yield return new ValidationResult("生产异常预警信息", new string[] { nameof(ProductionWarningContent) });
            //    }
            //    else if (ProductionWarningContent.Length > 1000)
            //    {
            //        yield return new ValidationResult("生产异常预警信息字数长度不能超过1000", new string[] { nameof(ProductionWarningContent) });
            //    }
            //}
        }

        /// <summary>
        /// 重置Model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (ShipDayReportType != ShipDayReportType.ProjectShip)
            {
                ProjectId = Guid.Empty;
                ConstructionArea = null;
            }
            if (ShipDayReportType != ShipDayReportType.DynamicShip)
            {
                PortId = null;
            }
            if (IsHaveProductionWarning == 0)
            {
                ProductionWarningContent = null;
            }
            EstimatedOutputAmount = EstimatedUnitPrice * ShipReportedProduction;
        }
    }
}

