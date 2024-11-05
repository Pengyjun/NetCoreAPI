using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData;
using GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData;
using GDCMasterDataReceiveApi.Domain.Models;
using Microsoft.Extensions.Logging;
using SqlSugar;

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

    }
}
