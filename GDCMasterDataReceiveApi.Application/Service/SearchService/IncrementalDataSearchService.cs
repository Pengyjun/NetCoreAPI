using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 统计数据列表实现
    /// </summary>
    public class IncrementalDataSearchService : IIncrementalDataSearchService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public IncrementalDataSearchService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 增量数据
        /// </summary>
        /// <param name="tableNames">需要获取增量数据的表</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> OperateDataAsync(List<string>? tableNames)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //获取当天日期
            var dateday = DateTime.Now.ToDateDay();

            //主表增改 查询当天数据  
            List<IncrementalData> insertT = new();
            List<IncrementalData> updateT = new();
            var dayData = await _dbContext.Queryable<IncrementalData>().Where(t => t.IsDelete == 1 && t.DateDay == dateday).ToListAsync();

            //细表 查询当天数据  
            List<IncrementalDetailsData> insertDt = new();
            List<IncrementalDetailsData> deleteDt = new();
            var dayDetailsData = await _dbContext.Queryable<IncrementalDetailsData>().Where(t => t.DateDay == dateday).ToListAsync();

            //需要读取的tables
            tableNames = tableNames != null && tableNames.Any() && tableNames.Where(x => x.Contains("t_")).ToList().Count != 0 ? tableNames.Where(x => x.Contains("t_")).ToList() : new List<string>() { "t_user", "t_institution" };

            //获取查询到的数据
            var selectList = await GetTableTsAsync(tableNames, dateday);

            foreach (var writeDatas in selectList)
            {
                var insertList = JsonConvert.DeserializeObject<List<IncrementalData>>(writeDatas.InsertData);
                var updateList = JsonConvert.DeserializeObject<List<IncrementalData>>(writeDatas.UpdateData);
                var insertNums = insertList == null ? 0 : insertList.Count;//新增数
                var updateNums = updateList == null ? 0 : updateList.Count;//修改数
                string tableName = writeDatas.TableName;//表名

                //主表
                var tData = dayData.FirstOrDefault(x => x.TableName == writeDatas.TableName);
                if (tData != null)
                {
                    //修改
                    tData.InsertNums = insertNums;
                    tData.UpdateNums = updateNums;
                    tData.TableName = tableName;
                    updateT.Add(tData);
                }
                else
                {
                    insertT.Add(new IncrementalData
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        DateDay = dateday,
                        DateMonth = Convert.ToInt32(dateday.ToString().Substring(0, 6)),
                        InsertNums = insertNums,
                        UpdateNums = updateNums,
                        TableName = tableName
                    });
                }

                //细表
                insertList.AddRange(updateList);
                foreach (var dt in insertList)
                {
                    var insert = new IncrementalDetailsData { Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(), RelationTableId = dt.Id, TableName = tableName, DateDay = dateday, DateMonth = Convert.ToInt32(dateday.ToString().Substring(0, 6)) };
                    insertDt.Add(insert);
                }

                deleteDt.AddRange(dayDetailsData.Where(x => x.TableName == tableName));
            }
            if (insertT != null && insertT.Any()) await _dbContext.Insertable(insertT).ExecuteCommandAsync();
            if (updateT != null && updateT.Any()) await _dbContext.Updateable(updateT).ExecuteCommandAsync();
            if (deleteDt != null && deleteDt.Any()) await _dbContext.Deleteable(deleteDt).WhereColumns(deleteDt, x => x.Id).ExecuteCommandAsync();
            if (insertDt != null && insertDt.Any()) await _dbContext.Insertable(insertDt).ExecuteCommandAsync();

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }

        /// <summary>
        /// 统计表增量数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        private async Task<List<TableDataResult>> GetTableTsAsync(List<string>? tableNames, int dateDay)
        {
            var result = new List<TableDataResult>();

            if (tableNames != null && tableNames.Any())
            {
                foreach (var tableName in tableNames)
                {
                    Utils.TryConvertDateTimeFromDateDay(dateDay, out DateTime dateTime);
                    //var dateTime = Convert.ToDateTime("2024-08-09");

                    // 使用 ADO.NET 动态查询
                    var insertData = await _dbContext.Ado.SqlQueryAsync<object>(
                        $"SELECT * FROM {tableName} WHERE CAST(createtime AS DATE) = CAST(@dateTime AS DATE)",
                        new { dateTime });

                    var updateData = await _dbContext.Ado.SqlQueryAsync<object>(
                        $"SELECT * FROM {tableName} WHERE CAST(updatetime AS DATE) = CAST(@dateTime AS DATE)",
                        new { dateTime });

                    // 分别添加到结果对象
                    TableDataResult r = new();
                    r.InsertData = insertData.ToJson();
                    r.UpdateData = updateData.ToJson();
                    r.TableName = tableName;
                    result.Add(r);
                }
            }
            else
            {
                throw new ArgumentException("表名为空");
            }

            return result;
        }
        /// <summary>
        /// 返回表数据
        /// </summary>
        private class TableDataResult
        {
            public string InsertData { get; set; } = string.Empty;
            public string UpdateData { get; set; } = string.Empty;
            public string TableName { get; set; } = string.Empty;
        }

    }
}
