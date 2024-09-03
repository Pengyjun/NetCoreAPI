using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using Newtonsoft.Json;
using SqlSugar;
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
        /// <param name="sql">前端给的sql语句  只拼接到from之前 不包含</param>
        /// <param name="filterParams">相关参数条件、符号</param>
        /// <param name="IsPaging">是否需要分页</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数量</param>
        /// <returns></returns>
        public async Task<List<T>> GetSearchListAsync<T>(string tableName, string sql, List<FilterParams> filterParams, bool IsPaging, int pageIndex, int pageSize) where T : class, new()
        {
            // 初始化条件列表、参数字典和括号栈
            var conditions = new List<string>();
            var di = new Dictionary<string, object>();
            var inParentheses = new Stack<bool>();

            // 检查是否有过滤参数
            if (filterParams != null && filterParams.Any())
            {
                foreach (var param in filterParams)
                {
                    // 处理开始括号
                    if (param.IsGroupStart)
                    {
                        conditions.Add("(");
                        inParentheses.Push(true);
                    }

                    // 获取字段类型
                    var fieldType = await GetFieldType(param.Name, tableName);

                    // 处理 LIKE 条件
                    if (param.Condition.Equals("LIKE", StringComparison.OrdinalIgnoreCase))
                    {
                        conditions.Add($"{param.Name} LIKE @{param.Name}");
                        di.Add($"@{param.Name}", $"%{param.Value.ToString()}%");
                    }
                    // 处理 IN 条件
                    else if (param.Condition.Equals("IN", StringComparison.OrdinalIgnoreCase))
                    {
                        var values = param.Value as IEnumerable<object>;
                        if (values != null)
                        {
                            var placeholders = values.Select((_, i) => $"@{param.Name}_{i}").ToList();
                            conditions.Add($"{param.Name} IN ({string.Join(", ", placeholders)})");
                            for (int i = 0; i < values.Count(); i++)
                            {
                                var value = values.ElementAt(i);
                                di[$"{param.Name}_{i}"] = fieldType == "DATE" ? Convert.ToDateTime(value) : value;
                            }
                        }
                    }
                    // 处理其他条件
                    else
                    {
                        // 处理字段类型和对应参数的逻辑
                        if (fieldType == "VARCHAR" || fieldType == "CHAR")
                        {
                            // 对于 VARCHAR 和 CHAR 类型的字段，通常是字符串类型
                            // 将参数值转换为字符串，并添加到参数字典中
                            di.Add($"@{param.Name}", param.Value.ToString());
                            conditions.Add($"{param.Name} = @{param.Name}");
                        }
                        else if (fieldType == "NUMBER" || fieldType == "INTEGER" || fieldType == "FLOAT")
                        {
                            // 对于 NUMBER、INTEGER 和 FLOAT 类型的字段，通常是数字类型
                            // 将参数值转换为十进制数字，并添加到参数字典中
                            di.Add($"@{param.Name}", Convert.ToDecimal(param.Value));
                            conditions.Add($"{param.Name} = @{param.Name}");
                        }
                        else if (fieldType == "DATE" || fieldType == "TIMESTAMP")
                        {
                            // 对于 DATE 和 TIMESTAMP 类型的字段，通常是日期或时间类型
                            // 将参数值转换为日期时间，并添加到参数字典中
                            di.Add($"@{param.Name}", Convert.ToDateTime(param.Value));
                            conditions.Add($"{param.Name} = @{param.Name}");
                        }
                        else if (fieldType == "BOOLEAN")
                        {
                            // 对于 BOOLEAN 类型的字段，通常是布尔值类型
                            // 将参数值转换为布尔值，并添加到参数字典中
                            di.Add($"@{param.Name}", Convert.ToBoolean(param.Value));
                            conditions.Add($"{param.Name} = @{param.Name}");
                        }
                        else if (fieldType == "BLOB" || fieldType == "CLOB")
                        {
                            // 对于 BLOB (Binary Large Object) 和 CLOB (Character Large Object) 类型的字段
                            // 通常用于存储大数据量的二进制数据或大文本数据
                            // 将参数值转换为字节数组（对于 BLOB）或字符串（对于 CLOB），并添加到参数字典中
                            if (param.Value is byte[] byteArray)
                            {
                                di.Add($"@{param.Name}", byteArray);
                                conditions.Add($"{param.Name} = @{param.Name}");
                            }
                            else if (param.Value is string stringValue)
                            {
                                di.Add($"@{param.Name}", stringValue);
                                conditions.Add($"{param.Name} = @{param.Name}");
                            }
                        }
                        else
                        {
                            // 处理其他未明确列出的字段类型
                            // 默认将参数值直接添加到参数字典中
                            di.Add($"@{param.Name}", param.Value);
                            conditions.Add($"{param.Name} = @{param.Name}");
                        }
                    }

                    // 添加逻辑操作符
                    if (!string.IsNullOrWhiteSpace(param.LogicOperator))
                    {
                        conditions.Add(param.LogicOperator);
                    }

                    // 处理结束括号
                    if (param.IsGroupEnd)
                    {
                        if (inParentheses.Any())
                        {
                            conditions.Add(")");
                            inParentheses.Pop();
                        }
                    }
                }
            }

            // 构建 SQL 查询的 WHERE 子句
            var whereClause = conditions.Any() ? " WHERE " + string.Join(" ", conditions) : string.Empty;

            //默认查询id，name
            sql = string.IsNullOrWhiteSpace(sql) ? "id,name" : sql;
            sql = "SELECT " + sql + $" FROM {tableName} {whereClause}";

            // 转换参数字典为参数数组
            var parameters = di.Select(kv => new SugarParameter(kv.Key, kv.Value)).ToArray();

            // 异步执行查询并获取结果
            var dataTable = await _dbContext.Ado.GetDataTableAsync(sql, parameters);

            // 将结果转换为 JSON 字符串
            var json = dataTable.ToJson();

            // 反序列化 JSON 字符串为对象列表
            var result = JsonConvert.DeserializeObject<List<T>>(json);

            //是否需要分页
            var pRes = IsPaging ? result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList() : result;

            return pRes;
        }

        private async Task<string> GetFieldType(string fieldName, string tableName)
        {
            // 查询字段数据类型
            var sql = "SELECT DATA_TYPE FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName;";
            var parameters = new
            {
                TableName = tableName.ToUpper(),
                ColumnName = fieldName.ToUpper()
            };

            // 执行查询并获取结果
            return await _dbContext.Ado.SqlQuerySingleAsync<string>(sql, parameters);
        }

    }
}
