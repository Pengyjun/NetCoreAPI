using AutoMapper;
using Dm.filter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System.Net.NetworkInformation;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.GovernanceData
{

    /// <summary>
    /// 数据治理实现层
    /// </summary>
    public class GovernanceDataService : IGovernanceDataService
    {
        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GovernanceDataService> logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GovernanceDataService(ISqlSugarClient dbContext, IMapper mapper, ILogger<GovernanceDataService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.logger = logger;
        }
        #endregion
        /// <summary>
        /// 治理数据  1是金融机构  2是物资明细编码  3 是往来单位数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<bool> GovernanceDataAsync(int type = 1)
        {
            var flag = false;
            try
            {
                var sql = string.Empty;
                List<long> deleteIds = new List<long>();
                #region 处理数据
                if (type == 1)
                {
                    //金融机构
                    sql = "SELECT  Id,PId FROM ( select mdcode  Id ,COUNT(1) PId from GDCMDM.t_financialinstitution GROUP BY mdcode  ) WHERE PId>1";
                    var res = _dbContext.Ado.SqlQuery<DbQueryResponseDto>(sql);
                    //查询所有ID
                    var ids = res.Select(x => x.Id).ToList();
                    var financialInstitutionList = await _dbContext.Queryable<FinancialInstitution>().Where(x => x.IsDelete == 1 && ids.Contains(x.ZFINC)).ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = financialInstitutionList.Where(x => x.ZFINC == item.Id).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }

                }
                else if (type == 2)
                {
                    //物资明细编码
                    sql = "SELECT  Id,PId FROM ( select mdcode  Id ,COUNT(1) PId from GDCMDM.t_devicedetailcode GROUP BY mdcode  ) WHERE PId>1";
                    var res = _dbContext.Ado.SqlQuery<DbQueryResponseDto>(sql);
                    //查询所有ID
                    var ids = res.Select(x => x.Id).ToList();
                    var financialInstitutionList = await _dbContext.Queryable<DeviceDetailCode>().Where(x => x.IsDelete == 1 && ids.Contains(x.ZMATERIAL)).ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = financialInstitutionList.Where(x => x.ZMATERIAL == item.Id).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 3)
                {
                    //金融机构
                    sql = "SELECT  Id,PId FROM ( select dealunitmdcode  Id ,COUNT(1) PId from GDCMDM.t_corresunit GROUP BY dealunitmdcode  ) WHERE PId>1";
                    var res = _dbContext.Ado.SqlQuery<DbQueryResponseDto>(sql);
                    //查询所有ID
                    var ids = res.Select(x => x.Id).ToList();
                    var financialInstitutionList = await _dbContext.Queryable<CorresUnit>().Where(x => x.IsDelete == 1 && ids.Contains(x.ZBP)).ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = financialInstitutionList.Where(x => x.ZBP == item.Id).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 4)
                {
                    //DH生产经营管理组织
                    sql = "SELECT ztreeid, zaco, COUNT(*) as count FROM GDCMDM.t_dh_mdmmanagementorgage GROUP BY ztreeid, zaco HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHMdmManagementOrgage>(sql);
                    //查询联合主键 zaco  ztreeid
                    var zacos = res.Select(x => x.Zaco).ToList();
                    var ztreeids = res.Select(x => x.Ztreeid).ToList();
                    var dhList = await _dbContext.Queryable<DHMdmManagementOrgage>()
                        .Where(x => x.IsDelete == 1 && zacos.Contains(x.Zaco) && ztreeids.Contains(x.Ztreeid))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.Zaco == item.Zaco && x.Ztreeid == item.Ztreeid).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 5)
                {
                    //DH核算部门
                    sql = "SELECT zdcode, COUNT(*) as count FROM GDCMDM.t_dh_accountingdept GROUP BY zdcode HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHAccountingDept>(sql);
                    //查询主键Zdcode
                    var zdcodes = res.Select(x => x.Zdcode).ToList();
                    var dhList = await _dbContext.Queryable<DHAccountingDept>()
                        .Where(x => x.IsDelete == 1 && zdcodes.Contains(x.Zdcode))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.Zdcode == item.Zdcode).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 6)
                {
                    //DH机构
                    sql = "SELECT oid, COUNT(*) as count FROM GDCMDM.t_dh_organzation GROUP BY oid HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHOrganzation>(sql);
                    //查询主键Oids
                    var oids = res.Select(x => x.OID).ToList();
                    var dhList = await _dbContext.Queryable<DHOrganzation>()
                        .Where(x => x.IsDelete == 1 && oids.Contains(x.OID))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.OID == item.OID).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 7)
                {
                    //DH行政和核算机构映射
                    sql = "SELECT fzid, COUNT(*) as count FROM GDCMDM.t_dh_administrative GROUP BY fzid HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHAdministrative>(sql);
                    //查询主键Fzid
                    var fzids = res.Select(x => x.Fzid).ToList();
                    var dhList = await _dbContext.Queryable<DHAdministrative>()
                        .Where(x => x.IsDelete == 1 && fzids.Contains(x.Fzid))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.Fzid == item.Fzid).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 8)
                {
                    //DH行政机构(多组织)
                    sql = "SELECT mdmcode, COUNT(*) as count FROM GDCMDM.t_dh_organzationdep GROUP BY mdmcode HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHOrganzationDep>(sql);
                    //查询主键MdmCode
                    var mdmcodes = res.Select(x => x.MdmCode).ToList();
                    var dhList = await _dbContext.Queryable<DHOrganzationDep>()
                        .Where(x => x.IsDelete == 1 && mdmcodes.Contains(x.MdmCode))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.MdmCode == item.MdmCode).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 9)
                {
                    //DH核算机构(多组织)
                    sql = "SELECT zaco, COUNT(*) as count FROM GDCMDM.t_dh_adjustaccountsmultipleorg GROUP BY zaco HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHAdjustAccountsMultipleOrg>(sql);
                    //查询主键zaco
                    var zacos = res.Select(x => x.Zaco).ToList();
                    var dhList = await _dbContext.Queryable<DHAdjustAccountsMultipleOrg>()
                        .Where(x => x.IsDelete == 1 && zacos.Contains(x.Zaco))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.Zaco == item.Zaco).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 10)
                {
                    //DH项目信息写入
                    sql = "SELECT zproject, COUNT(*) as count FROM GDCMDM.t_dh_projects GROUP BY zproject HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHProjects>(sql);
                    //查询主键ZPROJECT
                    var zpjects = res.Select(x => x.ZPROJECT).ToList();
                    var dhList = await _dbContext.Queryable<DHProjects>()
                        .Where(x => x.IsDelete == 1 && zpjects.Contains(x.ZPROJECT))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.ZPROJECT == item.ZPROJECT).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 11)
                {
                    //DH商机项目
                    sql = "SELECT zbop, COUNT(*) as count FROM GDCMDM.t_dh_opportunity GROUP BY zbop HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHOpportunity>(sql);
                    //查询主键zbop
                    var zbops = res.Select(x => x.ZBOP).ToList();
                    var dhList = await _dbContext.Queryable<DHOpportunity>()
                        .Where(x => x.IsDelete == 1 && zbops.Contains(x.ZBOP))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.ZBOP == item.ZBOP).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 12)
                {
                    //DH科研项目
                    sql = "SELECT fzsrpcode, COUNT(*) as count FROM GDCMDM.t_dh_research GROUP BY fzsrpcode HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHResearch>(sql);
                    //查询主键FzsrpCode
                    var fzsrpCodes = res.Select(x => x.FzsrpCode).ToList();
                    var dhList = await _dbContext.Queryable<DHResearch>()
                        .Where(x => x.IsDelete == 1 && fzsrpCodes.Contains(x.FzsrpCode))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.FzsrpCode == item.FzsrpCode).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                else if (type == 13)
                {
                    //DH委托关系
                    sql = "SELECT zmviewflag, znumc4x,mdmcode, COUNT(*) as count FROM GDCMDM.t_dh_mdmmultorgagencyrelpage GROUP BY mdmcode, zmviewflag,znumc4x HAVING COUNT(*) > 1";
                    var res = _dbContext.Ado.SqlQuery<DHMdmMultOrgAgencyRelPage>(sql);
                    //查询联合主键 MdmCode  Znumc4x ZmviewFlag
                    var zmviewFlags = res.Select(x => x.ZmviewFlag).ToList();
                    var znumc4xs = res.Select(x => x.Znumc4x).ToList();
                    var mdmCodes = res.Select(x => x.MdmCode).ToList();
                    var dhList = await _dbContext.Queryable<DHMdmMultOrgAgencyRelPage>()
                        .Where(x => x.IsDelete == 1 && zmviewFlags.Contains(x.ZmviewFlag) && znumc4xs.Contains(x.Znumc4x) && mdmCodes.Contains(x.MdmCode))
                        .ToListAsync();

                    foreach (var item in res)
                    {
                        var deleteData = dhList.Where(x => x.ZmviewFlag == item.ZmviewFlag && x.Znumc4x == item.Znumc4x && x.MdmCode == item.MdmCode).OrderBy(x => x.CreateTime).ToList();
                        if (deleteData.Count == 2)
                        {
                            var dataIds = deleteData.OrderBy(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.Add(dataIds.Id);
                            }
                        }
                        else if (deleteData.Count > 2)
                        {
                            var dataIds = deleteData.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                            if (dataIds != null)
                            {
                                deleteIds.AddRange(deleteData.Where(x => x.CreateTime != dataIds.CreateTime).Select(x => x.Id).ToList());
                            }
                        }
                    }
                }
                #endregion

                #region 删除数据
                if (deleteIds.Count > 0)
                {
                    if (type == 1)
                    {
                        await _dbContext.Deleteable<FinancialInstitution>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 2)
                    {
                        await _dbContext.Deleteable<DeviceDetailCode>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 3)
                    {
                        await _dbContext.Deleteable<CorresUnit>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 4)
                    {
                        await _dbContext.Deleteable<DHMdmManagementOrgage>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 5)
                    {
                        await _dbContext.Deleteable<DHAccountingDept>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 6)
                    {
                        await _dbContext.Deleteable<DHOrganzation>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 7)
                    {
                        await _dbContext.Deleteable<DHAdministrative>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 8)
                    {
                        await _dbContext.Deleteable<DHOrganzationDep>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 9)
                    {
                        await _dbContext.Deleteable<DHAdjustAccountsMultipleOrg>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 10)
                    {
                        await _dbContext.Deleteable<DHProjects>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 11)
                    {
                        await _dbContext.Deleteable<DHOpportunity>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 12)
                    {
                        await _dbContext.Deleteable<DHResearch>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                    else if (type == 13)
                    {
                        await _dbContext.Deleteable<DHMdmMultOrgAgencyRelPage>().In(deleteIds).ExecuteCommandAsync();
                        flag = true;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"数据治理出现错误:类型:{type}: 异常:{ex}");
            }
            return flag;
        }

        #region 数据资源
        /// <summary>
        /// 获取数据资源列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<MetaDataDto>>> SearchMetaDataAsync(BaseRequestDto requestDto)
        {
            ResponseAjaxResult<List<MetaDataDto>> responseAjaxResult = new();
            RefAsync<int> total = 0;

            var dbColumns = _dbContext.DbMaintenance.GetColumnInfosByTableName(requestDto.TableName, false);
            var dr = dbColumns
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => x.DbColumnName.Contains(requestDto.KeyWords))
                .Select(x => new MetaDataDto
                {
                    IsPrimaryKey = x.IsPrimarykey,
                    ColumComment = x.ColumnDescription,
                    ColumName = x.DbColumnName,
                    DataLength = x.Length,
                    DataType = x.OracleDataType,
                    IsAllowToBeEmpty = x.IsNullable,
                    DataDecimalPlaces = x.Scale,
                    DefaultValue = x.DefaultValue
                })
                .ToList();

            if (!requestDto.IsFullExport)
                responseAjaxResult.SuccessResult(dr.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList());
            else
                responseAjaxResult.SuccessResult(dr.ToList());

            responseAjaxResult.Count = dr.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取数据资源所有表
        /// </summary>
        /// <returns></returns>
        public ResponseAjaxResult<List<Tables>> SearchTables(int type)
        {
            ResponseAjaxResult<List<Tables>> responseAjaxResult = new();
            List<string> ignoreTable = new List<string>();
            ignoreTable.Add("t_auditlogs");
            var dti = _dbContext.DbMaintenance.GetTableInfoList(false)
                .WhereIF(type == 3, x => x.Name.Contains("t_dh_"))
                .Where(x => !ignoreTable.Contains(x.Name))
                .Select(x => new Tables()
                {
                    TableName = x.Name,
                    TName = GetTableComment(x.Name)
                }).ToList();

            responseAjaxResult.Data = dti;
            responseAjaxResult.Count = dti.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取表注释的自定义方法
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetTableComment(string tableName)
        {
            // 查询达梦数据库中表的注释
            var sql = $"SELECT COMMENTS FROM USER_TAB_COMMENTS WHERE TABLE_NAME = '{tableName}'";

            // 执行 SQL 查询
            var result = _dbContext.Ado.SqlQuery<string>(sql).FirstOrDefault();
            return result ?? string.Empty; // 如果没有注释，返回空字符串
        }
        /// <summary>
        /// 增改数据资源列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveMetaDataAsync(MetaDataRequestDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();
            //传递过来的列名
            var columnName = requestDto.Mds.ColumName.ToLower().TrimAll();
            List<MetaDataRelation> dtmdr = new();
            var columnsInfo = _dbContext.DbMaintenance.GetColumnInfosByTableName(requestDto.TableName, false)
                 .Where(x => SqlFunc.Equals(x.DbColumnName.ToLower(), columnName))
                 .Select(x => new MetaDataDto
                 {
                     IsPrimaryKey = x.IsPrimarykey,
                     ColumComment = x.ColumnDescription,
                     ColumName = x.DbColumnName,
                     DataLength = x.Length,
                     DataType = x.OracleDataType,
                     IsAllowToBeEmpty = x.IsNullable,
                     DataDecimalPlaces = x.Scale
                 })
                 .FirstOrDefault();

            if (requestDto.Type == 1 && columnsInfo == null)//新增字段
            {
                DbColumnInfo dbColumn = new DbColumnInfo();
                try
                {
                    // 添加列操作
                    dbColumn = new DbColumnInfo
                    {
                        Length =/* requestDto.Mds.DataLength*/requestDto.Mds.DataType == "int" ? 0 : requestDto.Mds.DataLength,
                        IsPrimarykey = requestDto.Mds.IsPrimaryKey,
                        TableName = requestDto.TableName,
                        DataType = requestDto.Mds.DataType,
                        IsNullable = requestDto.Mds.IsAllowToBeEmpty,
                        DbColumnName = requestDto.Mds.ColumName.ToLower(),
                        DefaultValue = requestDto.Mds.DefaultValue,
                        ColumnDescription = requestDto.Mds.ColumComment,
                        DecimalDigits = requestDto.Mds.DataDecimalPlaces
                    };
                    //添加列基本信息
                    _dbContext.DbMaintenance.AddColumn(requestDto.TableName, dbColumn);
                    //添加列备注
                    _dbContext.DbMaintenance.AddColumnRemark(requestDto.Mds.ColumName.ToLower(), requestDto.TableName, requestDto.Mds.ColumComment);
                    //添加默认值
                    _dbContext.DbMaintenance.AddDefaultValue(requestDto.TableName, requestDto.Mds.ColumName.ToLower(), requestDto.Mds.DefaultValue);
                    // 添加到副表
                    dtmdr.Add(new MetaDataRelation
                    {
                        ColumnsLength = requestDto.Mds.DataType == "int" ? 10 : requestDto.Mds.DataLength,
                        ColumnsName = requestDto.Mds.ColumName.ToLower(),
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        TableName = requestDto.TableName,
                        CreateTime = DateTime.Now,
                    });
                    await _dbContext.Insertable(dtmdr).ExecuteCommandAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"添加数据表列添加出现异常");
                    try
                    {
                        _dbContext.DbMaintenance.DropColumn(requestDto.TableName, requestDto.Mds.ColumName.ToLower());
                    }
                    catch (Exception ex1)
                    {
                    }
                }

            }
            else if (requestDto.Type == 2 && columnsInfo != null)//修改字段
            {
                //根据表获取关联表初始字段长度
                var columnLength = await _dbContext.Queryable<MetaDataRelation>()
                    .Where(t => t.IsDelete == 1 && t.TableName == requestDto.TableName && t.ColumnsName == columnName)
                    .FirstAsync();
                if (requestDto.Mds.DataLength < columnLength.ColumnsLength)
                {
                    responseAjaxResult.SuccessResult(false, "字段:" + columnLength.ColumnsName + "小于初始设定长度:" + columnLength.ColumnsLength);
                    return responseAjaxResult;
                }
                var dbColumn = new DbColumnInfo
                {
                    Length = requestDto.Mds.DataLength,
                    IsPrimarykey = requestDto.Mds.IsPrimaryKey,
                    TableName = requestDto.TableName,
                    DataType = requestDto.Mds.DataType,
                    IsNullable = requestDto.Mds.IsAllowToBeEmpty,
                    DbColumnName = requestDto.Mds.ColumName.ToLower(),
                    DecimalDigits = requestDto.Mds.DataDecimalPlaces
                };

                //添加列基本信息
                _dbContext.DbMaintenance.UpdateColumn(requestDto.TableName, dbColumn);
                ////添加列备注
                _dbContext.DbMaintenance.AddColumnRemark(requestDto.Mds.ColumName.ToLower(), requestDto.TableName, requestDto.Mds.ColumComment);
                ////添加默认值
                _dbContext.DbMaintenance.AddDefaultValue(requestDto.TableName, requestDto.Mds.ColumName.ToLower(), requestDto.Mds.DefaultValue);

                //副表增加字段
                dtmdr.Add(new MetaDataRelation
                {
                    ColumnsLength = requestDto.Mds.DataLength,
                    ColumnsName = requestDto.Mds.ColumName.ToPascal(),
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    TableName = requestDto.TableName,
                    UpdateTime = DateTime.Now
                });
                await _dbContext.Updateable(dtmdr).WhereColumns(t => t.Id).ExecuteCommandAsync();
                responseAjaxResult.SuccessResult(true);
            }
            else if (requestDto.Type == 3 && columnsInfo != null)
            {
                //删除操作
                _dbContext.DbMaintenance.DropColumn(requestDto.TableName, requestDto.Mds.ColumName.ToLower());
                var isExist = await _dbContext.Queryable<MetaDataRelation>().Where(x => x.TableName == requestDto.TableName && x.IsDelete == 1 && x.ColumnsName ==
                requestDto.Mds.ColumName.ToLower()).FirstAsync();
                isExist.IsDelete = 0;
                await _dbContext.Updateable(isExist).ExecuteCommandAsync();
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取数据类型信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ColumnsInfo>>> GetColumnsTypesAsync()
        {
            ResponseAjaxResult<List<ColumnsInfo>> responseAjaxResult = new();
            List<ColumnsInfo> rt = new();

            var dt = await _dbContext.Ado.SqlQueryAsync<ColumnsInfoMapper>("SELECT TYPE_NAME,COLUMN_SIZE FROM SYSTYPEINFOS;");
            rt = dt.GroupBy(x => new { x.TYPE_NAME, x.COLUMN_SIZE })
                .Select(x => new ColumnsInfo
                {
                    ColumnSize = x.Key.COLUMN_SIZE,
                    TypeName = x.Key.TYPE_NAME
                }).ToList();

            responseAjaxResult.SuccessResult(rt);
            return responseAjaxResult;
        }
        #endregion

        #region 数据质量
        /// <summary>
        /// 规则配置列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DataQualityResponseDto>>> SearchTableDataAsync(DataQualityRequestDto requestDto)
        {
            ResponseAjaxResult<List<DataQualityResponseDto>> responseAjaxResult = new();
            List<DataQualityResponseDto> rt = new();
            RefAsync<int> total = 0;

            var dt = await _dbContext.Queryable<DataGruleSetting>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), t => t.Column.Contains(requestDto.KeyWords) || t.Table.Contains(requestDto.KeyWords))
                .Where(t => t.IsDelete == 1 && requestDto.Type == (int)t.Type)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
            dt = dt.OrderByDescending(t => t.Grade).ThenByDescending(x => x.CreateTime).ToList();

            foreach (var item in dt)
            {
                rt.Add(new DataQualityResponseDto
                {
                    Column = item.Column,
                    Grade = item.Grade,
                    Name = item.Name,
                    Soure = item.Soure,
                    Status = item.Status,
                    Table = item.Table,
                    Type = item.Type,
                    ColumnName = item.ColumnName,
                    Hour = item.Hour,
                    TableName = item.TableName,
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(rt);
            return responseAjaxResult;
        }
        /// <summary>
        /// 保存数据规则配置
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveDataQualityAsync(SaveDataQualityDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();
            if (requestDto.DQ != null)
            {
                DataGruleSetting rt = new();

                if (requestDto.Type == 1)
                {
                    var dquailty = await _dbContext.Queryable<DataGruleSetting>()
                        .Where(t => t.IsDelete == 1 && requestDto.DQ.Type == t.Type && requestDto.DQ.Table == t.Table && requestDto.DQ.Column == t.Column)
                        .FirstAsync();
                    if (string.IsNullOrWhiteSpace(requestDto.DQ.Id))
                    {
                        rt = new DataGruleSetting
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            Column = requestDto.DQ.Column,
                            CreateTime = DateTime.Now,
                            Grade = requestDto.DQ.Grade,
                            Name = requestDto.DQ.Name,
                            Soure = requestDto.DQ.Soure,
                            Status = requestDto.DQ.Status,
                            Table = requestDto.DQ.Table,
                            Type = requestDto.DQ.Type,
                            ColumnName = requestDto.DQ.ColumnName,
                            TableName = requestDto.DQ.TableName,
                            Hour = requestDto.DQ.Hour
                        };

                        await _dbContext.Insertable(rt).ExecuteCommandAsync();
                        responseAjaxResult.SuccessResult(true);
                        return responseAjaxResult;
                    }
                }
                else if (requestDto.Type == 2)
                {
                    if (!string.IsNullOrWhiteSpace(requestDto.DQ.Id))
                    {
                        var dquailty = await _dbContext.Queryable<DataGruleSetting>()
                            .Where(t => t.IsDelete == 1 && requestDto.DQ.Id == t.Id.ToString())
                            .FirstAsync();
                        if (dquailty != null)
                        {
                            dquailty.Column = requestDto.DQ.Column;
                            dquailty.UpdateTime = DateTime.Now;
                            dquailty.Grade = requestDto.DQ.Grade;
                            dquailty.Name = requestDto.DQ.Name;
                            dquailty.Soure = requestDto.DQ.Soure;
                            dquailty.Status = requestDto.DQ.Status;
                            dquailty.Table = requestDto.DQ.Table;
                            dquailty.Type = requestDto.DQ.Type;
                            dquailty.ColumnName = requestDto.DQ.ColumnName;
                            dquailty.TableName = requestDto.DQ.TableName;
                            dquailty.Hour = requestDto.DQ.Hour;

                            await _dbContext.Updateable(dquailty).WhereColumns(x => x.Id).ExecuteCommandAsync();
                            responseAjaxResult.SuccessResult(true);
                            return responseAjaxResult;
                        }
                    }
                }
                else if (requestDto.Type == 3)
                {
                    if (!string.IsNullOrWhiteSpace(requestDto.DQ.Id))
                    {
                        var dquailty = await _dbContext.Queryable<DataGruleSetting>()
                      .Where(t => t.IsDelete == 1 && requestDto.DQ.Id == t.Id.ToString())
                      .FirstAsync();
                        if (dquailty != null)
                        {
                            dquailty.IsDelete = 0;
                            dquailty.DeleteTime = DateTime.Now;

                            await _dbContext.Updateable(dquailty).ExecuteCommandAsync();
                            responseAjaxResult.SuccessResult(true);
                            return responseAjaxResult;
                        }
                    }
                }
            }
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 数据质量报告列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DataReportResponseDto>>> SearchDataQualityReportAsync(DataReportRequestDto requestDto)
        {
            ResponseAjaxResult<List<DataReportResponseDto>> responseAjaxResult = new();
            List<DataReportResponseDto> rt = new();

            //获取规则当前表规则配置
            var tbGrules = await _dbContext.Queryable<DataGruleSetting>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.Table), t => t.Table == requestDto.Table)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), t => t.Name.Contains(requestDto.KeyWords))
                .Where(t => t.IsDelete == 1 && t.Status == "1")
                .ToListAsync();

            var gtb = tbGrules.GroupBy(x => new { x.Table, x.Grade }).ToList();
            foreach (var item in gtb)
            {
                var grules = tbGrules.Where(x => x.Table == item.Key.Table && x.Grade == item.Key.Grade).ToList();
                //每张表的规则查询数据
                foreach (var item2 in grules)
                {
                    string ids = string.Empty;
                    if (item2.Type == GruleType.WZX)
                    {
                        ids = _dbContext.Queryable<object>()
                            .AS(item2.Table.ToLower())
                            .Where($@"{item2.Column.ToLower()} is null or {item2.Column.ToLower()} = ''")
                            .Select($"CAST(id AS VARCHAR) AS id")
                            .ToJson();
                    }
                    else if (item2.Type == GruleType.WYX)
                    {
                        ids = _dbContext.Queryable<object>()
                            .AS(item2.Table.ToLower())
                            .GroupBy(item2.Column.ToLower())
                            .Having($"count({item2.Column.ToLower()}) > 1")
                            .Select($"CAST(id AS VARCHAR) AS id")
                            .ToJson();
                    }
                    else if (item2.Type == GruleType.YXX)
                    {
                        // 获取当前时间，并减去 传入 小时
                        var currentTime = DateTime.Now.AddHours(-item2.Hour);
                        var ctTimeStamp = new DateTimeOffset(currentTime).ToUnixTimeSeconds();
                        // 查询数据库，筛选出 timestamp 小于当前时间减 传入 小时的记录
                        ids = _dbContext.Queryable<object>()
                                 .AS(item2.Table.ToLower())
                                 .Where($"timestamp < {ctTimeStamp}")
                                 .Select($"CAST(id AS VARCHAR) AS id")
                                 .ToJson();
                    }
                    if (ids != null && ids.Any())
                    {
                        var js = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(ids);
                        List<string> its = new List<string>();
                        foreach (var it in js)
                        {
                            if (it.ContainsKey("id"))
                            {
                                its.Add(it["id"]);
                            }
                        }
                        foreach (var item3 in its)
                        {
                            rt.Add(new DataReportResponseDto
                            {
                                Id = item3,
                                Column = item2.ColumnName,
                                CreateTime = item2.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                Grade = item2.Grade,
                                Name = item2.Name,
                                Soure = item2.Soure,
                                Status = item2.Status,
                                Table = item2.TableName,
                                Type = item2.Type
                            });
                        }
                    }
                }
            }
            rt = rt.OrderByDescending(x => x.Grade).ThenBy(x => x.Table).ToList();
            responseAjaxResult.Count = rt.Count;

            if (!requestDto.IsFullExport)
            {
                responseAjaxResult.SuccessResult(rt.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList());
            }
            else
            {
                responseAjaxResult.SuccessResult(rt.ToList());
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<UserSearchDetailsDto>> GetUserDetailsByIdAsync(string id)
        {
            ResponseAjaxResult<UserSearchDetailsDto> responseAjaxResult = new();
            var rt = await _dbContext.Queryable<User>()
                .Where(t => t.Id.ToString() == id && t.IsDelete == 1)
                .Select(u => new UserSearchDetailsDto
                {
                    Id = u.Id.ToString(),
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "有效" : "禁用",
                    Phone = u.PHONE,
                    Attribute1 = u.ATTRIBUTE1,
                    Attribute2 = u.ATTRIBUTE2,
                    Attribute3 = u.ATTRIBUTE3,
                    Attribute4 = u.ATTRIBUTE4,
                    Attribute5 = u.ATTRIBUTE5,
                    Birthday = u.BIRTHDAY,
                    CertType = u.CERT_TYPE,
                    DispatchunitName = u.DISPATCHUNITNAME,
                    DispatchunitShortName = u.DISPATCHUNITSHORTNAME,
                    EmpSort = u.EMP_SORT,
                    EnName = u.EN_NAME,
                    EntryTime = u.ENTRY_TIME,
                    Externaluser = u.EXTERNALUSER,
                    Fax = u.FAX,
                    HighEstGrade = u.HIGHESTGRADE,
                    HrEmpCode = u.HR_EMP_CODE,
                    JobName = u.JOB_NAME,
                    JobType = u.JOB_TYPE,
                    NameSpell = u.NAME_SPELL,
                    Nation = u.NATION,
                    CountryRegion = u.NATIONALITY,
                    Nationality = u.NATIONALITY,
                    OfficeNum = u.OFFICE_NUM,
                    PoliticsFace = u.POLITICSFACE,
                    PositionGrade = u.POSITION_GRADE,
                    PositionGradeNorm = u.POSITIONGRADENORM,
                    PositionName = u.POSITION_NAME,
                    Positions = u.POSITIONS,
                    SameHighEstGrade = u.SAMEHIGHESTGRADE,
                    Sex = u.SEX == "01" ? "男性" : "女性",
                    Sno = u.SNO,
                    UserInfoStatus = u.EMP_STATUS,
                    SubDepts = u.SUB_DEPTS,
                    Tel = u.TEL,
                    UserLogin = u.USER_LOGIN,
                    CreateTime = u.CreateTime,
                    UpdateTime = u.UpdateTime,
                    OwnerSystem = u.OwnerSystem
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(rt);
            return responseAjaxResult;
        }
        /// <summary>
        /// 导出的数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<DataReportReportResponse>> GetUserInfosAsync()
        {
            //获取用户ids
            var response = await SearchDataQualityReportAsync(new DataReportRequestDto { IsFullExport = true, Table = "t_user" });
            var ids = response.Data.Select(x => x.Id).ToList();
            //获取用户
            var users = await _dbContext.Queryable<User>()
                .Where(t => t.IsDelete == 1 && ids.Contains(t.Id.ToString()))
                .Select(u => new DataReportReportResponse
                {
                    Name = u.NAME,
                    EmpCode = u.EMP_CODE,
                    UserInfoStatus = u.EMP_STATUS,
                    OfficeDepId = u.OFFICE_DEPID,
                })
                .ToListAsync();

            //机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME, OCode = t.OCODE })
                .ToListAsync();

            //值域信息
            var us = users.Where(x => !string.IsNullOrWhiteSpace(x.UserInfoStatus)).Select(x => x.UserInfoStatus).ToList();
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();
            var uStatus = valDomain.Where(x => us.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEMPSTATE").ToList();
            foreach (var uInfo in users)
            {
                uInfo.UserInfoStatus = uStatus.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.UserInfoStatus)?.ZDOM_NAME;
                uInfo.CompanyName = GetUserCompany(uInfo.OfficeDepId, institutions);
                uInfo.OfficeDepIdName = institutions.FirstOrDefault(x => x.Oid == uInfo.OfficeDepId)?.Name;
            }
            return users;
        } /// <summary>
          /// 获取用户所属公司
          /// </summary>
          /// <param name="oid"></param>
          /// <param name="uInstutionDtos"></param>
          /// <returns></returns>
        private string GetUserCompany(string? oid, List<InstutionRespDto> uInstutionDtos)
        {
            var uInsInfo = uInstutionDtos.FirstOrDefault(x => x.Oid == oid);
            if (uInsInfo != null)
            {
                //获取用户机构全部规则  反查机构信息
                if (!string.IsNullOrWhiteSpace(uInsInfo.Grule))
                {
                    var ruleIds = uInsInfo.Grule.Trim('-').Split('-').ToList();
                    var companyNames = ruleIds
                        .Select(id => uInstutionDtos.FirstOrDefault(inst => inst.Oid == id))
                        .Where(inst => inst != null)
                        .Select(inst => inst?.Name)
                        .ToList();

                    if (companyNames.Any())
                    {
                        return string.Join("/", companyNames);
                    }
                }
            }
            return string.Empty;
        }
        #endregion

        #region 数据标准
        /// <summary>
        /// 获取值域(左侧列表)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ValueDomainTypeResponseDto>>> SearchValueDomainTypeAsync()
        {
            ResponseAjaxResult<List<ValueDomainTypeResponseDto>> responseAjaxResult = new();

            var valueDomainTypeList = await _dbContext.Queryable<ValueDomain>()
                  .Where(x => x.IsDelete == 1)// && (x.ZDOM_CODE == "ZNATION" || x.ZDOM_CODE == "ZGENDER")
                  .Select(x => new ValueDomainTypeResponseDto() { Code = x.ZDOM_CODE, Desc = x.ZDOM_DESC })
                  .ToListAsync();
            valueDomainTypeList = valueDomainTypeList.GroupBy(i => i.Desc, (ii, key) => key.OrderByDescending(x => x.Desc).First()).ToList();

            responseAjaxResult.Count = valueDomainTypeList.Count;
            responseAjaxResult.SuccessResult(valueDomainTypeList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 标准列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DataStardardDto>>> SearchStardardAsync(DataStardardRequestDto requestDto)
        {
            ResponseAjaxResult<List<DataStardardDto>> responseAjaxResult = new();
            RefAsync<int> total = 0;

            var drt = await _dbContext.Queryable<ValueDomain>()
                  .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), t => t.ZDOM_VALUE.Contains(requestDto.KeyWords) || t.ZDOM_NAME.Contains(requestDto.KeyWords))
                  .Where(t => t.IsDelete == 1 && t.ZDOM_CODE == requestDto.Code)
                  .Select(t => new DataStardardDto
                  {
                      Id = t.Id.ToString(),
                      CreateTime = t.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                      TypeCode = t.ZDOM_CODE,
                      TypeName = t.ZDOM_DESC,
                      StardardCode = t.ZDOM_VALUE,
                      StardardName = t.ZDOM_NAME,
                      StatusName = "1",
                      UTime = t.UpdateTime
                  })
                  .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            foreach (var item in drt)
            {
                item.UpdateTime = string.IsNullOrWhiteSpace(item.UTime.ToString()) ? null : item.UTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(drt);
            return responseAjaxResult;
        }
        /// <summary>
        /// 保存数据标准
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveStardardAsync(SaveVDomainDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (requestDto.Vd != null)
            {
                ValueDomain tabVd = new();
                var vdomains = await _dbContext.Queryable<ValueDomain>()
                  .Where(x => x.IsDelete == 1 && (x.ZDOM_CODE == "ZNATION" || x.ZDOM_CODE == "ZGENDER"))
                  .ToListAsync();
                if (requestDto.Type == 1)
                {
                    //新增  是否存在相同的数据
                    var ivd = vdomains.FirstOrDefault(x => x.ZDOM_CODE == requestDto.Vd.ZDOM_CODE && x.ZDOM_DESC == requestDto.Vd.ZDOM_DESC && x.ZDOM_VALUE == requestDto.Vd.ZDOM_VALUE && x.ZDOM_NAME == requestDto.Vd.ZDOM_NAME);
                    if (ivd != null)
                    {
                        responseAjaxResult.SuccessResult(true, "数据存在");
                        return responseAjaxResult;
                    }
                    tabVd = new ValueDomain
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        ZCHTIME = requestDto.Vd.ZCHTIME,
                        ZDELETE = requestDto.Vd.ZDELETE,
                        ZDOM_CODE = requestDto.Vd.ZDOM_CODE,
                        ZDOM_DESC = requestDto.Vd.ZDOM_DESC,
                        ZDOM_LEVEL = requestDto.Vd.ZDOM_LEVEL,
                        ZDOM_NAME = requestDto.Vd.ZDOM_NAME,
                        ZDOM_SUP = requestDto.Vd.ZDOM_SUP,
                        ZDOM_VALUE = requestDto.Vd.ZDOM_VALUE,
                        ZREMARKS = requestDto.Vd.ZREMARKS,
                        ZSTATE = requestDto.Vd.ZSTATE,
                        ZVERSION = requestDto.Vd.ZVERSION
                    };
                    await _dbContext.Insertable(tabVd).ExecuteCommandAsync();

                    responseAjaxResult.SuccessResult(true);
                    return responseAjaxResult;
                }
                else if (requestDto.Type == 2)
                {
                    if (!string.IsNullOrEmpty(requestDto.Vd.Id))
                    {
                        var uvd = vdomains.FirstOrDefault(x => x.Id.ToString() == requestDto.Vd.Id);
                        if (uvd != null)
                        {
                            uvd.ZCHTIME = requestDto.Vd.ZCHTIME;
                            uvd.ZDOM_CODE = requestDto.Vd.ZDOM_CODE;
                            uvd.ZDOM_DESC = requestDto.Vd.ZDOM_DESC;
                            uvd.ZDOM_VALUE = requestDto.Vd.ZDOM_VALUE;
                            uvd.ZDOM_NAME = requestDto.Vd.ZDOM_NAME;
                            uvd.ZDOM_LEVEL = requestDto.Vd.ZDOM_LEVEL;
                            uvd.ZDOM_SUP = requestDto.Vd.ZDOM_SUP;
                            uvd.ZREMARKS = requestDto.Vd.ZREMARKS;
                            uvd.ZVERSION = requestDto.Vd.ZVERSION;
                            uvd.ZSTATE = "1";
                            uvd.UpdateTime = DateTime.Now;
                            await _dbContext.Updateable(uvd).WhereColumns(x => x.Id).ExecuteCommandAsync();
                            responseAjaxResult.SuccessResult(true);
                            return responseAjaxResult;
                        }
                    }
                }
                else if (requestDto.Type == 3)
                {
                    if (!string.IsNullOrEmpty(requestDto.Vd.Id))
                    {
                        var uvd = vdomains.FirstOrDefault(x => x.Id.ToString() == requestDto.Vd.Id);
                        if (uvd != null)
                        {
                            uvd.IsDelete = 0;
                            uvd.DeleteTime = DateTime.Now;
                            await _dbContext.Updateable(uvd).ExecuteCommandAsync();
                            responseAjaxResult.SuccessResult(true);
                            return responseAjaxResult;
                        }
                    }
                }
            }
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }

        #endregion
    }
}
