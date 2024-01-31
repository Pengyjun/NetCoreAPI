using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Contracts.Dto.Push;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey;
using GHMonitoringCenterApi.Application.Contracts.Dto.Timing;
using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.ProjectMonthReportResponseDto;

namespace GHMonitoringCenterApi.Application.Contracts.AutoMapper
{
    public class AutoMapperProfileFile : Profile
    {
        public static void AutoMapperProfileInit(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.CreateMap<PomCurrencyResponseDto, Currency>()
                  .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));

            mapperConfigurationExpression.CreateMap<PomDealingUnitResponseDto, DealingUnit>()
                  .ForMember(x => x.PomId, y => y.MapFrom(u => u.PomId))
                  .ForMember(x => x.CreateId, y => y.MapFrom(u => u.CreatedBy))
                  .ForMember(x => x.UpdateId, y => y.MapFrom(u => u.UpdatedBy));


            mapperConfigurationExpression.CreateMap<PomProjectResponseDto, Project>()
                .ForMember(x => x.CompanyId, y => y.MapFrom(u => u.Company))
                 .ForMember(x => x.GradeId, y => y.MapFrom(u => u.Scale))
                 .ForMember(x => x.TypeId, y => y.MapFrom(u => u.Type))
                 .ForMember(x => x.CurrencyId, y => y.MapFrom(u => u.Currency));

            mapperConfigurationExpression.CreateMap<Project, ProjectDetailResponseDto>();
            mapperConfigurationExpression.CreateMap<ProjectDetailResponseDto, Project>();
            mapperConfigurationExpression.CreateMap<AddOrUpdateProjectRequestDto, Project>()
                .ForMember(x => x.Constructor, y => y.MapFrom(u => u.ConstructorNum));
            mapperConfigurationExpression.CreateMap<ProjectOrgDtos, ProjectOrg>();
            mapperConfigurationExpression.CreateMap<ProjectOrg, ProjectOrgDtos>();
            mapperConfigurationExpression.CreateMap<ProjectDutyDtos, ProjectLeader>();
            mapperConfigurationExpression.CreateMap<ProjectLeader, ProjectDutyDtos>();
            mapperConfigurationExpression.CreateMap<UploadResponseDto, Files>();
            mapperConfigurationExpression.CreateMap<ProjectOrg, ProjectOrgDto>();
            mapperConfigurationExpression.CreateMap<ProjectLeader, ProjectDutyDto>();
            mapperConfigurationExpression.CreateMap<PomProjectStatusResponseDto, ProjectStatus>()
                .ForMember(x => x.StatusId, y => y.MapFrom(u => u.Id));

            mapperConfigurationExpression.CreateMap<PomProjectAreaResponseDto, ProjectArea>()
               .ForMember(x => x.AreaId, y => y.MapFrom(u => u.Id));


