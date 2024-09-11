using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using Newtonsoft.Json;
using SqlSugar;
using System.Data;
using System.Reflection;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application
{
    /// <summary>
    /// 基本接口层实现类
    /// </summary>
    public class BaseService : CurrentUser, IBaseService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 上下文
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="sqlColumns">查询的sql列</param>
        /// <param name="filterConditions">条件</param>
        /// <param name="IsPaging">是否需要分页</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数量</param>
        /// <returns></returns>
        public async Task<List<T>> GetSearchListAsync<T>(string tableName, string sqlColumns, List<string>? filterConditions, bool IsPaging, int pageIndex, int pageSize) where T : class, new()
        {
            //无显示列 返回空数组
            if (string.IsNullOrWhiteSpace(sqlColumns)) return new List<T>();

            // 初始化条件列表、参数字典
            var di = new Dictionary<string, object>();

            //var filterConditions = FilterConditions(encryptedText);

            // 构建 SQL 查询的 WHERE 子句
            var whereClause = filterConditions != null && filterConditions.Any() ? " WHERE " + string.Join(" ", filterConditions) : string.Empty;

            var sql = sqlColumns = "SELECT " + sqlColumns + $" FROM {tableName} {whereClause};";

            // 异步执行查询并获取结果
            var dataTable = await _dbContext.Ado.GetDataTableAsync(sql);

            // 如果有加密列
            var encryptions = await _dbContext.Queryable<ColumnsEncryption>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new Encryption { Key = x.Column, Value = x.Encrytion })
                .ToListAsync();
            if (encryptions != null && encryptions.Any())
            {
                // 遍历加密列表
                foreach (var encryption in encryptions)
                {
                    var column = encryption.Key;
                    var encryptedValue = encryption.Value;

                    // 检查列是否存在于 DataTable
                    if (column != null && dataTable.Columns.Contains(column))
                    {
                        // 对数据表进行加密处理
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (row[column] != DBNull.Value)
                            {
                                var value = row[column].ToString();
                                var encryptedColumnValue = encryptedValue;
                                row[column] = encryptedColumnValue;
                            }
                        }
                    }
                }
            }

            // 将结果转换为 JSON 字符串
            var json = dataTable.ToJson();

            // 反序列化 JSON 字符串为对象列表
            var result = JsonConvert.DeserializeObject<List<T>>(json);

            //是否需要分页
            var pRes = IsPaging ? result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList() : result;

            return pRes;
        }
        /// <summary>
        /// 默认加载获取条件参数
        /// </summary>
        /// <returns></returns>
        public ResponseAjaxResult<List<FilterParams>> GetFilterParams()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<FilterParams>>();

            var val = new List<FilterParams>();
            val.Add(new FilterParams { Key = "等于", Value = "=" });
            val.Add(new FilterParams { Key = "不等于", Value = "!=" });
            val.Add(new FilterParams { Key = "小于", Value = "<" });
            val.Add(new FilterParams { Key = "小于等于", Value = "<=" });
            val.Add(new FilterParams { Key = "大于", Value = ">" });
            val.Add(new FilterParams { Key = "大于等于", Value = ">=" });
            val.Add(new FilterParams { Key = "包含", Value = "LIKE %param%" });
            val.Add(new FilterParams { Key = "不包含", Value = "NOT LIKE %param%" });
            val.Add(new FilterParams { Key = "开头为", Value = "LIKE param%" });
            val.Add(new FilterParams { Key = "开头不为", Value = "NOT LIKE param%" });
            val.Add(new FilterParams { Key = "结尾为", Value = "LIKE %param" });
            val.Add(new FilterParams { Key = "结尾不为", Value = "NOT LIKE %param" });
            val.Add(new FilterParams { Key = "为空", Value = "IS NULL" });
            val.Add(new FilterParams { Key = "非空", Value = "IS NOT NULL" });
            val.Add(new FilterParams { Key = "空字符", Value = "=''" });
            val.Add(new FilterParams { Key = "非空字符串", Value = "!=''" });
            val.Add(new FilterParams { Key = "介于", Value = "BETWEEN AND" });
            val.Add(new FilterParams { Key = "不介于", Value = "NOT BETWEEN AND" });

            responseAjaxResult.Count=val.Count;
            responseAjaxResult.SuccessResult(val);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取所有属性名称
        /// </summary>
        /// <returns></returns>
        public string GetPropertyNames<T>()
        {
            var dt = new List<string>();
            // 获取 UserSearchResponseDto 类型
            Type dtoType = typeof(T);

            // 获取所有公共属性
            PropertyInfo[] properties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 获取所有属性名称
            foreach (var property in properties)
            {
                dt.Add(property.Name.ToLower());
            }

            // 返回列名
            return string.Join(", ", dt);
        }
        /// <summary>
        /// 解密sql条件数组
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        private List<string> FilterConditions(string? encryptedText)
        {
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(encryptedText))
            {
                var key = Convert.FromBase64String("MTIzNDU2NTQ3ODk2NTIxMjM2NTI1MjUyMzY1MjE0MjU=");

                var decryptedText = CryptoHelper.ToAesDecrypt("MakgyGTZYwfl7CN1ku8/Rw==", "MTIzNDU2NTQ3ODk2NTIxMjM2NTI1MjUyMzY1MjE0MjU");

            }

            return result;
        }



        /// <summary>
        /// <summary>
        /// 接收数据记录
        /// </summary>
        /// <param name="receiveRecordLog"></param>
        /// <param name="dataOperationType">操作类型</param>
        /// <returns></returns>
        public async Task ReceiveRecordLogAsync(ReceiveRecordLog receiveRecordLog, DataOperationType dataOperationType)
        {
            if ((int)dataOperationType == 1)
            {
                //插入操作
              await  _dbContext.CopyNew().Insertable<ReceiveRecordLog>(receiveRecordLog).ExecuteCommandAsync();
            }

            if ((int)dataOperationType ==2)
            {
                //修改操作
                await _dbContext.CopyNew().Updateable<ReceiveRecordLog>(receiveRecordLog).ExecuteCommandAsync();
            }
        }
    }
}
