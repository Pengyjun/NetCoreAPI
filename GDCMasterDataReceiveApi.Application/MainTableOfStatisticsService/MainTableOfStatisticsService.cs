using Dm;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics;
using GDCMasterDataReceiveApi.Application.Contracts.IMainTableOfStatistics;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;
using System.Data;

namespace GDCMasterDataReceiveApi.Application.MainTableOfStatisticsService
{
    /// <summary>
    /// 统计数据库每日增改量 接口实现
    /// </summary>
    public class MainTableOfStatisticsService : IMainTableOfStatisticsService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="dbContext"></param>
        public MainTableOfStatisticsService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// 统计当前模式所有表（当前指定数据库） 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public ResponseAjaxResult<bool> InsertModifyHourIncrementalData(MainTableOfStatisticsRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            List<MainTableOfStatistics> insertMainTableOfStatisticsList = new List<MainTableOfStatistics>();
            List<MainTableOfStatistics> modifyMainTableOfStatisticsList = new List<MainTableOfStatistics>();
            List<MainTableOfStatisticsDetails> insertMainTableOfStatisticsDetailsList = new List<MainTableOfStatisticsDetails>();
            List<MainTableOfStatisticsDetails> modifyMainTableOfStatisticsDetailsList = new List<MainTableOfStatisticsDetails>();

