using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.ProjectAdjustMonthReport
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
        public async Task<ResponseAjaxResult<ProjectAdjustResponseDto>> SearchProjectAdjustMonthReportAsync(string projectId)
        {
            ResponseAjaxResult<ProjectAdjustResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectAdjustResponseDto>();
            ProjectAdjustResponseDto tempData = new ProjectAdjustResponseDto()
            { 
             projectAdjustProductionValueResponseDtos=new List<ProjectAdjustProductionValueResponseDto>()
            };
            //项目ID
            var proId = projectId.ToGuid();
            //查询字典表
            var dictTable = await _dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 7).ToListAsync();
            //查询项目
            var project = await _dbContext.Queryable<Project>()
                .InnerJoin<ProjectStatus>((x, y)=>x.StatusId==y.StatusId)
                .Where((x,y) => x.IsDelete == 1 && x.Id == proId).Select((x, y) => new {Name= x.Name,projectStatus= y.Name, Rate = x.Rate, }).FirstAsync();
            //查询币种
            var currencyId = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == proId).Select(x => SqlFunc.ToString(x.CurrencyId)).FirstAsync();
            var year = DateTime.Now.AddYears(-1).Year;
            //查询汇率
            var exchangeRate = await _dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1 && x.Year == year && x.CurrencyId == currencyId).Select(x => new { ExchangeRate= x.ExchangeRate,  Remark =x.Remark }).FirstAsync();
            //查询项目wbs
            var projectWbsList = await _dbContext.Queryable<ProjectAdjustWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == projectId)
                 .Select(x => new ProjectAdjustProductionValueResponseDto()
                 {
                     ConstructionClassificationName = x.Name,
                     ExchangeRate = exchangeRate.ExchangeRate.Value,
                     IsNew = 0,
                     NodeId = x.KeyId,
                     PNodeId = x.Pid,
                     UnitPrice = x.UnitPrice.Value,
                     //WorkQuantities=x.EngQuantity.Value,
                     //SourceWorkQuantities=x.EngQuantity.Value,
                     SourceUnitPrice= x.UnitPrice.Value,
                     ProjectId = proId,
                     WbsId = x.Id
                 })
                .ToListAsync();
            //项目的所有资源
            var projectResource = await _dbContext.Queryable<MonthReportDetail>().Where(x => x.IsDelete == 1 && x.ProjectId == proId).ToListAsync();
            //往来单位  对应资源
            var dealingUnitList = await _dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && (x.ZBPTYPE == "02" || x.ZBPTYPE == "03")).ToListAsync();
            //自有船舶数据
            var owinShip = await _dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1).ToListAsync();
            //分包船舶数据
            var subShip = await _dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1).ToListAsync();

            #region 基本信息赋值
            tempData.ProjectName = $"{project.Name}({project.projectStatus})(2024年12月)项目月报(税率:{Math.Round(project.Rate.Value*100,0)}%)";
            tempData.ExchangeRate = exchangeRate.ExchangeRate.Value;
            tempData.DateMonth = 202412;
            if (exchangeRate.Remark.Contains("欧元"))
            {
                tempData.Current = "CHE-WIR欧元";
            } else if (exchangeRate.Remark.Contains("人民币"))
            {
                tempData.Current = "CNY-人民币";
            }
            else if (exchangeRate.Remark.Contains("美元"))
            {
                tempData.Current = "USD-美元";
            }

            #endregion

            #region  数据组合
            foreach (var item in projectWbsList)
            {
                var index = 0;
                var currentNodeAllResource = projectResource.Where(x => x.ProjectWBSId == item.WbsId).ToList();
                foreach (var sub in currentNodeAllResource)
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
                    if (string.IsNullOrWhiteSpace(resourceName)&&( sub.OutPutType.ObjToInt() == 2||sub.OutPutType.ObjToInt() == 3))
                    {
                        resourceName = dealingUnitList.Where(x => x.PomId == sub.ShipId).Select(x => x.ZBPNAME_ZH).FirstOrDefault();
                    }

                    tempData.projectAdjustProductionValueResponseDtos.Add(new ProjectAdjustProductionValueResponseDto()
                    {
                        ProjectId = proId,
                        ConstructionType = sub.ConstructionNature.ToString(),
                        ConstructionTypeName= dictTable.Where(x=>x.Type== sub.ConstructionNature.Value).Select(x=>x.Name).FirstOrDefault(),
                        ProductionProperty = ((int)sub.OutPutType).ToString(),
                        ProductionPropertyName = outType,
                        ResourceId = sub.ShipId.ToString(),
                        ResourceName = resourceName,
                        UnitPrice = sub.UnitPrice,
                        SourceUnitPrice = sub.UnitPrice,
                        ExchangeRate = exchangeRate.ExchangeRate.Value,
                        WbsId = sub.ProjectWBSId,
                        IsNew = 0,
                        PNodeId = item.NodeId,
                        NodeId = (index += 1).ToString(),
                        WorkQuantities = sub.CompletedQuantity,
                        ProductionValue = sub.UnitPrice * sub.CompletedQuantity,
                        OutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                        SourceOutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                        SourceProductionValue = sub.UnitPrice*sub.CompletedQuantity,
                         RmbProductionValue= sub.UnitPrice * sub.CompletedQuantity* exchangeRate.ExchangeRate.Value,
                          SourceRmbProductionValue= sub.UnitPrice * sub.CompletedQuantity * exchangeRate.ExchangeRate.Value,
                        SourceWorkQuantities = sub.CompletedQuantity
                    }) ;
                }
            }
            projectWbsList.AddRange(tempData.projectAdjustProductionValueResponseDtos);
            #endregion

            #region 递归查询树
            var treeMonthReport= ListToTreeUtil.GetMonthReportTree("0", projectWbsList);
            #endregion

            #region 递归计算
            
            foreach (var item in treeMonthReport)
            {
                item.CalculateParentValues();
            }
            #endregion

            #region 添加一条历史数据

            #region 查询历史数据
            var historyMonthReport=await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.ProjectId == proId && x.DateMonth <= 202306).FirstAsync();
            #endregion

            treeMonthReport.Add(new Domain.Shared.Util.ProjectAdjustProductionValueResponseDto()
            {
                ConstructionClassificationName = "旧系统开累数据（2023年7月前）",
                ProjectId = proId,
                IsNew = 0,
                SourceOutsourcingExpenditure = historyMonthReport.OutsourcingExpensesAmount,
                SourceProductionValue = historyMonthReport.CompleteProductionAmount,
                SourceWorkQuantities = historyMonthReport.CompletedQuantity,
                ExchangeRate = exchangeRate.ExchangeRate.Value,
                OutsourcingExpenditure = historyMonthReport.OutsourcingExpensesAmount,
                ProductionValue = historyMonthReport.CompleteProductionAmount,
                WorkQuantities = historyMonthReport.CompletedQuantity
            }) ;
            #endregion

            #region 拼接数据
            tempData.projectAdjustProductionValueResponseDtos = treeMonthReport;
            #endregion

            responseAjaxResult.Data = tempData;
            responseAjaxResult.Count = 1;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 保存开累数产值
        /// </summary>
        /// <param name="projectAdjustResponseDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ResponseAjaxResult<bool>> SearchProjectAdjustMonthReportAsync(ProjectAdjustResponseDto projectAdjustResponseDto)
        {
            throw new NotImplementedException();
        }
    }
}
