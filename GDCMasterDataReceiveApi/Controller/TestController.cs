using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        /// <summary>
        /// 列表查询测试
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchDelineTest")]
        public async Task<ResponseAjaxResult<List<DealingUnit>>> SearchDelineTestAsync([FromQuery] BaseRequestDto requestDto)
            => await _testService.SearchDelineTest(requestDto);
        /// <summary>
        /// 大数据插入测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddTest")]
        public async Task<ResponseAjaxResult<bool>> AddTestAsync()
            => await _testService.AddTestAsync();
        /// <summary>
        /// 统计数据库所有表每天的增量数据
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetDailyIncrementalData")]
        public ResponseAjaxResult<bool> GetDailyIncrementalData([FromQuery] string schema, [FromQuery] DateTime date)
            => _testService.GetDailyIncrementalData(schema, date);
    }
}
