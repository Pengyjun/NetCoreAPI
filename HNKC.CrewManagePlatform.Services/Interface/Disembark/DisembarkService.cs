using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.ComponentModel;
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
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public DisembarkService(ISqlSugarClient dbContext, IBaseService baseService, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._baseService = baseService;
            this._mapper = mapper;
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
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
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
                        var user = rs?.UserName + (string.IsNullOrEmpty(rs?.OnboardName) ? "" : $"({rs?.OnboardName})") ?? string.Empty;
                        otherUserNames = string.Join("、", user);
                    }
                }
                jiabanUIds.Add(leader1?.BusinessId);
                jiabanUIds.Add(leader2?.BusinessId);
                if (otherUser != null && otherUser.Any()) jiabanUIds.AddRange(otherUser);
                //甲板剩下的非班人员
                var feibanuser = userOnboard.Where(x => x.RotaType == RotaEnum.JiaBan && !jiabanUIds.Contains(x.BusinessId)).ToList();
                List<FeiBanDetails> jiabanFeiban = new();
                if (feibanuser.Any())
                {
                    foreach (var item3 in feibanuser)
                    {
                        jiabanFeiban.Add(new FeiBanDetails
                        {
                            UserName = item3?.UserName + (string.IsNullOrEmpty(item3?.OnboardName) ? "" : $"({item3?.OnboardName})") ?? string.Empty,
                            PositionName = string.IsNullOrEmpty(item3?.OnboardName) ? "" : item3?.OnboardName
                        });
                    }
                }
                jiaBans.Add(new CrewRotaDetailsDto
                {
                    FLeaderUserName = leader1?.UserName + (string.IsNullOrEmpty(leader1?.OnboardName) ? "" : $"({leader1?.OnboardName})") ?? string.Empty,
                    SLeaderUserName = leader2?.UserName + (string.IsNullOrEmpty(leader2?.OnboardName) ? "" : $"({leader2?.OnboardName})") ?? string.Empty,
                    FeiBanUsers = jiabanFeiban,
                    OhterUserName = otherUserNames,
                    //TimeSlotTypeName =item.TimeType== TimeEnum.FixedTime? EnumUtil.GetDescription(item.TimeSlotType):
                });
            }
            return rt;
        }
        #endregion

        /// <summary>
        /// 年休假计划列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<PageResult<AnnualLeavePlanResponseDto>> SearchAnnualLeavePlanAsync(AnnualLeavePlanRequestDto requestDto)
        {
            //for (int i = 0; i < 1000; i++)
            //{
            //    ShipPersonnelPosition model = new ShipPersonnelPosition();
            //    model.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            //    model.BusinessId = GuidUtil.Next();
            //    await _dbContext.Insertable(model).ExecuteCommandAsync();
            //}
            PageResult<AnnualLeavePlanResponseDto> result = new();
            RefAsync<int> total = 0;
            var data = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1)
                .LeftJoin(_dbContext.Queryable<LeavePlanSubmitInfo>().Where(t => t.IsDelete == 1 && t.Year == requestDto.Year), (x, y) => x.BusinessId == y.ShipId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ShipName), (x, y) => SqlFunc.Equals(x.ShipName, requestDto.ShipName))
                .WhereIF(requestDto.IsSubmit != null && requestDto.IsSubmit == true, (x, y) => y.IsSubmit == requestDto.IsSubmit)
                .WhereIF(requestDto.IsSubmit != null && requestDto.IsSubmit == false, (x, y) => y.ShipId == null)
                .Select((x, y) => new AnnualLeavePlanResponseDto
                {
                    ShipId = x.BusinessId,
                    LeaveYear = requestDto.Year <= 0 ? DateTime.Now.Year : requestDto.Year,
                    ShipName = x.ShipName,
                    ShipType = x.ShipType,
                    CountryId = x.Country,
                    CompanyId = x.Company,
                    SubStatus = y.IsSubmit,
                    SubUser = y.SubUser,
                    SubTime = y.Created.Value.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
            //获取国家
            var countryInfo = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1 && data.Select(t => t.CountryId).Contains(t.BusinessId)).ToListAsync();
            //获取公司
            var institutionInfo = await _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && data.Select(t => t.CompanyId).Contains(t.BusinessId)).ToListAsync();
            //获取船舶关联项目
            var shipProjectInfo = await _dbContext.Queryable<ShipProjectRelation>().Where(t => t.IsDelete == 1 && data.Select(t => t.ShipId).Contains(t.RelationShipId)).ToListAsync();

            var shipIds = data.Select(t => t.ShipId.ToString()).Distinct().ToArray();
            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .Where(t => shipIds.Contains(t.OnShip))
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId)
                .Select((t, ws) => new SearchLeavePlanUserResponseDto
                {
                    UserId = t.BusinessId,
                    ShipId = ws.OnShip,
                    UserName = t.Name,
                    JobTypeId = ws.Postition,
                })
                .ToListAsync();
            foreach (var item in data)
            {
                item.ShipTypeName = GetEnumDescription(item.ShipType);
                item.CountryName = countryInfo.FirstOrDefault(t => t.BusinessId == item.CountryId)?.Name ?? "";
                item.ProjectName = shipProjectInfo.FirstOrDefault(t => t.BusinessId == item.ShipId)?.ProjectName ?? "";
                item.CompanyName = institutionInfo.FirstOrDefault(t => t.BusinessId == item.CompanyId)?.Name ?? "";
                item.Captain = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "93f80b81-cf29-11ef-82f9-ecd68ace58a2")?.UserName ?? "";
                item.Secretary = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "")?.UserName ?? "";
                item.ChiefEngineer = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "93f86f23-cf29-11ef-82f9-ecd68ace58a2")?.UserName ?? "";
            }
            result.List = data;
            result.TotalCount = total;
            return result;
        }

        /// <summary>
        /// 年休计划  获取船舶人员信息
        /// </summary>
        /// <param name="ShipId"></param>
        /// <returns></returns>
        public async Task<Result> SearchLeavePlanUserAsync(SearchLeavePlanUserRequestDto requestDto)
        {
            #region 任职船舶
            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .Where(t => t.OnShip == requestDto.ShipId.ToString())
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion

            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), (t, ws) => SqlFunc.Contains(t.Name, requestDto.KeyWords) || SqlFunc.Contains(t.Phone, requestDto.KeyWords) || SqlFunc.Contains(t.CardId, requestDto.KeyWords) || SqlFunc.Contains(t.WorkNumber, requestDto.KeyWords))
                .Select((t, ws) => new SearchLeavePlanUserResponseDto
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    JobTypeId = ws.Postition,
                })
                .ToListAsync();
            var userIds = userInfos.Select(t => t.UserId).ToArray();
            var jobTypeId = userInfos.Select(t => t.JobTypeId).ToArray();

            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            //文件
            var fileAll = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && userIds.Contains(t.UserId)).ToListAsync();
            var certificateInfo = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => t.Type == CertificatesEnum.FCertificate && userIds.Contains(t.CertificateId)).ToListAsync();
            var positionInfo = await _dbContext.Queryable<PositionOnBoard>().Where(t => jobTypeId.Contains(t.BusinessId.ToString())).ToListAsync();
            foreach (var item in userInfos)
            {
                //第一适任证书职务
                var position = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FPosition;
                var area = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FNavigationArea;
                //第一适任扫描件
                var scans = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FScans;
                item.JobTypeName = positionInfo.FirstOrDefault(x => x.BusinessId.ToString() == position)?.Name ?? "";
                item.CertificateId = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.BusinessId;
                item.CertificateName = position + area;
                item.CertificateScans = url + fileAll.FirstOrDefault(t => t.FileId == scans)?.Name;
            }

            return Result.Success(userInfos);
        }

        /// <summary>
        /// 新增或修改年休计划
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Result> SaveLeavePlanUserVacationAsync(AddLeavePlanVacationRequestDto requestDto)
        {
            try
            {
                List<LeavePlanUser> leavePlans = new List<LeavePlanUser>();
                List<LeavePlanUserVacation> addLeavePlanUsers = new List<LeavePlanUserVacation>();
                List<LeavePlanUserVacation> updateLeavePlanUsers = new List<LeavePlanUserVacation>();
                //获取对应船舶年休数据
                var vacationList = await _dbContext.Queryable<LeavePlanUserVacation>().Where(t => t.IsDelete == 1 && t.ShipId == requestDto.ShipId && t.Year == requestDto.Year).ToListAsync();
                LeavePlanSubmitInfo leavePlanBaseInfo = new LeavePlanSubmitInfo();
                leavePlanBaseInfo.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                leavePlanBaseInfo.BusinessId = GuidUtil.Next();
                leavePlanBaseInfo.ShipId = requestDto.ShipId;
                leavePlanBaseInfo.Year = requestDto.Year;
                leavePlanBaseInfo.IsSubmit = true;
                leavePlanBaseInfo.SubUser = GlobalCurrentUser.Name ?? "";
                foreach (var item in requestDto.vacationVBases)
                {
                    LeavePlanUser leavePlanUser = new LeavePlanUser();
                    leavePlanUser.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    leavePlanUser.BusinessId = GuidUtil.Next();
                    leavePlanUser.UserId = item.UserId;
                    leavePlanUser.ShipId = requestDto.ShipId;
                    leavePlanUser.UserName = item.UserName;
                    leavePlanUser.JobTypeId = item.JobTypeId;
                    leavePlanUser.CertificateId = item.CertificateId;
                    leavePlanUser.IsOnShipLastYear = item.IsOnShipLastYear;
                    leavePlanUser.IsOnShipCurrentYear = item.IsOnShipCurrentYear;
                    leavePlanUser.Remark = item.Remark;
                    leavePlanUser.Year = requestDto.Year;
                    leavePlans.Add(leavePlanUser);
                    foreach (var item2 in item.vacationInfos)
                    {
                        //查询用户当前年月是否有数据
                        var data = vacationList.Where(t => t.UserId == item.UserId && t.Month == item2.Month && t.VacationHalfMonth == item2.VacationHalfMonth).FirstOrDefault();
                        if (data == null)
                        {
                            LeavePlanUserVacation model = new LeavePlanUserVacation();
                            model.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                            model.BusinessId = GuidUtil.Next();
                            model.ShipId = requestDto.ShipId;
                            model.UserId = item.UserId;
                            model.Year = requestDto.Year;
                            model.Month = item2.Month;
                            model.VacationHalfMonth = item2.VacationHalfMonth;
                            model.VacationMonth = 0.5;
                            addLeavePlanUsers.Add(model);
                        }
                        else
                        {
                            data.IsDelete = 0;
                            updateLeavePlanUsers.Add(data);
                        }
                    }
                }

                await _dbContext.Insertable(leavePlanBaseInfo).ExecuteCommandAsync();
                if (leavePlans.Any())
                {
                    await _dbContext.Insertable(leavePlans).ExecuteCommandAsync();
                }
                if (addLeavePlanUsers.Any())
                {
                    await _dbContext.Insertable(addLeavePlanUsers).ExecuteCommandAsync();
                }
                if (updateLeavePlanUsers.Any())
                {
                    await _dbContext.Updateable(updateLeavePlanUsers).WhereColumns(t => t.BusinessId).UpdateColumns(t => new { t.IsDelete, t.ModifiedBy, t.Modified }).ExecuteCommandAsync();
                }

            }
            catch (Exception)
            {
                return Result.Fail("提交失败");
            }
            return Result.Success("提交成功");
        }

        /// <summary>
        /// 获取休假日期详情
        /// </summary>
        /// <returns></returns>
        public async Task<Result> SearchLeaveDetailAsync(SearchLeavePlanUserRequestDto requestDto)
        {
            List<SearchLeaveDetailResponseDto> searchLeave = new List<SearchLeaveDetailResponseDto>();
            if (requestDto.Year <= 0)
            {
                requestDto.Year = DateTime.Now.Year;
            }
            var data = await SearchLeavePlanUserAsync(requestDto);
            var list = data.Data == null ? new List<SearchLeavePlanUserResponseDto>() : (List<SearchLeavePlanUserResponseDto>)data.Data;
            var userIds = list.Select(t => t.UserId).Distinct().ToArray();
            //获取休假日期明细
            var leaveInfo = await _dbContext.Queryable<LeavePlanUserVacation>().Where(t => t.IsDelete == 1 && userIds.Contains(t.UserId) && t.ShipId == requestDto.ShipId && t.Year == requestDto.Year).ToListAsync();

            foreach (var item in list)
            {
                SearchLeaveDetailResponseDto model = new SearchLeaveDetailResponseDto();
                model = _mapper.Map<SearchLeavePlanUserResponseDto, SearchLeaveDetailResponseDto>(item);
                model.Year = requestDto.Year;
                for (int i = 1; i <= 12; i++)
                {
                    VacationList model2 = new VacationList();
                    //查询对应月份上半是否休假
                    var firstHalf = leaveInfo.FirstOrDefault(t => t.UserId == item.UserId && t.Month == i && t.VacationHalfMonth == 1);
                    model2.Month = i;
                    if (firstHalf != null) model2.FirstHalf = true;
                    //查询对应月份下半是否休假
                    var lowerHalf = leaveInfo.FirstOrDefault(t => t.UserId == item.UserId && t.Month == i && t.VacationHalfMonth == 2);
                    if (lowerHalf != null) model2.LowerHalf = true;
                    model.vacationLists.Add(model2);
                }
                //统计当前人员总休假月数
                model.LeaveMonth = leaveInfo.Where(t => t.UserId == item.UserId).Select(t => t.VacationMonth).Sum();
                model.OnShipMonth = 12 - model.LeaveMonth;
                searchLeave.Add(model);
            }
            return Result.Success(searchLeave);
        }

        /// <summary>
        /// 年休假规则验证
        /// </summary>
        /// <returns></returns>
        public async Task<Result> LeaveCheckRuleAsync(LeaveCheckRuleRequestDto requestDto)
        {
            #region 任职船舶
            //任职船舶 
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .Where(t => t.OnShip == requestDto.ShipId.ToString())
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion
            //获取所有在船的人员数据
            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId)
                .Select((t, ws) => new SearchLeavePlanUserResponseDto
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    JobTypeId = ws.Postition,
                })
                .ToListAsync();

            //获取所有已休假的人员
            var leaveVacation = _dbContext.Queryable<LeavePlanUserVacation>()
                .Where(t => t.IsDelete == 1 && t.UserId == requestDto.UserId && t.ShipId == requestDto.ShipId
                            && t.Year == requestDto.Year && t.Month == requestDto.Month
                            && t.VacationHalfMonth == requestDto.VacationHalfMonth);
            var leaveUser = await _dbContext.Queryable<LeavePlanUser>().InnerJoin(leaveVacation, (x, y) => x.UserId == y.UserId && x.ShipId == y.ShipId && x.Year == y.Year)
                                      .Where((x, y) => x.IsDelete == 1)
                                      .Select((x, y) => new
                                      {
                                          x.UserId,
                                          x.UserName,
                                          x.JobTypeId,
                                          x.ShipId,
                                          x.Year,
                                          y.Month,
                                          y.VacationHalfMonth
                                      })
                                      .ToListAsync();

            //获取对应船舶的定员标准
            var shipStandard = await _dbContext.Queryable<ShipPersonnelStandard>().Where(t => t.IsDelete == 1 && t.ShipStatus == 1 && t.ShipId == requestDto.ShipId)
                .InnerJoin(_dbContext.Queryable<ShipPersonnelPosition>().Where(t => t.IsDelete == 1), (x, y) => x.BusinessId == y.ConnectionId)
                .Where((x, y) => y.PositionId == requestDto.JobTypeId)
                .Select((x, y) => new
                {
                    x.ShipId,
                    x.ShipName,
                    y.PositionId,
                    y.Position,
                    y.Num
                })
                .FirstAsync();
            var result = false;
            //先查出同条船上相同职务的人员数量
            var positionNum = userInfos.Where(t => t.JobTypeId == requestDto.JobTypeId.ToString() && t.UserId != requestDto.UserId).Count();
            //在判断当前人员是否有休假
            if (leaveUser.Any())
            {
                if (shipStandard != null)
                {
                    result = positionNum < shipStandard.Num;
                    return Result.Success(result);
                }
            }
            return Result.Success(result);
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
