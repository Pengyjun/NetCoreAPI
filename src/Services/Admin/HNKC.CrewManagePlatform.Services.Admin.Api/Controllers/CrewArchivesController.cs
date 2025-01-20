using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 船员档案控制器 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrewArchivesController : BaseController
    {
        private ICrewArchivesService _service;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public CrewArchivesController(ICrewArchivesService service)
        {
            this._service = service;
        }
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SearchCrewArchives")]
        public async Task<IActionResult> SearchCrewArchivesAsync([FromBody] SearchCrewArchivesRequest requestBody)
        {
            var data = await _service.SearchCrewArchivesAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 船员数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("CrewArchivesCount")]
        public async Task<Result> CrewArchivesCountAsync()
        {
            return await _service.CrewArchivesCountAsync();
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveUser")]
        [Transactional]
        public async Task<Result> SaveUserAsync([FromBody] CrewArchivesRequest requestBody)
        {
            return await _service.SaveUserAsync(requestBody);
        }
        /// <summary>
        /// 切换船员状态（删除/恢复）
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("ToggleUserStatus")]
        [Transactional]
        public async Task<Result> ToggleUserStatusAsync([FromBody] ToggleUserStatus requestBody)
        {
            return await _service.ToggleUserStatusAsync(requestBody);
        }
        /// <summary>
        /// 船员调任
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("CrewTransfer")]
        public async Task<Result> CrewTransferAsync(CrewTransferRequest requestBody)
        {
            return await _service.CrewTransferAsync(requestBody);
        }
        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveNotes")]
        public async Task<Result> SaveNotesAsync([FromBody] NotesRequest requestBody)
        {
            return await _service.SaveNotesAsync(requestBody);
        }

        #region 船员动态
        /// <summary>
        /// 船员动态列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchCrewDynamics")]
        public async Task<IActionResult> SearchCrewDynamicsAsync([FromQuery] CrewDynamicsRequest requestBody)
        {
            var data = await _service.SearchCrewDynamicsAsync(requestBody);
            return Ok(data);
        }

        #endregion
        #region 下拉列表
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetDropDownList")]
        public async Task<Result> GetDropDownListAsync([FromQuery] int type)
        {
            return await _service.GetDropDownListAsync(type);
        }
        #endregion

        #region 详情
        /// <summary>
        /// 获取基本详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("BasesicDetails")]
        public async Task<Result> GetBasesicDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetBasesicDetailsAsync(bId);
        }
        /// <summary>
        /// 获取劳务详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetLaborServicesDetails")]
        public async Task<Result> GetLaborServicesDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetLaborServicesDetailsAsync(bId);
        }
        /// <summary>
        /// 获取适任证书详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetCertificateOfCompetencyDetails")]
        public async Task<Result> GetCertificateOfCompetencyDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetCertificateOfCompetencyDetailsAsync(bId);
        }
        /// <summary>
        /// 获取学历详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetEducationalBackgroundDetails")]
        public async Task<Result> GetEducationalBackgroundDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetEducationalBackgroundDetailsAsync(bId);
        }
        /// <summary>
        /// 获取职务晋升详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetPromotionDetails")]
        public async Task<Result> GetPromotionDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetPromotionDetailsAsync(bId);
        }
        /// <summary>
        /// 获取任职船舶详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetWorkShipDetails")]
        public async Task<Result> GetWorkShipDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetWorkShipDetailsAsync(bId);
        }
        /// <summary>
        /// 获取培训记录详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetTrainingRecordDetails")]
        public async Task<Result> GetTrainingRecordDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetTrainingRecordDetailsAsync(bId);
        }
        /// <summary>
        /// 获取年度考核详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetYearCheckDetail")]
        public async Task<Result> GetYearCheckDetailAsync([FromQuery] string bId)
        {
            return await _service.GetYearCheckDetailAsync(bId);
        }
        /// <summary>
        /// 获取备注详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetNotesDetails")]
        public async Task<Result> GetNotesDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetNotesDetailsAsync(bId);

        }
        #endregion

        #region 文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("UploadFile")]
        public async Task<Result> UploadFileAsync(IFormFile file)
        {
            return await SingleFileUpdateAsync(file, "DefaultAllowFileType");
        }

        #endregion
    }
}
