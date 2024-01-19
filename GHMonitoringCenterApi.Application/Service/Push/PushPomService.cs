using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;
using GHMonitoringCenterApi.Application.Contracts.Dto.Push;
using GHMonitoringCenterApi.Domain;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using SqlSugar.Extensions;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using GHMonitoringCenterApi.Domain.Shared.Util;

namespace GHMonitoringCenterApi.Application.Service.Push
{
	/// <summary>
	/// 推送到Pom的业务层
	/// </summary>
	public class PushPomService : IPushPomService
	{


		#region 注入实例

		/// <summary>
		/// 项目
		/// </summary>
		private readonly IBaseRepository<Project> _dbProject;

		/// <summary>
		/// 项目日报
		/// </summary>
		private readonly IBaseRepository<DayReport> _dbDayReport;

		/// <summary>
		///项目日报-施工日志
		/// </summary>
		private readonly IBaseRepository<DayReportConstruction> _dbDayReportConstruction;

		/// <summary>
		/// 文件
		/// </summary>
		private readonly IBaseRepository<Files> _dbFile;

		/// <summary>
		/// 往来单位
		/// </summary>
		private readonly IBaseRepository<DealingUnit> _dbDealingUnit;

		/// <summary>
		/// 分包船舶
		/// </summary>
		private readonly IBaseRepository<SubShip> _dbSubShip;

		/// <summary>
		/// 自有船舶
		/// </summary>
		private readonly IBaseRepository<OwnerShip> _dbOwnerShip;

		/// <summary>
		/// 施工分类
		/// </summary>
		private readonly IBaseRepository<ProjectWBS> _dbProjectWBS;

		/// <summary>
		/// 字典表
		/// </summary>
		private readonly IBaseRepository<DictionaryTable> _dbDictionaryTable;

		/// <summary>
		/// 用户表
		/// </summary>
		private readonly IBaseRepository<Domain.Models.User> _dbUser;

		/// <summary>
		/// 月份交叉表
		/// </summary>

		private readonly IBaseRepository<CrossMonth> _dbcrossMonth;

		/// <summary>
		/// 安监日报
		/// </summary>
		private readonly IBaseRepository<SafeSupervisionDayReport> _dbSafeSupervisionDayReport;

		/// <summary>
		/// 船舶日报
		/// </summary>
		private readonly IBaseRepository<ShipDayReport> _dbShipDayReport;

		/// <summary>
		/// 港口
		/// </summary>
		private readonly IBaseRepository<PortData> _dbPortData;

		/// <summary>
		/// 项目月报
		/// </summary>
		private readonly IBaseRepository<MonthReport> _dbMonthReport;

		/// <summary>
		/// 项目月报-明细
		/// </summary>
		private readonly IBaseRepository<MonthReportDetail> _dbMonthReportDetail;

		/// <summary>
		/// 船舶动态月报明细
		/// </summary>
		private readonly IBaseRepository<ShipDynamicMonthReportDetail> _dbShipDynamicMonthReportDetail;

		/// <summary>
		/// 分包船舶月报
		/// </summary>
		private readonly IBaseRepository<SubShipMonthReport> _dbSubShipMonthReport;

		/// <summary>
		/// 自有船舶月报
		/// </summary>
		private readonly IBaseRepository<OwnerShipMonthReport> _dbOwnerShipMonthReport;

		/// <summary>
		///施工土质
		/// </summary>
		private readonly IBaseRepository<OwnerShipMonthReportSoil> _dbOwnerShipMonthReportSoil;

		/// <summary>
		/// 船舶进出场
		/// </summary>
		private readonly IBaseRepository<ShipMovement> _dbShipMovement;

		/// <summary>
		/// 船舶类型
		/// </summary>
		private readonly IBaseRepository<ShipPingType> _dbShipPingType;

		/// <summary>
		/// 年度计划
		/// </summary>
		private readonly IBaseRepository<ProjectPlanProduction> _dbProjectAnnualPlan;

		/// <summary>
		/// 项目类型
		/// </summary>
		private readonly IBaseRepository<ProjectType> _dbProjectType;

		/// <summary>
		/// 项目状态
		/// </summary>
		private readonly IBaseRepository<ProjectStatus> _dbProjectStatus;

		/// <summary>
		/// 机构（公司）
		/// </summary>
		private readonly IBaseRepository<Institution> _dbInstitution;

		/// <summary>
		/// 省份
		/// </summary>
		private readonly IBaseRepository<Province> _dbProvince;

		/// <summary>
		/// 区域
		/// </summary>
		private readonly IBaseRepository<ProjectArea> _dbProjectArea;

		/// <summary>
		/// 项目干系单位
		/// </summary>
		private readonly IBaseRepository<ProjectOrg> _dbProjectOrg;

		/// <summary>
		/// 项目施工资质
		/// </summary>
		private readonly IBaseRepository<ConstructionQualification> _dbConstructionQualification;

		/// <summary>
		/// 项目行业分类标准
		/// </summary>
		private readonly IBaseRepository<IndustryClassification> _dbIndustryClassification;

		/// <summary>
		/// 项目规模
		/// </summary>
		private readonly IBaseRepository<ProjectScale> _dbProjectScale;
		/// <summary>
		/// 项目工况级数
		/// </summary>
		private readonly IBaseRepository<WaterCarriage> _dbWaterCarriage;

		/// <summary>
		/// 项目领导班子
		/// </summary>
		private readonly IBaseRepository<ProjectLeader> _dbProjectLeader;

		/// <summary>
		/// 项目币种
		/// </summary>
		private readonly IBaseRepository<Currency> _dbCurrency;

		/// <summary>
		/// 对象变更记录
		/// </summary>
		private readonly IBaseRepository<EntityChangeRecord> _dbEntityChangeRecord;

		/// <summary>
		/// 匹配
		/// </summary>
		private readonly IMapper _mapper;

		/// <summary>
		/// 全局对象
		/// </summary>
		private readonly GlobalObject _globalObject;

		/// <summary>
		/// 数据库上下文
		/// </summary>
		public ISqlSugarClient _dbContext;
		#endregion

