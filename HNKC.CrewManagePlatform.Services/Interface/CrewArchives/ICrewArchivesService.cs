using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;

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
        Task<Result> CrewArchivesCountAsync();
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Result> GetDropDownListAsync(int type);
        /// <summary>
        /// 用户保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveUserAsync(CrewArchivesRequest requestBody);
        /// <summary>
        /// 切换用户状态
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> ToggleUserStatusAsync(ToggleUserStatus requestBody);
        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveNotesAsync(NotesRequest requestBody);
        /// <summary>
        /// 船员调任
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> CrewTransferAsync(CrewTransferRequest requestBody);
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchCrewArchivesResponse>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody);
        /// <summary>
        /// 学历
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetEducationalBackgroundDetailsAsync(string bId);
        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="bId"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        Task<Result> GetNotesDetailsAsync(string bId,string? keyWords);
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetPromotionDetailsAsync(string bId);
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetTrainingRecordDetailsAsync(string bId);
        /// <summary>
        /// 任职船舶
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetWorkShipDetailsAsync(string bId);
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetYearCheckDetailAsync(string bId);
        /// <summary>
        /// 适任证书
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetCertificateOfCompetencyDetailsAsync(string bId, CertificatesEnum type);
        /// <summary>
        /// 劳务
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetLaborServicesDetailsAsync(string bId);
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        Task<Result> GetBasesicDetailsAsync(string bId);
        /// <summary>
        /// 船员动态
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchCrewDynamics>> SearchCrewDynamicsAsync(CrewDynamicsRequest requestBody);
    }
}
