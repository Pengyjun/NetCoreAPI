using HNKC.CrewManagePlatform.Models.CommonResult;
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
        /// 搜索机构数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Result> SearchInstitutionTreeAsync();
        /// <summary>
        /// 通过身份证与当前日期计算年龄
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        int CalculateAgeFromIdCard(string idCard);
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="deleteResonEnum"></param>
        /// <returns></returns>
        CrewStatusEnum ShipUserStatus(DateTime departureTime, CrewStatusEnum deleteResonEnum);
    }
}
