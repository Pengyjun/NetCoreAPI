using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 船员档案控制器 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        public async Task<ResponsePageResult<List<SearchCrewArchivesResponse>>> SearchCrewArchivesAsync([FromBody] SearchCrewArchivesRequest requestBody)
        {
            return await _service.SearchCrewArchivesAsync(requestBody);
        }
        /// <summary>
        /// 船员数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("CrewArchivesCount")]
        public async Task<ResponseResult<CrewArchivesResponse>> CrewArchivesCountAsync()
        {
            return await _service.CrewArchivesCountAsync();
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveData")]
        public async Task<ResponseResult<bool>> SaveDataAsync([FromBody] CrewArchivesRequest requestBody)
        {
            return await _service.SaveDataAsync(requestBody);
        }

        #region 下拉列表
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetDropDownList")]
        public async Task<ResponseResult<List<DropDownResponse>>> GetDropDownListAsync([FromQuery] int type)
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
        public async Task<ResponseResult<BaseInfoDetails>> GetBasesicDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetBasesicDetailsAsync(bId);
        }
        /// <summary>
        /// 获取劳务详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetLaborServicesDetails")]
        public async Task<ResponseResult<LaborServicesInfoDetails>> GetLaborServicesDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetLaborServicesDetailsAsync(bId);
        }
        /// <summary>
        /// 获取适任证书详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetCertificateOfCompetencyDetails")]
        public async Task<ResponseResult<CertificateOfCompetencyDetails>> GetCertificateOfCompetencyDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetCertificateOfCompetencyDetailsAsync(bId);
        }
        /// <summary>
        /// 获取学历详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetEducationalBackgroundDetails")]
        public async Task<ResponseResult<EducationalBackgroundDetails>> GetEducationalBackgroundDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetEducationalBackgroundDetailsAsync(bId);
        }
        /// <summary>
        /// 获取职务晋升详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetPromotionDetails")]
        public async Task<ResponseResult<PromotionDetails>> GetPromotionDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetPromotionDetailsAsync(bId);
        }
        /// <summary>
        /// 获取任职船舶详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetWorkShipDetails")]
        public async Task<ResponseResult<WorkShipDetails>> GetWorkShipDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetWorkShipDetailsAsync(bId);
        }
        /// <summary>
        /// 获取培训记录详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetTrainingRecordDetails")]
        public async Task<ResponseResult<TrainingRecordDetails>> GetTrainingRecordDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetTrainingRecordDetailsAsync(bId);
        }
        /// <summary>
        /// 获取年度考核详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetYearCheckDetail")]
        public async Task<ResponseResult<YearCheckDetails>> GetYearCheckDetailAsync([FromQuery] string bId)
        {
            return await _service.GetYearCheckDetailAsync(bId);
        }
        /// <summary>
        /// 获取年度考核详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpGet("GetNotesDetails")]
        public async Task<ResponseResult<NotesDetails>> GetNotesDetailsAsync([FromQuery] string bId)
        {
            return await _service.GetNotesDetailsAsync(bId);
        }
        #endregion
    }
}
