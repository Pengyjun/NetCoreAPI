﻿using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestService : ITestService
    {
        /// <summary>
        /// 上下文
        /// </summary>
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="dbContext"></param>
        public TestService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DealingUnit>>> SearchDelineTest(BaseRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DealingUnit>>();
            RefAsync<int> total = 0;
            var data = await _dbContext.Queryable<DealingUnit>()
                .Where(x => x.IsDelete == 1)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageIndex, total);

            responseAjaxResult.SuccessResult(data);
            return responseAjaxResult;
        }
        /// <summary>
        /// 大数量新增测试
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddTestAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            var res = new DealingUnit()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
            };
            var res2 = new DealingUnit()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                CreateTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyyyyyy-MM-dd HH:mm:ss"))
            };
            var list = new List<DealingUnit>();
            list.Add(res);
            list.Add(res2);
            var user = new User()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
            };
            var user2 = new User()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                CreateTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyyyyyy-MM-dd HH:mm:ss"))
            };
            var userList = new List<User>();
            userList.Add(user);
            userList.Add(user2);
            //await _dbContext.Fastest<DealingUnit>().BulkCopyAsync(list);
            await _dbContext.Insertable(list).ExecuteCommandAsync();
            //await _dbContext.Fastest<User>().BulkCopyAsync(userList);
            await _dbContext.Insertable(userList).ExecuteCommandAsync();

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
       
    }
}
