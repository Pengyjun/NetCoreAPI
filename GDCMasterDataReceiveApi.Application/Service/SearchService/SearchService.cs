using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 列表接口实现
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IBaseService _baseService;
        private readonly IMapper _mapper;
        private readonly IDataAuthorityService _dataAuthorityService;
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="dataAuthorityService"></param>
        /// <param name="baseService"></param>
        public SearchService(ISqlSugarClient dbContext, IBaseService baseService, IMapper mapper, IDataAuthorityService dataAuthorityService)
        {
            this._dbContext = dbContext;
            this._baseService = baseService;
            this._dataAuthorityService = dataAuthorityService;
            this._mapper = mapper;
        }
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync(LouDongRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LouDongDto>>();

            //读取数据
            var result = await _baseService.GetSearchListAsync<LouDongDto>("t_loudong", requestDto.Sql, requestDto.FilterParams, true, requestDto.PageIndex, requestDto.PageSize);

            // 设置响应结果的总数
            responseAjaxResult.Count = result.Count;
            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 增改楼栋
        /// </summary>
        /// <param name="receiveDtos"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddOrModifyLouDongAsync(List<LouDongReceiveDto> receiveDtos)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var afterAddMapper = new List<LouDongReceiveDto>();

            var add = new LouDongReceiveDto
            {
                ZBLDG = "1",
                ZBLDG_NAME = "2",
                ZFORMATINF = "3",
                ZPROJECT = "4",
                ZSTATE = "5",
                ZSYSTEM = "6",
                ZZSERIAL = "7"
            };
            afterAddMapper.Add(add);
            var addMap = _mapper.Map<List<LouDongReceiveDto>, List<Domain.Models.LouDong>>(afterAddMapper);

            foreach (var item in addMap)
            {
                item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            }

            await _dbContext.Insertable(addMap).ExecuteCommandAsync();

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }

    }
}