            string connectionString = $"Server={requestDto.Server};User Id={requestDto.UserId};PWD={requestDto.Pwd};SCHEMA={requestDto.Schema};DATABASE={requestDto.DataBase}";
            using (var connection = new DmConnection(connectionString))//swagger端口8881
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    responseAjaxResult.FailResult(HttpStatusCode.LinkFail, "链接数据库失败" + ex.Message);
                    return responseAjaxResult;
                }
                var tables = GetTables(connection, requestDto.Schema, requestDto.ScreenTables);
                var hourNowDay = Convert.ToInt32(requestDto.Date.ToString("yyyyMMddHH"));
                var nowDay = Convert.ToInt32(requestDto.Date.ToString("yyyyMMdd"));
                var incrementalData = new List<DataTable>();
                //获取全表主表当日增量数据
                var tablesMainData = _dbContext.Queryable<MainTableOfStatistics>()
                    .Where(x => x.IsDelete == 1 && x.DateDay == nowDay)
                    .ToList();
                var tablesDetailsData = _dbContext.Queryable<MainTableOfStatisticsDetails>()
                    .Where(x => x.IsDelete == 1 && tablesMainData.Select(x => x.Id).Contains(x.MainTableOfStatisticsId))
                    .ToList();
                foreach (var table in tables)
                {
                    var data = GetTableIncrementalData(connection, requestDto.Schema, table, requestDto.Date);//链接表
                    if (data.Rows.Count > 0)//当前表存在数据
                    {
                        int insertNums = 0;//增量数
                        int modifyNums = 0;//修改数
                        foreach (DataRow row in data.Rows)
                        {
                            var objCTimeValue = row["createtime"];
                            var objMTimeValue = row["updatetime"];
                            if (!string.IsNullOrEmpty(objCTimeValue.ToString()) && Convert.ToDateTime(objCTimeValue) != DateTime.MinValue)
                            {
                                var hourInsertNowDay = Convert.ToInt32(Convert.ToDateTime(objCTimeValue).ToString("yyyyMMddHH"));
                                if (hourInsertNowDay == hourNowDay) { insertNums++; }
                            }
                            if (!string.IsNullOrEmpty(objMTimeValue.ToString()) && Convert.ToDateTime(objMTimeValue) != DateTime.MinValue)
                            {
                                var hourModifyNowDay = Convert.ToInt32(Convert.ToDateTime(objMTimeValue).ToString("yyyyMMddHH"));
                                if (hourModifyNowDay == hourNowDay) { modifyNums++; }
                            }
                        }
                        //增量主表数据
                        //主表id的雪花id 
                        long snowFlakeId = new long();
                        var isExist = tablesMainData.FirstOrDefault(x => x.HourOfTheDay == hourNowDay && x.TableName == table);
                        if (isExist == null)
                        {
                            //更新雪花id
                            snowFlakeId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                            insertMainTableOfStatisticsList.Add(new MainTableOfStatistics
                            {
                                Id = snowFlakeId,
                                DateDay = requestDto.Date.ToDateDay(),
                                HourOfTheDay = hourNowDay,
                                InsertNums = insertNums,
                                ModifyNums = modifyNums,
                                TableName = table,
                                CreateTime = DateTime.Now
                            });
                        }
                        else
                        {
                            //原本的雪花id 修改的
                            snowFlakeId = isExist.Id;
                            modifyMainTableOfStatisticsList.Add(new MainTableOfStatistics
                            {
                                Id = snowFlakeId,
                                DateDay = requestDto.Date.ToDateDay(),
                                HourOfTheDay = hourNowDay,
                                InsertNums = insertNums,
                                ModifyNums = modifyNums,
                                TableName = table,
                                CreateTime = DateTime.Now
                            });
                        }
                        //增量细表数据
                        foreach (DataRow row in data.Rows)
                        {
                            var objCTimeValue = row["createtime"];
                            var objMTimeValue = row["updatetime"];

                            //如果时间不是空 && 时间不是最小 datetime.minvalue
                            if (!string.IsNullOrEmpty(objCTimeValue.ToString()) && Convert.ToDateTime(objCTimeValue) != DateTime.MinValue)
                            {
                                //细表的主键雪花id
                                long detailsSnowId = new long();
                                var isExistDetails = tablesDetailsData.FirstOrDefault(x => x.TableId == row["id"].ToString() && x.MainTableOfStatisticsId == snowFlakeId && x.InsertOrModify == "insert");
                                if (isExistDetails == null)
                                {
                                    //新的雪花id
                                    detailsSnowId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                                    //新增增量的
                                    //转换为日期格式
                                    var hourInsertNowDay = Convert.ToInt32(Convert.ToDateTime(objCTimeValue).ToString("yyyyMMddHH"));
                                    if (hourInsertNowDay == hourNowDay)
                                    {
                                        insertMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                                        {
                                            Id = detailsSnowId,
                                            InsertOrModify = "insert",
                                            MainTableOfStatisticsId = snowFlakeId,
                                            TableId = row["id"].ToString(),//防止报错 这里tableid 取字符串类型 方便后续查询   snowid  guid 归为字符串类型,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                                else
                                {
                                    //主键不变
                                    detailsSnowId = isExistDetails.Id;
                                    //新增修改的
                                    //转换为日期格式
                                    var hourInsertNowDay = Convert.ToInt32(Convert.ToDateTime(objCTimeValue).ToString("yyyyMMddHH"));
                                    if (hourInsertNowDay == hourNowDay)
                                    {
                                        modifyMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                                        {
                                            Id = detailsSnowId,
                                            InsertOrModify = "insert",
                                            MainTableOfStatisticsId = snowFlakeId,
                                            TableId = row["id"].ToString(),//防止报错 这里tableid 取字符串类型 方便后续查询   snowid  guid 归为字符串类型,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(objMTimeValue.ToString()) && Convert.ToDateTime(objMTimeValue) != DateTime.MinValue)
                            {
                                //细表的主键雪花id
                                long detailsSnowId = new long();
                                var isExistDetails = tablesDetailsData.FirstOrDefault(x => x.TableId == row["id"].ToString() && x.MainTableOfStatisticsId == snowFlakeId && x.InsertOrModify == "modify");
                                if (isExistDetails == null)
                                {
                                    //新的雪花id
                                    detailsSnowId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                                    //新增改量的
                                    //转换为日期格式
                                    var hourModifyNowDay = Convert.ToInt32(Convert.ToDateTime(objMTimeValue).ToString("yyyyMMddHH"));
                                    if (hourModifyNowDay == hourNowDay)
                                    {
                                        insertMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                                        {
                                            Id = detailsSnowId,
                                            InsertOrModify = "modify",
                                            MainTableOfStatisticsId = snowFlakeId,
                                            TableId = row["id"].ToString(),//防止报错 这里tableid 取字符串类型 方便后续查询   snowid  guid 归为字符串类型,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                                else
                                {
                                    //主键不变
                                    detailsSnowId = isExistDetails.Id;
                                    //新增 修改的
                                    //转换为日期格式
                                    var hourModifyNowDay = Convert.ToInt32(Convert.ToDateTime(objMTimeValue).ToString("yyyyMMddHH"));
                                    if (hourModifyNowDay == hourNowDay)
                                    {
                                        modifyMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                                        {
                                            Id = detailsSnowId,
                                            InsertOrModify = "modify",
                                            MainTableOfStatisticsId = snowFlakeId,
                                            TableId = row["id"].ToString(),//防止报错 这里tableid 取字符串类型 方便后续查询   snowid  guid 归为字符串类型,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (insertMainTableOfStatisticsList.Count() > 0) { _dbContext.Fastest<MainTableOfStatistics>().PageSize(100000).BulkCopy(insertMainTableOfStatisticsList); }
            if (modifyMainTableOfStatisticsList.Count() > 0) { _dbContext.Updateable(modifyMainTableOfStatisticsList).UpdateColumns(x => new { x.InsertNums, x.ModifyNums, x.UpdateTime, x.Timestamp }).ExecuteCommand(); }
            if (insertMainTableOfStatisticsDetailsList.Count() > 0) { _dbContext.Fastest<MainTableOfStatisticsDetails>().PageSize(100000).BulkCopy(insertMainTableOfStatisticsDetailsList); }
            if (modifyMainTableOfStatisticsDetailsList.Count() > 0) { _dbContext.Updateable(modifyMainTableOfStatisticsDetailsList).UpdateColumns(x => new { x.UpdateTime, x.Timestamp }).ExecuteCommand(); }

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="schema"></param>
        /// <param name="screenTables">过滤掉的表</param>
        /// <returns></returns>
        private List<string> GetTables(DmConnection connection, string schema, List<string>? screenTables)
        {
            var tables = new List<string>();
            var query = $"SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = '{schema}'";
            using (var command = new DmCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //排除掉审计表&统计增量表&统计增量表详细
                    if (!reader.GetString(0).Contains("##")
                        && reader.GetString(0) != "t_auditlogs"
                        && reader.GetString(0) != "t_maintableofstatisticsdetails"
                        && reader.GetString(0) != "t_maintableofstatistics")
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
                if (screenTables != null && screenTables.Any())
                {
                    //过滤掉不需要统计的表
                    foreach (var table in screenTables)
                    {
                        tables = tables.Where(x => x != table).ToList();
                    }
                }
            }
            return tables;
        }
        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private DataTable GetTableIncrementalData(DmConnection connection, string schema, string table, DateTime date)
        {
            var query = $"SELECT * FROM {schema}.{table} WHERE CREATETIME LIKE :date";
            using (var command = new DmCommand(query, connection))
            {
                command.Parameters.Add(new DmParameter(":date", "%" + date.ToString("yyyy-MM-dd HH") + "%"));
                var adapter = new DmDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
