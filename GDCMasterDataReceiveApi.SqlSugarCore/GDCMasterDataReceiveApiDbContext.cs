using GDCMasterDataReceiveApi.Domain;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.SqlSugarCore
{
    /// <summary>
    /// Sqlsugar上下文
    /// </summary>
    public static class GDCMasterDataReceiveApiDbContext  
    {
        public static void AddSqlSugarContext(this IServiceCollection services, IConfiguration configuration, string dbCon)
        {
            
            //var gdcmasterdatareceiveapi = "Server=10.10.74.3;PORT=5088; User Id=datasecurity; PWD=datasecurity@sql;SCHEMA=DATASECURITY";
            //var gdcdatasecurityapi = "Server=10.10.54.3;PORT=8011; User Id=GDCMDM; PWD=GDCMDMdb123;SCHEMA=GDCMDM";//正式环境

            var gdcmasterdatareceiveapi = "Server=10.10.74.3;PORT=5088; User Id=GDCMDM; PWD=GDCMDMdb123;SCHEMA=GDCMDM";
            //var gdcdatasecurityapi = "Server=10.10.74.3;PORT=5088; User Id=GDCMDM; PWD=GDCMDMdb123;SCHEMA=DATASECURITY";//测试环境
            //是否打开无参数化sql监视
            bool isOpenSql = false;
            SqlSugarClient sqlSugarClient = new SqlSugarClient(new List<ConnectionConfig>()
            {
                 new ConnectionConfig(){ConfigId="gdcmasterdatareceiveapi",ConnectionString = gdcmasterdatareceiveapi, DbType = DbType.Dm,IsAutoCloseConnection = true},
                //new ConnectionConfig(){ConfigId="gdcdatasecurityapi",ConnectionString = gdcdatasecurityapi,DbType = DbType.Dm,IsAutoCloseConnection = true},
               // new ConnectionConfig(){ConfigId="finance",ConnectionString = finance,DbType = DbType.Dm,IsAutoCloseConnection = true}
            }, db =>
            {
                var sqlParmae = string.Empty;
                db.Aop.OnLogExecuting = (sql, parames) =>
                {
                    if (sql.IndexOf("t_auditlogs") < 0)
                    {
                        if (isOpenSql)
                        {
                            //获取无参数化sql  会影响性能  建议调试使用生产环境禁止使用
                            sqlParmae = UtilMethods.GetSqlString(DbType.Dm, sql, parames);
                        }
                        else
                        {
                            sqlParmae = sql;
                        }
                    }
                    //调试时打印sql语句 生产时注释掉
                    if (isOpenSql)
                    {
                        Console.WriteLine($"{sqlParmae}");
                    }
                };
                db.Aop.OnLogExecuted = (sql, parames) =>
                {
                    #region 审计日志
                    if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
                    {
                        if (sql.IndexOf("t_auditlogs") < 0)
                        {
                            var httpContent = HttpContentAccessFactory.Current;
                            if (httpContent != null)
                            {
                                //不记录定时的审计日志
                                if (!httpContent.Request.Path.ToString().ToLower().Contains("timing"))
                                {
                                    //监控所有sql执行时间 会有性能损耗  可以考虑监控sql执行时间超过1秒钟的
                                    if (db.Ado.SqlExecutionTime.TotalSeconds >= 0)
                                    {
                                        var redis = RedisUtil.Instance;
                                        var cacheResult = redis.Get(httpContent.TraceIdentifier.ToLower());
                                        if (!string.IsNullOrWhiteSpace(cacheResult))
                                        {
                                            var recordRequestInfo = JsonConvert.DeserializeObject<RecordRequestInfo>(cacheResult);
                                            if (recordRequestInfo != null && recordRequestInfo.SqlExecInfos != null)
                                            {
                                                recordRequestInfo.SqlExecInfos.Add(new SqlExecInfo() { Sql = sqlParmae + Environment.NewLine, SqlTotalTime = db.Ado.SqlExecutionTime.TotalSeconds.ToString() });
                                                int cacheSeconds = int.Parse(AppsettingsHelper.GetValue("Redis:DefaultKeyCacheSeconds"));
                                                redis.Set(httpContent.TraceIdentifier.ToLower(), recordRequestInfo.ToJson(), cacheSeconds);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                };
                db.Aop.DataExecuting = (value, entityInfo) =>
                {
                    if (entityInfo.OperationType == DataFilterType.InsertByObject)
                    {
                        if (entityInfo.PropertyName == "CreateTime")
                        {
                            entityInfo.SetValue(DateTime.Now);
                        }
                        if (entityInfo.PropertyName == "IsDelete")
                        {
                            entityInfo.SetValue(1);
                        }
                        if (entityInfo.PropertyName == "CreateId")
                        {
                            var httpContent = HttpContentAccessFactory.Current;
                            if (httpContent != null)
                            {
                                var token = httpContent.Request.Headers["Authorization"];
                                token = token.ToString().Replace("Bearer", "").Trim();
                                #region 获取当前用户信息
                                //var tokenUrl = AppsettingsHelper.GetValue("ParseTokenUrl") + token;
                                //WebHelper webHelper = new WebHelper();
                                //var tokenResult = webHelper.DoGetAsync(tokenUrl).GetAwaiter().GetResult();
                                //var account = string.Empty;
                                //if (tokenResult.Code == 200)
                                //{
                                //    var code = JObject.Parse(tokenResult.Result);
                                //    account = ((Newtonsoft.Json.Linq.JValue)code["account"]).Value.ToString();
                                //}
                                #endregion
                                //var redis = RedisUtil.Instance;

                                //bool existKey = redis.Exists(account);
                                //if (existKey)
                                //{

                                //    var userValue = redis.Get(account);
                                //    if (!string.IsNullOrWhiteSpace(userValue))
                                //    {
                                //        var userInfo = JsonConvert.DeserializeObject<GlobalCurrentUser>(userValue);
                                //        if (userInfo != null)
                                //        {
                                //            entityInfo.SetValue(userInfo.Id);
                                //        }
                                //    }
                                //}
                            }

                        }

                        if (entityInfo.PropertyName == "Timestamp")
                        {
                            entityInfo.SetValue(Utils.GetTimeSpan());
                        }
                    }
                    if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                    {
                        if (!(entityInfo.EntityValue is BaseEntity<long>))
                        //if (!(entityInfo.EntityValue is BaseEntity<Guid>))//mysql数据库默认是guid
                        {
                            return;
                        }
                        var isDelete = ((GDCMasterDataReceiveApi.Domain.BaseEntity<long>)entityInfo.EntityValue).IsDelete;
                        //var isDelete = ((GDCMasterDataReceiveApi.Domain.BaseEntity<Guid>)entityInfo.EntityValue).IsDelete;//mysql数据库默认是guid
                        if (isDelete == 1 && entityInfo.PropertyName == "UpdateTime")
                        {
                            entityInfo.SetValue(DateTime.Now);
                        }
                        if (isDelete == 1 && entityInfo.PropertyName == "UpdateId")
                        {
                            var httpContent = HttpContentAccessFactory.Current;
                            if (httpContent != null)
                            {
                                var token = httpContent.Request.Headers["Authorization"];
                                token = token.ToString().Replace("Bearer", "").Trim();
                                #region 获取当前用户信息
                                //var tokenUrl = AppsettingsHelper.GetValue("ParseTokenUrl") + token;
                                //WebHelper webHelper = new WebHelper();
                                //var tokenResult = webHelper.DoGetAsync(tokenUrl).GetAwaiter().GetResult();
                                //var account = string.Empty;
                                //if (tokenResult.Code == 200)
                                //{
                                //    var code = JObject.Parse(tokenResult.Result);
                                //    account = ((Newtonsoft.Json.Linq.JValue)code["account"]).Value.ToString();
                                //}
                                #endregion
                                //var redis = RedisUtil.Instance;

                                //bool existKey = redis.Exists(account);
                                //if (existKey)
                                //{

                                //    var userValue = redis.Get(account);
                                //    if (!string.IsNullOrWhiteSpace(userValue))
                                //    {
                                //        var userInfo = JsonConvert.DeserializeObject<GlobalCurrentUser>(userValue);
                                //        if (userInfo != null)
                                //        {
                                //            entityInfo.SetValue(userInfo.Id);
                                //        }
                                //    }
                                //}
                            }
                        }
                        if (isDelete == 0 && entityInfo.PropertyName == "DeleteTime")
                        {
                            entityInfo.SetValue(DateTime.Now);
                        }
                        if (isDelete == 0 && entityInfo.PropertyName == "DeleteId")
                        {
                            var httpContent = HttpContentAccessFactory.Current;
                            if (httpContent != null)
                            {
                                var token = httpContent.Request.Headers["Authorization"];
                                token = token.ToString().Replace("Bearer", "").Trim();
                                #region 获取当前用户信息
                                //var tokenUrl = AppsettingsHelper.GetValue("ParseTokenUrl") + token;
                                //WebHelper webHelper = new WebHelper();
                                //var tokenResult = webHelper.DoGetAsync(tokenUrl).GetAwaiter().GetResult();
                                //var account = string.Empty;
                                //if (tokenResult.Code == 200)
                                //{
                                //    var code = JObject.Parse(tokenResult.Result);
                                //    account = ((Newtonsoft.Json.Linq.JValue)code["account"]).Value.ToString();
                                //}
                                #endregion
                                //var redis = RedisUtil.Instance;

                                //bool existKey = redis.Exists(account);
                                //if (existKey)
                                //{

                                //    var userValue = redis.Get(account);
                                //    //Console.WriteLine("2222:" + userValue);
                                //    if (!string.IsNullOrWhiteSpace(userValue))
                                //    {
                                //        var userInfo = JsonConvert.DeserializeObject<GlobalCurrentUser>(userValue);
                                //        if (userInfo != null)
                                //        {
                                //            entityInfo.SetValue(userInfo.Id);
                                //        }
                                //    }
                                //}
                            }
                        }

                        if (isDelete == 1 && entityInfo.PropertyName == "Timestamp")
                        {
                            if (entityInfo.PropertyName == "Timestamp")
                            {
                                entityInfo.SetValue(Utils.GetTimeSpan());
                            }
                        }
                    }
                };
                db.Aop.OnDiffLogEvent = async it =>
                {
                    #region 业务操作日志 （可优化）
                    try
                    {
                        var deleteTotal = 0;
                        var lgoObject = it.BusinessData as LogInfo;
                        //插入之前的数据
                        var editBeforeData = it.BeforeData;
                        //插入之后的数据
                        var editAfterData = it.AfterData;
                        foreach (var item in editAfterData)
                        {
                            deleteTotal += editAfterData.Select(x => x.Columns.Where(x => x.Value.ToString() == "1" && x.ColumnName.Equals("IsDelete", StringComparison.OrdinalIgnoreCase))).ToList()[0].Count();
                        }
                        //说明不是删除数据而是更新数据
                        if (it.DiffType == DiffType.update && deleteTotal == editAfterData.Count)
                        {
                            lgoObject.OperationType = 2;
                            lgoObject.logDiffDtos = new List<LogDiffDto>();
                            lgoObject.OperationObject = editBeforeData.Select(x => x.TableName).FirstOrDefault();
                            var i = 0;
                            foreach (var item in editBeforeData)
                            {
                                foreach (var beforeItem in item.Columns)
                                {
                                    if (beforeItem.ColumnName.ToLower() == "Id" ||
                                    beforeItem.ColumnName.ToLower() == "CreateTime" ||
                                    beforeItem.ColumnName.ToLower() == "UpdateTime" ||
                                    beforeItem.ColumnName.ToLower() == "DeleteTime" ||
                                    beforeItem.ColumnName.ToLower() == "CreateId" ||
                                    beforeItem.ColumnName.ToLower() == "UpdateId" ||
                                    beforeItem.ColumnName.ToLower() == "DeleteId")
                                    {
                                        continue;
                                    }
                                    LogDiffDto logDiffDto = new LogDiffDto()
                                    {
                                        TableName = lgoObject.OperationObject,
                                        Describe = beforeItem.ColumnDescription,
                                        ColumnName = beforeItem.ColumnName.ToLower(),
                                        OriginalValue = beforeItem.Value.ToString(),
                                    };
                                    var newValue = editAfterData[i].Columns.Where(x => x.ColumnName == beforeItem.ColumnName).Select(x => x.Value.ToString()).ToList();
                                    if (newValue.Any())
                                    {
                                        //如果旧值等于新值  则不记录
                                        if (logDiffDto.OriginalValue.TrimAll() != newValue[0].ToString().TrimAll())
                                        {
                                            logDiffDto.ChangeValue = newValue[0].ToString();
                                            lgoObject.logDiffDtos.Add(logDiffDto);
                                        }
                                    }
                                }
                                i++;
                            }
                        }
                        else if (it.DiffType == DiffType.insert)
                        {
                            //新增日志操作
                            lgoObject.OperationType = 1;
                            lgoObject.logDiffDtos = new List<LogDiffDto>();
                            lgoObject.OperationObject = editAfterData.Select(x => x.TableName).FirstOrDefault();
                            foreach (var item in editAfterData)
                            {
                                foreach (var beforeItem in item.Columns)
                                {
                                    if (beforeItem.ColumnName.ToLower() == "Id" ||
                                    beforeItem.ColumnName.ToLower() == "CreateTime" ||
                                    beforeItem.ColumnName.ToLower() == "UpdateTime" ||
                                    beforeItem.ColumnName.ToLower() == "DeleteTime" ||
                                    beforeItem.ColumnName.ToLower() == "CreateId" ||
                                    beforeItem.ColumnName.ToLower() == "UpdateId" ||
                                    beforeItem.ColumnName.ToLower() == "DeleteId" ||
                                    beforeItem.ColumnName.ToLower() == "IsDelete")
                                    {
                                        continue;
                                    }
                                    LogDiffDto logDiffDto = new LogDiffDto()
                                    {
                                        TableName = lgoObject.OperationObject,
                                        Describe = beforeItem.ColumnDescription,
                                        ColumnName = beforeItem.ColumnName.ToLower(),
                                        ChangeValue = beforeItem.Value.ToString(),

                                    };
                                    lgoObject.logDiffDtos.Add(logDiffDto);
                                }
                            }
                        }
                        else
                        {
                            //删除日志操作
                            lgoObject.OperationType = 3;
                            lgoObject.logDiffDtos = new List<LogDiffDto>();
                            lgoObject.OperationObject = editBeforeData.Select(x => x.TableName).FirstOrDefault();
                            foreach (var item in editBeforeData)
                            {
                                foreach (var beforeItem in item.Columns)
                                {
                                    if (beforeItem.ColumnName.ToLower() == "IsDelete".ToLower())
                                    {
                                        LogDiffDto logDiffDto = new LogDiffDto()
                                        {
                                            TableName = lgoObject.OperationObject,
                                            Describe = beforeItem.ColumnDescription,
                                            ColumnName = beforeItem.ColumnName.ToLower(),
                                            OriginalValue = beforeItem.Value.ToString(),
                                        };
                                        var newValue = editAfterData.SelectMany(x => x.Columns.Where(x => x.ColumnName == beforeItem.ColumnName).Select(x => x.Value.ToString())).ToList();
                                        if (newValue.Any())
                                        {
                                            //如果旧值等于新值  则不记录
                                            if (logDiffDto.OriginalValue.TrimAll() != newValue[0].ToString().TrimAll())
                                            {
                                                logDiffDto.ChangeValue = newValue[0].ToString();
                                                lgoObject.logDiffDtos.Add(logDiffDto);
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        try
                        {
                            #region Aop方式Http记录日志
                            var url = Path.Combine($"{AppsettingsHelper.GetValue("Log:Url")}/logapi/OperateLogs/InsLogs");
                            if (lgoObject != null && lgoObject.Id != Guid.Empty)
                            {
                                WebHelper webHelper = new WebHelper();
                                Dictionary<string, object> parames = new Dictionary<string, object>();
                                parames.Add("operationType", lgoObject.OperationType);
                                parames.Add("systemLogSource", 2);
                                parames.Add("businessModule", lgoObject.BusinessModule);
                                parames.Add("businessRemark", lgoObject.BusinessRemark);
                                parames.Add("operationObject", lgoObject.OperationObject);
                                parames.Add("clientIp", IpHelper.GetClientIp());
                                parames.Add("deviceinformation", HttpContentAccessFactory.Current.Request.Headers["User-Agent"].ToString());
                                parames.Add("operationId", lgoObject.OperationId);
                                parames.Add("operationName", lgoObject.OperationName);
                                parames.Add("operationTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                parames.Add("diffLogsDtos", lgoObject.logDiffDtos);
                                parames.Add("dataid", lgoObject.DataId);
                                parames.Add("GroupIdentity", lgoObject.GroupIdentity);
                                //webHelper.DoPost(url, parames);
                                webHelper.DoPostAsync(url, parames);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            //可以提供日志访问  记录日志文件中
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion
                };
            });
            services.AddScoped<ISqlSugarClient>(context => { return sqlSugarClient.CopyNew(); });
        }
    }
}
