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
        public async Task<IActionResult> CrewArchivesCountAsync()
        {
            var data = await _service.CrewArchivesCountAsync();
            return Ok(data);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveUser")]
        [Transactional]
        public async Task<IActionResult> SaveUserAsync([FromBody] CrewArchivesRequest requestBody)
        {
            var data = await _service.SaveUserAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 切换船员状态（删除/恢复）
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("ToggleUserStatus")]
        [Transactional]
        public async Task<IActionResult> ToggleUserStatusAsync([FromBody] ToggleUserStatus requestBody)
        {
            var data = await _service.ToggleUserStatusAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 船员调任
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("CrewTransfer")]
        public async Task<IActionResult> CrewTransferAsync(CrewTransferRequest requestBody)
        {
            var data = await _service.CrewTransferAsync(requestBody);
            return Ok(data);
        }
        #region 下拉列表
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownListAsync([FromQuery] int type)
        {
            var data = await _service.GetDropDownListAsync(type);
            return Ok(data);
        }
        #endregion
        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveNotes")]
        public async Task<IActionResult> SaveNotesAsync([FromBody] NotesRequest requestBody)
        {
            var data = await _service.SaveNotesAsync(requestBody);
            return Ok(data);
        }

        #region 详情
        /// <summary>
        /// 获取基本详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("BasesicDetails")]
        public async Task<IActionResult> GetBasesicDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetBasesicDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取劳务详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetLaborServicesDetails")]
        public async Task<IActionResult> GetLaborServicesDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetLaborServicesDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取适任证书详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetCertificateOfCompetencyDetails")]
        public async Task<IActionResult> GetCertificateOfCompetencyDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetCertificateOfCompetencyDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取学历详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetEducationalBackgroundDetails")]
        public async Task<IActionResult> GetEducationalBackgroundDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetEducationalBackgroundDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取职务晋升详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetPromotionDetails")]
        public async Task<IActionResult> GetPromotionDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetPromotionDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取任职船舶详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetWorkShipDetails")]
        public async Task<IActionResult> GetWorkShipDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetWorkShipDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取培训记录详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetTrainingRecordDetails")]
        public async Task<IActionResult> GetTrainingRecordDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetTrainingRecordDetailsAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取年度考核详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetYearCheckDetail")]
        public async Task<IActionResult> GetYearCheckDetailAsync([FromQuery] string bId)
        {
            var data = await _service.GetYearCheckDetailAsync(bId);
            return Ok(data);
        }
        /// <summary>
        /// 获取备注详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetNotesDetails")]
        public async Task<IActionResult> GetNotesDetailsAsync([FromQuery] string bId)
        {
            var data = await _service.GetNotesDetailsAsync(bId);
            return Ok(data);
        }
        #endregion

        #region 文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            var data = await SingleFileUpdateAsync(file, "DefaultAllowFileType");
            return Ok(data);
        }

        #endregion
    }
}
