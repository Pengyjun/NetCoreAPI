using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.IService.OperationExecution;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
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
        public async Task<ResponseAjaxResult<bool>> SaveDataAsync(OperationExecutionRequestDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (requestDto.Table == 3)
            {
                responseAjaxResult = await OpreateCorresUnitAsync(requestDto.EntityJson, requestDto.OperateType);
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 增改往来单位
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="type">1增 2改</param>
        /// <returns></returns>
        private async Task<ResponseAjaxResult<bool>> OpreateCorresUnitAsync(string obj, int type)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (obj != null)
            {
                //数据转换
                var convertObj = JsonConvert.DeserializeObject<CorresUnitDetailsDto>(obj);
                var map = _mapper.Map<CorresUnitDetailsDto, CorresUnit>(convertObj);
                //是否重复数据
                var dt = await _dbContext.Queryable<CorresUnit>()
                    .Where(t => t.IsDelete == 1 && t.ZUSCC == map.ZUSCC && t.ZBP == map.ZBP)
                    .FirstAsync();
                if (type == 1)
                {
                    if (dt != null)
                    {
                        responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_EXIST, HttpStatusCode.DataEXIST);
                        return responseAjaxResult;
                    }
                    map.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    map.CreateTime = DateTime.Now;
                    map.ZBPSTATE = "01";
                    await _dbContext.Insertable(map).ExecuteCommandAsync();
                    responseAjaxResult.SuccessResult(true);
                }
                if (map.OwnerSystem == false)
                {
                    //不是接收的数据
                    responseAjaxResult.Success(ResponseMessage.OPERATION_PROHIBIT, HttpStatusCode.Data_Prohibit);
                    return responseAjaxResult;
                }
                else
                {
                    if (type == 2)
                    {
                        if (map != null)
                        {
                            await _dbContext.Updateable(map).WhereColumns(x => x.Id).IgnoreNullColumns(true).ExecuteCommandAsync();
                        }
                    }
                    else if (type == 3)
                    {
                        if (map != null)
                        {
                            await _dbContext.Updateable(map).WhereColumns(x => x.Id).UpdateColumns(x => x.IsDelete == 1).ExecuteCommandAsync();
                        }
                    }
                    responseAjaxResult.SuccessResult(true);
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
