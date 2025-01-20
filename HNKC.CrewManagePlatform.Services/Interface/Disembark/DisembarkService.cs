using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.Disembark
{
    /// <summary>
    /// 离船
    /// </summary>
    public class DisembarkService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, IDisembarkService
    {
        private readonly ISqlSugarClient _dbContext;
        private IBaseService _baseService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public DisembarkService(ISqlSugarClient dbContext, IBaseService baseService)
        {
            this._dbContext = dbContext;
            this._baseService = baseService;
        }
        #region 离船申请
        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveCrewDisembarkAsync(DisembarkRequest requestBody)
        {
            if (requestBody.DisembarkDtos != null && requestBody.DisembarkDtos.Any())
            {
                List<DepartureApplication> add = new();
                List<DepartureApplication> mod = new();
                List<WorkShip> wsMod = new();
                var upBIds = requestBody.DisembarkDtos.Where(x => !string.IsNullOrWhiteSpace(x.BId.ToString())).Select(x => x.BId).ToList();
                var uIds = requestBody.DisembarkDtos.Where(x => !string.IsNullOrWhiteSpace(x.BId.ToString())).Select(x => x.UserId).ToList();
                var workShips = await _dbContext.Queryable<WorkShip>().Where(x => x.IsDelete == 1 && uIds.Contains(x.WorkShipId)).ToListAsync();
                var updateTable = await _dbContext.Queryable<DepartureApplication>()
                    .Where(t => t.IsDelete == 1 && upBIds.Contains(t.BusinessId))
                    .ToListAsync();
                var insertTable = requestBody.DisembarkDtos.Where(x => string.IsNullOrWhiteSpace(x.BId.ToString())).ToList();
                foreach (var up in updateTable)
                {
                    if (!string.IsNullOrWhiteSpace(up.RealDisembarkTime.ToString()) && up.DisembarkTime < up.RealDisembarkTime)
                        return Result.Fail("实际离船日期不能小于下船日期" + up.RealDisembarkTime);
                    if (!ValidateUtils.ValidatePhone(up.Phone))
                        return Result.Fail("联系电话不正确" + up.Phone);
                    if (!ValidateUtils.ValidatePhoneNumber(up.FiexdLine))
                        return Result.Fail("家庭固话不正确" + up.FiexdLine);
                    mod.Add(new DepartureApplication
                    {
                        ApproveUserId = requestBody.ApproveUserId,
                        DisembarkId = GlobalCurrentUser.UserBusinessId,
                        DisembarkTime = up.DisembarkTime,
                        FiexdLine = up.FiexdLine,
                        Phone = up.Phone,
                        ReliefUserId = up.ReliefUserId,
                        Remark = up.Remark,
                        ShipId = up.ShipId,
                        ReturnShipTime = up.ReturnShipTime,
                        UserId = up.UserId,
                        RealDisembarkTime = up.RealDisembarkTime,
                        RealReturnShipTime = up.RealReturnShipTime
                    });
                    if (!string.IsNullOrWhiteSpace(up.RealDisembarkTime.ToString()))
                    {
                        var wShip = workShips.Where(x => x.WorkShipId == up.UserId).OrderByDescending(x => x.WorkShipStartTime).FirstOrDefault();
                        if (wShip != null)
                        {
                            wShip.WorkShipEndTime = up.RealDisembarkTime;
                            wsMod.Add(wShip);
                        }
                    }
                }
                foreach (var ins in insertTable)
                {
                    if (!string.IsNullOrWhiteSpace(ins.DisembarkTime.ToString()) && ins.DisembarkTime > ins.ReturnShipTime)
                        return Result.Fail("离船日期不能大于归船日期" + ins.ReturnShipTime);
                    if (!ValidateUtils.ValidatePhone(ins.Phone))
                        return Result.Fail("联系电话不正确" + ins.Phone);
                    if (!ValidateUtils.ValidatePhoneNumber(ins.FiexdLine))
                        return Result.Fail("家庭固话不正确" + ins.FiexdLine);
                    add.Add(new DepartureApplication
                    {
                        ApproveUserId = requestBody.ApproveUserId,
                        DisembarkId = GlobalCurrentUser.UserBusinessId,
                        DisembarkTime = ins.DisembarkTime,
                        FiexdLine = ins.FiexdLine,
                        Phone = ins.Phone,
                        ReliefUserId = ins.ReliefUserId,
                        Remark = ins.Remark,
                        ShipId = ins.ShipId,
                        ReturnShipTime = ins.ReturnShipTime,
                        UserId = ins.UserId,
                        DisembarkDate = ins.DisembarkDate,
                        BusinessId = GuidUtil.Next(),
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId()
                    });
                }
                if (add.Any()) await _dbContext.Insertable(add).ExecuteCommandAsync();
                if (mod.Any()) await _dbContext.Updateable(mod).ExecuteCommandAsync();
                if (wsMod.Any()) await _dbContext.Updateable(wsMod).UpdateColumns(x => new { x.WorkShipEndTime }).ExecuteCommandAsync();
                return Result.Success(true, "提交成功");
            }
            else return Result.Fail("提交失败");
        }
        /// <summary>
        /// 离船申请列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<SearchDisembark>> SearchCrewDisembarkAsync(SearchDisembarkRequest requestBody)
        {
            PageResult<SearchDisembark> rt = new();
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<SearchDisembark>(); }
            #region 船舶相关
            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion

            var rr = await _dbContext.Queryable<DepartureApplication>()
                .Where(t => t.IsDelete == 1 && t.ApproveStatus == Models.Enums.ApproveStatus.PExamination)
                .LeftJoin<User>((t, u) => t.UserId == u.BusinessId)
                .InnerJoin(wShip, (t, u, ws) => ws.WorkShipId == t.UserId)
                .LeftJoin<OwnerShip>((t, u, ws, ow) => ws.OnShip == ow.BusinessId.ToString())
                .WhereIF(roleType == 3, (t, u, ws, ow) => t.DisembarkId == GlobalCurrentUser.UserBusinessId)
                .Select((t, u, ws, ow) => new SearchDisembark
                {
                    BId = t.BusinessId,
                    UserId = t.UserId,
                    UserName = u.Name,
                    ShipType = ow.ShipType,
                    ShipName = ow.ShipName,
                    Company = ow.Company,
                    Country = ow.Country,
                    ShipId = ow.BusinessId,
                    ProjectName = ow.ProjectName,
                    ApproveStatus = t.ApproveStatus,
                    WorkNumber = u.WorkNumber,
                    DisembarkTime = t.DisembarkTime,
                    RealDisembarkTime = t.RealDisembarkTime,
                    RealReturnShipTime = t.RealReturnShipTime,
                    ReturnShipTime = t.ReturnShipTime,
                    Time = t.Created,
                    DisembarkDate = t.DisembarkDate
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            var countrys = rr.Select(x => x.Country).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrys.Contains(t.BusinessId))
                .ToListAsync();
            var ins = rr.Select(x => x.Company).ToList();
            var companys = await _dbContext.Queryable<Institution>()
               .Where(t => t.IsDelete == 1 && ins.Contains(t.BusinessId))
               .ToListAsync();
            foreach (var item in rr)
            {
                item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
                item.ApproveStatusName = EnumUtil.GetDescription(item.ApproveStatus);
                item.CountryName = country.FirstOrDefault(x => x.BusinessId == item.Country)?.Name;
                item.CompanyName = companys.FirstOrDefault(x => x.BusinessId == item.Company)?.Name;
                item.HolidayNums = TimeHelper.GetTimeSpan(
                                    string.IsNullOrWhiteSpace(item.RealDisembarkTime.ToString()) ? item.DisembarkTime.Value : item.RealDisembarkTime.Value,
                                    string.IsNullOrWhiteSpace(item.RealDisembarkTime.ToString()) ? item.ReturnShipTime.Value : item.RealReturnShipTime.Value
                                    ).Days + 1;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }
        #endregion
        #region 船舶值班
        /// <summary>
        /// 保存船舶排班
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveCrewRotaAsync(SaveSchedulingRequest requestBody)
        {
            List<CrewRota> addCrewRotas = new();
            if (requestBody.SaveScheduling != null && requestBody.SaveScheduling.Any())
            {
                //获取最大版本
                var verMax = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1 && t.ShipId == requestBody.ShipId).MaxAsync(x => x.Version);
                //统一时间版本
                var timeNow = DateTime.Now;
                foreach (var item in requestBody.SaveScheduling)
                {
                    addCrewRotas.Add(new CrewRota
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        BusinessId = GuidUtil.Next(),
                        FLeaderUserId = item.FLeaderUserId,
                        OhterUserId = item.OhterUserId,
                        RotaId = GlobalCurrentUser.UserBusinessId,
                        RotaType = item.RotaType,
                        SchedulingTime = timeNow,
                        ShipId = requestBody.ShipId,
                        SLeaderUserId = item.SLeaderUserId,
                        TeamGroup = item.TeamGroup,
                        TeamGroupDesc = item.TeamGroupDesc,
                        TimeSlotType = item.TimeSlotType,
                        TimeType = requestBody.TimeType,
                        Version = verMax + 1
                    });
                }
                await _dbContext.Insertable(addCrewRotas).ExecuteCommandAsync();

                return Result.Success("提交成功");
            }
            else
            {
                return Result.Fail("提交失败");
            }
        }
        /// <summary>
        /// 船员船舶排班回显
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> CrewRotaListAsync(SchedulingRequest requestBody)
        {
            SearchScheduling rt = new();
            List<SearchSchedulingDto> jiaBanSchedulings = new();
            List<SearchSchedulingDto> lunJichedulings = new();

            var rr = await _dbContext.Queryable<CrewRota>()
                .Where(t => t.IsDelete == 1 && requestBody.ShipId == t.ShipId && t.TimeType == requestBody.TimeType)
                .OrderByDescending(x => x.Version)
                .ToListAsync();
            var verMax = rr.Max(x => x.Version);
            rr = rr.Where(x => x.Version == verMax).ToList();

            foreach (var item in rr)
            {
                if (item.RotaType == RotaEnum.JiaBan)
                {
                    jiaBanSchedulings.Add(new SearchSchedulingDto
                    {
                        FLeaderUserId = item.FLeaderUserId,
                        OhterUserId = item.OhterUserId,
                        SLeaderUserId = item.SLeaderUserId,
                        TimeSlotType = item.TimeSlotType,
                        TimeSlotTypeName = EnumUtil.GetDescription(item.TimeSlotType)
                    });
                }
                else if (item.RotaType == RotaEnum.LunJi)
                {
                    lunJichedulings.Add(new SearchSchedulingDto
                    {
                        FLeaderUserId = item.FLeaderUserId,
                        OhterUserId = item.OhterUserId,
                        SLeaderUserId = item.SLeaderUserId,
                        TeamGroup = item.TeamGroup,
                        TeamGroupDesc = item.TeamGroupDesc
                    });
                }
            }
            rt.JiaBanSchedulings = jiaBanSchedulings;
            rt.LunJichedulings = lunJichedulings;
            return Result.Success(rt);
        }
        /// <summary>
        /// 船舶排班用户列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public async Task<Result> CrewRotaUserListAsync(Guid shipId)
        {
            #region 任职船舶
            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion

            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId && shipId.ToString() == ws.OnShip)
                .InnerJoin<PositionOnBoard>((t, ws, po) => po.BusinessId.ToString() == ws.OnShip)
                .Select((t, ws, po) => new SchedulingUser
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    Position = ws.Postition,
                    PositionName = po.Name,
                    RotaEnum = po.RotaType
                })
                .ToListAsync();
            var posiIds = userInfos.Select(x => x.Position).ToList();
            var position = await _dbContext.Queryable<PositionOnBoard>().Where(t => posiIds.Contains(t.BusinessId.ToString())).ToListAsync();
            foreach (var item in userInfos)
            {
                item.PositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == item.Position)?.Name;
            }

            return Result.Success(userInfos);
        }
        /// <summary>
        /// 值班管理列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<SearchCrewRota>> SearchCrewRotaAsync(SearchCrewRotaRequest requestBody)
        {
            PageResult<SearchCrewRota> rt = new();
            RefAsync<int> total = 0;

            var rr = await _dbContext.Queryable<OwnerShip>()
                .Where(t => t.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ShipId.ToString()), (t) => t.BusinessId == requestBody.ShipId)
                .Select((t) => new SearchCrewRota
                {
                    ShipId = t.BusinessId,
                    Company = t.Company,
                    Country = t.Country,
                    ProjectName = t.ProjectName,
                    ShipName = t.ShipName,
                    ShipType = t.ShipType,
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            if (rr.Any())
            {
                var cIds = rr.Select(x => x.Company).ToList();
                var company = await _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && cIds.Contains(t.BusinessId)).ToListAsync();
                var coIds = rr.Select(x => x.Country).ToList();
                var country = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1 && coIds.Contains(t.BusinessId)).ToListAsync();

                //任职船舶 
                var crewWorkShip = _dbContext.Queryable<WorkShip>()
                  .GroupBy(u => u.WorkShipId)
                  .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
                var wShip = _dbContext.Queryable<WorkShip>()
                  .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

                var userInfo = await _dbContext.Queryable<User>()
                    .Where(t => t.IsLoginUser == 1)
                    .InnerJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                    .InnerJoin<PositionOnBoard>((t, ws, po) => ws.OnShip == po.BusinessId.ToString() && (po.Name == "船长" || po.Name == "书记" || po.Name == "轮机长"))
                    .Select((t, ws, po) => new
                    {
                        t.Name,
                        t.BusinessId,
                        OnBoardName = po.Name,
                        ShipId = ws.OnShip
                    })
                    .ToListAsync();

                var schedulingTimeData = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1).OrderByDescending(t => t.SchedulingTime).ToListAsync();
                foreach (var item in rr)
                {
                    item.CompanyName = company.FirstOrDefault(x => x.BusinessId == item.Company)?.Name;
                    item.CountryName = country.FirstOrDefault(x => x.BusinessId == item.Country)?.Name;
                    item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
                    item.Captain = userInfo.FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "船长")?.OnBoardName;
                    item.Secretary = userInfo.FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "书记")?.OnBoardName;
                    item.ChiefEngineer = userInfo.FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "轮机长")?.OnBoardName;
                    item.SchedulingTime = schedulingTimeData.Where(x => x.ShipId == item.ShipId).OrderByDescending(x => x.SchedulingTime).FirstOrDefault()?.SchedulingTime;
                }
            }

            return rt;
        }
        /// <summary>
        /// 获取船舶值班详情
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public async Task<CrewRotaDetails> CrewRotaDetailsAsync(Guid shipId)
        {
            CrewRotaDetails rt = new();
            HeaderTtitle title = new();
            List<CrewRotaDetailsDto> jiaBans = new();
            List<CrewRotaDetailsDto> lunJis = new();
            List<FeiBanDetails> jiabanFeiban = new();
            List<FeiBanDetails> lunjiFeiban = new();

            //拿最新的12条数据
            var crewRotaData = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1)
                .OrderByDescending(x => x.SchedulingTime)
                .Take(12)
                .ToListAsync();

            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            var onboard = await _dbContext.Queryable<User>().Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                .InnerJoin<PositionOnBoard>((t, ws, po) => po.BusinessId.ToString() == ws.Postition)
                .Select((t, ws, po) => new
                {
                    t.BusinessId,
                    UserName = t.Name,
                    OnboardName = po.Name,
                })
                .ToListAsync();

            var jiaban = crewRotaData.Where(x => x.RotaType == RotaEnum.JiaBan).ToList();
            var lunji = crewRotaData.Where(x => x.RotaType == RotaEnum.LunJi).ToList();

            foreach (var item in jiaban.OrderBy(x => x.TimeSlotType))
            {
                var leader = onboard.FirstOrDefault(x => x.BusinessId == item.FLeaderUserId);
                var otherUser = item.OhterUserId?.Split(',').ToList();
                jiaBans.Add(new CrewRotaDetailsDto
                {
                    FLeaderUserName = leader?.UserName + (string.IsNullOrEmpty(leader?.OnboardName) ? "" : $"({leader?.OnboardName})") ?? string.Empty,

                });
            }
            return rt;
        }
        #endregion
    }
}
