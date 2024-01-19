using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService.Ship;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;

namespace GHMonitoringCenterApi.Application.Service.Ship
{
    /// <summary>
    /// 船舶相关服务层
    /// </summary>
    public class ShipService : IShipService
    {
        /// <summary>
        /// 上下文
        /// </summary>
        public ISqlSugarClient _dbContext { get; set; }
        #region 依赖注入

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="dbContext"></param>
        public ShipService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region 船机成本维护相关
        /// <summary>
        /// 船机成本维护列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCostMaintenanceResponseDto>>> SearchShipCostMaintenanceAsync(ShipCostMaintenanceRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCostMaintenanceResponseDto>>();
            var data = new List<ShipCostMaintenanceResponseDto>();

            //当年
            int strYear = requestDto.NowTime(requestDto.StartDate).Year;
            int endYear = requestDto.NowTime(requestDto.EndDate).Year;

            //当年船舶成本数据
            var shipCostData = await _dbContext.Queryable<ShipMonthCost>().Where(x => x.IsDelete == 1 && x.DateYear >= strYear && x.DateYear <= endYear).ToListAsync();
            //船舶ids
            var shipIds = shipCostData.Select(x => x.ShipId).ToList();
            //自有船名
            var ownShips = await _dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && shipIds.Contains(x.PomId)).Select(x => new { x.PomId, x.Name }).ToListAsync();

            //日期差
            List<int> dateMonth = new List<int>();
            int monthDiffer = Utils.CalculateMonthDifference(Convert.ToDateTime(requestDto.StartDate), Convert.ToDateTime(requestDto.EndDate));
            var date = requestDto.NowTime(requestDto.StartDate);
            for (int i = 0; i <= monthDiffer; i++)
            {
                if (i == 0)
                {
                    int month = date.ToDateMonth();
                    dateMonth.Add(month);
                }
                else
                {
                    date = date.AddMonths(1);
                    int month = date.ToDateMonth();
                    dateMonth.Add(month);
                }
            }

