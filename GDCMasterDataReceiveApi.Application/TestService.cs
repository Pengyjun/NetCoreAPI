using Dm;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;
using System.Data;
using UtilsSharp;

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
                Id = SnowflakeAlgorithmUtil.GenerateSnowflakeId(),
            };
            var res2 = new DealingUnit()
            {
                Id = SnowflakeAlgorithmUtil.GenerateSnowflakeId(),
                CreateTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyyyyyy-MM-dd HH:mm:ss"))
            };
            var list = new List<DealingUnit>();
            list.Add(res);
            list.Add(res2);
            var user = new User()
            {
                Id = SnowflakeAlgorithmUtil.GenerateSnowflakeId(),
            };
            var user2 = new User()
            {
                Id = SnowflakeAlgorithmUtil.GenerateSnowflakeId(),
                CreateTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyyyyyy-MM-dd HH:mm:ss"))
            };
            var userList = new List<User>();
            userList.Add(user);
            userList.Add(user2);
            await _dbContext.Fastest<DealingUnit>().BulkCopyAsync(list);
            await _dbContext.Fastest<User>().BulkCopyAsync(userList);

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 统计当前模式所有表（当前指定数据库） 
        /// </summary>
        /// <param name="schema">当前指定数据库</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseAjaxResult<bool> GetDailyIncrementalData(string schema, DateTime date)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var incrementalData = new List<DataTable>();

            using (var connection = new DmConnection(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString")))
            {
                connection.Open();
                var tables = GetTables(connection, schema);

                foreach (var table in tables)
                {
                    var data = GetTableIncrementalData(connection, schema, table, date);
                    if (data.Rows.Count > 0)
                    {
                        incrementalData.Add(data);
                    }
                }
                //关闭链接
            }
            //业务逻辑处理 插入数据变化表
            foreach (var data in incrementalData)
            {
                //var a=data.Rows.
            }
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private List<string> GetTables(DmConnection connection, string schema)
        {
            var tables = new List<string>();
            var query = $"SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = '{schema}'";
            using (var command = new DmCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.GetString(0).Contains("##") && reader.GetString(0) != "t_auditlogs")
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
            return tables;
        }
        /// <summary>
        /// 获取表数量
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
                command.Parameters.Add(new DmParameter(":date", "%" + date.ToString("yyyy-MM-dd HH:mm") + "%"));
                var adapter = new DmDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
