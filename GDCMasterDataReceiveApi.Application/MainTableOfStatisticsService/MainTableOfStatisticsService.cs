﻿using Dm;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics;
using GDCMasterDataReceiveApi.Application.Contracts.IMainTableOfStatistics;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using MySqlConnector;
using SqlSugar;
using System.Data;
using System.Text;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

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
        #region 达梦链接方式
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
        #endregion

        #region Mysql链接方式
        /// <summary>
        /// 统计当前模式所有表（Mysql当前指定数据库） 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsertModifyHourIncrementalData(MainTableOfStatisticsMysqlRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            List<MainTableOfStatistics> insertMainTableOfStatisticsList = new List<MainTableOfStatistics>();
            List<MainTableOfStatistics> modifyMainTableOfStatisticsList = new List<MainTableOfStatistics>();
            List<MainTableOfStatisticsDetails> insertMainTableOfStatisticsDetailsList = new List<MainTableOfStatisticsDetails>();
            List<MainTableOfStatisticsDetails> modifyMainTableOfStatisticsDetailsList = new List<MainTableOfStatisticsDetails>();

            string connectionString = $"server={requestDto.Server};port={requestDto.Port};user={requestDto.User};password={requestDto.PassWord};database={requestDto.DataBase};sslMode=None;AllowLoadLocalInfile=true";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    responseAjaxResult.FailResult(HttpStatusCode.LinkFail, "链接失败：" + ex.Message);
                    return responseAjaxResult;
                }

                //获取当天主表所有数据
                var nowDay = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                var nowHour = Convert.ToInt32(DateTime.Now.ToString("yyyyMMddHH"));
                var oneHourGo = Convert.ToInt32(DateTime.Now.AddHours(-1).ToString("yyyyMMddHH"));
                var nowDayAllData = await _dbContext.Queryable<MainTableOfStatistics>()
                    .Where(x => x.IsDelete == 1 && x.DateDay == nowDay)
                    .ToListAsync();

                //获取当天细表所有数据
                var nowDayAllDetailsData = await _dbContext.Queryable<MainTableOfStatisticsDetails>()
                    .Where(x => x.IsDelete == 1 && nowDayAllData.Select(y => y.Id).Contains(x.MainTableOfStatisticsId))
                    .ToListAsync();

                //创建视图/修改视图
                if (requestDto.IsCreateView)
                {
                    bool success = await CreateModifyView(requestDto.ViewName, connectionString, requestDto.DataBase, requestDto.ScreenTables);
                    if (!success)
                    {
                        responseAjaxResult.FailResult(HttpStatusCode.ViewFail, "创建/修改视图失败");
                        return responseAjaxResult;
                    }
                }

                //获取视图数据 主表数据
                //获取当前表截止到现在位置的所有数量字典集合
                var tables = GetDicTables(connectionString, requestDto.ViewName);
                //获取视图数据 详细数据
                var tableDetails = GetDicTables(connectionString, $"{requestDto.ViewName}Details");
                // 遍历每个表，获取每日增量数据
                foreach (var table in tables)
                {
                    #region 统计写入主表数据
                    int nums = 0;
                    //获取前一小时的数据
                    var oneHourData = nowDayAllData.FirstOrDefault(x => x.TableName == table.TableName && x.HourOfTheDay == oneHourGo);
                    if (oneHourData == null)
                    {
                        //如果前一小时没有数据 获取当天最新的数据 并且!=此刻的时间
                        var nowDayFirst = nowDayAllData.OrderByDescending(x => x.HourOfTheDay).FirstOrDefault(x => x.TableName == table.TableName && x.HourOfTheDay != nowHour);
                        nums = nowDayFirst == null ? table.TableRows : table.TableRows - nowDayFirst.BeforeInsertNums < 0 ? 0 : table.TableRows - nowDayFirst.BeforeInsertNums;
                    }
                    else
                    {
                        nums = table.TableRows - oneHourData.BeforeInsertNums < 0 ? 0 : table.TableRows - oneHourData.BeforeInsertNums;
                    }
                    //新增统计当前最新的主表数据
                    //主表是否已经存在数据
                    var isExist = nowDayAllData.FirstOrDefault(x => x.TableName == table.TableName && x.HourOfTheDay == nowHour);
                    long snowFlakId = new long();
                    if (isExist != null)//数据修改
                    {
                        snowFlakId = isExist.Id;
                        isExist.InsertNums = nums;
                        isExist.UpdateTime = DateTime.Now;
                        isExist.BeforeInsertNums = table.TableRows;
                        isExist.Timestamp = Utils.GetTimeSpan();
                        modifyMainTableOfStatisticsList.Add(isExist);
                    }
                    else
                    {
                        snowFlakId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        MainTableOfStatistics mainTableOfStatistics = new MainTableOfStatistics()
                        {
                            Id = snowFlakId,
                            InsertNums = nums,
                            DateDay = nowDay,
                            HourOfTheDay = nowHour,
                            TableName = table.TableName,
                            CreateTime = DateTime.Now,
                            BeforeInsertNums = table.TableRows,
                            Timestamp = Utils.GetTimeSpan()
                        };
                        insertMainTableOfStatisticsList.Add(mainTableOfStatistics);
                    }
                    #endregion

                    #region 统计写入细表数据
                    //根据表查找详细数据
                    var details = tableDetails.Where(x => x.TableName == table.TableName).ToList();
                    foreach (var det in details)
                    {
                        //查询细表当天是否已经存在相同数据
                        var ssameDetails = nowDayAllDetailsData.Where(x => x.TableId == det.TableId && snowFlakId == x.MainTableOfStatisticsId).ToList();
                        foreach (var det2 in ssameDetails)
                        {
                            var sameDetails = nowDayAllDetailsData.FirstOrDefault(x => x.TableId == det2.TableId && x.InsertOrModify == "modify");
                            if (sameDetails != null)
                            {
                                //存在修改的数据
                                if (sameDetails.InsertOrModify == "modify")
                                {
                                    sameDetails.UpdateTime = DateTime.Now;
                                    sameDetails.Timestamp = Utils.GetTimeSpan();
                                    modifyMainTableOfStatisticsDetailsList.Add(sameDetails);
                                }
                            }
                            else
                            {
                                //新增修改的数据
                                insertMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                                {
                                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                    Timestamp = Utils.GetTimeSpan(),
                                    CreateTime = DateTime.Now,
                                    InsertOrModify = "modify",
                                    MainTableOfStatisticsId = snowFlakId,
                                    TableId = det2.TableId
                                });
                            }
                        }
                        if (!ssameDetails.Any())
                        {
                            insertMainTableOfStatisticsDetailsList.Add(new MainTableOfStatisticsDetails
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Timestamp = Utils.GetTimeSpan(),
                                CreateTime = DateTime.Now,
                                InsertOrModify = "insert",
                                MainTableOfStatisticsId = snowFlakId,
                                TableId = det.TableId
                            });
                        }
                    }
                    #endregion
                }
            }

            if (insertMainTableOfStatisticsList != null && insertMainTableOfStatisticsList.Count() > 0)
            {
                await _dbContext.Fastest<MainTableOfStatistics>().PageSize(100000).BulkCopyAsync(insertMainTableOfStatisticsList);
            }

            if (modifyMainTableOfStatisticsList != null && modifyMainTableOfStatisticsList.Count() > 0)
            {
                await _dbContext.Fastest<MainTableOfStatistics>().BulkUpdateAsync(modifyMainTableOfStatisticsList, new string[] { "id" }, new string[] { "insertnums", "beforeinsertnums", "modifynums", "updatetime", "timestamp" });
            }

            if (insertMainTableOfStatisticsDetailsList != null && insertMainTableOfStatisticsDetailsList.Count() > 0)
            {
                await _dbContext.Fastest<MainTableOfStatisticsDetails>().PageSize(100000).BulkCopyAsync(insertMainTableOfStatisticsDetailsList);
            }

            if (modifyMainTableOfStatisticsDetailsList != null && modifyMainTableOfStatisticsDetailsList.Count() > 0)
            {
                await _dbContext.Fastest<MainTableOfStatisticsDetails>().BulkUpdateAsync(modifyMainTableOfStatisticsDetailsList, new string[] { "id" }, new string[] { "updatetime", "timestamp" });
            }

            //修改库推送字段值
            await ModifyDataBaseColumForView(connectionString, requestDto.DataBase);

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取字典tables
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private static List<MainTableDictoryDto> GetDicTables(string connstring, string viewName)
        {
            var dic = new List<Dictionary<object, object>>();
            var column = viewName.Contains("Details") ? "id" : "tablerows";
            //获取视图数据
            string sql = $"select {column}, tablename from  {viewName}";
            using (var connection = new MySqlConnection(connstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var a = viewName.Contains("Details") ? reader.GetString(0) : reader.GetValue(0);
                        var b = reader.GetString(1);

                        var c = new Dictionary<object, object>();
                        c.Add(b, a);
                        dic.Add(c);
                    }
                }
            }

            var tables = new List<MainTableDictoryDto>();

            foreach (var item in dic)
            {
                var dicList = item.ToList();
                foreach (var d in dicList)
                {
                    tables.Add(new MainTableDictoryDto
                    {
                        TableName = d.Key.ToString(),
                        TableRows = viewName.Contains("Details") ? 0 : Convert.ToInt32(d.Value),
                        TableId = viewName.Contains("Details") ? d.Value.ToString() : null
                    });
                }
            }

            return tables;
        }
        /// <summary>
        /// 创建/更新视图
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="connString"></param>
        /// <param name="schema"></param>
        /// <param name="screenTables">过滤掉的表</param>
        /// <returns></returns>
        private static async Task<bool> CreateModifyView(string viewName, string connString, string schema, List<string>? screenTables)
        {
            #region 原来的
            var tables = new List<string>();
            var sql = new StringBuilder();
            var sql2 = new StringBuilder();
            sql.Append($"DROP VIEW IF EXISTS {viewName} ; create VIEW {viewName} as ");
            sql2.Append($"DROP VIEW IF EXISTS {viewName}Details ; create VIEW {viewName}Details as ");
            var query = $"SELECT table_name FROM information_schema.tables WHERE table_schema = '{schema}' AND table_type = 'BASE TABLE';";
            try
            {
                // 第一次连接并读取表名
                using (var connection = new MySqlConnection(connString))
                {
                    await connection.OpenAsync(); // 使用异步方法打开连接
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var reader = await command.ExecuteReaderAsync(); // 使用异步方法执行读取
                        while (await reader.ReadAsync()) // 使用异步方法读取数据
                        {
                            // 排除掉审计表&统计增量表&统计增量表详细
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
                            // 过滤掉不需要统计的表
                            foreach (var table in screenTables)
                            {
                                tables = tables.Where(x => x != table).ToList();
                            }
                        }
                    }
                }

                // 第二次连接并执行统计查询
                using (var connection = new MySqlConnection(connString))
                {
                    await connection.OpenAsync(); // 使用异步方法打开连接
                    foreach (var table in tables)
                    {
                        sql.Append($"select Count(*) as tablerows,'{table}' as 'tablename' from {schema}.{table} where push = 0 ");
                        sql2.Append($"select '{table}' as 'tablename',id as 'id' from {schema}.{table} where push = 0  ");
                        sql.Append($" union all ");
                        sql2.Append($" union all ");
                    }
                    sql = sql.Remove(sql.Length - 10, 10);
                    sql2 = sql2.Remove(sql2.Length - 10, 10);

                    using (var cmd = new MySqlCommand(sql.ToString(), connection))
                    {
                        await cmd.ExecuteNonQueryAsync(); // 使用异步方法执行非查询
                    }
                    using (var cmd = new MySqlCommand(sql2.ToString(), connection))
                    {
                        await cmd.ExecuteNonQueryAsync(); // 使用异步方法执行非查询
                    }
                }
            }
            catch (Exception ex)
            {
                // 考虑记录异常以便于调试
                throw;
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 插入数据后 最终修改库所有推送字段值
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private static async Task<bool> ModifyDataBaseColumForView(string connstring, string schema)
        {
            var query = $"SELECT table_name FROM information_schema.tables WHERE table_schema = '{schema}' AND table_type = 'BASE TABLE';";
            var sql = new StringBuilder();
            var tables = new List<string>();
            using (var connection = new MySqlConnection(connstring))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
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
                    foreach (var table in tables)
                    {
                        //sql.Append($"alter table {schema}.{table} add column push int default 1 ");//新增字段sql
                        // 使用分页查询来处理大数据量
                        sql.Append($"UPDATE `{schema}`.`{table}` SET push = 1 WHERE push = 0 AND EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = '{schema}' AND table_name = '{table}' AND column_name = 'push'); ");
                    }
                }
            }

            using (var connection = new MySqlConnection(connstring))
            {
                await connection.OpenAsync();
                using (var cmd = new MySqlCommand(sql.ToString(), connection))
                {
                    cmd.CommandTimeout = 600;
                    await cmd.ExecuteNonQueryAsync(); // 使用异步方法执行非查询
                }
            }
            return true;
        }
        #endregion
    }
}
