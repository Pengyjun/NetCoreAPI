using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public IBaseService baseService { get; set; }
        public ITestService _testService { get; set; }
        public TestController(IBaseService baseService, ITestService testService)
        {
            this.baseService = baseService;
            this._testService = testService;
        }

        [HttpGet]
        public GlobalCurrentUser Test()
        {
            var a = SnowflakeAlgorithmUtil.GenerateSnowflakeId();
            return null;
        }
        [HttpGet("SearchDelineTest")]
        public async Task<ResponseAjaxResult<List<DealingUnit>>> SearchDelineTestAsync([FromQuery] BaseRequestDto requestDto)
            => await _testService.SearchDelineTest(requestDto);
    }
}
