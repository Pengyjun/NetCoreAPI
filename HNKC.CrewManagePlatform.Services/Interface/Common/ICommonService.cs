
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement;

namespace HNKC.CrewManagePlatform.Services.Interface.Common
{
    /// <summary>
    /// 公共服务业务接口
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<OwnerShipVo>> GetShipList(OwnerShipDto dto);
    }
}
