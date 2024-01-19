using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.HSSF.Record.Chart;
using Spire.Pdf.Exporting.XPS.Schema;
using SqlSugar;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.OperationLog
{

    /// <summary>
    /// http记录日志接口实现层
    /// </summary>
    public class LogService : ILogService
    {

        #region 依赖注入

        private readonly IBaseRepository<LogDiffDto> _dbProject;
        public ILogger<LogService> logger { get; set; }
        public ISqlSugarClient dbContent { get; set; }
        public LogService(IBaseRepository<LogDiffDto> dbProject, ILogger<LogService> logger, ISqlSugarClient dbContent)
        {
            _dbProject = dbProject;
            this.logger = logger;
            this.dbContent = dbContent;
        }
        #endregion

        /// <summary>
        /// 写日志  
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task WriteLogAsync(LogInfo logRequestDto)
        {
            try
            {
                var url = AppsettingsHelper.GetValue("Log:Url");
                WebHelper webHelper = new WebHelper();
                Dictionary<string, object> parames = new Dictionary<string, object>();
                parames.Add("operationType", logRequestDto.OperationType);
                parames.Add("systemLogSource", 1);
                parames.Add("businessModule", logRequestDto.BusinessModule);
                parames.Add("businessRemark", logRequestDto.BusinessRemark);
                parames.Add("operationObject", logRequestDto.OperationObject);
                parames.Add("clientIp", IpHelper.GetClientIp());
                parames.Add("deviceinformation", HttpContentAccessFactory.Current.Request.Headers["User-Agent"].ToString());
                parames.Add("operationId", logRequestDto.OperationId);
                parames.Add("operationName", logRequestDto.OperationName);
                parames.Add("operationTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                parames.Add("diffLogsDtos", logRequestDto.logDiffDtos);
                parames.Add("DataId", logRequestDto.DataId);
                await Task.Factory.StartNew(async () =>
                {
                    var a = await webHelper.DoPostAsync(url, parames);
                });
            }
            catch (Exception ex)
            {
                logger.LogError("Http记录日志出现错误;", ex);
            }
        }
        /// <summary>
        /// 日志信息
        /// </summary>
        /// <param name="getProjectChangesRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchProjectChangesResponseDto>>> GetProjectChangesAsync(GetProjectChangesRequestDto getProjectChangesRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SearchProjectChangesResponseDto>>();
            #region 获取日志
            try
            {
                var url = AppsettingsHelper.GetValue("LogRecord:Url");
                WebHelper webHelper = new WebHelper();
                Dictionary<string, object> parames = new Dictionary<string, object>();
                parames.Add("tableName", getProjectChangesRequestDto.ModuleType.ToString());
                parames.Add("id", getProjectChangesRequestDto.ProjectId);
                parames.Add("pageIndex", getProjectChangesRequestDto.PageIndex);
                parames.Add("pageSize", getProjectChangesRequestDto.PageSize);
                #region 获取表数据
                //获取表数据
                var logInformation = await webHelper.DoGetAsync<HttpBaseResponseDto<GetProjectChangesResponseDto>>(url, parames);
                var logInformationLsit = logInformation.Result.Data;
                if (!logInformationLsit.Any())
                {
                    responseAjaxResult.Success();
                    return responseAjaxResult;
                }
                #endregion
                #region 项目信息清单
                //项目信息清单
                if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_project)
                {
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var user = await dbContent.Queryable<Domain.Models.User>().ToListAsync();
                    var projectStatus = await dbContent.Queryable<ProjectStatus>().ToListAsync();
                    var projectType = await dbContent.Queryable<ProjectType>().ToListAsync();
                    var waterCarriage = await dbContent.Queryable<WaterCarriage>().ToListAsync();
                    var projectScale = await dbContent.Queryable<ProjectScale>().ToListAsync();
                    var currencyConverter = await dbContent.Queryable<CurrencyConverter>().ToListAsync();
                    var constructionQualification = await dbContent.Queryable<ConstructionQualification>().ToListAsync();
                    var industryClassification = await dbContent.Queryable<IndustryClassification>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var institution = await dbContent.Queryable<Institution>().ToListAsync();
                    var province = await dbContent.Queryable<Province>().ToListAsync();
                    foreach (var item in logInformationLsit)
                    {
                        var allUnitList = item.getDiffLogs.Where(x => x.ColumnName.Contains("单位Id"))
                            .Select(x => new { x.ChangeValue, x.OriginalValue }).ToList();
                        var changeBeforeValue = allUnitList.Where(x => !string.IsNullOrWhiteSpace(x.OriginalValue)).Select(x => x.OriginalValue.ToGuid()).ToList();
                        var changeAfterValue = allUnitList.Where(x => !string.IsNullOrWhiteSpace(x.ChangeValue)).Select(x => x.ChangeValue.ToGuid()).ToList();
                        changeBeforeValue.AddRange(changeAfterValue);
                        var dealingUnit = await dbContent.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && changeBeforeValue.Contains(x.PomId.Value)).Select(x => new { x.ZBPNAME_ZH, x.PomId, x.IsDelete }).ToListAsync();
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {

                            #region 业务判断
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目Id") || item.getDiffLogs[i].ColumnName.Contains("项目 Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("单位Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dealingUnit.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).ZBPNAME_ZH;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dealingUnit.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).ZBPNAME_ZH;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("人员 Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = user.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = user.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName == "Id" || item.getDiffLogs[i].ColumnName == "pomId")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Any())
                            {
                                item.getDiffLogs[i].ColumnName = item.getDiffLogs[i].ColumnName.Replace("id", "").Replace("Id", "");
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目所属项目部"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = institution.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Shortname;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = institution.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Shortname;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目状态"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = projectStatus.SingleOrDefault(x => x.StatusId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = projectStatus.SingleOrDefault(x => x.StatusId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目类型"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = projectType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = projectType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("工况级数"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = waterCarriage.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Remarks;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = waterCarriage.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Remarks;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目规模"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = projectScale.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = projectScale.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("币种"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = currencyConverter.SingleOrDefault(x => x.CurrencyId.ToString() == item.getDiffLogs[i].OriginalValue).Remark;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = currencyConverter.SingleOrDefault(x => x.CurrencyId.ToString() == item.getDiffLogs[i].ChangeValue).Remark;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目施工资质"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = constructionQualification.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = constructionQualification.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("行业分类标准"))
                            {
                                string input = item.getDiffLogs[i].OriginalValue;
                                item.getDiffLogs[i].OriginalValue = null;
                                string[] output = input.Split(",");
                                for (int s = 0; s < output.Length; s++)
                                {
                                    if (output[s].Length > 35)
                                    {
                                        item.getDiffLogs[i].OriginalValue += industryClassification.SingleOrDefault(x => x.PomId.ToString() == output[s]).Name + ",";
                                    }
                                    else
                                    {
                                        item.getDiffLogs[i].OriginalValue = "空";
                                        break;
                                    }
                                }
                                string input1 = item.getDiffLogs[i].ChangeValue;
                                item.getDiffLogs[i].ChangeValue = null;
                                string[] output1 = input1.Split(",");
                                for (int z = 0; z < output1.Length; z++)
                                {

                                    item.getDiffLogs[i].ChangeValue += industryClassification.SingleOrDefault(x => x.PomId.ToString() == output1[z]).Name + ",";
                                }

                                //item.getDiffLogs[i].OriginalValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                //item.getDiffLogs[i].ChangeValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("是否完成编制"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "否";
                                }
                                else
                                {
                                    item.getDiffLogs[i].OriginalValue = "是";
                                }
                                if (item.getDiffLogs[i].ChangeValue == "0")
                                {
                                    item.getDiffLogs[i].ChangeValue = "否";
                                }
                                else
                                {
                                    item.getDiffLogs[i].ChangeValue = "是";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("新兴工程类别标签（0：传统；1：新兴）"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "传统";
                                }
                                else
                                {
                                    item.getDiffLogs[i].OriginalValue = "新兴";
                                }
                                if (item.getDiffLogs[i].ChangeValue == "0")
                                {
                                    item.getDiffLogs[i].ChangeValue = "传统";
                                }
                                else
                                {
                                    item.getDiffLogs[i].ChangeValue = "新兴";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("现汇、投资工程类别标签（0：现汇；1：投资）"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "现汇";
                                }
                                else
                                {
                                    item.getDiffLogs[i].OriginalValue = "投资";
                                }
                                if (item.getDiffLogs[i].ChangeValue == "0")
                                {
                                    item.getDiffLogs[i].ChangeValue = "现汇";
                                }
                                else
                                {
                                    item.getDiffLogs[i].ChangeValue = "投资";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目类别 0 境内  1 境外"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "境内";
                                }
                                else
                                {
                                    item.getDiffLogs[i].OriginalValue = "境外";
                                }
                                if (item.getDiffLogs[i].ChangeValue == "0")
                                {
                                    item.getDiffLogs[i].ChangeValue = "境内";
                                }
                                else
                                {
                                    item.getDiffLogs[i].ChangeValue = "境外";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("具有特殊社会效应"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "否";
                                }
                                else
                                {
                                    item.getDiffLogs[i].OriginalValue = "是";
                                }
                                if (item.getDiffLogs[i].ChangeValue == "0")
                                {
                                    item.getDiffLogs[i].ChangeValue = "否";
                                }
                                else
                                {
                                    item.getDiffLogs[i].ChangeValue = "是";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("施工地点"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = province.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Zaddvsname;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = province.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Zaddvsname;
                                }

                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("开始日期") || item.getDiffLogs[i].ColumnName.Contains("结束日期") || item.getDiffLogs[i].ColumnName.Contains("变更时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目干系人员职位类型"))
                            {
                                item.getDiffLogs[i].ColumnName = "项目干系人员职位类型";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.IsDelete == 1 && x.TypeNo == 1 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.IsDelete == 1 && x.TypeNo == 1 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("是否在任 true:在任 false:离任"))
                            {
                                item.getDiffLogs[i].ColumnName = "是否在任";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "true")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "是";
                                    }
                                    else
                                    {
                                        item.getDiffLogs[i].OriginalValue = "否";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "true")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "是";
                                    }
                                    else
                                    {
                                        item.getDiffLogs[i].ChangeValue = "否";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("单位类型"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.IsDelete == 1 && x.TypeNo == 2 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.IsDelete == 1 && x.TypeNo == 2 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("开工时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("完工时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            #endregion
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                DtoType = item.getDiffLogs[i].DtoType,
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 项目组织结构
                // 项目组织结构
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_projectwbs)
                {
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Any())
                            {
                                item.getDiffLogs[i].ColumnName = item.getDiffLogs[i].ColumnName.Replace("id", "").Replace("Id", "");
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 船舶进退场
                //船舶进退场
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_shipmovement)
                {
                    var subShip = await dbContent.Queryable<Domain.Models.SubShip>().ToListAsync();
                    var ownerShip = await dbContent.Queryable<OwnerShip>().ToListAsync();
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船舶Id（PomId）"))
                            {
                                item.getDiffLogs[i].ColumnName = "船舶";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    var shipPings = ownerShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.Name;
                                    if (shipPings.Any())
                                    {
                                        item.getDiffLogs[i].OriginalValue = shipPings;
                                    }
                                    else
                                    {
                                        item.getDiffLogs[i].OriginalValue = subShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    var shipPings = ownerShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.Name;
                                    if (shipPings.Any())
                                    {
                                        item.getDiffLogs[i].ChangeValue = shipPings;
                                    }
                                    else
                                    {
                                        item.getDiffLogs[i].ChangeValue = subShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Any())
                            {
                                item.getDiffLogs[i].ColumnName = item.getDiffLogs[i].ColumnName.Replace("id", "").Replace("Id", "");
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船舶类型（1：自有船舶，2：分包船舶）"))
                            {
                                item.getDiffLogs[i].ColumnName = "船舶类型";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "自有船舶";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "分包船舶";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "自有船舶";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "分包船舶";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("状态"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "0")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "未进场";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "进场";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "退场";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "0")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "未进场";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "进场";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "退场";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("进场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("退场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {

                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 产值日报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_dayreport)
                {
                    var projectWBS = await dbContent.Queryable<ProjectWBS>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var subShip = await dbContent.Queryable<Domain.Models.SubShip>().ToListAsync();
                    var ownerShip = await dbContent.Queryable<OwnerShip>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    var currency = await dbContent.Queryable<Currency>().ToListAsync();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        var allUnitList = item.getDiffLogs.Where(x => x.ColumnName.Contains("单位Id"))
                            .Select(x => new { x.ChangeValue, x.OriginalValue }).ToList();
                        var changeBeforeValue = allUnitList.Where(x => !string.IsNullOrWhiteSpace(x.OriginalValue)).Select(x => x.OriginalValue.ToGuid()).ToList();
                        var changeAfterValue = allUnitList.Where(x => !string.IsNullOrWhiteSpace(x.ChangeValue)).Select(x => x.ChangeValue.ToGuid()).ToList();
                        changeBeforeValue.AddRange(changeAfterValue);
                        var dealingUnit = await dbContent.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && changeBeforeValue.Contains(x.PomId.Value)).ToListAsync();
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue) || item.getDiffLogs[i].OriginalValue == Guid.Empty.ToString())
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue) || item.getDiffLogs[i].ChangeValue == Guid.Empty.ToString())
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if ((item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空") || item.getDiffLogs[i].ColumnName.Contains("项目日报Id") || item.getDiffLogs[i].ColumnName.Contains("填报月份"))
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("产值属性"))
                            {
                                item.getDiffLogs[i].ColumnName = "产值属性";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "自有";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "分包";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "自有";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "分包";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("文件"))
                            {
                                item.getDiffLogs[i].ColumnName = "文件";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = item.getDiffLogs[i].OriginalValue.Split(",").Count() + "个文件";
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = item.getDiffLogs[i].ChangeValue.Split(",").Count() + "个文件";
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("自有船舶Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "自有船舶";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = ownerShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = ownerShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("币种Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "币种";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = currency.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.Zcurrencyname;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = currency.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.Zcurrencyname;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("分包船舶Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue =
                                        subShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.Name == null ?
                                        dealingUnit.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.ZBPNAME_ZH == null ?
                                        dictionaryTable.FirstOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue)?.Name == null ?
                                        item.getDiffLogs[i].OriginalValue :
                                        dictionaryTable.FirstOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue)?.Name
                                        : dealingUnit.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.ZBPNAME_ZH :
                                        subShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue)?.Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue =
                                        subShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.Name == null ?
                                        dealingUnit.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.ZBPNAME_ZH == null ?
                                        dictionaryTable.FirstOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue)?.Name == null ?
                                        item.getDiffLogs[i].ChangeValue :
                                        dictionaryTable.FirstOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue)?.Name
                                        : dealingUnit.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.ZBPNAME_ZH :
                                        subShip.FirstOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue)?.Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("施工分类"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目id") || item.getDiffLogs[i].ColumnName.Contains("项目 Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("天气"))
                            {
                                if (item.getDiffLogs[i].OriginalValue == "0")
                                {
                                    item.getDiffLogs[i].OriginalValue = "空";
                                }

                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 4 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue)?.Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 4 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue)?.Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("日报进程状态"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "填写步骤中";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "已提交";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue != "空")
                                    {
                                        if (item.getDiffLogs[i].ChangeValue == "1")
                                        {
                                            item.getDiffLogs[i].ChangeValue = "填写步骤中";
                                        }
                                        else if (item.getDiffLogs[i].ChangeValue == "2")
                                        {
                                            item.getDiffLogs[i].ChangeValue = "已提交";
                                        }
                                    }
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 安监日报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_safesupervisiondayreport)
                {
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("地方政府许可复工日期是否明确  1 明确 0 不明确"))
                            {
                                item.getDiffLogs[i].ColumnName = "地方政府许可复工日期是否明确";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "0")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "明确";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "不明确";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "0")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "明确";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "不明确";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("是否完成隶属单位复工审批  是 1 否 0"))
                            {
                                item.getDiffLogs[i].ColumnName = "是否完成隶属单位复工审批";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "0")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "是";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "否";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "0")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "是";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "否";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目复工状态（1：春节未复工，2：未开工或停工未复工，3：已复工，4：已完工）"))
                            {
                                item.getDiffLogs[i].ColumnName = "项目复工状态";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "春节未复工";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "未开工或停工未复工";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "3")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "已复工";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "4")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "已完工";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "春节未复工";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "未开工或停工未复工";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "3")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "已复工";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "4")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "已完工";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("是否接受过上级督查  是 1  否 0"))
                            {
                                item.getDiffLogs[i].ColumnName = "是否接受过上级督查";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "0")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "是";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "否";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "0")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "是";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "否";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("上级督查形式(1:远程督查,2:现场督查)"))
                            {
                                item.getDiffLogs[i].ColumnName = "上级督查形式";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "远程督查";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "现场督查";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "远程督查";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "现场督查";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("当日安全生产情况(1:安全，2：事故)"))
                            {
                                item.getDiffLogs[i].ColumnName = "是否接受过上级督查";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "安全";
                                    }
                                    else if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "事故";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "安全";
                                    }
                                    else if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "事故";
                                    }
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 船舶日报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_shipdayreport)
                {
                    var portDataawait = await dbContent.Queryable<PortData>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("所在港口"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = portDataawait.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = portDataawait.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("所在港口"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 5 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 5 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 项目月报
                //项目月报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_monthreport)
                {
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var shipName = await dbContent.Queryable<OwnerShip>().ToListAsync();
                    var constructionQualification = await dbContent.Queryable<ConstructionQualification>().ToListAsync();
                    var projectWBS = await dbContent.Queryable<ProjectWBS>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    var subShip = await dbContent.Queryable<SubShip>().ToListAsync();
                    var dateunit = await dbContent.Queryable<DealingUnit>().ToListAsync();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>(),
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("产值属性"))
                            {
                                item.getDiffLogs[i].ColumnName = "产值属性";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    if (item.getDiffLogs[i].OriginalValue == "1")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "自有";
                                    }
                                    if (item.getDiffLogs[i].OriginalValue == "2")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "分包";
                                    }
                                    if (item.getDiffLogs[i].OriginalValue == "3")
                                    {
                                        item.getDiffLogs[i].OriginalValue = "自有-分包";
                                    }
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    if (item.getDiffLogs[i].ChangeValue == "1")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "自有";
                                    }
                                    if (item.getDiffLogs[i].ChangeValue == "2")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "分包";
                                    }
                                    if (item.getDiffLogs[i].ChangeValue == "3")
                                    {
                                        item.getDiffLogs[i].ChangeValue = "自有-分包";
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船舶Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    var naturalResourcesId = item.getDiffLogs[i].ChangeValue;
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.Id.ToString() == naturalResourcesId)?.Name;
                                    if (item.getDiffLogs[i].OriginalValue == null)
                                    {
                                        item.getDiffLogs[i].OriginalValue = shipName.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.Name;
                                        if (item.getDiffLogs[i].OriginalValue == null)
                                        {
                                            item.getDiffLogs[i].OriginalValue = subShip.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.Name;
                                            if (item.getDiffLogs[i].OriginalValue == null)
                                            {
                                                item.getDiffLogs[i].OriginalValue = dateunit.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.ZBPNAME_ZH;
                                            }
                                        }
                                    }

                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    var naturalResourcesId = item.getDiffLogs[i].ChangeValue;
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.Id.ToString() == naturalResourcesId)?.Name;
                                    if (item.getDiffLogs[i].ChangeValue == null)
                                    {
                                        item.getDiffLogs[i].ChangeValue = shipName.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.Name;
                                        if (item.getDiffLogs[i].ChangeValue == null)
                                        {
                                            item.getDiffLogs[i].ChangeValue = subShip.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.Name;
                                            if (item.getDiffLogs[i].ChangeValue == null)
                                            {
                                                item.getDiffLogs[i].ChangeValue = dateunit.SingleOrDefault(x => x.PomId.ToString() == naturalResourcesId)?.ZBPNAME_ZH;
                                            }
                                        }
                                    }
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目id") || item.getDiffLogs[i].ColumnName.Contains("项目 Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目施工资质"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = constructionQualification.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = constructionQualification.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("施工分类"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = projectWBS.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            bool convert = false;
                            try
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    var guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                }
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            if (convert)
                            {
                                continue;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 自有船舶月报
                //自有船舶月报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_ownershipmonthreport)
                {
                    var shipWorkMode = await dbContent.Queryable<ShipWorkMode>().ToListAsync();
                    var soilGrade = await dbContent.Queryable<SoilGrade>().ToListAsync();
                    var soil = await dbContent.Queryable<Soil>().ToListAsync();
                    var shipWorkType = await dbContent.Queryable<ShipWorkType>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var ownerShip = await dbContent.Queryable<OwnerShip>().ToListAsync();
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    var waterCarriage = await dbContent.Queryable<WaterCarriage>().ToListAsync();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if ((item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空") || item.getDiffLogs[i].ColumnName == "自有船舶月报Id")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("合同清单类型(字典表typeno=9)"))
                            {
                                item.getDiffLogs[i].ColumnName = "合同清单类型";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 9 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 9 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("工艺方式Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "工艺方式";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = shipWorkMode.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = shipWorkMode.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("进场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("退场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "项目";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("工况级别Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "工况级别";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = waterCarriage.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Remarks;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = waterCarriage.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Remarks;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("疏浚吹填分类Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "疏浚吹填分类";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = shipWorkType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = shipWorkType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("自有船舶Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "自有船舶";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = ownerShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = ownerShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("疏浚土分类ID"))
                            {
                                item.getDiffLogs[i].ColumnName = "疏浚土分类";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = soil.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = soil.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("疏浚土分级Id"))
                            {
                                item.getDiffLogs[i].ColumnName = "疏浚土分级";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = soilGrade.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).StatusDescription;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = soilGrade.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).StatusDescription;
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 分包船舶月报
                //分包船舶月报
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_subshipmonthreport)
                {
                    var subShip = await dbContent.Queryable<Domain.Models.SubShip>().ToListAsync();
                    var project = await dbContent.Queryable<Project>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {
                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue))
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue))
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if (item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空")
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("合同清单类型(字典表typeno=9)"))
                            {
                                item.getDiffLogs[i].ColumnName = "合同清单类型";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 9 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 9 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("当前动态描述(字典表typeno=10)"))
                            {
                                item.getDiffLogs[i].ColumnName = "当前动态描述";
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 10 && x.Type.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = dictionaryTable.SingleOrDefault(x => x.TypeNo == 10 && x.Type.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("进场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("项目Id") || item.getDiffLogs[i].ColumnName.Contains("项目 Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = project.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("分包船舶Id"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = subShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = subShip.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("退场时间"))
                            {
                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = DateTime.Parse(item.getDiffLogs[i].OriginalValue).ToString("yyyy-MM-dd");
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = DateTime.Parse(item.getDiffLogs[i].ChangeValue).ToString("yyyy-MM-dd");
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
                #region 分包船舶编辑
                //分包船舶编辑
                else if (getProjectChangesRequestDto.ModuleType == LogSystemModuleType.t_subship)
                {
                    var shipStatus = await dbContent.Queryable<ShipStatus>().ToListAsync();
                    var shipClassic = await dbContent.Queryable<ShipClassic>().ToListAsync();
                    var shipPingType = await dbContent.Queryable<ShipPingType>().ToListAsync();
                    var dictionaryTable = await dbContent.Queryable<DictionaryTable>().ToListAsync();
                    var searchProjectChangesResponseDto = new List<SearchProjectChangesResponseDto>();
                    foreach (var item in logInformationLsit)
                    {

                        if (item.getDiffLogs.Count == 0)
                        {
                            continue;
                        }
                        SearchProjectChangesResponseDto content = new SearchProjectChangesResponseDto()
                        {
                            Id = item.Id,
                            Name = item.OperationName,
                            DateTime = item.OperationTime,
                            TableName = item.OperationObjectName,
                            operationRecords = new List<OperationRecords>()
                        };
                        //content.LogInformation = item.OperationName + "在" + item.OperationTime + "将" + item.OperationObjectName + "表中的";
                        for (int i = 0; i < item.getDiffLogs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].OriginalValue) || item.getDiffLogs[i].OriginalValue == Guid.Empty.ToString())
                            {
                                item.getDiffLogs[i].OriginalValue = "空";
                            }
                            if (string.IsNullOrWhiteSpace(item.getDiffLogs[i].ChangeValue) || item.getDiffLogs[i].OriginalValue == Guid.Empty.ToString())
                            {
                                item.getDiffLogs[i].ChangeValue = "空";
                            }
                            if ((item.getDiffLogs[i].OriginalValue == "空" && item.getDiffLogs[i].ChangeValue == "空") || item.getDiffLogs[i].ColumnName.Contains("船舶GUID") || item.getDiffLogs[i].ColumnName.Contains("所属公司Id"))
                            {
                                continue;
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船舶类型GUID"))
                            {
                                item.getDiffLogs[i].ColumnName = "船舶类型";

                                if (item.getDiffLogs[i].OriginalValue != "空" && item.getDiffLogs[i].OriginalValue != Guid.Empty.ToString())
                                {
                                    item.getDiffLogs[i].OriginalValue = shipPingType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = shipPingType.SingleOrDefault(x => x.PomId.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船级社GUID"))
                            {
                                item.getDiffLogs[i].ColumnName = "船级社";

                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = shipClassic.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = shipClassic.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            if (item.getDiffLogs[i].ColumnName.Contains("船舶状态GUID"))
                            {
                                item.getDiffLogs[i].ColumnName = "船舶状态";

                                if (item.getDiffLogs[i].OriginalValue != "空")
                                {
                                    item.getDiffLogs[i].OriginalValue = shipStatus.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].OriginalValue).Name;
                                }
                                if (item.getDiffLogs[i].ChangeValue != "空")
                                {
                                    item.getDiffLogs[i].ChangeValue = shipStatus.SingleOrDefault(x => x.Id.ToString() == item.getDiffLogs[i].ChangeValue).Name;
                                }
                            }
                            bool convert = false;
                            try
                            {
                                var guid = item.getDiffLogs[i].ChangeValue.ToGuid();
                                guid = item.getDiffLogs[i].OriginalValue.ToGuid();
                                convert = true;
                            }
                            catch
                            {
                                convert = false;
                            }
                            OperationRecords operationRecords = new OperationRecords()
                            {
                                LoggingId = Guid.NewGuid(),
                                FieldName = item.getDiffLogs[i].ColumnName,
                                Original = item.getDiffLogs[i].OriginalValue,
                                Modified = item.getDiffLogs[i].ChangeValue
                            };
                            //content.LogInformation += (item.getDiffLogs.Count < i) ? item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"、" : item.getDiffLogs[i].ColumnName + "由\"" + item.getDiffLogs[i].OriginalValue + "\"改为\"" + item.getDiffLogs[i].ChangeValue + "\"。";
                            content.operationRecords.Add(operationRecords);
                        }
                        searchProjectChangesResponseDto.Add(content);
                    }
                    responseAjaxResult.Data = searchProjectChangesResponseDto;
                    responseAjaxResult.Count = logInformation.Result.Count;
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.LogError("Http记录日志出现错误;", ex);
            }
            #endregion
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
