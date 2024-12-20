using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;

namespace HNKC.CrewManagePlatform.Services.Interface.CrewArchives
{
    /// <summary>
    /// 船员档案
    /// </summary>
    public interface ICrewArchivesService
    {
        /// <summary>
        /// 首页占比 及 数量 统计
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<CrewArchivesResponse>> CrewArchivesCountAsync();
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownResponse>>> GetDropDownListAsync(int type);
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> SaveDataAsync(CrewArchivesRequest requestBody);
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponsePageResult<List<SearchCrewArchivesResponse>>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody);
        /// <summary>
        /// 学历
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<EducationalBackgroundDetails>> GetEducationalBackgroundDetailsAsync(string bId);
        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<NotesDetails>> GetNotesDetailsAsync(string bId);
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<PromotionDetails>> GetPromotionDetailsAsync(string bId);
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<TrainingRecordDetails>> GetTrainingRecordDetailsAsync(string bId);
        /// <summary>
        /// 任职船舶
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<WorkShipDetails>> GetWorkShipDetailsAsync(string bId);
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<YearCheckDetails>> GetYearCheckDetailAsync(string bId);
        /// <summary>
        /// 适任证书
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<CertificateOfCompetencyDetails>> GetCertificateOfCompetencyDetailsAsync(string bId);
        /// <summary>
        /// 劳务
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<LaborServicesInfoDetails>> GetLaborServicesDetailsAsync(string bId);
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<ResponseResult<BaseInfoDetails>> GetBasesicDetailsAsync(string bId);
    }
}
