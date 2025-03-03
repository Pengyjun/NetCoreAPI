using Autofac;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Application.Contracts.IService.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
using GHMonitoringCenterApi.Application.Contracts.IService.Menu;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectMasterData;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Application.Contracts.IService.RecordPushDayReport;
using GHMonitoringCenterApi.Application.Contracts.IService.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.Role;
using GHMonitoringCenterApi.Application.Contracts.IService.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipSurvey;
using GHMonitoringCenterApi.Application.Contracts.IService.SystemUpdatLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Timing;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Application.Contracts.IService.Word;
using GHMonitoringCenterApi.Application.Service;
using GHMonitoringCenterApi.Application.Service.Authorize;
using GHMonitoringCenterApi.Application.Service.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Service.ConstructionLog;
using GHMonitoringCenterApi.Application.Service.EquipmentManagement;
using GHMonitoringCenterApi.Application.Service.File;
using GHMonitoringCenterApi.Application.Service.HelpCenter;
using GHMonitoringCenterApi.Application.Service.JjtSendMessage;
using GHMonitoringCenterApi.Application.Service.Job;
using GHMonitoringCenterApi.Application.Service.Menu;
using GHMonitoringCenterApi.Application.Service.OperationLog;
using GHMonitoringCenterApi.Application.Service.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Service.ProductionValueImport;
using GHMonitoringCenterApi.Application.Service.ProjectMasterData;
using GHMonitoringCenterApi.Application.Service.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Service.Projects;
using GHMonitoringCenterApi.Application.Service.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Service.Push;
using GHMonitoringCenterApi.Application.Service.RecordPushDayReportNew;
using GHMonitoringCenterApi.Application.Service.RepairParts;
using GHMonitoringCenterApi.Application.Service.ResourceManagement;
using GHMonitoringCenterApi.Application.Service.Role;
using GHMonitoringCenterApi.Application.Service.Ship;
using GHMonitoringCenterApi.Application.Service.ShipPlan;
using GHMonitoringCenterApi.Application.Service.ShipSurvey;
using GHMonitoringCenterApi.Application.Service.SystemUpdatLog;
using GHMonitoringCenterApi.Application.Service.Timing;
using GHMonitoringCenterApi.Application.Service.User;
using GHMonitoringCenterApi.Application.Service.Word;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.SqlSugarCore;

namespace GHMonitoringCenterApi.Ioc
{
    /// <summary>
    /// AutoFac初始化
    /// </summary>
    public class AutoFacInit
    {
        /// <summary>
        /// 初始化IoC容器并注册所有的接口
        /// </summary>
        /// <param name="builder"></param>
        public static void Init(ContainerBuilder builder)
        {
            //注入工作单元
            builder.RegisterType<SqlSugarUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            //注入基本仓储
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));


            builder.RegisterType<TimeService>().As<ITimeService>().InstancePerLifetimeScope();

            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();

            builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();

            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();

            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectReportService>().As<IProjectReportService>().InstancePerLifetimeScope();

            builder.RegisterType<SystemUpdatLogService>().As<ISystemUpdatLogService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectDepartMentService>().As<IProjectDepartMentService>().InstancePerLifetimeScope();

            builder.RegisterType<EntityChangeService>().As<IEntityChangeService>().InstancePerLifetimeScope();

            builder.RegisterType<HelpCenterService>().As<IHelpCenterService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectMasterDataService>().As<IProjectMasterDataService>().InstancePerLifetimeScope();

            builder.RegisterType<PersonnelOrganizeAdjustService>().As<IPersonnelOrganizeAdjustService>().InstancePerLifetimeScope();

            builder.RegisterType<ConstructionLogService>().As<IConstructionLogService>().InstancePerLifetimeScope();

            builder.RegisterType<JobService>().As<IJobService>().InstancePerLifetimeScope();

            builder.RegisterType<JjtSendMessageService>().As<IJjtSendMessageService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectShipMovementsService>().As<IProjectShipMovementsService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectProductionReportService>().As<IProjectProductionReportService>().InstancePerLifetimeScope();

            builder.RegisterType<ResourceManagementService>().As<IResourceManagementService>().InstancePerLifetimeScope();

            builder.RegisterType<PushPomService>().As<IPushPomService>().InstancePerLifetimeScope();

            builder.RegisterType<WordService>().As<IWordService>().InstancePerLifetimeScope();

            builder.RegisterType<EquipmentManagementService>().As<IEquipmentManagementService>().InstancePerLifetimeScope();

            builder.RegisterType<ShipService>().As<IShipService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectApproverService>().As<IProjectApproverService>().InstancePerLifetimeScope();
            builder.RegisterType<RepairPartsService>().As<IRepairPartsService>().InstancePerLifetimeScope();
            builder.RegisterType<BizAuthorizeService>().As<IBizAuthorizeService>().InstancePerLifetimeScope();
            builder.RegisterType<ShipSurveyService>().As<IShipSurveyService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectYearPlanService>().As<IProjectYearPlanService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductionValueImportService>().As<IProductionValueImportService>().InstancePerLifetimeScope();

            builder.RegisterType<ExternalApiService>().As<IExternalApiService>().InstancePerLifetimeScope();
            builder.RegisterType<MonthReportForProjectService>().As<IMonthReportForProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<ShipPlanService>().As<IShipPlanService>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyProductionValueInfoService>().As<ICompanyProductionValueInfoService>().InstancePerLifetimeScope();

            builder.RegisterType<RecordPushDayReportService>().As<IRecordPushDayReportService>().InstancePerLifetimeScope();
        }
    }
}
