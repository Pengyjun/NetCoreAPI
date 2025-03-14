using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using NPOI.OpenXmlFormats.Spreadsheet;
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
                projectAdjustProductionValueResponseDtos = new List<ProjectAdjustProductionValueResponseDto>()
            };
            //查询字典表
            var dictTable = await _dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 7).ToListAsync();
            //项目ID
            var proId = projectId.ToGuid();
            //查询开累数调整主表  如果主表已经存在就取主表的 不会再取月报明细表以及wbs表
            var mainMonthReport = await _dbContext.Queryable<ProjectAdjustProductionValue>().Where(x => x.IsDelete == 1 && x.ProjectId == proId).FirstAsync();
            //查询项目
            var project = await _dbContext.Queryable<Project>()
                .InnerJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
                .Where((x, y) => x.IsDelete == 1 && x.Id == proId).Select((x, y) => new { Name = x.Name, projectStatus = y.Name, Rate = x.Rate, }).FirstAsync();
            //查询币种
            var currencyId = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == proId).Select(x => SqlFunc.ToString(x.CurrencyId)).FirstAsync();
            var year = DateTime.Now.AddYears(-1).Year;
            //往来单位  对应资源
            var dealingUnitList = await _dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && (x.ZBPTYPE == "02" || x.ZBPTYPE == "03")).ToListAsync();
            //自有船舶数据
            var owinShip = await _dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1).ToListAsync();
            //分包船舶数据
            var subShip = await _dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1).ToListAsync();
            //查询汇率
            var exchangeRate = await _dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1 && x.Year == year && x.CurrencyId == currencyId).Select(x => new { ExchangeRate = x.ExchangeRate, Remark = x.Remark }).FirstAsync();
            List<ProjectAdjustProductionValueResponseDto> projectWbsList = null;
            List<MonthReportDetail> projectResource = null;
            //判断
            if (mainMonthReport == null)
            {
                //查询项目wbs
                projectWbsList = await _dbContext.Queryable<ProjectAdjustWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == projectId)
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
                        SourceUnitPrice = x.UnitPrice.Value,
                        ProjectId = proId,
                        WbsId = x.Id
                    })
                   .ToListAsync();
                //项目的所有资源  月报明细表
                projectResource = await _dbContext.Queryable<MonthReportDetail>().Where(x => x.IsDelete == 1 && x.ProjectId == proId).ToListAsync();
            }
            else
            {

                projectWbsList = await _dbContext.Queryable<ProjectAdjustProductionValueDetails>().Where(x => x.IsDelete == 1 && x.ProjectId == proId)
                    .Select(x => new ProjectAdjustProductionValueResponseDto()
                    {
                        ProjectId = x.ProjectId,
                        ConstructionClassificationName = x.ConstructionClassificationName,
                        ConstructionType = x.ConstructionType,
                        ConstructionTypeName = x.ConstructionTypeName,
                        ExchangeRate = x.ExchangeRate,
                        IsNew = x.IsNew,
                        MonthDetailId = x.MonthDetailId,
                        NodeId = x.NodeId,
                        OutsourcingExpenditure = x.OutsourcingExpenditure.Value,
                        PNodeId = x.PNodeId,
                        ProductionProperty = x.ProductionProperty,
                        ProductionPropertyName = x.ProductionPropertyName,
                        ProductionValue = x.ProductionValue,
                        ResourceId = x.ResourceId,
                        ResourceName = x.ResourceName,
                        RmbOutsourcingExpenditure = x.RmbOutsourcingExpenditure.Value,
                        RmbProductionValue = x.RmbProductionValue.Value,
                        SourceOutsourcingExpenditure = x.SourceOutsourcingExpenditure.Value,
                        SourceProductionValue = x.SourceProductionValue.Value,
                        SourceRmbOutsourcingExpenditure = x.SourceRmbOutsourcingExpenditure.Value,
                        SourceRmbProductionValue = x.SourceRmbProductionValue.Value,
                        SourceUnitPrice = x.SourceUnitPrice.Value,
                        SourceWorkQuantities = x.SourceWorkQuantities,
                        UnitPrice = x.UnitPrice.Value,
                        WbsId = x.WbsId,
                        WorkQuantities = x.WorkQuantities.Value
                    }).ToListAsync();
            }

            #region 基本信息赋值
            tempData.ProjectName = $"{project.Name}({project.projectStatus})(2024年12月)项目月报(税率:{Math.Round(project.Rate.Value * 100, 0)}%)";
            tempData.ExchangeRate = exchangeRate.ExchangeRate.Value;
            tempData.DateMonth = 202412;
            if (exchangeRate.Remark.Contains("欧元"))
            {
                tempData.Current = "CHE-WIR欧元";
            }
            else if (exchangeRate.Remark.Contains("人民币"))
            {
                tempData.Current = "CNY-人民币";
            }
            else if (exchangeRate.Remark.Contains("美元"))
            {
                tempData.Current = "USD-美元";
            }

            #endregion

            #region  数据组合
            if (mainMonthReport == null)
            {
                var index = 1000;
                foreach (var item in projectWbsList)
                {
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
                        if (string.IsNullOrWhiteSpace(resourceName) && (sub.OutPutType.ObjToInt() == 2 || sub.OutPutType.ObjToInt() == 3))
                        {
                            resourceName = dealingUnitList.Where(x => x.PomId == sub.ShipId).Select(x => x.ZBPNAME_ZH).FirstOrDefault();
                        }

                        tempData.projectAdjustProductionValueResponseDtos.Add(new ProjectAdjustProductionValueResponseDto()
                        {
                            MonthDetailId = sub.Id,
                            ProjectId = proId,
                            ConstructionType = sub.ConstructionNature.ToString(),
                            ConstructionTypeName = dictTable.Where(x => x.Type == sub.ConstructionNature.Value).Select(x => x.Name).FirstOrDefault(),
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
                            OutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                            SourceOutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                            ProductionValue = sub.UnitPrice * sub.CompletedQuantity,
                            SourceProductionValue = sub.UnitPrice * sub.CompletedQuantity,
                            //人民币
                            RmbProductionValue = sub.UnitPrice * sub.CompletedQuantity,
                            SourceRmbProductionValue = sub.UnitPrice * sub.CompletedQuantity * exchangeRate.ExchangeRate.Value,
                            SourceWorkQuantities = sub.CompletedQuantity,
                            RmbOutsourcingExpenditure = sub.OutsourcingExpensesAmount,
                            SourceRmbOutsourcingExpenditure = sub.OutsourcingExpensesAmount * exchangeRate.ExchangeRate.Value,

                        });
                    }
                }
                projectWbsList.AddRange(tempData.projectAdjustProductionValueResponseDtos);
            }
            #endregion

            #region 递归查询树
            var treeMonthReport = ListToTreeUtil.GetMonthReportTree("0", projectWbsList);
            #endregion

            #region 递归计算

            foreach (var item in treeMonthReport)
            {
                item.CalculateParentValues();
            }
            #endregion

            #region 添加一条历史数据

            #region 查询历史数据
            if (mainMonthReport == null)
            {
                var historyMonthReport = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.ProjectId == proId && x.DateMonth <= 202306).FirstAsync();

                treeMonthReport.Add(new Domain.Shared.Util.ProjectAdjustProductionValueResponseDto()
                {
                    ConstructionClassificationName = "旧系统开累数据（2023年7月前）",
                    ProjectId = proId,
                    IsNew = 0,
                    SourceOutsourcingExpenditure = historyMonthReport!=null? historyMonthReport.OutsourcingExpensesAmount:0,
                    SourceProductionValue = historyMonthReport != null ? historyMonthReport.CompleteProductionAmount : 0,
                    SourceWorkQuantities = historyMonthReport != null ? historyMonthReport.CompletedQuantity : 0,
                    ExchangeRate = exchangeRate.ExchangeRate.Value,
                    OutsourcingExpenditure = historyMonthReport != null ? historyMonthReport.OutsourcingExpensesAmount : 0,
                    ProductionValue = historyMonthReport != null ? historyMonthReport.CompleteProductionAmount : 0,
                    WorkQuantities = historyMonthReport != null ? historyMonthReport.CompletedQuantity : 0,
                });

            }
              
            #endregion

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
        public async Task<ResponseAjaxResult<bool>> SaveProjectAdjustMonthReportAsync(ProjectAdjustItemResponseDto projectAdjustItemResponseDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            //主表
            ProjectAdjustProductionValue projectAdjustProductionValue = new ProjectAdjustProductionValue()
            {
                ProjectId = projectAdjustItemResponseDto.ProjectId,
                SourceTotalOutsourcingExpenditure = projectAdjustItemResponseDto.SourceTotalProductionValue,
                SourceTotalProductionValue = projectAdjustItemResponseDto.SourceTotalProductionValue,
                SourceTotalWorkQuantities = projectAdjustItemResponseDto.SourceTotalWorkQuantities,
                SourceTotalRmbOutsourcingExpenditure = projectAdjustItemResponseDto.SourceTotalProductionValue,
                TotalOutsourcingExpenditure = projectAdjustItemResponseDto.TotalProductionValue,
                SourceTotalRmbProductionValue = projectAdjustItemResponseDto.SourceTotalProductionValue,
                TotalProductionValue = projectAdjustItemResponseDto.TotalProductionValue,
                TotalRmbOutsourcingExpenditure = projectAdjustItemResponseDto.SourceTotalProductionValue,
                TotalWorkQuantities = projectAdjustItemResponseDto.SourceTotalProductionValue,
                TotalRmbProductionValue = projectAdjustItemResponseDto.TotalRmbProductionValue
            };
            //明细表
            var data = _mapper.Map<List<ProjectAdjustProductionValueResponseDto>, List<ProjectAdjustProductionValueDetails>>(projectAdjustItemResponseDto.projectAdjustProductionValueResponseDtos);
            #region 删除已存在的
            var mainTableData = await _dbContext.Queryable<ProjectAdjustProductionValue>().Where(x => x.IsDelete == 1 && x.ProjectId ==
           projectAdjustItemResponseDto.ProjectId).ToListAsync();
            var detailsTableData = await _dbContext.Queryable<ProjectAdjustProductionValueDetails>().Where(x => x.IsDelete == 1 && x.ProjectId ==
          projectAdjustItemResponseDto.ProjectId).ToListAsync();
            if (mainTableData.Count > 0)
            {
                await _dbContext.Deleteable(mainTableData).ExecuteCommandAsync();
                await _dbContext.Deleteable(detailsTableData).ExecuteCommandAsync();
            }
            #endregion


            //搜索旧系统数据
            var historyData= data.Where(x =>x.ConstructionClassificationName!=null&& x.ConstructionClassificationName.Contains("旧系统") && x.MonthDetailId == Guid.Empty && x.NodeId == null).FirstOrDefault();
            if (historyData != null)
            {
                historyData.PNodeId = "0";
                historyData.NodeId = "-1";
            }
            await _dbContext.Insertable<ProjectAdjustProductionValue>(projectAdjustProductionValue).ExecuteCommandAsync();
            await _dbContext.Insertable<ProjectAdjustProductionValueDetails>(data).ExecuteCommandAsync();
            responseAjaxResult.Success();
            responseAjaxResult.Data = true;
            return responseAjaxResult;
        }
    }
}
