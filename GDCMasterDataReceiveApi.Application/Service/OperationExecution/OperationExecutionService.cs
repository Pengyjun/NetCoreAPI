using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.IService.OperationExecution;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.OperationExecution
{
    /// <summary>
    /// 增删改实现
    /// </summary>
    public class OperationExecutionService : IOperationExecutionService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        private static bool success = false;
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public OperationExecutionService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        /// <summary>
        /// 增改用户信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsertOrUpdateAsync(OperationExecutionRequestDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (requestDto.EntityJson != null)
            {
                await OpreateUserAsync(requestDto.EntityJson, requestDto.OperateType);
            }
            responseAjaxResult.SuccessResult(success);
            return responseAjaxResult;
        }
        /// <summary>
        /// 增改往来单位
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="type">1增 2改</param>
        /// <returns></returns>
        private async Task<ResponseAjaxResult<bool>> OpreateUserAsync(object obj, OperateType type)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (obj != null)
            {
                //数据转换
                var convertObj = JsonConvert.DeserializeObject<List<CorresUnitDetailsDto>>(obj.ToString());
                var map = _mapper.Map<List<CorresUnitDetailsDto>, List<CorresUnit>>(convertObj);
                if (type == OperateType.Insert)
                {
                    map.Select(x => x.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId());
                    map.Select(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Insertable(map).ExecuteCommandAsync();
                    responseAjaxResult.SuccessResult(true);
                }
                else if (type == OperateType.Update)
                {
                    //不是接收的数据
                    int isOwner = convertObj.Where(x => x.OwnerSystem == false).Count();
                    if (isOwner > 0)
                    {
                        responseAjaxResult.Fail("数据不可删除");
                        return responseAjaxResult;
                    }
                    else
                    {
                        if (map.Any())
                        {
                            await _dbContext.Updateable(map).WhereColumns(x => x.Id).IgnoreNullColumns(true).ExecuteCommandAsync();
                            responseAjaxResult.SuccessResult(true);
                        }
                    }
                }
            }
            else
            {
                responseAjaxResult.Fail();
            }
            return responseAjaxResult;
        }
    }
}
