using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;

namespace GHMonitoringCenterApi.Application.Service.ProductionValueImport
{
    /// <summary>
    ///  生产日报每天推送和节假日日报推送历史数据导出实现层
    /// </summary>
    public class ProductionValueImportService : IProductionValueImportService
    {
        /// <summary>
        /// 上下文注入
        /// </summary>
        public ISqlSugarClient _dbContext { get; set; }
        /// <summary>
        /// 映射注入
        /// </summary>
        public IMapper _mapper { get; set; }
        /// <summary>
        /// 注入发消息内容
        /// </summary>
        public IJjtSendMessageService _jjtSendMessageService { get; set; }
        /// <summary>
        /// 注入元旦期间在建项目带班生产动态
        /// </summary>
        public IProjectReportService _projectReportService { get; set; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="jjtSendMessageService"></param>
        /// <param name="projectReportService"></param>
        /// <param name="dbContext"></param>
        public ProductionValueImportService(IJjtSendMessageService jjtSendMessageService, IProjectReportService projectReportService, ISqlSugarClient dbContext, IMapper mapper)
        {
            this._jjtSendMessageService = jjtSendMessageService;
            this._projectReportService = projectReportService;
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        /// <summary>
        /// 导出历史数据产值信息
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> ImportProductionValuesAsync(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Excel 智慧运营监控中心图片数据写入表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ExcelJJtSendMessageWriteAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //调用监控中心图片信息方法
            var getData = await _jjtSendMessageService.JjtTextCardMsgDetailsAsync();

            //各个公司基本项目情况
            var excelCompanyProjectBasePoduction = getData.Data.projectBasePoduction.CompanyProjectBasePoductions;
            //各个公司基本产值情况
            var excelCompanyBasePoductionValue = getData.Data.projectBasePoduction.CompanyBasePoductionValues;
            //自有船施工运转情况
            var excelOwnerShipBuildInfo = getData.Data.OwnerShipBuildInfo.companyShipBuildInfos;
            //各个公司自有船施工产值情况集合
            var excelCompanyShipProductionValueInfo = getData.Data.OwnerShipBuildInfo.companyShipProductionValueInfos;
            //前五船舶产值
            var excelShipProductionValue = getData.Data.OwnerShipBuildInfo.companyShipTopFiveInfoList;
            //特殊情况
            var excelSpecialProjectInfo = getData.Data.SpecialProjectInfo;
            //各单位填报情况
            var excelCompanyWriteReportInfo = getData.Data.CompanyWriteReportInfos;
            //项目生产数据存在不完整部分主要是以下项目未填报
            var excelCompanyUnWriteReportInfo = getData.Data.CompanyUnWriteReportInfos;
            //船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            var excelCompanyShipUnWriteReportInfo = getData.Data.CompanyShipUnWriteReportInfos;

            //调用元旦期间各在建项目带班生产动态
            var getNewYearDayData = await _projectReportService.GetProjectShiftProductionAsync();

            //项目带班生产动态已填报项目
            var excelProjectShiftProductionInfo = getNewYearDayData.Data.projectShiftProductionInfos;
            //项目带班生产动态未填报项目
            var excelUnProjectShitInfo = getNewYearDayData.Data.unProjectShitInfos;

            //数据写入(只写了新增)
            var year = DateTime.Now.AddDays(-1).Year;
            var month = DateTime.Now.AddDays(-1).Month;
            var dateDay = DateTime.Now.AddDays(-1).ToDateDay();
            //titile集合
            var excelTitles = new List<ExcelTitle>();

            //各个公司基本项目情况
            if (excelCompanyProjectBasePoduction.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyProjectBasePoduction>, List<ExcelCompanyProjectBasePoduction>>(excelCompanyProjectBasePoduction);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleOne = "广航局合同项目" + getData.Data.projectBasePoduction.TotalOnContractProjectCount + "个，在建" + getData.Data.projectBasePoduction.TotalOnBuildProjectCount + "个， 停缓建" + getData.Data.projectBasePoduction.TotalStopBuildProjectCount + "个，在建数量占" + getData.Data.projectBasePoduction.TotalBuildCountPercent + "%；今日现场施工设备" + getData.Data.projectBasePoduction.TotalFacilityCount + "台，现场工人" + getData.Data.projectBasePoduction.TotalWorkerCount + "名， 危大工程施工" + getData.Data.projectBasePoduction.TotalRiskWorkCount + "项。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 项目总体生产情况",
                    TtileContent = titleOne,
                    DateDay = dateDay,
                    Type = 1,
                    Year = year
                });
            }
            //各个公司基本产值情况
            if (excelCompanyBasePoductionValue.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyBasePoductionValue>, List<ExcelCompanyBasePoductionValue>>(excelCompanyBasePoductionValue);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleTwo = DateTime.Now.AddDays(-1).ToString("MM月dd日") + " 广航局在建项目产值" + getData.Data.projectBasePoduction.DayProductionValue + "万元，" + getData.Data.Year + "年累计施工产值" + getData.Data.projectBasePoduction.TotalYearProductionValue + "亿元。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 项目总体生产情况",
                    TtileContent = titleTwo,
                    DateDay = dateDay,
                    Type = 2,
                    Year = year
                });
            }
            //自有船施工运转情况
            if (excelOwnerShipBuildInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyShipBuildInfo>, List<ExcelCompanyShipBuildInfo>>(excelOwnerShipBuildInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.ShipTypeDesc = GetShipDesc(item.ShipTypeName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleThree = "广航局三类船舶" + getData.Data.OwnerShipBuildInfo.TotalCount + "艘，其中施工" + getData.Data.OwnerShipBuildInfo.BuildCount + "艘，检修" + getData.Data.OwnerShipBuildInfo.ReconditionCount + "艘，调遣" + getData.Data.OwnerShipBuildInfo.AssignCount + "艘，待命" + getData.Data.OwnerShipBuildInfo.AwaitCount + "艘，开工率" + getData.Data.OwnerShipBuildInfo.BuildPercent + "%；";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleThree,
                    DateDay = dateDay,
                    Type = 3,
                    Year = year
                });
            }
            //各个公司自有船施工产值情况集合
            if (excelCompanyShipProductionValueInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyShipProductionValueInfo>, List<ExcelCompanyShipProductionValueInfo>>(excelCompanyShipProductionValueInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.ShipTypeDesc = GetShipDesc(item.ShipTypeName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleFour = "自有船施工产值" + getData.Data.OwnerShipBuildInfo.BulidProductionValue + "万元，运转时间" + getData.Data.OwnerShipBuildInfo.DayTurnHours + "h；当年累计船舶产值" + getData.Data.OwnerShipBuildInfo.YearTotalProductionValue + "亿元，累计运转时间" + getData.Data.OwnerShipBuildInfo.YearTotalTurnHours + "h。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleFour,
                    DateDay = dateDay,
                    Type = 4,
                    Year = year
                });
            }
            //前五船舶产值
            if (excelShipProductionValue.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<ShipProductionValue>, List<ExcelShipProductionValue>>(excelShipProductionValue);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                //无合计行  新增
                result.Add(new ExcelShipProductionValue
                {
                    Id = GuidUtil.Next(),
                    CreateTime = DateTime.Now,
                    DateDay = dateDay,
                    Month = month,
                    ShipDayOutput = getData.Data.projectBasePoduction.YearTopFiveTotalOutput,
                    ShipName = "合计",
                    ShipYearOutput = getData.Data.projectBasePoduction.YearFiveTotalOutput,
                    TimePercent = getData.Data.projectBasePoduction.YearTotalTopFiveOutputPercent,
                    Year = year
                });
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleFive = getData.Data.Year + "年自有船产值前五的船舶生产情况如下表所示，当年合计产值" + getData.Data.projectBasePoduction.YearFiveTotalOutput + "亿元，占当年自有船总产值" + getData.Data.projectBasePoduction.YearFiveTimeRate + "%。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleFive,
                    DateDay = dateDay,
                    Type = 5,
                    Year = year
                });
            }
            //特殊情况
            if (excelSpecialProjectInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<SpecialProjectInfo>, List<ExcelSpecialProjectInfo>>(excelSpecialProjectInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.TypeDescription = item.Type == 3 ? "提醒事项" : item.Type == 1 ? "异常预警" : item.Type == 2 ? "嘉奖通报" : "";
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleSix = "当日统计特殊情况" + result.Count() + "项，如列表所示：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 特殊情况通报",
                    TtileContent = titleSix,
                    DateDay = dateDay,
                    Type = 6,
                    Year = year
                });
            }
            //各单位填报情况
            if (excelCompanyWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyWriteReportInfo>, List<ExcelCompanyWriteReportInfo>>(excelCompanyWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.CompanyName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleSeven = "各单位产值日报填报率情况：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleSeven,
                    DateDay = dateDay,
                    Type = 7,
                    Year = year
                });
            }
            //项目生产数据存在不完整部分主要是以下项目未填报
            if (excelCompanyUnWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyUnWriteReportInfo>, List<ExcelCompanyUnWriteReportInfo>>(excelCompanyUnWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleEight = "说明：项目生产数据存在不完整部分主要是以下项目未填报：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleEight,
                    DateDay = dateDay,
                    Type = 8,
                    Year = year
                });
            }
            //船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            if (excelCompanyShipUnWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<CompanyShipUnWriteReportInfo>, List<ExcelCompanyShipUnWriteReportInfo>>(excelCompanyShipUnWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleNine = "说明：船舶生产数据存在不完整部分主要是项目部未填报以下船舶：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleNine,
                    DateDay = dateDay,
                    Type = 9,
                    Year = year
                });
            }
            //项目带班生产动态已填报项目
            if (excelProjectShiftProductionInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<ProjectShiftProductionInfo>, List<ExcelProjectShiftProductionInfo>>(excelProjectShiftProductionInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleTen = getNewYearDayData.Data.TimeValue.ToString("yyyy年MM月dd日") + "，广航局在建项目" + result.Count() + "个，现场管理人员" + getNewYearDayData.Data.sumInfo.SumSiteManagementPersonNum + "人，陆域作业" + getNewYearDayData.Data.sumInfo.SumSiteConstructionPersonNum + "人， 陆域设备" + getNewYearDayData.Data.sumInfo.SumConstructionDeviceNum + "台，在场船舶" + getNewYearDayData.Data.sumInfo.SumSiteShipNum + "艘，在船人员" + getNewYearDayData.Data.sumInfo.SumOnShipPersonNum + "人，危大工程施工" + getNewYearDayData.Data.sumInfo.SumHazardousConstructionNum + "项，陆地3~9人作业工点" + getNewYearDayData.Data.sumInfo.SumFewLandWorkplace + "处，陆地10人以上作业工点" + getNewYearDayData.Data.sumInfo.SumLandWorkplace + "处。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 元旦期间各在建项目带班生产动态",
                    TtileContent = titleTen,
                    DateDay = dateDay,
                    Type = 10,
                    Year = year
                });
            }
            //项目带班生产动态未填报项目
            if (excelUnProjectShitInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<UnProjectShitInfo>, List<ExcelUnProjectShitInfo>>(excelUnProjectShitInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleEleven = "注：陆域3～9人作业工点要求项目领导带班旁站，陆域10人及以上作业工点要求所属三级单位领导带班驻点；以下为未填报项目清单：" + result.Count() + "个。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 元旦期间各在建项目带班生产动态",
                    TtileContent = titleEleven,
                    DateDay = dateDay,
                    Type = 11,
                    Year = year
                });
            }

            await _dbContext.Insertable(excelTitles).ExecuteCommandAsync();
            responseAjaxResult.Success();
            responseAjaxResult.Data = true;

            return responseAjaxResult;
        }
        /// <summary>
        /// 获取单位序号
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public int GetUnitDesc(string unitName)
        {
            int num = 1;
            switch (unitName)
            {
                case "疏浚公司":
                    break;
                case "交建公司":
                    num = 2;
                    break;
                case "三公司":
                    num = 3;
                    break;
                case "四公司":
                    num = 4;
                    break;
                case "五公司":
                    num = 5;
                    break;
                case "福建公司":
                    num = 6;
                    break;
                case "直营项目":
                    num = 7;
                    break;
                case "广航局总体":
                    num = 8;
                    break;
            }
            return num;
        }
        /// <summary>
        /// 获取船舶类型排序
        /// </summary>
        /// <param name="shipTypeName"></param>
        /// <returns></returns>
        public int GetShipDesc(string shipTypeName)
        {
            int num = 1;
            switch (shipTypeName)
            {
                case "耙吸船":
                    break;
                case "绞吸船":
                    num = 2;
                    break;
                case "抓斗船":
                    num = 3;
                    break;
                case "合计":
                    num = 4;
                    break;
            }
            return num;
        }
    }
}
