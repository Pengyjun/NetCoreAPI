using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
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
        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveDisembarkAsync(DisembarkRequest requestBody)
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
        public async Task<PageResult<SearchDisembark>> SearchDisembarkAsync(SearchDisembarkRequest requestBody)
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
    }
}
