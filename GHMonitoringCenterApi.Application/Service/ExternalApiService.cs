using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.Shared;
using SqlSugar;

namespace GHMonitoringCenterApi.Application.Service
{
    /// <summary>
    /// 对外接口实现层
    /// </summary>
    public class ExternalApiService : IExternalApiService
    {
        /// <summary>
        /// 上下文注入
        /// </summary>
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        public ExternalApiService(ISqlSugarClient sqlSugarClient)
        {
            this._dbContext = sqlSugarClient;
        }
        /// <summary>
        /// s获取人员信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UserInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.User>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new UserInfos
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    DepartmentId = x.DepartmentId,
                    GroupCode = x.GroupCode,
                    IdentityCard = x.IdentityCard,
                    LoginAccount = x.LoginAccount,
                    LoginName = x.LoginName,
                    Name = x.Name,
                    Number = x.Number,
                    Phone = x.Phone,
                    PomId = x.PomId
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Data = data;
            responseAjaxResult.Success();

            return responseAjaxResult;
        }
        /// <summary>
        /// 获取机构信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InstutionInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.Institution>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new InstutionInfos
                {
                    Gpoid = x.Gpoid,
                    Grade = x.Grade,
                    Grule = x.Grule,
                    Id = x.Id,
                    Name = x.Name,
                    Ocode = x.Ocode,
                    Oid = x.Oid,
                    Orule = x.Orule,
                    Poid = x.Poid,
                    PomId = x.PomId,
                    Shortname = x.Shortname,
                    Sno = x.Sno,
                    Status = x.Status

                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Data = data;
            responseAjaxResult.Success();

            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.Project>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectInfos
                {
                    Administrator = x.Administrator,
                    Amount = x.Amount,
                    BidWinningDate = x.BidWinningDate,
                    BudgetaryReasons = x.BudgetaryReasons,
                    BudgetInterestRate = x.BudgetInterestRate,
                    Category = x.Category,
                    ClassifyStandard = x.ClassifyStandard,
                    Code = x.Code,
                    CommencementTime = x.CommencementTime,
                    CompanyId = x.CompanyId,
                    CompilationTime = x.CompilationTime,
                    CompleteOutput = x.CompleteOutput,
                    CompleteQuantity = x.CompleteQuantity,
                    CompletionDate = x.CompletionDate,
                    CompletionTime = x.CompletionTime,
                    Constructor = x.Constructor,
                    ContractChangeInfo = x.ContractChangeInfo,
                    ContractMeaPayProp = x.ContractMeaPayProp,
                    DurationInformation = x.DurationInformation,
                    ECAmount = x.ECAmount,
                    EndContractDuration = x.EndContractDuration,
                    Id = x.Id,
                    IsMajor = x.IsMajor,
                    IsStrength = x.IsStrength,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Name = x.Name,
                    PomId = x.PomId,
                    ProjectDept = x.ProjectDept,
                    ProjectDeptAddress = x.ProjectDeptAddress,
                    ProjectLocation = x.ProjectLocation,
                    Quantity = x.Quantity,
                    QuantityRemarks = x.QuantityRemarks,
                    ReclamationArea = x.ReclamationArea,
                    Remarks = x.Remarks,
                    ReportFormer = x.ReportFormer,
                    ReportForMertel = x.ReportForMertel,
                    ShortName = x.ShortName,
                    ShutdownDate = x.ShutdownDate,
                    ShutDownReason = x.ShutDownReason,
                    SocietySpecEffect = x.SocietySpecEffect,
                    StartContractDuration = x.StartContractDuration,
                    Tag = x.Tag,
                    Tag2 = x.Tag2
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Data = data;
            responseAjaxResult.Success();

            return responseAjaxResult;

        }
        /// <summary>
        /// 获取项目干系人列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectLeaderInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.ProjectLeader>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectLeaderInfos
                {
                    AssistantManagerId = x.AssistantManagerId,
                    BeginDate = x.BeginDate,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    IsPresent = x.IsPresent,
                    PomId = x.PomId,
                    ProjectId = x.ProjectId,
                    Remarks = x.Remarks,
                    Type = x.Type
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Data = data;
            responseAjaxResult.Success();

            return responseAjaxResult;

        }
    }
}
