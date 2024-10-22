using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData;
using GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData;
using GDCMasterDataReceiveApi.Domain.Models;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    sql = "SELECT  Id,PId FROM ( select dealunitmdcode  Id ,COUNT(1) PId from GDCMDM.t_financialinstitution GROUP BY dealunitmdcode  ) WHERE PId>1";
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
