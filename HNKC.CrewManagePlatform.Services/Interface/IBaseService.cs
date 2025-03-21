using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Services.Interface
{

    /// <summary>
    /// 基本业务层
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Result> ReadExcelAsModelAsync(Stream stream);
        /// <summary>
        /// 搜索机构树
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Result> SearchInstitutionTreeAsync();
        /// <summary>
        /// 添加机构树
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<Result> AddInstitutionTreeAsync(AddInstutionRequestDto requestDto);
        /// <summary>
        /// 通过身份证与当前日期计算年龄
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        int CalculateAgeFromIdCard(string? idCard);
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="deleteResonEnum"></param>
        /// <param name="holidayTime"></param>
        /// <returns></returns>
        CrewStatusEnum ShipUserStatus(DateTime? departureTime, CrewStatusEnum deleteResonEnum, DateTime? holidayTime);
        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="uploadResponse"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<Result> InsertFileAsync(List<UploadResponse> uploadResponse, Guid? uId);
        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="uploadResponse"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<Result> UpdateFileAsync(List<UploadResponse> uploadResponse, Guid? uId);
        /// <summary>
        /// 当前角色类型
        /// </summary>
        /// <returns></returns>
        Task<int> CurRoleType();
    }
}
