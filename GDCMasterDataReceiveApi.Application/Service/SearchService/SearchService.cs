using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using Newtonsoft.Json;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 列表接口实现
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IDataAuthorityService _dataAuthorityService;
        /// <summary>
        /// 注入上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dataAuthorityService"></param>
        public SearchService(ISqlSugarClient dbContext, IDataAuthorityService dataAuthorityService)
        {
            this._dbContext = dbContext;
            this._dataAuthorityService = dataAuthorityService;
        }
        /// <summary>
        /// 读取动态列
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync(LouDongRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LouDongDto>>();

            //sql条件
            string sqlParam = "";
            //表名
            string tableName = "t_loudong";
            //sql语句
            string sql = "";
            if (!(requestDto.Columns != null && requestDto.Columns.Any())) sql = NativeSql.InitialSql + $@" {tableName} where isdelete=1 ";
            else
            {
                var dynamicColumns = string.Join(",", requestDto.Columns);
                sql = $@"select {dynamicColumns} from {tableName} where isdelete=1 ";
            }
            //拼接条件 获取表数据
            sql += !string.IsNullOrEmpty(sqlParam) ? $@"and @sqlParam={sqlParam}" : null;
            var dataTable = await _dbContext.Ado.GetDataTableAsync(sql);

            //序列化dataTable
            var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            //反序列化对象 
            var entitys = JsonConvert.DeserializeObject<List<LouDongDto>>(json);

            //分页
            var result = entitys.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();

            responseAjaxResult.Count = entitys.Count;
            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }

    }
}
