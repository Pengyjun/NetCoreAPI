using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 对接DH相关数据 写入（后续定时跑）
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveDHDataController : BaseController
    {
        private readonly IReceiveDHDataService _receiveDHDataService;
        /// <summary>
        /// 
        /// </summary>
        public ReceiveDHDataController(IReceiveDHDataService receiveDHDataService)
        {
            this._receiveDHDataService = receiveDHDataService;
        }
        /// <summary>
        /// DH机构
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveDHOrganzation")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync()
        {
            return await _receiveDHDataService.ReceiveOrganzationAsync();
        }
        /// <summary>
        /// DH行政和核算机构映射
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveDHAdministrative")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveAdministrativeAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else
            {
                DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"), out datetime);
            }
            return await _receiveDHDataService.ReceiveAdministrativeAsync(datetime);
        }
        /// <summary>
        /// DH行政机构(多组织)
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveDHOrganzationDep")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationDepAsync()
        {
            return await _receiveDHDataService.ReceiveOrganzationDepAsync();
        }
        /// <summary>
        /// DH核算机构(多组织)
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveDHAdjustAccountsMultipleOrg")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveAdjustAccountsMultipleOrgAsync()
        {
            return await _receiveDHDataService.ReceiveAdjustAccountsMultipleOrgAsync();
        }
        /// <summary>
        /// DH核算部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReveiveDHAccountingDept")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReveiveAccountingDeptAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else {
                DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"), out datetime);
            }
            return await _receiveDHDataService.ReveiveAccountingDeptAsync(datetime);
        }
        /// <summary>
        /// DH项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReveiveDHProjects")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveProjectsAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else
            {
                 DateTime.TryParse("1900-01-01", out datetime);
            }
            return await _receiveDHDataService.ReceiveProjectsAsync(datetime,true);
        }
        /// <summary>
        /// DH虚拟项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReveiveDHVirtualProject")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveVirtualProjectAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else
            {
                DateTime.TryParse("1900-01-01", out datetime);
                // DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"), out datetime);
            }
            return await _receiveDHDataService.ReceiveVirtualProjectAsync(datetime,true);
        }
        /// <summary>
        /// DH商机项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReveiveDHOpportunity")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveOpportunityAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else
            {
                DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"), out datetime);
            }
            return await _receiveDHDataService.ReceiveOpportunityAsync(datetime,true);
        }
        /// <summary>
        /// DH科研项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReveiveDHResearch")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveResearchListAsync(string? dateTime)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var datetime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                var isOk = DateTime.TryParse(dateTime, out datetime);
                if (!isOk)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(message: "时间格式转换失败", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }
            else
            {
                DateTime.TryParse("1900-01-01", out datetime);
                // DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd"), out datetime);
            }
            return await _receiveDHDataService.ReceiveResearchListAsync(datetime,true);
        }
        /// <summary>
        /// DH生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveGetMdmManagementOrgage")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveGetMdmManagementOrgageListAsync()
        {
            return await _receiveDHDataService.ReceiveGetMdmManagementOrgageListAsync();
        }
        /// <summary>
        /// DH委托关系
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveGetDHMdmMultOrgAgencyRelPage")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ReceiveGetDHMdmMultOrgAgencyRelPageListAsync()
        {
            return await _receiveDHDataService.ReceiveGetDHMdmMultOrgAgencyRelPageListAsync();
        }
    }
}
