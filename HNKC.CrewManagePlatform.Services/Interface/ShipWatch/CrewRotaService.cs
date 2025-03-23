using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Models.Dtos.ShipDuty;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using UtilsSharp;

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
            var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                         .GroupBy(u => u.WorkShipId)
                         .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);

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

                var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                          .GroupBy(u => u.WorkShipId)
                          .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
                var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                  .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);

                var userInfo = await _dbContext.Queryable<User>()
                    .Where(t => t.IsLoginUser == 1)
                    .InnerJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                    .InnerJoin<PositionOnBoard>((t, ws, po) =>
                        ws.Postition == po.BusinessId.ToString() &&
                        (po.Name.Contains("船长") || po.Name.Contains("书记") || po.Name.Contains("轮机长")))
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
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName.Contains("船长"))?.Name;
                    item.Secretary = userInfo
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName.Contains("书记"))?.Name;
                    item.ChiefEngineer = userInfo
                        .FirstOrDefault(x => x.ShipId == item.ShipId.ToString() && x.OnBoardName.Contains("轮机长"))?.Name;
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
            var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
               .GroupBy(u => u.WorkShipId)
               .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);
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
                var url = AppsettingsHelper.GetValue("UpdateItem:Url");
                //用户信息
                var userInfo = await _dbContext.Queryable<User>().Where(t => t.IsDelete == 1).ToListAsync();
                //职务信息
                var positionInfo = await _dbContext.Queryable<PositionOnBoard>().Where(t => t.IsDelete == 1).ToListAsync();
                var fileInfo = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1).ToListAsync();
                //获取船舶上的人员总数
                var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && t.OnShip == GlobalCurrentUser.ShipId)
                  .GroupBy(u => u.WorkShipId)
                  .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
                var workShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && t.OnShip == GlobalCurrentUser.ShipId)
                  .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId  && x.WorkShipStartTime == y.WorkShipStartTime)
                  .Where((x, y) => x.OnShip == request.BId.ToString());
                var wShip = await workShip.InnerJoin(_dbContext.Queryable<User>().Where(t => t.IsDelete == 1), (x, y, u) => x.WorkShipId == u.BusinessId).ToListAsync();
                shipDuty.ShipId = ship.BusinessId;
                shipDuty.ShipName = ship.ShipName;
                shipDuty.Country = ship.Name;
                shipDuty.ProjectName = ship.ProjectName;
                shipDuty.Total = wShip.Count;
                //船长
                LeaderInfo leaderInfo = new LeaderInfo();
                var wFirst = wShip.FirstOrDefault(t => t.Postition == "93f80b81-cf29-11ef-82f9-ecd68ace58a2");
                if (wFirst != null)
                {
                    leaderInfo.UserId = userInfo.FirstOrDefault(t => t.BusinessId == wFirst.WorkShipId)?.BusinessId.ToString();
                    leaderInfo.Name = userInfo.FirstOrDefault(t => t.BusinessId == wFirst.WorkShipId)?.Name;
                    leaderInfo.JobType = positionInfo.FirstOrDefault(t => t.BusinessId.ToString() == wFirst.Postition)?.Name;
                    var file = userInfo.FirstOrDefault(t => t.BusinessId == wFirst.WorkShipId)?.CrewPhoto;
                    leaderInfo.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file)?.Name;
                }
                //书记
                LeaderInfo leaderInfo2 = new LeaderInfo();
                List<string> str = new List<string>
                {
                    "93f8e8f2-cf29-11ef-82f9-ecd68ace58a2",
                    "93f9025e-cf29-11ef-82f9-ecd68ace58a2",
                    "93f91a6f-cf29-11ef-82f9-ecd68ace58a2",
                    "93f93632-cf29-11ef-82f9-ecd68ace58a2",
                    "93f94b37-cf29-11ef-82f9-ecd68ace58a2",
                    "93f961ad-cf29-11ef-82f9-ecd68ace58a2",
                    "93f982d6-cf29-11ef-82f9-ecd68ace58a2",
                    "93f99dfe-cf29-11ef-82f9-ecd68ace58a2 ",
                    "93f9b563-cf29-11ef-82f9-ecd68ace58a2",
                    "93f9d3bb-cf29-11ef-82f9-ecd68ace58a2"
                };
                var wFirst2 = wShip.OrderByDescending(t => t.Postition).FirstOrDefault(t => str.Contains(t.Postition));
                if (wFirst2 != null)
                {
                    leaderInfo2.UserId = userInfo.FirstOrDefault(t => t.BusinessId == wFirst2.WorkShipId)?.BusinessId.ToString();
                    leaderInfo2.Name = userInfo.FirstOrDefault(t => t.BusinessId == wFirst2.WorkShipId)?.Name;
                    leaderInfo2.JobType = positionInfo.FirstOrDefault(t => t.BusinessId.ToString() == wFirst2.Postition)?.Name;
                    var file = userInfo.FirstOrDefault(t => t.BusinessId == wFirst2.WorkShipId)?.CrewPhoto;
                    leaderInfo2.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file)?.Name;
                }
                //轮机长
                LeaderInfo leaderInfo3 = new LeaderInfo();
                var wFirst3 = wShip.FirstOrDefault(t => t.Postition == "93f86f23-cf29-11ef-82f9-ecd68ace58a2");
                if (wFirst3 != null)
                {
                    leaderInfo3.UserId = userInfo.FirstOrDefault(t => t.BusinessId == wFirst3.WorkShipId)?.BusinessId.ToString();
                    leaderInfo3.Name = userInfo.FirstOrDefault(t => t.BusinessId == wFirst3.WorkShipId)?.Name;
                    leaderInfo3.JobType = positionInfo.FirstOrDefault(t => t.BusinessId.ToString() == wFirst3.Postition)?.Name;
                    var file = userInfo.FirstOrDefault(t => t.BusinessId == wFirst3.WorkShipId)?.CrewPhoto;
                    leaderInfo3.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file)?.Name;
                }
                shipDuty.leaderInfo.Add(leaderInfo);
                shipDuty.leaderInfo.Add(leaderInfo2);
                shipDuty.leaderInfo.Add(leaderInfo3);

                //获取审批通过的离船申请
                var depApply = await _dbContext.Queryable<DepartureApply>().Where(t => t.IsDelete == 1 && t.ShipId == request.BId && t.ApproveStatus == ApproveStatus.Pass).ToListAsync();
                var applyCodes = depApply.Select(t => t.ApplyCode).Distinct().ToArray();
                var time = DateTime.Now;
                //获取离船用户的详细信息
                var depApplyUser = await _dbContext.Queryable<DepartureApplyUser>()
                    .InnerJoin(_dbContext.Queryable<User>().Where(t => t.IsDelete == 1), (x, u) => x.UserId == u.BusinessId)
                    .LeftJoin(workShip, (x, u, y) => x.UserId == y.WorkShipId)
                    .LeftJoin(_dbContext.Queryable<PositionOnBoard>(), (x, u, y, z) => y.Postition == z.BusinessId.ToString())
                    .Where((x, u, y, z) => x.IsDelete == 1 && y.IsDelete == 1 && z.IsDelete == 1 && applyCodes.Contains(x.ApplyCode))
                    .Select((x, u, y, z) => new DepartureResponseDto
                    {
                        UserId = x.UserId,
                        DisembarkDate = x.DisembarkDate,
                        ReturnShipDate = x.ReturnShipDate,
                        RotaType = z.RotaType
                    })
                    .ToListAsync();
                //查看船员是否有离船申请 并且未归船的 这种类型的船员为休假中的
                var dutyUser = depApplyUser.Where(t => t.DisembarkDate <= time && t.ReturnShipDate >= time).Select(t => t.UserId).Distinct().ToList();
                shipDuty.Holiday = dutyUser.Count;
                shipDuty.OnDuty = wShip.Count - dutyUser.Count;
                //获取排班信息
                var crew = await _dbContext.Queryable<CrewRota>().Where(t => t.IsDelete == 1 && t.ShipId == request.BId).ToListAsync();
                List<RotaEnum> rotaEnums = new List<RotaEnum>();
                rotaEnums.Add(RotaEnum.JiaBan);
                rotaEnums.Add(RotaEnum.LunJi);
                foreach (var item in rotaEnums)
                {
                    var result = await GetCrewRotaInfo(crew, userInfo, positionInfo, wShip, depApplyUser, fileInfo, time, item, url);
                    shipDuty.dutyPeople.Add(result);
                }

                //获取非值班人员

                foreach (var item in rotaEnums)
                {
                    var result = await GetOffCrewRotaInfo(crew, userInfo, positionInfo, wShip, depApplyUser, fileInfo, time, item, url);
                    shipDuty.offDutyPeople.Add(result);
                }
            }

            return Result.Success(shipDuty);
        }

        /// <summary>
        /// 获取甲板部 和 轮机部 值班人员的数据
        /// </summary>
        /// <param name="crew"></param>
        /// <param name="userInfo"></param>
        /// <param name="positionInfo"></param>
        /// <param name="wShip"></param>
        /// <param name="depApplyUser"></param>
        /// <param name="time"></param>
        /// <param name="rota"></param>
        /// <returns></returns>
        public async Task<DutyPerson> GetCrewRotaInfo(List<CrewRota> crew, List<User> userInfo, List<PositionOnBoard> positionInfo, List<WorkShip> wShip, List<DepartureResponseDto> depApplyUser, List<Files> fileInfo, DateTime time, RotaEnum rota, string url)
        {
            //按版本号倒排 获取前三天最新的甲板部排班人员
            var deck = crew.Where(t => t.RotaType == rota).OrderByDescending(t => t.Version).Take(3).ToList();
            DutyPerson dutyPerson = new DutyPerson();
            List<string> otherUser = new List<string>();
            foreach (var item in deck)
            {
                TeamsGroup team = new TeamsGroup();
                if (item.TimeType == TimeEnum.FixedTime)
                {
                    team.TimeslotType = GetEnumDescription(item.TimeSlotType);
                }
                else if (item.TimeType == TimeEnum.NotFixedTime)
                {
                    team.TimeslotType = item.TeamGroup;
                }
                var user1 = userInfo.Where(t => t.BusinessId == item.FLeaderUserId).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user1))
                {
                    //获取对应职务
                    var position = wShip.Where(t => t.WorkShipId == item.FLeaderUserId).FirstOrDefault()?.Postition;
                    var positionName = positionInfo.Where(t => t.BusinessId.ToString() == position).FirstOrDefault()?.Name;
                    team.UserId1 = userInfo.Where(t => t.BusinessId == item.FLeaderUserId).FirstOrDefault()?.BusinessId.ToString();
                    ////判断当前值班人员是否休假
                    //var isDep = depApplyUser.Where(t => t.UserId == item.FLeaderUserId).FirstOrDefault();
                    team.Person1 = user1 + "（" + positionName + "）";
                    var file1 = userInfo.FirstOrDefault(t => t.BusinessId == item.FLeaderUserId)?.CrewPhoto;
                    team.Icon1 = url + fileInfo.FirstOrDefault(t => t.FileId == file1)?.Name;
                }
                var user2 = userInfo.Where(t => t.BusinessId == item.SLeaderUserId).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user2))
                {
                    //获取对应职务
                    var position2 = wShip.Where(t => t.WorkShipId == item.SLeaderUserId).FirstOrDefault()?.Postition;
                    var positionName2 = positionInfo.Where(t => t.BusinessId.ToString() == position2).FirstOrDefault()?.Name;
                    team.UserId2 = userInfo.Where(t => t.BusinessId == item.FLeaderUserId).FirstOrDefault()?.BusinessId.ToString();
                    team.Person2 = user2 + "（" + positionName2 + "）";
                    var file2 = userInfo.FirstOrDefault(t => t.BusinessId == item.FLeaderUserId)?.CrewPhoto;
                    team.Icon2 = url + fileInfo.FirstOrDefault(t => t.FileId == file2)?.Name;
                }
                dutyPerson.teamsGroup.Add(team);
                var other = item.OhterUserId?.Split(',').ToList();
                otherUser.AddRange(other);


            }
            //查看船员是否有离船申请 并且未归船的 这种类型的船员为休假中的
            var dutyUser = depApplyUser.Where(t => t.RotaType == rota && t.DisembarkDate <= time && t.ReturnShipDate >= time).ToList();
            List<UserInfo> userList = new List<UserInfo>();
            foreach (var item in dutyUser)
            {
                UserInfo userInfo1 = new UserInfo();
                var user1 = userInfo.Where(t => t.BusinessId == item.UserId).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user1))
                {
                    //获取对应职务
                    var position = wShip.Where(t => t.WorkShipId == item.UserId).FirstOrDefault()?.Postition;
                    var positionName = positionInfo.Where(t => t.BusinessId.ToString() == position).FirstOrDefault()?.Name;
                    var file1 = userInfo.FirstOrDefault(t => t.BusinessId == item.UserId)?.CrewPhoto;
                    userInfo1.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file1)?.Name;
                    userInfo1.UserId = userInfo.Where(t => t.BusinessId == item.UserId).FirstOrDefault()?.BusinessId.ToString();
                    userInfo1.UserName = user1 + "（" + positionName + "）";
                    userList.Add(userInfo1);
                }
            }
            foreach (var item in otherUser)
            {
                var user1 = userInfo.Where(t => t.BusinessId.ToString() == item).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user1))
                {
                    //获取对应职务
                    var position = wShip.Where(t => t.WorkShipId.ToString() == item).FirstOrDefault()?.Postition;
                    var positionName = positionInfo.Where(t => t.BusinessId.ToString() == position).FirstOrDefault()?.Name;
                    var result = user1 + "（" + positionName + "）";
                    dutyPerson.OtherPerson.Add(result);
                }
            }
            dutyPerson.HolidayPerson = userList;
            return dutyPerson;
        }

        /// <summary>
        /// 获取甲板部 和 轮机部 非班人员的数据
        /// </summary>
        /// <param name="crew"></param>
        /// <param name="userInfo"></param>
        /// <param name="positionInfo"></param>
        /// <param name="wShip"></param>
        /// <param name="depApplyUser"></param>
        /// <param name="time"></param>
        /// <param name="rota"></param>
        /// <returns></returns>
        public async Task<OffDutyPerson> GetOffCrewRotaInfo(List<CrewRota> crew, List<User> userInfo, List<PositionOnBoard> positionInfo, List<WorkShip> wShip, List<DepartureResponseDto> depApplyUser, List<Files> fileInfo, DateTime time, RotaEnum rota, string url)
        {
            //当前已排班的人员
            List<string> crewIds = new List<string>();
            OffDutyPerson offDutyPerson = new OffDutyPerson();
            var userId1 = crew.Where(t => t.RotaType == rota).Select(t => t.FLeaderUserId.ToString() ?? "").ToList();
            var userId2 = crew.Where(t => t.RotaType == rota).Select(t => t.SLeaderUserId.ToString() ?? "").ToList();
            if (userId1 != null && userId1.Any()) crewIds.AddRange(userId1);
            if (userId2 != null && userId2.Any()) crewIds.AddRange(userId2);
            foreach (var item in crew)
            {
                var splitUser = item.OhterUserId?.Split(',').ToList();
                crewIds.AddRange(splitUser);
            }
            //所在部门筛选
            var depPositionIds = positionInfo.Where(t => t.RotaType == rota).Select(t => t.BusinessId.ToString()).ToList();
            //非班人员
            var offUser = wShip.Where(t => !crewIds.Contains(t.WorkShipId.ToString()) && depPositionIds.Contains(t.Postition)).ToList();

            foreach (var item in offUser)
            {
                UserInfo userInfo1 = new UserInfo();
                var user1 = userInfo.Where(t => t.BusinessId == item.WorkShipId).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user1))
                {
                    //获取对应职务
                    var position = wShip.Where(t => t.WorkShipId == item.WorkShipId).FirstOrDefault()?.Postition;
                    var positionName = positionInfo.Where(t => t.BusinessId.ToString() == position).FirstOrDefault()?.Name;
                    userInfo1.UserId = userInfo.FirstOrDefault(t => t.BusinessId == item.WorkShipId)?.BusinessId.ToString();
                    userInfo1.UserName = user1 + "（" + positionName + "）";
                    var file = userInfo.FirstOrDefault(t => t.BusinessId == item.WorkShipId)?.CrewPhoto;
                    userInfo1.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file)?.Name;
                    offDutyPerson.Person1.Add(userInfo1);
                }
            }
            var offUserIds = offUser.Select(t => t.WorkShipId).Distinct().ToArray();
            //查看非班中的船员是否有离船申请 并且未归船的 这种类型的船员为休假中的
            var dutyUser = depApplyUser.Where(t => t.RotaType == rota && offUserIds.Contains(t.UserId) && t.DisembarkDate <= time && t.ReturnShipDate >= time).ToList();
            List<UserInfo> userList = new List<UserInfo>();
            foreach (var item in dutyUser)
            {
                UserInfo userInfo1 = new UserInfo();
                var user1 = userInfo.Where(t => t.BusinessId == item.UserId).FirstOrDefault()?.Name;
                if (!string.IsNullOrWhiteSpace(user1))
                {
                    //获取对应职务
                    var position = wShip.Where(t => t.WorkShipId == item.UserId).FirstOrDefault()?.Postition;
                    var positionName = positionInfo.Where(t => t.BusinessId.ToString() == position).FirstOrDefault()?.Name;
                    userInfo1.UserId = userInfo.FirstOrDefault(t => t.BusinessId == item.UserId)?.BusinessId.ToString();
                    userInfo1.UserName = user1 + "（" + positionName + "）";
                    var file = userInfo.FirstOrDefault(t => t.BusinessId == item.UserId)?.CrewPhoto;
                    userInfo1.Icon = url + fileInfo.FirstOrDefault(t => t.FileId == file)?.Name;
                    userList.Add(userInfo1);
                }
            }
            offDutyPerson.HolidayPerson = userList;
            return offDutyPerson;
        }

        /// <summary>
        /// 获取枚举项的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}