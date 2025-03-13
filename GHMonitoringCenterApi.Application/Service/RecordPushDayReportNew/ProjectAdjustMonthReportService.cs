using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectAdjustProductionValueResponseDto = GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport.ProjectAdjustProductionValueResponseDto;

namespace GHMonitoringCenterApi.Application.Service.RecordPushDayReportNew
{
    /// <summary>
    /// 项目开累数调整实现层
    /// </summary>
    public class ProjectAdjustMonthReportService : IProjectAdjustMonthReportService
    {

        #region 依赖注入
        private readonly IMapper _mapper;
        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _dbContext;

        public ProjectAdjustMonthReportService(IMapper mapper, GlobalObject globalObject, ISqlSugarClient dbContext)
        {
            _mapper = mapper;
            _globalObject = globalObject;
            _dbContext = dbContext;
        }

        #endregion
        public async Task<ResponseAjaxResult<List<ProjectAdjustProductionValueResponseDto>>> SearchProjectAdjustMonthReportAsync(string projectId)
        {
            ResponseAjaxResult<List<ProjectAdjustProductionValueResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectAdjustProductionValueResponseDto>>();
            //项目ID
            var proId = projectId.ToGuid();
            //查询币种
            var currencyId= await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == proId).Select(x=>SqlFunc.ToString( x.CurrencyId)).FirstAsync();
            var year = DateTime.Now.Year;
            //查询汇率
            var exchangeRate= await _dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1&&x.Year== year && x.CurrencyId== currencyId).Select(x => x.ExchangeRate).FirstAsync();
            //查询项目wbs
            var projectWbsList = await _dbContext.Queryable<ProjectAdjustWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == projectId)
                 .Select(x => new ProjectAdjustProductionValueResponseDto() {
                     ConstructionClassificationName = x.Name,
                     ExchangeRate = exchangeRate.Value,
                     IsNew = 0,
                     NodeId = x.KeyId,
                     PNodeId = x.Pid,
                     UnitPrice = x.UnitPrice.Value,
                     ProjectId = proId,
                     WbsId = x.Id
                 })
                .ToListAsync();
            //项目的所有资源
            var projectResource= await _dbContext.Queryable<MonthReportDetail>().Where(x => x.IsDelete == 1&& x.ProjectId == proId).ToListAsync();
            //往来单位  对应资源
            var dealingUnitList = await _dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 &&(x.ZBPTYPE=="02"|| x.ZBPTYPE == "03")).ToListAsync();
            //自有船舶数据
            var owinShip = await _dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 ).ToListAsync();
            //分包船舶数据
            var subShip = await _dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1).ToListAsync();
            //数据组合
            foreach (var item in projectWbsList)
            {
               var currentNodeAllResource= projectResource.Where(x => x.ProjectWBSId == item.WbsId).ToList();
                item.Childs = new List<ProjectAdjustProductionValueResponseDto>();
                foreach (var  sub in currentNodeAllResource)
                {
                    var resourceName = string.Empty;
                    var outType = sub.OutPutType.ObjToInt() == 1 ? "自有" : sub.OutPutType.ObjToInt() == 2 ? "分包" : "分包-自有";
                    if (sub.OutPutType.ObjToInt() == 1)
                    {
                         resourceName = owinShip.Where(x => x.PomId == sub.ShipId).Select(x => x.Name).FirstOrDefault();
                    }
                    else if (sub.OutPutType.ObjToInt() == 2)
                    {
                         resourceName = subShip.Where(x => x.PomId == sub.ShipId).Select(x => x.Name).FirstOrDefault();
                    }
                    else if (sub.OutPutType.ObjToInt() ==3)
                    {
                        resourceName = dealingUnitList.Where(x => x.PomId == sub.ShipId).Select(x => x.ZBPNAME_ZH).FirstOrDefault();
                    }

                    item.Childs.Add(new ProjectAdjustProductionValueResponseDto()
                    {
                        ConstructionType = sub.ConstructionNature.ToString(),
                        ProductionProperty = sub.OutPutType.ToString(),
                        ProductionPropertyName = outType,
                        ResourceName = resourceName,
                        UnitPrice = sub.UnitPrice,
                        ExchangeRate = exchangeRate.Value,
                        WbsId = sub.ProjectWBSId,
                        IsNew = 0,
                        PNodeId = item.NodeId,

                        WorkQuantities = sub.CompletedQuantity,
                        ProductionValue = sub.CompleteProductionAmount,
                        OutsourcingExpenditure = sub.OutsourcingExpensesAmount,

                        SourceOutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                        SourceProductionValue = sub.CompleteProductionAmount,
                        SourceWorkQuantities = sub.CompletedQuantity
                    });
                }
            }

            return null;
        }
    }
}