            mapperConfigurationExpression.CreateMap<PomProjectLeaderResponseDto, ProjectLeader>()
              .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomInstitutionResponseDto, Institution>()
              .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));

            mapperConfigurationExpression.CreateMap<PomUserResponseDto, User>()
              .ForMember(x => x.LoginAccount, y => y.MapFrom(u => u.GroupCode))
                   .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id))
              .ForMember(x => x.GroupCode, y => y.MapFrom(u => u.GroupCode));

            mapperConfigurationExpression.CreateMap<PomProvinceResponseDto, Province>()
                .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));

            mapperConfigurationExpression.CreateMap<PomProjectTypeResponseDto, ProjectType>()
                  .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));


            mapperConfigurationExpression.CreateMap<AddOrUpdateMenuRequestDto, Menu>();

            mapperConfigurationExpression.CreateMap<PomProjectScaleResponseDto, ProjectScale>()
             .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomConstructionQualificationResponseDto, ConstructionQualification>()
                .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomIndustryClassificationResponseDto, IndustryClassification>()
                .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomWaterCarriageResponseDto, WaterCarriage>()
                .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomProjectOrgResponseDto, ProjectOrg>()
                .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomOwnerShipResponseDto, OwnerShip>()
               .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomSubShipResponseDto, SubShip>()
               .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomProjectDepartmentResponseDto, ProjectDepartment>()
               .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<ProjectResponseDto, ProjectExcelImportResponseDto>();
            mapperConfigurationExpression.CreateMap<ProjectExcelSearchResponseDto, ProjectExcelImportResponseDto>();
            mapperConfigurationExpression.CreateMap<AddOrUpdateProjectWBSRequsetDto, ProjectWBS>();

            mapperConfigurationExpression.CreateMap<AddOrUpdateDayReportRequestDto.ReqDayReport, DayReport>();
            mapperConfigurationExpression.CreateMap<AddOrUpdateDayReportRequestDto.ReqConstruction, DayReportConstruction>();
            mapperConfigurationExpression.CreateMap<AddOrUpdateDayReportRequestDto.ReqDayReportConstructionInfo, DayReport>();
            mapperConfigurationExpression.CreateMap<DayReport, ProjectDayReportResponseDto.ResDayReport>();
            mapperConfigurationExpression.CreateMap<DayReport, ProjectDayReportResponseDto.ResDayReportConstructionInfo>();
            mapperConfigurationExpression.CreateMap<DayReportConstruction, ProjectDayReportResponseDto.ResConstruction>()
                .ForMember(x => x.DayReportConstructionId, y => y.MapFrom(u => u.Id));

            mapperConfigurationExpression.CreateMap<ProjectYearPlanDto, ProjectAnnualPlan>();//年度计划
            mapperConfigurationExpression.CreateMap<JjtWriteDataRecordResponseDto, JjtWriteDataRecord>();//jjt发送消息数值 写入中间表  每日写一次

            #region 项目主数据
            mapperConfigurationExpression.CreateMap<PomProjectMasterDataResponseDto, ProjectMasterData>()
            .ForMember(x => x.ProjectMasterCode, y => y.MapFrom(u => u.ZPROJECT))
            .ForMember(x => x.ProjectName, y => y.MapFrom(u => u.ZPROJNAME))
            .ForMember(x => x.ShortName, y => y.MapFrom(u => u.ZHEREINAFTER))
            .ForMember(x => x.Foreign, y => y.MapFrom(u => u.ZPROJENAME))
            .ForMember(x => x.SuperiorMasterCode, y => y.MapFrom(u => u.ZPROJECTUP))
            .ForMember(x => x.BeforeName, y => y.MapFrom(u => u.ZOLDNAME))
            .ForMember(x => x.ProjectType, y => y.MapFrom(u => u.ZPROJTYPE))
            .ForMember(x => x.BusinessClassification, y => y.MapFrom(u => u.ZCPBC))
            .ForMember(x => x.ProjectCountry, y => y.MapFrom(u => u.ZZCOUNTRY))
            .ForMember(x => x.ProjectCity, y => y.MapFrom(u => u.ZPROJLOC))
            .ForMember(x => x.AffiliationDept, y => y.MapFrom(u => u.ZBIZDEPT))
            .ForMember(x => x.RevenueSource, y => y.MapFrom(u => u.ZSI))
            .ForMember(x => x.InvestBody, y => y.MapFrom(u => u.ZINVERSTOR))
            .ForMember(x => x.ApprovalNo, y => y.MapFrom(u => u.ZAPPROVAL))
            .ForMember(x => x.ResolutionDate, y => y.MapFrom(u => u.ZAPVLDATE))
            .ForMember(x => x.ProjectOrgCode, y => y.MapFrom(u => u.ZPRO_ORG))
            .ForMember(x => x.TaxMethod, y => y.MapFrom(u => u.ZTAXMETHOD))
            .ForMember(x => x.ProjectYear, y => y.MapFrom(u => u.ZPROJYEAR))
            .ForMember(x => x.PlanStartDate, y => y.MapFrom(u => u.ZSTARTDATE))
            .ForMember(x => x.PlanEndDate, y => y.MapFrom(u => u.ZFINDATE))
            .ForMember(x => x.ProjectOrgForm, y => y.MapFrom(u => u.ZPOS))
            .ForMember(x => x.WinningBidder, y => y.MapFrom(u => u.ZAWARDMAI))
            .ForMember(x => x.ProjectConstructionName, y => y.MapFrom(u => u.ZENG))
            .ForMember(x => x.Responsible, y => y.MapFrom(u => u.ZRESP))
            .ForMember(x => x.LandNo, y => y.MapFrom(u => u.ZLDLOC))
            .ForMember(x => x.ProjectAcquireDate, y => y.MapFrom(u => u.ZLDLOCGT))
            .ForMember(x => x.CommerceChangeDate, y => y.MapFrom(u => u.ZCBR))
            .ForMember(x => x.TradingSituation, y => y.MapFrom(u => u.ZTRADER))
            .ForMember(x => x.MergeTableSituation, y => y.MapFrom(u => u.ZCS))
            .ForMember(x => x.InsureOrgName, y => y.MapFrom(u => u.ZINSURANCE))
            .ForMember(x => x.PolicyNo, y => y.MapFrom(u => u.ZPOLICYNO))
            .ForMember(x => x.InsurePeople, y => y.MapFrom(u => u.ZINSURED))
            .ForMember(x => x.InsuranceStartDate, y => y.MapFrom(u => u.ZISTARTDATE))
            .ForMember(x => x.InsuranceEndDate, y => y.MapFrom(u => u.ZIFINDATE))
            .ForMember(x => x.FundMasterCode, y => y.MapFrom(u => u.ZFUND))
            .ForMember(x => x.FundName, y => y.MapFrom(u => u.ZFUNDNAME))
            .ForMember(x => x.FundNo, y => y.MapFrom(u => u.ZFUNDNO))
            .ForMember(x => x.Currency, y => y.MapFrom(u => u.ZZCURRENCY))
            .ForMember(x => x.FundOrgForm, y => y.MapFrom(u => u.ZFUNDORGFORM))
            .ForMember(x => x.FundManageType, y => y.MapFrom(u => u.ZFUNDMTYPE))
            .ForMember(x => x.FundStateDate, y => y.MapFrom(u => u.ZFSTARTDATE))
            .ForMember(x => x.FundEndDate, y => y.MapFrom(u => u.ZFFINDATE))
            .ForMember(x => x.CustodianName, y => y.MapFrom(u => u.ZCUSTODIAN))
            .ForMember(x => x.TenantName, y => y.MapFrom(u => u.ZLESSEE))
            .ForMember(x => x.TenantTyoe, y => y.MapFrom(u => u.ZLESSEETYPE))
            .ForMember(x => x.Leasesname, y => y.MapFrom(u => u.ZLEASESNAME))
            .ForMember(x => x.LeaseStartDate, y => y.MapFrom(u => u.ZLEASEINCEPTION))
            .ForMember(x => x.LeaseEndDate, y => y.MapFrom(u => u.ZLEASEDUE))
            .ForMember(x => x.SecondaryUnit, y => y.MapFrom(u => u.Z2NDORG))
            .ForMember(x => x.Organization, y => y.MapFrom(u => u.ZPRO_BP))
            .ForMember(x => x.State, y => y.MapFrom(u => u.ZSTATE))
            .ForMember(x => x.StopReason, y => y.MapFrom(u => u.ZSTOPREASON))
            .ForMember(x => x.ProjectManageStyle, y => y.MapFrom(u => u.ZMANAGE_MODE))
            .ForMember(x => x.ParticipateUnit, y => y.MapFrom(u => u.ZCY2NDORG))
            .ForMember(x => x.Projectid, y => y.MapFrom(u => u.ZPROJECTID));
            #endregion

            mapperConfigurationExpression.CreateMap<SafeSupervisionDayReport, SafeSupervisionDayReportResponseDto>();
            mapperConfigurationExpression.CreateMap<AddOrUpdateSafeSupervisionDayReportRequestDto, SafeSupervisionDayReport>();

            mapperConfigurationExpression.CreateMap<User, InformationResponseDto>()
                .ForMember(x => x.UserId, y => y.MapFrom(u => u.Id))
                .ForMember(x => x.Name, y => y.MapFrom(u => u.Name))
                .ForMember(x => x.Phone, y => y.MapFrom(u => u.Phone))
                .ForMember(x => x.LoginAccount, y => y.MapFrom(u => u.LoginAccount));

            mapperConfigurationExpression.CreateMap<User, SearchAddedPersonnelReponseDto>()
                .ForMember(x => x.Id, y => y.MapFrom(u => u.Id))
                .ForMember(x => x.Name, y => y.MapFrom(u => u.Name))
                .ForMember(x => x.CompanyId, y => y.MapFrom(u => u.CompanyId))
                .ForMember(x => x.DepartmentId, y => y.MapFrom(u => u.DepartmentId)
                );


            mapperConfigurationExpression.CreateMap<SaveShipDayReportRequestDto, ShipDayReport>();
            mapperConfigurationExpression.CreateMap<ShipDayReport, ShipDayReportResponseDto>();

            mapperConfigurationExpression.CreateMap<SaveProjectWBSRequestDto.ReqProjectWBSNode, ProjectWBS>();
            mapperConfigurationExpression.CreateMap<ProjectWBS, ProjectWBSComposeResponseDto.ResProjectWBSNode>();
            mapperConfigurationExpression.CreateMap<JobRecord, JobRecordsResponseDto.ResJobRecord>();
            mapperConfigurationExpression.CreateMap<AddShipMovementRequestDto, ShipMovement>();
            mapperConfigurationExpression.CreateMap<ShipMovement, ShipMovementResponseDto>().ForMember(x => x.ShipMovementId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<SaveShipDynamicDayReportRequestDto, ShipDynamicDayReport>();
            mapperConfigurationExpression.CreateMap<ShipDynamicDayReport, ShipDynamicDayReportReportResponseDto>();
            mapperConfigurationExpression.CreateMap<PomPortDataResponseDto, PortData>()
               .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<PomShipPingTypeResponseDto, ShipPingType>()
              .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<SaveShipDynamicMonthReportRequestDto, ShipDynamicMonthReport>();
            mapperConfigurationExpression.CreateMap<ShipDynamicMonthReport, ShipDynamicMonthReportResponseDto>();
            mapperConfigurationExpression.CreateMap<SaveShipDynamicMonthReportRequestDto.ReqShipDynamicMonthReportDetail, ShipDynamicMonthReportDetail>();
            mapperConfigurationExpression.CreateMap<ShipDynamicMonthReportDetail, ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail>().ForMember(x => x.DetailId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<ShipDynamicDayReport, ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail>();
            mapperConfigurationExpression.CreateMap<SaveProjectMonthReportRequestDto, MonthReport>();
            mapperConfigurationExpression.CreateMap<SaveProjectMonthReportRequestDto.ReqMonthReportDetail, MonthReportDetail>();
            mapperConfigurationExpression.CreateMap<MonthReport, ProjectMonthReportResponseDto>();
            mapperConfigurationExpression.CreateMap<MonthReportDetail, ProjectMonthReportResponseDto.ResMonthReportDetail>().ForMember(x => x.DetailId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<StagingMonthReportRequestDto, ProjectMonthReportResponseDto>();
            mapperConfigurationExpression.CreateMap<StagingMonthReportRequestDto.ReqStagingTreeProjectWBSDetailDto, ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto>();
            mapperConfigurationExpression.CreateMap<StagingMonthReportRequestDto.ReqStagingMonthReportDetail, ProjectMonthReportResponseDto.ResMonthReportDetail>();
            mapperConfigurationExpression.CreateMap<SaveOwnerShipMonthReportRequestDto, OwnerShipMonthReport>();
            mapperConfigurationExpression.CreateMap<SaveOwnerShipMonthReportRequestDto.ReqOwnerShipMonthReportSoil, OwnerShipMonthReportSoil>();
            mapperConfigurationExpression.CreateMap<OwnerShipMonthReport, OwnerShipMonthReportResponseDto>();
            mapperConfigurationExpression.CreateMap<OwnerShipMonthReportSoil, OwnerShipMonthReportResponseDto.ResOwnerShipMonthReportSoil>().ForMember(x => x.MonthReportSoilId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<SaveSubShipMonthReportRequestDto, SubShipMonthReport>();
            mapperConfigurationExpression.CreateMap<SubShipMonthReport, SubShipMonthReportResponseDto>();
            mapperConfigurationExpression.CreateMap<PomShipClassicResponseDto, ShipClassic>().ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));
            mapperConfigurationExpression.CreateMap<AddSubShipUserRequestDto, SubShip>();
            mapperConfigurationExpression.CreateMap<ResTreeProjectWBSDetailDto, ProjectWbsData>();
            mapperConfigurationExpression.CreateMap<ResMonthReportDetail, ProjectWbsData>();
            mapperConfigurationExpression.CreateMap<Project, ModifyInsertProjectRequestDto>()
                .ForMember(x => x.ProjectId, y => y.MapFrom(u => u.PomId))
                .ForMember(x => x.Scale, y => y.MapFrom(u => u.GradeId));
            mapperConfigurationExpression.CreateMap<ProjectLeader, ProjectStakeholders>();
            mapperConfigurationExpression.CreateMap<ProjectOrg, ProjectStakeholderUnit>();
            mapperConfigurationExpression.CreateMap<SaveProjectPlanRequestDto, ProjectPlanProduction>();
            mapperConfigurationExpression.CreateMap<ProjectPlanInfo, ProjectPlanProduction>();
            mapperConfigurationExpression.CreateMap<SaveProjectApproverRequestDto, ProjectApprover>();
            mapperConfigurationExpression.CreateMap<ProjectApprover, ProjectApproverResponseDto>();
            mapperConfigurationExpression.CreateMap<SaveSendShipSparePartListRequestDto, SendShipSparePartList>();
            mapperConfigurationExpression.CreateMap<SparePartStorageList, GetSparePartStorageListResponseDto>();
            mapperConfigurationExpression.CreateMap<SaveSparePartStorageListResponseDto, SparePartStorageList>();
            mapperConfigurationExpression.CreateMap<RepairProjectList, GetRepairItemsListResponseDto>();
            mapperConfigurationExpression.CreateMap<SaveRepairItemsListRequestDto, RepairProjectList>();
            mapperConfigurationExpression.CreateMap<SparePartProjectList, SparePartProjectListResponseDto>();
            mapperConfigurationExpression.CreateMap<SaveSparePartProjectListRequestDto, SparePartProjectList>();
            mapperConfigurationExpression.CreateMap<SparePartProjectListRequestDto, RepairProjectList>();
            mapperConfigurationExpression.CreateMap<ExcelSparePartProjectListRequseDto, SparePartProjectList>();
            mapperConfigurationExpression.CreateMap<ExcelSendShipSparePartRequestDto, SendShipSparePartList>();
            mapperConfigurationExpression.CreateMap<ExcelSparePartStorageRequerDto, SparePartStorageList>();
            mapperConfigurationExpression.CreateMap<SaveShipSurveyRequestDto, GHMonitoringCenterApi.Domain.Models.ShipSurvey>();
            mapperConfigurationExpression.CreateMap<InsertProjectYearPlanRequestDto, YearInitProjectPlan>();

            //运营监控中心 需要导入的映射
            mapperConfigurationExpression.CreateMap<CompanyProjectBasePoduction, ExcelCompanyProjectBasePoduction>()
                .ForMember(x => x.UnitName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<CompanyBasePoductionValue, ExcelCompanyBasePoductionValue>()
                .ForMember(x => x.UnitName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<CompanyShipBuildInfo, ExcelCompanyShipBuildInfo>()
                .ForMember(x => x.ShipTypeName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<CompanyShipProductionValueInfo, ExcelCompanyShipProductionValueInfo>()
                .ForMember(x => x.ShipTypeName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<ShipProductionValue, ExcelShipProductionValue>();
            mapperConfigurationExpression.CreateMap<SpecialProjectInfo, ExcelSpecialProjectInfo>();
            mapperConfigurationExpression.CreateMap<CompanyWriteReportInfo, ExcelCompanyWriteReportInfo>()
                .ForMember(x => x.CompanyName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<CompanyUnWriteReportInfo, ExcelCompanyUnWriteReportInfo>()
                .ForMember(x => x.UnitName, y => y.MapFrom(u => u.Name));
            mapperConfigurationExpression.CreateMap<CompanyShipUnWriteReportInfo, ExcelCompanyShipUnWriteReportInfo>();
            mapperConfigurationExpression.CreateMap<ProjectShiftProductionInfo, ExcelProjectShiftProductionInfo>();
            mapperConfigurationExpression.CreateMap<UnProjectShitInfo, ExcelUnProjectShitInfo>()
                .ForMember(x => x.UnitName, y => y.MapFrom(u => u.CompanyName));
        }
    }
}
