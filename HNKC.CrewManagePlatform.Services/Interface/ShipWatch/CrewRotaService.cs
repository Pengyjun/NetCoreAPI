using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Models.Dtos.ShipDuty;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;

namespace HNKC.CrewManagePlatform.Services.Interface.ShipWatch
{
    /// <summary>
    /// 值班服务接口实现
    /// </summary>
    public class CrewRotaService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService,
        ICrewRotaService
    {
        private readonly ISqlSugarClient _dbContext;
        private IBaseService _baseService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public CrewRotaService(ISqlSugarClient dbContext, IBaseService baseService)
        {
            this._dbContext = dbContext;
            this._baseService = baseService;
        }

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
                var verMax = await _dbContext.Queryable<CrewRota>()
                    .Where(t => t.IsDelete == 1 && t.ShipId == requestBody.ShipId).MaxAsync(x => x.Version);
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

            if (requestBody.ShipId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.ShipId.ToString()))
            {
                return Result.Fail("船舶ID不能为空");
            }

            SearchScheduling rt = new();
            List<SearchSchedulingDto> jiaBanSchedulings = new();
            List<SearchSchedulingDto> lunJichedulings = new();

            var rr = await _dbContext.Queryable<CrewRota>()
                .Where(t => t.IsDelete == 1 && requestBody.ShipId == t.ShipId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.TimeType.ToString()),
                    t => t.TimeType == requestBody.TimeType)
                .OrderByDescending(x => x.Version)
                .ToListAsync();
            if (rr.Count > 0)
            {
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
                            TimeSlotTypeName = EnumUtil.GetDescription(item.TimeSlotType),
                            TeamGroup = item.TeamGroup,
                            TeamGroupDesc = item.TeamGroupDesc
                        });
                    }
                    else if (item.RotaType == RotaEnum.LunJi)
                    {
                        lunJichedulings.Add(new SearchSchedulingDto
                        {
                            FLeaderUserId = item.FLeaderUserId,
                            OhterUserId = item.OhterUserId,
                            SLeaderUserId = item.SLeaderUserId,
                            TimeSlotType = item.TimeSlotType,
                            TimeSlotTypeName = EnumUtil.GetDescription(item.TimeSlotType),
                            TeamGroup = item.TeamGroup,
                            TeamGroupDesc = item.TeamGroupDesc
                        });
                    }
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
                .InnerJoin(crewWorkShip,
                    (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

            #endregion

            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId && shipId.ToString() == ws.OnShip)
                .InnerJoin<PositionOnBoard>((t, ws, po) => po.BusinessId.ToString() == ws.Postition)
                .Select((t, ws, po) => new SchedulingUser
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    Position = ws.Postition,
                    PositionName = po.Name,
                    RotaEnum = po.RotaType
                })
                .ToListAsync();
            /*var posiIds = userInfos.Select(x => x.Position).ToList();
            var position = await _dbContext.Queryable<PositionOnBoard>()
                .Where(t => posiIds.Contains(t.BusinessId.ToString())).ToListAsync();
            foreach (var item in userInfos)
            {
                item.PositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == item.Position)?.Name;
            }*/

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
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ShipId.ToString()),
                    (t) => t.BusinessId == requestBody.ShipId)
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
                var company = await _dbContext.Queryable<Institution>()
                    .Where(t => t.IsDelete == 1 && cIds.Contains(t.BusinessId)).ToListAsync();
                var coIds = rr.Select(x => x.Country).ToList();
                var country = await _dbContext.Queryable<CountryRegion>()
                    .Where(t => t.IsDelete == 1 && coIds.Contains(t.BusinessId)).ToListAsync();

                //任职船舶
                var crewWorkShip = _dbContext.Queryable<WorkShip>()
                    .GroupBy(u => u.WorkShipId)
                    .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
                var wShip = _dbContext.Queryable<WorkShip>()
                    .InnerJoin(crewWorkShip,
                        (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

                var userInfo = await _dbContext.Queryable<User>()
                    .Where(t => t.IsLoginUser == 1)
                    .InnerJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                    .InnerJoin<PositionOnBoard>((t, ws, po) =>
                        ws.OnShip == po.BusinessId.ToString() &&
                        (po.Name == "船长" || po.Name == "书记" || po.Name == "轮机长"))
                    .Select((t, ws, po) => new
                    {
                        t.Name,
                        t.BusinessId,
                        OnBoardName = po.Name,
                        ShipId = ws.OnShip
                    })
                    .ToListAsync();

                var schedulingTimeData = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1)
                    .OrderByDescending(t => new { t.Version, t.SchedulingTime }).ToListAsync();
                foreach (var item in rr)
                {
                    item.CompanyName = company.FirstOrDefault(x => x.BusinessId == item.Company)?.Name;
                    item.CountryName = country.FirstOrDefault(x => x.BusinessId == item.Country)?.Name;
                    item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
                    item.Captain = userInfo
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "船长")?.OnBoardName;
                    item.Secretary = userInfo
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "书记")?.OnBoardName;
                    item.ChiefEngineer = userInfo
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName == "轮机长")?.OnBoardName;
                    var scheduling = schedulingTimeData.Where(x => x.ShipId == item.ShipId)
                        .OrderByDescending(x => x.SchedulingTime).FirstOrDefault();
                    item.SchedulingTime = scheduling?.SchedulingTime;
                    item.TimeType = scheduling?.TimeType;
                }
            }

            rt.List = rr;
            rt.TotalCount = total;
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
            //List<FeiBanDetails> lunjiFeiban = new();

            //拿最新的6条数据
            var crewRotaData = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1)
                .OrderByDescending(x => x.SchedulingTime)
                .Take(6)
                .ToListAsync();

            //任职船舶
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .GroupBy(u => u.WorkShipId)
                .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
                .InnerJoin(crewWorkShip,
                    (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            var userOnboard = await _dbContext.Queryable<User>().Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                .InnerJoin<PositionOnBoard>((t, ws, po) => po.BusinessId.ToString() == ws.Postition)
                .Select((t, ws, po) => new
                {
                    t.BusinessId,
                    UserName = t.Name,
                    OnboardName = po.Name,
                    po.RotaType
                })
                .ToListAsync();

            var jiaban = crewRotaData.Where(x => x.RotaType == RotaEnum.JiaBan).ToList();
            var lunji = crewRotaData.Where(x => x.RotaType == RotaEnum.LunJi).ToList();

            List<Guid?> jiabanUIds = new();
            foreach (var item in jiaban.OrderBy(x => x.TimeSlotType))
            {
                var leader1 = userOnboard.FirstOrDefault(x => x.BusinessId == item.FLeaderUserId);
                var leader2 = userOnboard.FirstOrDefault(x => x.BusinessId == item.SLeaderUserId);
                var otherUser = item.OhterUserId?.Split(',')
                    .Select(id => string.IsNullOrEmpty(id) ? (Guid?)null : Guid.Parse(id))
                    .ToList();
                var otherUserNames = string.Empty;
                if (otherUser != null && otherUser.Any())
                {
                    foreach (var item2 in otherUser)
                    {
                        var rs = userOnboard.FirstOrDefault(x => x.BusinessId == item2);
                        var user =
                            rs?.UserName + (string.IsNullOrEmpty(rs?.OnboardName) ? "" : $"({rs?.OnboardName})") ??
                            string.Empty;
                        otherUserNames = string.Join("、", user);
                    }
                }

                jiabanUIds.Add(leader1?.BusinessId);
                jiabanUIds.Add(leader2?.BusinessId);
                if (otherUser != null && otherUser.Any()) jiabanUIds.AddRange(otherUser);
                //甲板剩下的非班人员
                var feibanuser = userOnboard
                    .Where(x => x.RotaType == RotaEnum.JiaBan && !jiabanUIds.Contains(x.BusinessId)).ToList();
                List<FeiBanDetails> jiabanFeiban = new();
                if (feibanuser.Any())
                {
                    foreach (var item3 in feibanuser)
                    {
                        jiabanFeiban.Add(new FeiBanDetails
                        {
                            UserName = item3?.UserName + (string.IsNullOrEmpty(item3?.OnboardName)
                                ? ""
                                : $"({item3?.OnboardName})") ?? string.Empty,
                            PositionName = string.IsNullOrEmpty(item3?.OnboardName) ? "" : item3?.OnboardName
                        });
                    }
                }

                jiaBans.Add(new CrewRotaDetailsDto
                {
                    FLeaderUserName =
                        leader1?.UserName + (string.IsNullOrEmpty(leader1?.OnboardName)
                            ? ""
                            : $"({leader1?.OnboardName})") ?? string.Empty,
                    SLeaderUserName =
                        leader2?.UserName + (string.IsNullOrEmpty(leader2?.OnboardName)
                            ? ""
                            : $"({leader2?.OnboardName})") ?? string.Empty,
                    FeiBanUsers = jiabanFeiban,
                    OhterUserName = otherUserNames,
                    //TimeSlotTypeName =item.TimeType== TimeEnum.FixedTime? EnumUtil.GetDescription(item.TimeSlotType):
                });
            }

            return rt;
        }

        #endregion


        /// <summary>
        /// 船舶值班查询
        /// </summary>
        /// <returns></returns>
        public async Task<Result> SearchShipDutyListAsync(BaseRequest request)
        {
            ShipDutyListResponseDto shipDuty = new ShipDutyListResponseDto();
            var ship = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1 && t.BusinessId == request.BId)
                .InnerJoin(_dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1), (x, y) => x.Country == y.BusinessId)
                .LeftJoin(_dbContext.Queryable<ShipProjectRelation>().Where(t => t.IsDelete == 1), (x, y, z) => x.BusinessId == z.RelationShipId)
                .Select((x, y, z) => new
                {
                    x.BusinessId,
                    x.ShipName,
                    y.Name,
                    z.ProjectName
                })
                .FirstAsync();
            if (ship != null)
            {
                //获取船舶上的人员总数
                var crewWorkShip = _dbContext.Queryable<WorkShip>()
                    .Where(t => t.OnShip == request.BId.ToString())
                  .GroupBy(u => u.WorkShipId)
                  .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
                var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.OnShip == request.BId.ToString())
                  .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
                shipDuty.ShipId = ship.BusinessId;
                shipDuty.ShipName = ship.ShipName;
                shipDuty.Country = ship.Name;
                shipDuty.ProjectName = ship.ProjectName;
            }


            return Result.Success();
        }


    }
}