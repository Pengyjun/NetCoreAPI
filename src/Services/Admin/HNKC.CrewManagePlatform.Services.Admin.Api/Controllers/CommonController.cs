using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.Services.Interface.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 公共服务接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : BaseController
    {
        private ICommonService _commonService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="commonService"></param>
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("GetShipList")]
        public async Task<IActionResult> GetShipListAsync([FromQuery] OwnerShipDto dto)
        {
            var data = await _commonService.GetShipList(dto);
            return Ok(data);
        }

        /*/// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        public Result Test()
        {
            return Result.Success("操作成功");
        }*/
    }
}