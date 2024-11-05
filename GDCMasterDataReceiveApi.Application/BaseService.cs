using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
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

            #region 旧版本
            //val.Add(new FilterParams { Key = "等于", Value = "=" });
            //val.Add(new FilterParams { Key = "不等于", Value = "!=" });
            //val.Add(new FilterParams { Key = "小于", Value = "<" });
            //val.Add(new FilterParams { Key = "小于等于", Value = "<=" });
            //val.Add(new FilterParams { Key = "大于", Value = ">" });
            //val.Add(new FilterParams { Key = "大于等于", Value = ">=" });
            //val.Add(new FilterParams { Key = "包含", Value = "LIKE %param%" });
            //val.Add(new FilterParams { Key = "不包含", Value = "NOT LIKE %param%" });
            //val.Add(new FilterParams { Key = "开头为", Value = "LIKE param%" });
            //val.Add(new FilterParams { Key = "开头不为", Value = "NOT LIKE param%" });
            //val.Add(new FilterParams { Key = "结尾为", Value = "LIKE %param" });
            //val.Add(new FilterParams { Key = "结尾不为", Value = "NOT LIKE %param" });
            //val.Add(new FilterParams { Key = "为空", Value = "IS NULL" });
            //val.Add(new FilterParams { Key = "非空", Value = "IS NOT NULL" });
            //val.Add(new FilterParams { Key = "空字符", Value = "=''" });
            //val.Add(new FilterParams { Key = "非空字符串", Value = "!=''" });
            //val.Add(new FilterParams { Key = "介于", Value = "BETWEEN AND" });
            //val.Add(new FilterParams { Key = "不介于", Value = "NOT BETWEEN AND" });
            #endregion
            //val.Add(new FilterParams() { Key= "Equal",Value="=" });
            //val.Add(new FilterParams() { Key= "Like", Value= "模糊查询" });
            //val.Add(new FilterParams() { Key= "GreaterThan", Value= "大于" });
            //val.Add(new FilterParams() { Key= "GreaterThanOrEqual", Value= "大于等于" });
            //val.Add(new FilterParams() { Key= "LessThan", Value="小于" });
            //val.Add(new FilterParams() { Key= "LessThanOrEqual", Value= "小于等于" });
            //val.Add(new FilterParams() { Key= "In", Value= "In操作" });
            //val.Add(new FilterParams() { Key= "NotIn", Value= "Not in操作" });
            //val.Add(new FilterParams() { Key= "LikeLeft", Value= "左模糊" });
            //val.Add(new FilterParams() { Key= "LikeRight", Value= "右模糊" });
            //val.Add(new FilterParams() { Key= "NoEqual", Value= "不等于" });
            //val.Add(new FilterParams() { Key= "IsNullOrEmpty", Value= "是null或者''" });
            //val.Add(new FilterParams() { Key= "IsNot", Value= "不等于" });
            //val.Add(new FilterParams() { Key= "NoLike", Value= "模糊查询取反" });
            //val.Add(new FilterParams() { Key= "EqualNull", Value= "不等于" });
            //val.Add(new FilterParams() { Key= "InLike", Value= "不等于" });

            val.Add(new FilterParams() { Key = "0", Value = "等于" });
            val.Add(new FilterParams() { Key = "1", Value = "模糊查询" });
            val.Add(new FilterParams() { Key = "2", Value = "大于" });
            val.Add(new FilterParams() { Key = "3", Value = "大于等于" });
            val.Add(new FilterParams() { Key = "4", Value = "小于" });
            val.Add(new FilterParams() { Key = "5", Value = "小于等于" });
            val.Add(new FilterParams() { Key = "6", Value = "In操作" });
            val.Add(new FilterParams() { Key = "7", Value = "Not in操作" });
            val.Add(new FilterParams() { Key = "8", Value = "左模糊" });
            val.Add(new FilterParams() { Key = "9", Value = "右模糊" });
            val.Add(new FilterParams() { Key = "10", Value = "不等于" });
            val.Add(new FilterParams() { Key = "11", Value = "是null或者''" });


            responseAjaxResult.Count = val.Count;
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
               await _dbContext.Insertable<ReceiveRecordLog>(receiveRecordLog).ExecuteCommandAsync();
            }

            if ((int)dataOperationType == 2)
            {
                //修改操作
                await _dbContext.Updateable<ReceiveRecordLog>(receiveRecordLog).ExecuteCommandAsync();
            }
        }
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> GetUserLoginInfoAsync(LoginDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            if (string.IsNullOrWhiteSpace(requestDto.LoginAccount))
            {
                responseAjaxResult.FailResult(HttpStatusCode.NoLogin, "登录账号不能为空");
                return responseAjaxResult;
            }
            if (string.IsNullOrWhiteSpace(requestDto.Password))
            {
                responseAjaxResult.FailResult(HttpStatusCode.NoLogin, "登录密码不能为空");
                return responseAjaxResult;
            }

            var userInfo = await _dbContext.Queryable<User>()
                .Where(x => x.HR_EMP_CODE == requestDto.LoginAccount && x.IsDelete == 1)
                .FirstAsync();

            if (userInfo != null)
            {
                if (!string.IsNullOrWhiteSpace(userInfo.PASSWORD))
                {
                    if (userInfo.PASSWORD == requestDto.Password)
                    {
                        responseAjaxResult.SuccessResult(true, "登录成功");
                    }
                    else
                    {
                        responseAjaxResult.SuccessResult(false, "密码错误，登录失败");
                    }
                }
                else
                {
                    responseAjaxResult.FailResult(HttpStatusCode.SetPassword, "无密码，请设置密码");
                }
            }
            else
            {
                responseAjaxResult.FailResult(HttpStatusCode.AccountNotEXIST, "账号信息不存在");
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 设置密码/修改密码
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SetPasswordAsync(LoginDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (string.IsNullOrWhiteSpace(requestDto.LoginAccount))
            {
                responseAjaxResult.FailResult(HttpStatusCode.NoLogin, "登录账号不能为空");
                return responseAjaxResult;
            }
            if (string.IsNullOrWhiteSpace(requestDto.Password))
            {
                responseAjaxResult.FailResult(HttpStatusCode.NoLogin, "登录密码不能为空");
                return responseAjaxResult;
            }
            //密码正则校验 复杂密码 此处省略
            var userInfo = await _dbContext.Queryable<User>()
                .Where(x => x.HR_EMP_CODE == requestDto.LoginAccount && x.IsDelete == 1)
                .FirstAsync();

            if (userInfo != null)
            {
                var tag = 0;//1:修改  2：初始设置
                tag = string.IsNullOrWhiteSpace(userInfo.PASSWORD) ? 2 : 1;
                userInfo.PASSWORD = requestDto.Password;
                await _dbContext.Updateable(userInfo).ExecuteCommandAsync();
                responseAjaxResult.SuccessResult(true, tag == 1 ? "密码修改成功" : "密码设置成功");
            }
            else
            {
                responseAjaxResult.FailResult(HttpStatusCode.AccountNotEXIST, "账号信息不存在");
            }

            return responseAjaxResult;
        }


        /// <summary>
        /// json转sql
        /// </summary>
        /// <param name="conditionalModels"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<IConditionalModel>> JsonToConventSqlAsync(List<JsonToSqlRequestDto> jsonToSqlRequestDtos)
        {
            var conditionalModel = new List<IConditionalModel>();
            //conditionalModel.Add(new ConditionalModel { FieldName = "name", ConditionalType = ConditionalType.Equal, FieldValue = "1=1" });

            if (jsonToSqlRequestDtos.Count > 0)
            {
                foreach (var item in jsonToSqlRequestDtos)
                {
                    conditionalModel.Add(new ConditionalCollections()
                    {
                        ConditionalList = new List<KeyValuePair<WhereType, ConditionalModel>>()
                        {
                            new KeyValuePair<WhereType, ConditionalModel>(
                             (WhereType)item.Type,
                            new ConditionalModel(){FieldName =item.FieldName,ConditionalType=item.ConditionalType,FieldValue=item.FieldValue}),
                        }
                    });
                   
                }
            }
            return conditionalModel;
        }
    }
}