            for (int i = 0; i <= monthDiffer; i++)
            {
                ConvertHelper.TryParseFromDateMonth(dateMonth[i], out DateTime nowDate);
                int month = Convert.ToInt32(dateMonth[i].ToString().Substring(4, 2));
                int year = Convert.ToInt32(dateMonth[i].ToString().Substring(0, 4));
                foreach (var item in shipCostData)
                {
                    data.Add(new ShipCostMaintenanceResponseDto
                    {
                        CategoryName = item.Category == 0 ? "境内" : item.Category == 1 ? "境外" : "存在错误类别",
                        Id = item.Id,
                        OwnShipName = ownShips.FirstOrDefault(x => x.PomId == item.ShipId)?.Name == "东祥" ? "金广" : ownShips.FirstOrDefault(x => x.PomId == item.ShipId)?.Name,
                        ShipCostTypeName = item.Type.ToDescription(),
                        ShipCostType = Convert.ToInt32(item.Type),
                        NoOilCost = GetOutputValue(shipCostData, year, month, item.Category, item.ShipId).NoOilCost,
                        DayShipCost = GetOutputValue(shipCostData, year, month, item.Category, item.ShipId).DayShipCost,
                        OilCost = GetOutputValue(shipCostData, year, month, item.Category, item.ShipId).OilCost,
                        ShipTotalCost = GetOutputValue(shipCostData, year, month, item.Category, item.ShipId).ShipTotalCost,
                        NowDate = nowDate.ToString("yyyy-MM"),
                        Category = item.Category,
                        OwnShipId = item.ShipId,
                        MergeCellId = GuidUtil.Next(),
                        IsDisabled = Convert.ToDateTime(nowDate.ToString("yyyy-MM")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM")) ? false : true
                    });
                }
            }
            data = data.GroupBy(x => new { x.OwnShipName, x.ShipCostType, x.NowDate, x.CategoryName }).Select(x => x.First()).OrderBy(x => x.ShipCostType).ThenBy(x => x.OwnShipName).ToList();
            #region 条件搜索
            if (!string.IsNullOrWhiteSpace(requestDto.OwnShipName))
            {
                data = data.Where(x => x.OwnShipName.Contains(requestDto.OwnShipName)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(requestDto.CategoryName))
            {
                data = data.Where(x => x.CategoryName.Contains(requestDto.CategoryName)).ToList();
            }
            #endregion

            //数据返回
            int skipCount = (requestDto.PageIndex - 1) * requestDto.PageSize;
            var result = data.Skip(skipCount).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Data = result;
            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 船机成本计算
        /// </summary>
        /// <param name="monthCost"></param>
        /// <param name="month"></param>
        /// <param name="category"></param>
        /// <param name="ownShipId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public OutputValue GetOutputValue(List<ShipMonthCost> monthCost, int year, int month, int category, Guid ownShipId)
        {
            decimal noOilCost = decimal.Zero;//不含油成本
            decimal dayShipCost = decimal.Zero;//固定船机成本
            decimal oilCost = decimal.Zero;//燃油成本
            decimal shipTotalCost = decimal.Zero;//船机总成本

            var isExistResult = monthCost.Where(x => x.ShipId == ownShipId && x.Category == category && x.DateYear == year).FirstOrDefault();
            if (isExistResult != null)
            {
                switch (month)
                {
                    case 1:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.OneNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.OneNoOilCost);
                            dayShipCost = isExistResult.OneNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.OneNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.OneDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.OneDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.OneNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.OneNoOilCostOverseas);
                            dayShipCost = isExistResult.OneNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.OneNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.OneDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.OneDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 2:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.TwoNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TwoNoOilCost);
                            dayShipCost = isExistResult.TwoNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.TwoNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.TwoDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TwoDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.TwoNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TwoNoOilCostOverseas);
                            dayShipCost = isExistResult.TwoNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.TwoNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.TwoDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TwoDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 3:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.ThreeNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.ThreeNoOilCost);
                            dayShipCost = isExistResult.ThreeNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.ThreeNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.ThreeDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.ThreeDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.ThreeNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.ThreeNoOilCostOverseas);
                            dayShipCost = isExistResult.ThreeNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.ThreeNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.ThreeDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.ThreeDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 4:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.FourNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.FourNoOilCost);
                            dayShipCost = isExistResult.FourNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.FourNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.FourDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.FourDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.FourNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.FourNoOilCostOverseas);
                            dayShipCost = isExistResult.FourNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.FourNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.FourDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.FourDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 5:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.FiveNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.FiveNoOilCost);
                            dayShipCost = isExistResult.FiveNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.FiveNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.FiveDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.FiveDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.FiveNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.FiveNoOilCostOverseas);
                            dayShipCost = isExistResult.FiveNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.FiveNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.FiveDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.FiveDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 6:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.SixNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.SixNoOilCost);
                            dayShipCost = isExistResult.SixNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.SixNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.SixDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.SixDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.SixNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.SixNoOilCostOverseas);
                            dayShipCost = isExistResult.SixNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.SixNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.SixDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.SixDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 7:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.SevenNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.SevenNoOilCost);
                            dayShipCost = isExistResult.SevenNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.SevenNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.SevenDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.SevenDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.SevenNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.SevenNoOilCostOverseas);
                            dayShipCost = isExistResult.SevenNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.SevenNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.SevenDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.SevenDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 8:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.EightNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.EightNoOilCost);
                            dayShipCost = isExistResult.EightNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.EightNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.EightDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.EightDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.EightNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.EightNoOilCostOverseas);
                            dayShipCost = isExistResult.EightNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.EightNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.EightDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.EightDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;

                        break;
                    case 9:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.NineNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.NineNoOilCost);
                            dayShipCost = isExistResult.NineNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.NineNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.NineDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.NineDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.NineNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.NineNoOilCostOverseas);
                            dayShipCost = isExistResult.NineNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.NineNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.NineDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.NineDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 10:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.TenNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TenNoOilCost);
                            dayShipCost = isExistResult.TenNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.TenNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.TenDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TenDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.TenNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TenNoOilCostOverseas);
                            dayShipCost = isExistResult.TenNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.TenNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.TenDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TenDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 11:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.ElevenNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.ElevenNoOilCost);
                            dayShipCost = isExistResult.ElevenNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.ElevenNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.ElevenDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.ElevenDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.ElevenNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.ElevenNoOilCostOverseas);
                            dayShipCost = isExistResult.ElevenNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.ElevenNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.ElevenDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.ElevenDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                    case 12:
                        if (category == 0)//境内
                        {
                            noOilCost = isExistResult.TwelveNoOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TwelveNoOilCost);
                            dayShipCost = isExistResult.TwelveNoOilCost == null ? 0M : (Convert.ToDecimal(isExistResult.TwelveNoOilCost) * 10000) / 30M;
                            oilCost = isExistResult.TwelveDayOilCost == null ? 0M : Convert.ToDecimal(isExistResult.TwelveDayOilCost);
                        }
                        else //境外
                        {
                            noOilCost = isExistResult.TwelveNoOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TwelveNoOilCostOverseas);
                            dayShipCost = isExistResult.TwelveNoOilCostOverseas == null ? 0M : (Convert.ToDecimal(isExistResult.TwelveNoOilCostOverseas) * 10000) / 30M;
                            oilCost = isExistResult.TwelveDayOilCostOverseas == null ? 0M : Convert.ToDecimal(isExistResult.TwelveDayOilCostOverseas);
                        }
                        shipTotalCost = dayShipCost + oilCost;
                        break;
                }
            }

            return new OutputValue
            {
                DayShipCost = Math.Round(dayShipCost, 2),
                NoOilCost = Math.Round(noOilCost, 2),
                OilCost = Math.Round(oilCost, 2),
                ShipTotalCost = Math.Round(shipTotalCost, 2)
            };
        }

        /// <summary>
        /// 船舶 and 分类 列表返回
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchOwnShipAndShipTypeDto>> SearchOwnShipAndShipTypeAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<SearchOwnShipAndShipTypeDto>();
            var searchOwnShip = new List<SearchOwnShip>();
            var searchShipType = new List<SearchShipType>();

            //获取船舶data
            var ownShipData = await _dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && CommonData.ShipTypes.Contains(x.TypeId) && x.Name != "智龙").Select(x => new { x.PomId, x.Name }).ToListAsync();
            foreach (var item in ownShipData)
            {
                searchOwnShip.Add(new SearchOwnShip
                {
                    OwnShipId = item.PomId,
                    OwnShipName = item.Name == "东祥" ? "金广" : item.Name
                });
            }

            searchShipType.Add(new SearchShipType { EnumKey = 1, EnumValueName = "2万方耙" });
            searchShipType.Add(new SearchShipType { EnumKey = 2, EnumValueName = "万方耙" });
            searchShipType.Add(new SearchShipType { EnumKey = 3, EnumValueName = "5000方耙" });
            searchShipType.Add(new SearchShipType { EnumKey = 4, EnumValueName = "8527" });
            searchShipType.Add(new SearchShipType { EnumKey = 5, EnumValueName = "7025" });
            searchShipType.Add(new SearchShipType { EnumKey = 6, EnumValueName = "抓斗船" });
            searchShipType.Add(new SearchShipType { EnumKey = 7, EnumValueName = "200方抓斗船" });

            responseAjaxResult.Data = new SearchOwnShipAndShipTypeDto
            {
                SearchOwnShips = searchOwnShip,
                SearchShipTypes = searchShipType
            };

            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 船舶成本维护增改
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddOrModifyShipCostMaintenanceAsync(AddOrModifyShipCostMaintenaceRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //年 、 月
            int year = Convert.ToDateTime(requestDto.Date).Year;
            int month = Convert.ToDateTime(requestDto.Date).Month;

            //数据新增
            if (requestDto.Id == Guid.Empty || string.IsNullOrWhiteSpace(requestDto.Id.ToString()))
            {
                //数据是否存在
                var isExistData = await _dbContext.Queryable<ShipMonthCost>().Where(x => x.IsDelete == 1 && year == x.DateYear && x.ShipId == requestDto.OwnShipId && x.Type == requestDto.ShipCostType && x.Category == requestDto.Category).FirstAsync();
                if (isExistData != null)//改
                {
                    var result = NoOilCost(isExistData, month, requestDto.Category, requestDto.NoOilCost, requestDto.OilCost, "insmodify"); result.Type = requestDto.ShipCostType;
                    if (requestDto.Category == 0)
                    {
                        result.DayShipCost = (requestDto.NoOilCost * 10000) / 30;
                    }
                    else if (requestDto.Category == 1)
                    {
                        result.DayShipCostOverseas = (requestDto.NoOilCost * 10000) / 30;
                    }
                    await _dbContext.Updateable(result).ExecuteCommandAsync();
                }
                else//增
                {
                    var addData = new ShipMonthCost
                    {
                        Id = GuidUtil.Next(),
                        Category = requestDto.Category,
                        Type = requestDto.ShipCostType,
                        DateYear = year,
                        ShipId = requestDto.OwnShipId,
                        DayShipCost = requestDto.Category == 0 ? (requestDto.NoOilCost * 10000) / 30 : 0,
                        DayShipCostOverseas = requestDto.Category == 1 ? (requestDto.NoOilCost * 10000) / 30 : 0
                    };
                    var result = NoOilCost(addData, month, requestDto.Category, requestDto.NoOilCost, requestDto.OilCost, "insmodify");
                    await _dbContext.Insertable(result).ExecuteCommandAsync();
                }
            }
            else
            {
                //改
                var isExistData = await _dbContext.Queryable<ShipMonthCost>().Where(x => x.Id == requestDto.Id).FirstAsync();
                if (isExistData != null)
                {
                    var result = NoOilCost(isExistData, month, requestDto.Category, requestDto.NoOilCost, requestDto.OilCost, "insmodify");
                    result.Type = requestDto.ShipCostType;
                    if (requestDto.Category == 0)
                    {
                        result.DayShipCost = (requestDto.NoOilCost * 10000) / 30; 
                    }
                    else if (requestDto.Category == 1)
                    {
                        result.DayShipCostOverseas = (requestDto.NoOilCost * 10000) / 30;
                    }
                    await _dbContext.Updateable(result).ExecuteCommandAsync();
                }
                else
                {
                    responseAjaxResult.FailResult(HttpStatusCode.UpdateFail, "主键id异常，修改失败");
                    return responseAjaxResult;
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 不含油成本
        /// </summary>
        /// <param name="monthCost"></param>
        /// <param name="month"></param>
        /// <param name="category"></param>
        /// <param name="noOilCost"></param>
        /// <param name="oilCost">燃油成本</param>
        /// <param name="insModifyOrDelete">增改或删</param>
        /// <returns></returns>
        public ShipMonthCost NoOilCost(ShipMonthCost monthCost, int month, int category, decimal noOilCost, decimal? oilCost, string insModifyOrDelete)
        {
            switch (month)
            {
                case 1:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.OneNoOilCost = noOilCost;
                            monthCost.OneDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.OneNoOilCost = decimal.Zero;
                            monthCost.OneDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.OneNoOilCostOverseas = noOilCost;
                            monthCost.OneDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.OneNoOilCostOverseas = decimal.Zero;
                            monthCost.OneDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 2:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TwoNoOilCost = noOilCost;
                            monthCost.TwoDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TwoNoOilCost = decimal.Zero;
                            monthCost.TwoDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TwoNoOilCostOverseas = noOilCost;
                            monthCost.TwoDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TwoNoOilCostOverseas = decimal.Zero;
                            monthCost.TwoDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 3:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.ThreeNoOilCost = noOilCost;
                            monthCost.ThreeDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.ThreeNoOilCost = decimal.Zero;
                            monthCost.ThreeDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.ThreeNoOilCostOverseas = noOilCost;
                            monthCost.ThreeDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.ThreeNoOilCostOverseas = decimal.Zero;
                            monthCost.ThreeDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 4:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.FourNoOilCost = noOilCost;
                            monthCost.FourDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.FourNoOilCost = decimal.Zero;
                            monthCost.FourDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.FourNoOilCostOverseas = noOilCost;
                            monthCost.FourDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.FourNoOilCostOverseas = decimal.Zero;
                            monthCost.FourDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 5:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.FiveNoOilCost = noOilCost;
                            monthCost.FiveDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.FiveNoOilCost = decimal.Zero;
                            monthCost.FiveDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.FiveNoOilCostOverseas = noOilCost;
                            monthCost.FiveDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.FiveNoOilCostOverseas = decimal.Zero;
                            monthCost.FiveDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 6:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.SixNoOilCost = noOilCost;
                            monthCost.SixDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.SixNoOilCost = decimal.Zero;
                            monthCost.SixDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.SixNoOilCostOverseas = noOilCost;
                            monthCost.SixDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.SixNoOilCostOverseas = decimal.Zero;
                            monthCost.SixDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 7:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.SevenNoOilCost = noOilCost;
                            monthCost.SevenDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.SevenNoOilCost = decimal.Zero;
                            monthCost.SevenDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.SevenNoOilCostOverseas = noOilCost;
                            monthCost.SevenDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.SevenNoOilCostOverseas = decimal.Zero;
                            monthCost.SevenDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 8:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.EightNoOilCost = noOilCost;
                            monthCost.EightDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.EightNoOilCost = decimal.Zero;
                            monthCost.EightDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.EightNoOilCostOverseas = noOilCost;
                            monthCost.EightDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.EightNoOilCostOverseas = decimal.Zero;
                            monthCost.EightDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;
                case 9:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.NineNoOilCost = noOilCost;
                            monthCost.NineDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.NineNoOilCost = decimal.Zero;
                            monthCost.NineDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.NineNoOilCostOverseas = noOilCost;
                            monthCost.NineDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.NineNoOilCostOverseas = decimal.Zero;
                            monthCost.NineDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 10:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TenNoOilCost = noOilCost;
                            monthCost.TenDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TenNoOilCost = decimal.Zero;
                            monthCost.TenDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TenNoOilCostOverseas = noOilCost;
                            monthCost.TenDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TenNoOilCostOverseas = decimal.Zero;
                            monthCost.TenDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 11:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.ElevenNoOilCost = noOilCost;
                            monthCost.ElevenDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.ElevenNoOilCost = decimal.Zero;
                            monthCost.ElevenDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.ElevenNoOilCostOverseas = noOilCost;
                            monthCost.ElevenDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.ElevenNoOilCostOverseas = decimal.Zero;
                            monthCost.ElevenDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;

                case 12:
                    if (category == 0)//境内
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TwelveNoOilCost = noOilCost;
                            monthCost.TwelveDayOilCost = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TwelveNoOilCost = decimal.Zero;
                            monthCost.TwelveDayOilCost = decimal.Zero;
                        }
                    }
                    else
                    {
                        if (insModifyOrDelete == "insmodify")
                        {
                            monthCost.TwelveNoOilCostOverseas = noOilCost;
                            monthCost.TwelveDayOilCostOverseas = oilCost;
                        }
                        else if (insModifyOrDelete == "delete")
                        {
                            monthCost.TwelveNoOilCostOverseas = decimal.Zero;
                            monthCost.TwelveDayOilCostOverseas = decimal.Zero;
                        }
                    }
                    break;
            }
            return monthCost;
        }

        /// <summary>
        /// 船舶成本维护删除
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeleteShipCostMaintenanceAsync(DeleteShipCostMaintenanceRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //年 、 月
            int year = Convert.ToDateTime(requestDto.Date).Year;
            int month = Convert.ToDateTime(requestDto.Date).Month;

            var isExistData = await _dbContext.Queryable<ShipMonthCost>().Where(x => x.IsDelete == 1 && x.Id == requestDto.Id).FirstAsync();
            if (isExistData != null)
            {
                //数据删除 更改数值为0
                var result = NoOilCost(isExistData, month, isExistData.Category, 0M, 0M, "delete");
                await _dbContext.Updateable(result).ExecuteCommandAsync();
            }
            else
            {
                responseAjaxResult.Data = false;
                return responseAjaxResult.FailResult(HttpStatusCode.DeleteFail, "数据不存在，删除失败");
            }

            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion
    }
}
