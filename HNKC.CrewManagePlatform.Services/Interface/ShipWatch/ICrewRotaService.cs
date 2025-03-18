using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;

namespace HNKC.CrewManagePlatform.Services.Interface.ShipWatch
{
    /// <summary>
    /// 值班服务接口
    /// </summary>
    public interface ICrewRotaService
    {
        /// <summary>
        /// 船舶排班
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveCrewRotaAsync(SaveSchedulingRequest requestBody);

        /// <summary>
        /// 船员船舶排班回显
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> CrewRotaListAsync(SchedulingRequest requestBody);

        /// <summary>
        /// 船舶排班用户列表
        /// </summary>
        /// <returns></returns>
        Task<Result> CrewRotaUserListAsync(Guid shipId);

        /// <summary>
        /// 值班管理列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchCrewRota>> SearchCrewRotaAsync(SearchCrewRotaRequest requestBody);

        /// <summary>
        /// 船舶值班查询
        /// </summary>
        /// <returns></returns>
        Task<Result> SearchShipDutyListAsync(BaseRequest request);
    }
}