		/// <summary>
		/// 构造
		/// </summary>
		public PushPomService(IBaseRepository<Project> dbProject
			, IBaseRepository<DayReport> dbDayReport
			, IBaseRepository<DayReportConstruction> dbDayReportConstruction
			, IBaseRepository<Files> dbFile
			, IBaseRepository<DealingUnit> dbDealingUnit
			, IBaseRepository<SubShip> dbSubShip
			, IBaseRepository<OwnerShip> dbOwnerShip
			, IBaseRepository<ProjectWBS> dbProjectWBS
			, IBaseRepository<Domain.Models.User> dbUser
			, IBaseRepository<DictionaryTable> dbDictionaryTable
			, IBaseRepository<SafeSupervisionDayReport> dbSafeSupervisionDayReport
			, IBaseRepository<ShipDayReport> dbShipDayReport
			, IBaseRepository<PortData> dbPortData
			, IBaseRepository<MonthReport> dbMonthReport
			, IBaseRepository<MonthReportDetail> dbMonthReportDetail
			, IBaseRepository<ShipDynamicMonthReportDetail> dbShipDynamicMonthReportDetail
			, IBaseRepository<SubShipMonthReport> dbSubShipMonthReport
			, IBaseRepository<OwnerShipMonthReport> dbOwnerShipMonthReport
			, IBaseRepository<OwnerShipMonthReportSoil> dbOwnerShipMonthReportSoil
			, IBaseRepository<ShipMovement> dbShipMovement
			, IBaseRepository<ShipPingType> dbShipPingType
			, IBaseRepository<ProjectPlanProduction> dbProjectAnnualPlan
			, IBaseRepository<ProjectType> dbProjectType
			, IBaseRepository<ProjectStatus> dbProjectStatus
			, IBaseRepository<Institution> dbInstitution
			, IBaseRepository<Province> dbProvince
			, IBaseRepository<CrossMonth> dbCrossMonth
			, IBaseRepository<ProjectArea> dbProjectArea
			, IBaseRepository<ProjectOrg> dbProjectOrg
			, IBaseRepository<IndustryClassification> dbIndustryClassification
			, IBaseRepository<WaterCarriage> dbWaterCarriage
			, IBaseRepository<ProjectLeader> dbProjectLeader
			, IBaseRepository<Currency> dbCurrency
			, IBaseRepository<ConstructionQualification> dbConstructionQualification
			, IBaseRepository<ProjectScale> dbProjectScale
			, IBaseRepository<EntityChangeRecord> dbEntityChangeRecord
			, IMapper mapper
			, GlobalObject globalObject
			, ISqlSugarClient dbContext
			)
		{

			_dbProject = dbProject;
			_dbDayReport = dbDayReport;
			_dbDayReportConstruction = dbDayReportConstruction;
			_dbFile = dbFile;
			_dbcrossMonth = dbCrossMonth;
			_dbSubShip = dbSubShip;
			_dbDealingUnit = dbDealingUnit;
			_dbOwnerShip = dbOwnerShip;
			_dbProjectWBS = dbProjectWBS;
			_dbUser = dbUser;
			_dbDictionaryTable = dbDictionaryTable;
			_dbSafeSupervisionDayReport = dbSafeSupervisionDayReport;
			_dbShipDayReport = dbShipDayReport;
			_dbPortData = dbPortData;
			_dbMonthReport = dbMonthReport;
			_dbMonthReportDetail = dbMonthReportDetail;
			_dbShipDynamicMonthReportDetail = dbShipDynamicMonthReportDetail;
			_dbSubShipMonthReport = dbSubShipMonthReport;
			_dbOwnerShipMonthReport = dbOwnerShipMonthReport;
			_dbOwnerShipMonthReportSoil = dbOwnerShipMonthReportSoil;
			_dbShipMovement = dbShipMovement;
			_dbShipPingType = dbShipPingType;
			_dbProjectAnnualPlan = dbProjectAnnualPlan;
			_dbProjectType = dbProjectType;
			_dbProjectStatus = dbProjectStatus;
			_dbInstitution = dbInstitution;
			_dbProvince = dbProvince;
			_dbProjectArea = dbProjectArea;
			_dbProjectOrg = dbProjectOrg;
			_dbConstructionQualification = dbConstructionQualification;
			_dbIndustryClassification = dbIndustryClassification;
			_dbProjectScale = dbProjectScale;
			_dbWaterCarriage = dbWaterCarriage;
			_dbProjectLeader = dbProjectLeader;
			_dbCurrency = dbCurrency;
			_dbEntityChangeRecord = dbEntityChangeRecord;
			_mapper = mapper;
			_globalObject = globalObject;
			_dbContext = dbContext;
		}
		#region 推送项目信息
		/// <summary>
		/// 推送项目信息
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> PushProjectAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifyProjectReportRep") ?? new PushToPomOption();
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送项目信息未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<Project>(EntityType.Project, maxChangeTime);
			var projectIds = changeModels.Select(t => t.Entity.Id).Distinct().ToList();
			projectIds.AddRange(changeModels.Select(t => (Guid)t.Entity.MasterProjectId).Distinct().ToList());
			var projectLeaders = await GetProjectLeaderAsync(projectIds.ToArray());
			var projectOrgs = await GetProjectOrgAsync(projectIds.ToArray());	
			var projectCompanys = await _dbInstitution.AsQueryable().Where(t => t.IsDelete == 1).ToListAsync();
			var projectDepartMents = await GetProjectCompanyAsync(changeModels.Select(t => t.Entity.ProjectDept).Distinct().ToArray());
			var projectTypes = await GetProjectTypeAsync(changeModels.Where(t => t.Entity.TypeId != null).Select(t => (Guid)t.Entity.TypeId).Distinct().ToArray());
			var projectStatus = await GetProjectStatusAsync(changeModels.Where(t => t.Entity.StatusId != null).Select(t => (Guid)t.Entity.StatusId).Distinct().ToArray());
			var projectGrads = await GetProjectGradAsync(changeModels.Where(t => t.Entity.GradeId != null).Select(t => (Guid)t.Entity.GradeId).Distinct().ToArray());
			var projectConditionGrades = await GetProjectConditionGradeAsync(changeModels.Where(t => t.Entity.ConditionGradeId != null && t.Entity.ConditionGradeId != "").Select(t => t.Entity.ConditionGradeId.ToGuid()).Distinct().ToArray());
			var projectConstructionQualifications = await GetProjectConstructionQualificationAsync(changeModels.Where(t => t.Entity.ProjectConstructionQualificationId != null).Select(t => (Guid)t.Entity.ProjectConstructionQualificationId).Distinct().ToArray());
			var projectRegions = await GetProjectRegionAsync(changeModels.Where(t => t.Entity.RegionId != null).Select(t => (Guid)t.Entity.RegionId).Distinct().ToArray());
			var projectCurrencys = await GetProjectCurrencyAsync(changeModels.Select(t => t.Entity.CurrencyId).Distinct().ToArray());
			var userIds = changeModels.Where(t => t.Entity.UpdateId != null).Select(t => (Guid)t.Entity.UpdateId).Distinct().ToList();
			userIds.AddRange(changeModels.Where(t => t.Entity.CreateId != null).Select(t => (Guid)t.Entity.CreateId).Distinct().ToList());
			var users = await GetUserPartAsync(userIds.Distinct().ToArray());

			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushProjectAsync(option, maxChangeTime, singleChangeModels,
					projectLeaders, projectOrgs, projectCompanys, projectDepartMents, projectTypes, projectStatus, projectGrads,
					projectConditionGrades, projectConstructionQualifications, projectRegions, projectCurrencys, users);
			}
			return result.SuccessResult(true);
		}
		/// <summary>
		/// 单次推送项目日报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushProjectAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<Project>[] changeModels,
			List<ProjectLeader> projectLeaders, List<ProjectOrg> projectOrgs, List<Institution> projectCompanys,List<Institution> projectDepartMents, List<ProjectType> projectTypes,
			List<ProjectStatus> projectStatus, List<ProjectScale> projectGrads, List<WaterCarriage> projectConditionGrades, List<ConstructionQualification> projectConstructionQualifications,
			List<ProjectArea> projectRegions, List<Currency> currencys, List<Domain.Models.User> users)
		{
			var pushModels = new List<ModifyInsertProjectRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var leaderList = projectLeaders.Where(x => x.ProjectId == changeModel.Entity.Id || x.ProjectId == changeModel.Entity.MasterProjectId).ToList();
				var orgList = projectOrgs.Where(x => x.ProjectId == changeModel.Entity.Id || x.ProjectId == changeModel.Entity.MasterProjectId).ToList();
				var typeFirst = projectTypes.FirstOrDefault(x => x.PomId == changeModel.Entity.TypeId);
				var statusFirst = projectStatus.FirstOrDefault(x => x.StatusId == changeModel.Entity.StatusId);
				var gradFirst = projectGrads.FirstOrDefault(x => x.PomId == changeModel.Entity.GradeId);
				WaterCarriage conditionFirst = new WaterCarriage();
				if (!string.IsNullOrWhiteSpace(changeModel.Entity.ConditionGradeId))
				{
					conditionFirst = projectConditionGrades.FirstOrDefault(x => x.PomId == changeModel.Entity.ConditionGradeId?.ToGuid());
				}
				var constructionQualificationFirst = projectConstructionQualifications.FirstOrDefault(x => x.PomId == changeModel.Entity.ProjectConstructionQualificationId);
				var regionFirst = projectRegions.FirstOrDefault(x => x.AreaId == changeModel.Entity.RegionId);
				var currencyFirst = currencys.FirstOrDefault(t => t.PomId == changeModel.Entity.CurrencyId);
				var createUser = users.FirstOrDefault(x => x.Id == changeModel.Entity.CreateId);
				var updateUser = users.FirstOrDefault(x => x.Id == changeModel.Entity.UpdateId);
				var departMent = projectDepartMents.FirstOrDefault(x => x.PomId == changeModel.Entity.ProjectDept);
				var tCompany = projectCompanys.Where(t=>t.PomId == changeModel.Entity.CompanyId).FirstOrDefault();
				pushModels.Add(ConvertProjectRequestDto(changeModel.Entity, leaderList, orgList, typeFirst, statusFirst, gradFirst,
					conditionFirst, constructionQualificationFirst, regionFirst, currencyFirst, createUser, updateUser, departMent, tCompany));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new PushPomModifyInsertProjectRequestDto()
				{
					ModifyInsRequestJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		/// 转换成推送pom请求的model
		/// </summary>
		/// <returns></returns>
		private ModifyInsertProjectRequestDto ConvertProjectRequestDto(Project project, List<ProjectLeader> projectLeaders, List<ProjectOrg> projectOrgs, ProjectType projectTypes,
			ProjectStatus projectStatus, ProjectScale projectScales, WaterCarriage waterCarriages, ConstructionQualification constructionQualifications,
			ProjectArea projectAreas, Currency currency, Domain.Models.User createUser, Domain.Models.User updateUser,Institution departMent, Institution tCompany)
		{
			var model = new ModifyInsertProjectRequestDto();
			var leaders = new List<ProjectStakeholders>();
			var orgs = new List<ProjectStakeholderUnit>();
			_mapper.Map(project, model);
			model.ProjectDeptName = departMent.Name;
			model.TCompanyId = tCompany.PomId.ToString();
			model.TCompanyName = tCompany.Name;
			model.Amount = Convert.ToDecimal(project.Amount);
			model.ECAmount = Convert.ToDecimal(project.ECAmount);
			model.TypeName = projectTypes?.Name;
			model.StatusName = projectStatus.Name;
			model.ScaleName = projectScales?.Name;
			model.ConditionGradeName = waterCarriages?.Remarks;
			model.ProjectConstructionQualificationName = constructionQualifications?.Name;
			model.RegionName = projectAreas?.Name;
			model.CurrencyName = currency?.Zcurrencyname;
			model.CreateBy = createUser?.PomId.ToString();
			model.CreateTime = project.CreateTime == null ? DateTime.MinValue : project.CreateTime.Value;
			model.Updateby = updateUser?.Name;
			model.UpdatebyTime = project.UpdateTime == null ? DateTime.MinValue : project.UpdateTime.Value;
			model.ProjectStakeholders = _mapper.Map(projectLeaders, leaders);
			foreach (var item in model.ProjectStakeholders)
			{
				item.ProjectId = project.MasterProjectId.ToString();
				item.IsPresent = item.IsPresent == "True" ? "true" : "false";
			}
			model.ProjectStakeholderUnit = _mapper.Map(projectOrgs, orgs);
			foreach (var item in model.ProjectStakeholderUnit)
			{
				item.ProjectId = project.MasterProjectId.ToString();
				if (item.Type == "13")
				{
					item.Type = "99";
				}
				else
				{
					item.Type = (Convert.ToInt32(item.Type) - 1).ToString();
				}
			}
			return model;
		}
		#endregion

		#region　推送项目月报
		/// <summary>
		/// 推送项目月报  
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> PushMonthReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:ModifyProjectMonthRep") ?? new PushToPomOption(); ;
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送项目月报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<MonthReport>(EntityType.MonthReport, maxChangeTime);
			var projectPlan = await GetProjectAnnualPlanAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray(), DateTime.Now.Year);
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushMonthReportsAsync(option, maxChangeTime, singleChangeModels, projects, projectPlan);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 推送项目月报特殊字段  
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> PushSpecialFieldsMonthReportsAsync(Guid monthRepId)
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:ModifyProjectMonthRep") ?? new PushToPomOption(); ;
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送项目月报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushSpecialFieldsChangeModelsAsync<MonthReport>(EntityType.MonthReport, maxChangeTime, monthRepId);
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushMonthReportsAsync(option, maxChangeTime, singleChangeModels, projects, new List<ProjectPlanProduction>());
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送项目月报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushMonthReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<MonthReport>[] changeModels, List<Project> projects, List<ProjectPlanProduction> projectAnnualPlans)
		{
			var pushModels = new List<PomProjectMonthRepRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				var plan = projectAnnualPlans.Where(t => t.ProjectId == changeModel.Entity.ProjectId).FirstOrDefault();
				//是否推送pom
				//if (!changeModel.Entity.IsPushPom)
				//{
				//    changeRecord.PushStatus = PushStatus.PushVerifyFail;
				//    changeRecord.FailReason = "该项目月报已标记为不推送";
				//    continue;
				//}
				pushModels.Add(ConvertPushMonthReportRequestDto(changeModel.Entity, project, plan));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new PushPomProjectMonthRequestDto()
				{
					ProjectMonthJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		///转换成推送pom的请求model
		/// </summary>
		/// <returns></returns>
		private PomProjectMonthRepRequestDto ConvertPushMonthReportRequestDto(MonthReport monthReport, Project project, ProjectPlanProduction? projectAnnualPlan)
		{
			//除税率
			var excludingTax = 1 + (project.Rate ?? 0);
			ConvertHelper.TryParseFromDateMonth(monthReport.DateMonth, out DateTime monthTime);
			var planValue = 0.00M;
			if (projectAnnualPlan != null)
			{
			switch (monthReport.DateMonth.ToString().Substring(4))
			{
				case "01":
					planValue = projectAnnualPlan.OnePlanProductionValue ?? 0;
					break;
				case "02":
					planValue = projectAnnualPlan.TwoPlanProductionValue ?? 0;
					break;
				case "03":
					planValue = projectAnnualPlan.ThreePlanProductionValue ?? 0;
					break;
				case "04":
					planValue = projectAnnualPlan.FourPlanProductionValue ?? 0;
					break;
				case "05":
					planValue = projectAnnualPlan.FivePlanProductionValue ?? 0;
					break;
				case "06":
					planValue = projectAnnualPlan.SixPlanProductionValue ?? 0;
					break;
				case "07":
					planValue = projectAnnualPlan.SevenPlanProductionValue ?? 0;
					break;
				case "08":
					planValue = projectAnnualPlan.EightPlanProductionValue ?? 0;
					break;
				case "09":
					planValue = projectAnnualPlan.NinePlanProductionValue ?? 0;
					break;
				case "10":
					planValue = projectAnnualPlan.TenPlanProductionValue ?? 0;
					break;
				case "11":
					planValue = projectAnnualPlan.ElevenPlanProductionValue ?? 0;
					break;
				case "12":
					planValue = projectAnnualPlan.TwelvePlanProductionValue ?? 0;
					break;
				default:
					break;
				}
			}
			else
			{
				planValue = 0;
            }
            return new PomProjectMonthRepRequestDto()
			{
				ProjectId = project.PomId,
				StatementMonth = monthTime,
				ProjectName = project.Name,
				PlannedOutputValueMonth = planValue,
				CompleteOutputValue = monthReport.CompleteProductionAmount / excludingTax,
				CompleteVolumeMonth = monthReport.CompletedQuantity,
				ConfirmedOutputValue = monthReport.PartyAConfirmedProductionAmount / excludingTax,
				PaymentAmountOfParty = monthReport.PartyAPayAmount / excludingTax,
				MonthAccountReceiva = monthReport.ReceivableAmount / excludingTax,
				ProgressDescription = string.IsNullOrWhiteSpace(monthReport.ProgressDescriptionPushPom) ? monthReport.ProgressDescription : monthReport.ProgressDescriptionPushPom,
				EstimatedCost = monthReport.NextMonthEstimateCostAmount / excludingTax,
				ActualCost = monthReport.CostAmount / excludingTax,
				EstimatedCostMonth = monthReport.MonthEstimateCostAmount / excludingTax,
				MainCause = EnumExtension.GetEnumDescription(monthReport.ProgressDeviationReason),
				CauseAnalysis = string.IsNullOrWhiteSpace(monthReport.ProgressDeviationDescriptionPushPom) ? monthReport.ProgressDeviationDescription : monthReport.ProgressDeviationDescriptionPushPom,
				MainCausesofCostDeviation = EnumExtension.GetEnumDescription(monthReport.CostDeviationReason),
				IsMonthRep = 0
			};
		}

		#endregion

		#region 自有船舶月报


		/// <summary>
		/// 推送自有船舶月报
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> PushOwnerShipMonthReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifyOwnShipMonthRep") ?? new PushToPomOption(); ;
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送自有船舶月报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<OwnerShipMonthReport>(EntityType.OwnerShipMonthReport, maxChangeTime);
			var reportDetails = await GetOwnerShipMonthReportSoilsAsync(changeModels.Select(t => t.Entity.Id).Distinct().ToArray());
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var ships = await GetShipPartsAsync(changeModels.Select(t => t.Entity.ShipId).Distinct().ToArray(), ShipType.OwnerShip);
			var contractDetails = await GetDictionarysAsync(DictionaryTypeNo.ContractDetail);
			var users = await GetUserPartAsync(changeModels.Where(t => t.Entity.CreateId != null).Select(t => (Guid)t.Entity.CreateId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushOwnerShipMonthReportsAsync(option, maxChangeTime, singleChangeModels, reportDetails, projects, ships, contractDetails, users);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送自有船舶月报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushOwnerShipMonthReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<OwnerShipMonthReport>[] changeModels, List<OwnerShipMonthReportSoil> reportSoils, List<Project> projects, List<ShipPartDto> ships, List<DictionaryTable> contractDetails, List<Domain.Models.User> users)
		{
			var pushModels = new List<PomProjectOwnShipMonthRepRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				var ship = ships.FirstOrDefault(t => t.PomId == changeModel.Entity.ShipId);
				if (ship == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "船舶不存在";
					continue;
				}
				var contractDetail = contractDetails.FirstOrDefault(t => t.Type == changeModel.Entity.ContractDetailType);
				if (contractDetail == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "合同清单不存在";
					continue;
				}
				var operater = users.FirstOrDefault(t => t.Id == changeModel.Entity.CreateId);
				var thisReportDetails = reportSoils.Where(t => t.OwnerShipMonthReportId == changeModel.Entity.Id).ToList();
				pushModels.Add(ConvertPushOwnerShipMonthReportRequestDto(project, changeModel.Entity, thisReportDetails, ship, contractDetail.Name, operater?.PomId));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new PushPomOwnerShipMonthReportRequestDto()
				{
					ProjectOwnShipMonthRepJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}


		/// <summary>
		/// 推送到Pom自有船舶月报
		/// </summary>
		/// <returns></returns>
		private PomProjectOwnShipMonthRepRequestDto ConvertPushOwnerShipMonthReportRequestDto(Project project, OwnerShipMonthReport monthReport, List<OwnerShipMonthReportSoil> reportSoils, ShipPartDto ship, string? contractDetailName, Guid? operateUserId)
		{
			// 除税率
			var excludingTax = 1 + (project.Rate ?? 0);
			ConvertHelper.TryParseFromDateMonth(monthReport.DateMonth, out DateTime monthTime);
			// pom系统数据库对于 工程量和产值存储单位是元
			return new PomProjectOwnShipMonthRepRequestDto()
			{
				ProjectId = project.PomId,
				StatementMonth = monthTime,
				OwnShipId = monthReport.ShipId.ToString(),
				OwnShipName = ship.Name,
				WorkHours = monthReport.WorkingHours,
				ConstructionDays = monthReport.ConstructionDays,
				Quantities = monthReport.Production,
				OutputValue = monthReport.ProductionAmount / excludingTax,
				WorkModeId = monthReport.WorkModeId.ToString(),
				WorkTypeId = monthReport.WorkTypeId.ToString(),
				ContractType = contractDetailName,
				ConditionGradeId = monthReport.ConditionGradeId.ToString(),
				HaulDistance = monthReport.HaulDistance,
				ApproachDate = monthReport.EnterTime.ToString(),
				ExitDate = monthReport.QuitTime.ToString(),
				ShipMonthRepSoils = reportSoils.Select(t => new PomShipMonthRepSoil()
				{
					ProjectId = project.PomId,
					OwnShipId = monthReport.ShipId.ToString(),
					SoilId = t.SoilId.ToString(),
					SoilGradeId = t.SoilGradeId.ToString(),
					Proportion = t.Proportion,
					StatementMonth = monthTime,
					CreateUser = operateUserId.ObjToString()
				}).ToList()
			};
		}


		/// <summary>
		/// 自有船舶月报明细集合
		/// </summary>
		/// <returns></returns>
		private async Task<List<OwnerShipMonthReportSoil>> GetOwnerShipMonthReportSoilsAsync(Guid[] ownerShipMonthReportIds)
		{
			return await _dbOwnerShipMonthReportSoil.GetListAsync(t => ownerShipMonthReportIds.Contains(t.OwnerShipMonthReportId) && t.IsDelete == 1);
		}

		#endregion

		#region 分包船舶月报

		/// <summary>
		/// 推送分包船舶月报
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> PushSubShipMonthReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifySubShipMonthRep") ?? new PushToPomOption(); ;
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送自有船舶月报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<SubShipMonthReport>(EntityType.SubShipMonthReport, maxChangeTime);
			//var reportDetails = await GetOwnerShipMonthReportSoilsAsync(changeModels.Select(t => t.Entity.Id).Distinct().ToArray());
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var ships = await GetShipPartsAsync(changeModels.Select(t => t.Entity.ShipId).Distinct().ToArray(), ShipType.SubShip);
			var contractDetails = await GetDictionarysAsync(DictionaryTypeNo.ContractDetail);
			var dynamicDescriptions = await GetDictionarysAsync(DictionaryTypeNo.DynamicDescription);
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushSubShipMonthReportsAsync(option, maxChangeTime, singleChangeModels, projects, ships, dynamicDescriptions, contractDetails);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送分包船舶月报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushSubShipMonthReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<SubShipMonthReport>[] changeModels, List<Project> projects, List<ShipPartDto> ships, List<DictionaryTable> dynamicDescriptions, List<DictionaryTable> contractDetails)
		{
			var pushModels = new List<PomProjectSubShipMonthRepRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				var ship = ships.FirstOrDefault(t => t.PomId == changeModel.Entity.ShipId);
				if (ship == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "船舶不存在";
					continue;
				}
				var contractDetail = contractDetails.FirstOrDefault(t => t.Type == changeModel.Entity.ContractDetailType);
				if (contractDetail == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "合同清单不存在";
					continue;
				}
				var dynamicDescription = dynamicDescriptions.FirstOrDefault(t => t.Type == changeModel.Entity.DynamicDescriptionType);
				if (dynamicDescription == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "当前动态描述不存在";
					continue;
				}
				pushModels.Add(ConvertPushSubShipMonthReportRequestDto(project, changeModel.Entity, ship, dynamicDescription.Name, contractDetail.Name));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new PushPomSubShipMonthReportRequestDto()
				{
					ProjectSubShipMonthRepJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}


		/// <summary>
		/// 推送到Pom分包船舶月报
		/// </summary>
		/// <returns></returns>
		private PomProjectSubShipMonthRepRequestDto ConvertPushSubShipMonthReportRequestDto(Project project, SubShipMonthReport monthReport, ShipPartDto ship, string? dynamicDescriptionName, string? contractDetailTypeName)
		{
			// 除税率
			var excludingTax = 1 + (project.Rate ?? 0);
			ConvertHelper.TryParseFromDateMonth(monthReport.DateMonth, out DateTime monthTime);
			// pom系统数据库对于 工程量和产值存储单位是
			return new PomProjectSubShipMonthRepRequestDto()
			{
				ProjectId = project.PomId,
				StatementMonth = monthTime,
				SubShipId = monthReport.ShipId.ToString(),
				SubShipName = ship.Name,
				SubWorkHours = monthReport.WorkingHours,
				SubConstructionDays = monthReport.ConstructionDays,
				SubQuantities = monthReport.Production,
				SubOutputValue = monthReport.ProductionAmount / excludingTax,
				SubDynamicDescription = dynamicDescriptionName,
				SubContractType = contractDetailTypeName,
				SubPlannedVolumeNextMonth = monthReport.NextMonthPlanProduction,
				SubPlannedOutputValueNextMonth = monthReport.NextMonthPlanProductionAmount,
				SubApproachDate = monthReport.EnterTime,
				SubExitDate = monthReport.QuitTime
			};

		}
		#endregion

		#region 推送船舶日报
		public async Task<ResponseAjaxResult<bool>> PushShipDayReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifyShipDayReportRep") ?? new PushToPomOption();
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送船舶日报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<ShipDayReport>(EntityType.ShipDayReport, maxChangeTime);
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var shipIds = changeModels.Select(x => x.Entity.ShipId).ToArray();
			var ships = await _dbOwnerShip.AsQueryable().Where(x => x.IsDelete == 1 && shipIds.Contains(x.PomId)).ToListAsync();
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushShipDayReportsAsync(option, maxChangeTime, singleChangeModels, projects, ships);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送项目日报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushShipDayReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<ShipDayReport>[] changeModels, List<Project> projects, List<OwnerShip> ships)
		{
			var pushModels = new List<OwnShipDayRepDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				//填报日期大于当天时则跳过
				if (changeModel.Entity.DateDay >= maxChangeTime.ToDateDay())
				{
					continue;
				}
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				if (changeModel.Entity.ShipDayReportType != ShipDayReportType.ProjectShip)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "未关联项目船舶不可推送";
					continue;
				}
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				var ship = ships.FirstOrDefault(t => t.PomId == changeModel.Entity.ShipId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				if (ship == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "船舶不存在";
					continue;
				}

				pushModels.Add(ConvertShipDayReportRequestDto(changeModel.Entity, project, ship));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new ProjectDayRepRequestJsonDto()
				{
					ProjectDayRepRequestJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		/// 转换成推送pom请求的model
		/// </summary>
		/// <param name="dayReport"></param>
		/// <param name="project"></param>
		/// <returns></returns>
		private OwnShipDayRepDto ConvertShipDayReportRequestDto(ShipDayReport dayReport, Project project, OwnerShip ship)
		{
			ConvertHelper.TryConvertDateTimeFromDateDay(dayReport.DateDay, out DateTime dayTime);
			return new OwnShipDayRepDto()
			{
				ProjectId = project.PomId,
				ProjectName = project.Name,
				OwnShipId = ship.PomId.ToString(),
				OwnShipName = ship.Name,
				ShipStatus = dayReport.ShipState.ToDescription(),
				SubmitDate = dayTime.AddDays(1)
			};
		}
		#endregion

		#region 推送项目日报
		public async Task<ResponseAjaxResult<bool>> PushDayReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifyDayReportRep") ?? new PushToPomOption();
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送项目日报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<DayReport>(EntityType.DayReport, maxChangeTime);
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var shipMovents = await GetShipMoventAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushProjectDayReportsAsync(option, maxChangeTime, singleChangeModels, projects, shipMovents);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送项目日报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushProjectDayReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<DayReport>[] changeModels, List<Project> projects, List<ShipMovement> shipMovements)
		{
			var pushModels = new List<ProjectOututValDayRepRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				//填报日期大于当天时则跳过
				if (changeModel.Entity.DateDay >= maxChangeTime.ToDateDay())
				{
					continue;
				}
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				var isExistShip = false;
				var shipMovementList = shipMovements.Where(t => t.ProjectId == changeModel.Entity.ProjectId).ToList();
				if (shipMovementList.Any())
				{
					ConvertHelper.TryConvertDateTimeFromDateDay(changeModel.Entity.DateDay, out DateTime dayTime);
					foreach (var item in shipMovementList)
					{
						if (dayTime >= item.EnterTime && item.QuitTime == null)
						{
							isExistShip = true;
						}
						else if (dayTime >= item.EnterTime && dayTime <= item.QuitTime)
						{
							isExistShip = true;
						}
					}
				}

				pushModels.Add(ConvertProjectDayReportRequestDto(changeModel.Entity, project, isExistShip));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new ProjectDayRepRequestJsonDto()
				{
					ProjectDayRepRequestJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		/// 转换成推送pom请求的model
		/// </summary>
		/// <param name="dayReport"></param>
		/// <param name="project"></param>
		/// <returns></returns>
		private ProjectOututValDayRepRequestDto ConvertProjectDayReportRequestDto(DayReport dayReport, Project project, bool isExistShip)
		{
			//除税率
			var excludingTax = 1 + (project.Rate ?? 0);
			ConvertHelper.TryConvertDateTimeFromDateDay(dayReport.DateDay, out DateTime dayTime);
			return new ProjectOututValDayRepRequestDto()
			{
				ProjectId = project.PomId,
				ProjectName = project.Name,
				PlannedOutputValue = dayReport.MonthPlannedProductionAmount / 10000 / excludingTax,
				ActualOutputValue = dayReport.MonthProductionAmount / excludingTax / 10000,
				YearActualOutputValue = dayReport.YearProductionAmount / excludingTax / 10000,
				CumulativeOutputValue = dayReport.CumulativeProductionAmount / excludingTax / 10000,
				ActualOutputValueOfDay = dayReport.DayActualProductionAmount / excludingTax / 10000,
				CompleteAmount = dayReport.CompleteAmount / excludingTax / 10000,
				SubmitDate = dayTime.AddDays(1),
				IsFillShip = isExistShip == true ? false : true
			};
		}
		#endregion

		#region 推送安监日报
		public async Task<ResponseAjaxResult<bool>> PushSafeDayReportsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifySafeDayReportRep") ?? new PushToPomOption();
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送安监日报未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<SafeSupervisionDayReport>(EntityType.SafeDayReport, maxChangeTime);
			var projects = await GetProjectPartsAsync(changeModels.Select(t => t.Entity.ProjectId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushSafeDayReportsAsync(option, maxChangeTime, singleChangeModels, projects);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送项目日报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushSafeDayReportsAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<SafeSupervisionDayReport>[] changeModels, List<Project> projects)
		{
			var pushModels = new List<ProjectSafeDayRepRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				//填报日期大于当天时则跳过
				if (changeModel.Entity.DateDay >= maxChangeTime.ToDateDay())
				{
					continue;
				}
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var project = projects.FirstOrDefault(t => t.Id == changeModel.Entity.ProjectId);
				if (project == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "项目不存在";
					continue;
				}
				pushModels.Add(ConvertSafeDayReportRequestDto(changeModel.Entity, project));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new ProjectDayRepRequestJsonDto()
				{
					ProjectDayRepRequestJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		/// 转换成推送pom请求的model
		/// </summary>
		/// <param name="SafedayReport"></param>
		/// <param name="project"></param>
		/// <returns></returns>
		private ProjectSafeDayRepRequestDto ConvertSafeDayReportRequestDto(SafeSupervisionDayReport SafedayReport, Project project)
		{
			ConvertHelper.TryConvertDateTimeFromDateDay(SafedayReport.DateDay, out DateTime dayTime);
			return new ProjectSafeDayRepRequestDto()
			{
				ProjectId = project.PomId.ToString(),
				ProjectName = project.Name,
				IsWork = SafedayReport.IsWork ? 1 : 0,
				WorkDate = SafedayReport.WorkDate,
				IsCompanyWork = SafedayReport.IsCompanyWork ? 1 : 0,
				WorkStatus = SafedayReport.WorkStatus.ToDescription(),
				ConstructionContent = SafedayReport.ConstructionContent,
				ProductionSituation = SafedayReport.ProductionSituation,
				PlanWorkDate = SafedayReport.PlanWorkDate,
				Reason = SafedayReport.Reason,
				Details = SafedayReport.Details,
				ActualWorkDate = SafedayReport.ActualWorkDate,
				ActualConstructionContent = SafedayReport.ActualConstructionContent,
				ActualSituation = SafedayReport.ActualSituation,
				ManagementNumber = SafedayReport.ManagementNumber,
				PersonNumber = SafedayReport.PersonNumber,
				WorkerNumber = SafedayReport.WorkerNumber,
				FromWuHanNumber = SafedayReport.FromHighRiskAreasPersonNumber,
				InManagementNumber = SafedayReport.InManagementNumber,
				InPersonNumber = SafedayReport.InPersonNumber,
				InWorkerNumber = SafedayReport.InWorkerNumber,
				InFromWuHanNumber = SafedayReport.InHighRiskAreasPersonNumber,
				QuarantineManagementNum = SafedayReport.QuarantineManagementNum,
				QuarantinePersonNum = SafedayReport.QuarantinePersonNum,
				QuarantineWorkerNum = SafedayReport.QuarantineWorkerNum,
				QuarantineReason = SafedayReport.QuarantineReason,
				DiagnosisNum = SafedayReport.DiagnosisNum,
				SuspectsNum = SafedayReport.SuspectsNum,
				ShouldManagementNum = SafedayReport.ShouldManagementNum,
				ShouldPersonNum = SafedayReport.ShouldPersonNum,
				ShouldWorkerNum = SafedayReport.ShouldWorkerNum,
				MaskNum = SafedayReport.MaskNum,
				ThermometerNum = SafedayReport.ThermometerNum,
				DisinfectantNum = SafedayReport.DisinfectantNum,
				Measures = SafedayReport.Measures,
				Situation = SafedayReport.Situation.ToDescription(),
				Other = SafedayReport.Other,
				IsSuperiorSupervision = SafedayReport.IsSuperiorSupervision ? 1 : 0,
				SuperiorSupervisionCount = SafedayReport.SuperiorSupervisionCount,
				SuperiorSupervisionForm = SafedayReport.SuperiorSupervisionForm.ToDescription(),
				SuperiorSupervisionDate = SafedayReport.SuperiorSupervisionDate,
				SupervisionUnit = SafedayReport.SupervisionUnit,
				SupervisionLeader = SafedayReport.SupervisionLeader,
				SupervisionOther = SafedayReport.SupervisionOther,
				SubmitDate = dayTime.AddDays(1)
			};
		}
		#endregion


		#region 推送分包船舶
		public async Task<ResponseAjaxResult<bool>> PushSubShipsAsync()
		{
			var result = new ResponseAjaxResult<bool>();
			var option = AppsettingsHelper.GetSection<PushToPomOption>("PushPomData:InsModifySubs") ?? new PushToPomOption();
			if (option.IsOpen != "true")
			{
				return result.FailResult(HttpStatusCode.PushFail, "推送分包船舶未开启");
			}
			var maxChangeTime = DateTime.Now;
			var changeModels = await GetPushChangeModelsAsync<SubShip>(EntityType.SubShip, maxChangeTime);
			var subShips = await GetSubShipAsync(changeModels.Select(t => t.Entity.PomId).Distinct().ToArray());
			var shipClassics = await GetShipClassicAsync(changeModels.Select(t => t.Entity.ClassicId).Distinct().ToArray());
			var splitModels = changeModels.Split(option.SinglePushNum);
			foreach (var singleChangeModels in splitModels)
			{
				await SinglePushSubShipAsync(option, maxChangeTime, singleChangeModels, subShips, shipClassics);
			}
			return result.SuccessResult(true);
		}

		/// <summary>
		/// 单次推送项目日报集合
		/// </summary>
		/// <returns></returns>
		public async Task SinglePushSubShipAsync(PushToPomOption option, DateTime maxChangeTime, PushChangeEntityDto<SubShip>[] changeModels, List<SubShip> subShips, List<ShipClassic> shipClassics)
		{
			var pushModels = new List<SubShipRequestDto>();
			var changeRecords = new List<EntityChangeRecord>();
			foreach (var changeModel in changeModels)
			{
				var changeRecord = new EntityChangeRecord()
				{
					Id = changeModel.RecordId,
					ItemId = changeModel.Entity.Id,
					PushTime = DateTime.Now,
					IsPush = true,
					PushStatus = PushStatus.UnPush
				};
				changeRecords.Add(changeRecord);
				var ship = subShips.FirstOrDefault(t => t.PomId == changeModel.Entity.PomId);
				if (ship == null)
				{
					changeRecord.PushStatus = PushStatus.PushVerifyFail;
					changeRecord.FailReason = "船舶不存在";
					continue;
				}
				if (changeModel.Entity.ClassicId != null && changeModel.Entity.ClassicId != Guid.Empty)
				{
					changeModel.Entity.ClassicId = shipClassics.Where(t => t.Id == changeModel.Entity.ClassicId).FirstOrDefault().PomId;
				}

				pushModels.Add(ConvertSubShipRequestDto(changeModel.Entity));
			}
			var pushStatus = PushStatus.UnPush;
			var pushFailMessage = string.Empty;
			if (pushModels.Any())
			{
				var pushPomModel = new PushPomSubShipRequestDto()
				{
					SubShipRequestJson = JsonConvert.SerializeObject(pushModels)
				};
				try
				{
					await HttpPushPomAsync(option, pushPomModel);
					pushStatus = PushStatus.PushSucess;
				}
				catch (Exception ex)
				{
					pushStatus = PushStatus.PushFail;
					pushFailMessage = ex.Message;
				}
			}
			changeRecords.ForEach(item =>
			{
				if (item.PushStatus == PushStatus.UnPush)
				{
					item.PushStatus = pushStatus;
					item.FailReason = pushFailMessage;
				}
			});
			// 变更记录状态的修改
			await ChangeRecordPushStatusAsync(changeRecords, maxChangeTime);
		}

		/// <summary>
		/// 转换成推送pom请求的model
		/// </summary>
		/// <param name="dayReport"></param>
		/// <param name="project"></param>
		/// <returns></returns>
		private SubShipRequestDto ConvertSubShipRequestDto(SubShip ship)
		{
			return new SubShipRequestDto()
			{
				Id = ship.PomId.ToString(),
				Name = ship.Name,
				TypeId = ship.TypeId.ToString(),
				ClassicId = ship.ClassicId.ToString(),
				CallSign = ship.CallSign,
				Mmsi = ship.Mmsi,
				Imo = ship.Imo,
				RegisterNumber = ship.RegisterNumber,
				FisrtRegisterNumber = ship.FisrtRegisterNumber,
				IdentNumber = ship.IdentNumber,
				RadioNumber = ship.RadioNumber,
				InspectType = ship.InspectType,
				NationType = ship.NationType,
				Belong = ship.Belong,
				RegistryPort = ship.RegistryPort,
				NavigateArea = ship.NavigateArea,
				ConstructionArea = ship.ConstructionArea,
				CompanyId = ship.CompanyId,
				CompanyName = ship.CompanyName,
				StatusId = ship.StatusId.ToString(),
				Designer = ship.Designer,
				Builder = ship.Builder,
				LengthOverall = ship.LengthOverall ?? 0,
				Breadth = ship.Breadth ?? 0,
				Depth = ship.Depth ?? 0,
				LoadedDraft = ship.LoadedDraft,
				LoadedDisplacement = ship.LoadedDisplacement,
				GrossTonnage = ship.GrossTonnage,
				NetTonnage = ship.NetTonnage,
				TotalPower = ship.TotalPower,
				Endurance = ship.Endurance,
				Height = ship.Height,
				FinishDate = ship.FinishDate,
				Remarks = ship.Remarks,
				CreatedBy = ship.CreateId.ToString(),
				CreatedAt = ship.CreateTime.Value,
				ShipOperationUnit = ship.BusinessUnit
			};
		}
		#endregion

		/// <summary>
		/// 获取所有变更的对象集合
		/// </summary>
		/// <returns></returns>
		private async Task<List<PushChangeEntityDto<TEntity>>> GetPushChangeModelsAsync<TEntity>(EntityType entityType, DateTime maxChangeTime) where TEntity : BaseEntity<Guid>, new()
		{
			return await _dbContext.Queryable<TEntity>().InnerJoin(_dbEntityChangeRecord.AsQueryable(), (a, b) => a.Id == b.ItemId)
				.Where((a, b) => b.Type == entityType && b.IsPush == false && b.ChangeTime <= maxChangeTime)
				.Select((a, b) => new PushChangeEntityDto<TEntity>() { Entity = a, RecordId = b.Id }).ToPageListAsync(1, 10000);
		}
		/// <summary>
		/// 推送项目月报特殊字段
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entityType"></param>
		/// <param name="maxChangeTime"></param>
		/// <param name="monthRepId">月报主键id</param>
		/// <returns></returns>
		private async Task<List<PushChangeEntityDto<TEntity>>> GetPushSpecialFieldsChangeModelsAsync<TEntity>(EntityType entityType, DateTime maxChangeTime, Guid monthRepId) where TEntity : BaseEntity<Guid>, new()
		{
			return await _dbContext.Queryable<TEntity>().InnerJoin(_dbEntityChangeRecord.AsQueryable(), (a, b) => a.Id == b.ItemId)
				.Where((a, b) => b.Type == entityType && b.IsPush == false && b.ChangeTime <= maxChangeTime && b.ItemId == monthRepId)
				.Select((a, b) => new PushChangeEntityDto<TEntity>() { Entity = a, RecordId = b.Id }).ToPageListAsync(1, 10000);
		}

		/// <summary>
		/// http推送到Pom系统
		/// </summary>
		/// <param name="option">配置项</param>
		/// <param name="requestParam">请求参数</param>
		/// <returns></returns>
		/// <exception cref="HttpRequestException">请求失败异常</exception>
		private async Task HttpPushPomAsync<TRequestParam>(PushToPomOption option, TRequestParam requestParam) where TRequestParam : class, new()
		{
			WebHelper webHelper = new WebHelper();
			webHelper.Headers.Add("token", option.Token);
			var httpResult = await webHelper.DoPostAsync<TRequestParam, HttpBaseResponseDto<string>>(option.Url, requestParam);
			if (httpResult.Code == 200)
			{
				if (!(httpResult.Result != null && httpResult.Result.Success && httpResult.Result.Code == 1))
				{
					throw new HttpRequestException(httpResult.Result == null ? "请求失败" : httpResult.Result.Message);
				}
			}
			else
			{
				throw new HttpRequestException(httpResult.Msg);
			}
		}

		/// <summary>
		/// 获取项目干系单位
		/// </summary>
		/// <param name="projectIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectOrg>> GetProjectOrgAsync(Guid[] projectIds)
		{
			return await _dbProjectOrg.AsQueryable().Where(x => projectIds.Contains(x.ProjectId.Value) && x.IsDelete == 1).ToListAsync();
		}
		/// <summary>
		/// 获取所属公司
		/// </summary>
		/// <param name="companyIds"></param>
		/// <returns></returns>
		public async Task<List<Institution>> GetProjectCompanyAsync(Guid?[] companyIds)
		{
			return await _dbInstitution.AsQueryable().Where(x => companyIds.Contains(x.PomId.Value) && x.IsDelete == 1)
				.Select(x => new Institution
				{
					PomId = x.PomId,
					Name = x.Name,
					Poid = x.Poid,
					Shortname = x.Shortname
				}).ToListAsync();
		}



		/// <summary>
		/// 获取项目状态
		/// </summary>
		/// <param name="statusIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectStatus>> GetProjectStatusAsync(Guid[] statusIds)
		{
			return await _dbProjectStatus.AsQueryable().Where(x => statusIds.Contains(x.StatusId) && x.IsDelete == 1)
				.Select(x => new ProjectStatus
				{
					StatusId = x.StatusId,
					Name = x.Name
				}).ToListAsync();
		}

		/// <summary>
		/// 获取项目规模
		/// </summary>
		/// <param name="gradIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectScale>> GetProjectGradAsync(Guid[] gradIds)
		{
			return await _dbProjectScale.AsQueryable().Where(x => gradIds.Contains(x.PomId) && x.IsDelete == 1)
				.Select(x => new ProjectScale
				{
					PomId = x.PomId,
					Name = x.Name
				}).ToListAsync();
		}

		/// <summary>
		/// 获取项目工况级数
		/// </summary>
		/// <param name="ConditiongGadeIds"></param>
		/// <returns></returns>
		public async Task<List<WaterCarriage>> GetProjectConditionGradeAsync(Guid[] ConditiongGadeIds)
		{
			return await _dbWaterCarriage.AsQueryable().Where(x => ConditiongGadeIds.Contains(x.PomId) && x.IsDelete == 1)
				.Select(x => new WaterCarriage
				{
					PomId = x.PomId,
					Grade = x.Grade,
					Remarks = x.Remarks
				}).ToListAsync();
		}

		/// <summary>
		/// 获取项目施工资质
		/// </summary>
		/// <param name="ConstructionQualificationIds"></param>
		/// <returns></returns>
		public async Task<List<ConstructionQualification>> GetProjectConstructionQualificationAsync(Guid[] ConstructionQualificationIds)
		{
			return await _dbConstructionQualification.AsQueryable().Where(x => ConstructionQualificationIds.Contains(x.PomId) && x.IsDelete == 1)
				.Select(x => new ConstructionQualification
				{
					PomId = x.PomId,
					Name = x.Name
				}).ToListAsync();
		}

		/// <summary>
		/// 获取项目施工区域
		/// </summary>
		/// <param name="regionIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectArea>> GetProjectRegionAsync(Guid[] regionIds)
		{
			return await _dbProjectArea.AsQueryable().Where(x => regionIds.Contains(x.AreaId) && x.IsDelete == 1)
				.Select(x => new ProjectArea
				{
					AreaId = x.AreaId,
					Name = x.Name
				}).ToListAsync();
		}
		/// <summary>
		/// 获取项目币种
		/// </summary>
		/// <param name="regionIds"></param>
		/// <returns></returns>
		public async Task<List<Currency>> GetProjectCurrencyAsync(Guid?[] currencyIds)
		{
			return await _dbCurrency.AsQueryable().Where(x => currencyIds.Contains(x.PomId) && x.IsDelete == 1)
				.Select(x => new Currency
				{
					PomId = x.PomId,
					Zcurrencyname = x.Zcurrencyname
				}).ToListAsync();
		}

		/// <summary>
		/// 获取项目相关进退场船舶
		/// </summary>
		/// <param name="projectIds"></param>
		/// <returns></returns>
		public async Task<List<ShipMovement>> GetShipMoventAsync(Guid[] projectIds)
		{
			return await _dbShipMovement.AsQueryable().Where(t => projectIds.Contains(t.ProjectId) && t.ShipType == ShipType.OwnerShip).ToListAsync();
		}
		/// <summary>
		///获取项目列表（部分字段（Id，PomId，Name,Rate））
		/// </summary>
		/// <param name="projectIds">项目id</param>
		/// <returns></returns>
		private async Task<List<Project>> GetProjectPartsAsync(Guid[] projectIds)
		{
			return await _dbProject.AsQueryable().Where(t => projectIds.Contains(t.Id) && t.IsDelete == 1).Select(t => new Project() { Id = t.Id, PomId = t.PomId, Name = t.Name, Rate = t.Rate }).ToListAsync();
		}

		/// <summary>
		/// 获取年度计划
		/// </summary>
		/// <returns></returns>
		private async Task<List<ProjectPlanProduction>> GetProjectAnnualPlanAsync(Guid[] projectIds, int year)
		{
			return await _dbProjectAnnualPlan.AsQueryable().Where(t => projectIds.Contains(t.ProjectId) && t.Year == year).ToListAsync();
		}

		/// <summary>
		/// 获取字典集合
		/// </summary>
		/// <returns></returns>
		private async Task<List<DictionaryTable>> GetDictionarysAsync(DictionaryTypeNo typeNo)
		{
			return await _dbDictionaryTable.GetListAsync(t => t.TypeNo == (int)typeNo && t.IsDelete == 1);
		}

		/// <summary>
		/// 获取船舶集合（部分字段（PomId, Name,ShipKindTypeId））
		/// </summary>
		/// <returns></returns>
		private async Task<List<ShipPartDto>> GetShipPartsAsync(Guid[] shipIds, ShipType shipType)
		{
			if (shipType == ShipType.OwnerShip)
			{
				return await _dbOwnerShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
			}
			else if (shipType == ShipType.SubShip)
			{
				return await _dbSubShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
			}
			else if (shipType == ShipType.SubBusinessUnit)
			{
				var dealingUnitIds = shipIds.Select(t => (Guid?)t).ToArray();
				return await _dbDealingUnit.AsQueryable().Where(t => dealingUnitIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.ZBPNAME_ZH }).ToListAsync();
			}
			return new List<ShipPartDto>();
		}

		/// <summary>
		/// 获取分包船舶
		/// </summary>
		/// <param name="shipIds"></param>
		/// <returns></returns>
		private async Task<List<SubShip>> GetSubShipAsync(Guid[] shipIds)
		{
			return await _dbSubShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).ToListAsync();
		}

		/// <summary>
		/// 获取船级社
		/// </summary>
		/// <param name="classicIds"></param>
		/// <returns></returns>
		private async Task<List<ShipClassic>> GetShipClassicAsync(Guid?[] classicIds)
		{
			return await _dbContext.Queryable<ShipClassic>().Where(t => classicIds.Contains(t.Id)).ToListAsync();
		}

		/// <summary>
		/// 获取用户部分信息（Id,PomId）
		/// </summary>
		/// <returns></returns>
		private async Task<List<Domain.Models.User>> GetUserPartAsync(Guid[] userIds)
		{
			return await _dbUser.AsQueryable().Where(t => t.IsDelete == 1 && userIds.Contains(t.Id)).Select(t => new Domain.Models.User() { Id = t.Id, PomId = t.PomId, Name = t.Name }).ToListAsync();
		}
		/// <summary>
		/// 获取项目干系人员
		/// </summary>
		/// <param name="projectIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectLeader>> GetProjectLeaderAsync(Guid[] projectIds)
		{
			return await _dbProjectLeader.AsQueryable().Where(x => projectIds.Contains(x.ProjectId.Value) && x.IsDelete == 1).ToListAsync();
		}
		/// <summary>
		/// 获取项目类型
		/// </summary>
		/// <param name="typeIds"></param>
		/// <returns></returns>
		public async Task<List<ProjectType>> GetProjectTypeAsync(Guid[] typeIds)
		{
			return await _dbProjectType.AsQueryable().Where(x => typeIds.Contains(x.PomId) && x.IsDelete == 1)
				.Select(x => new ProjectType
				{
					PomId = x.PomId,
					Name = x.Name
				}).ToListAsync();
		}
		/// <summary>
		/// 变更记录状态修改
		/// </summary>
		/// <returns></returns>
		private async Task ChangeRecordPushStatusAsync(List<EntityChangeRecord> records, DateTime maxChangeTime)
		{
			await _dbEntityChangeRecord.AsUpdateable(records).UpdateColumns(t => new { t.PushStatus, t.PushTime, t.IsPush, t.FailReason }).ExecuteCommandAsync();
		}
	}
}
