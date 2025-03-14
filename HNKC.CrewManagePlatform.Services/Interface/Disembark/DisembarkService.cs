using System.Runtime.InteropServices.JavaScript;
using HNKC.CrewManagePlatform.Exceptions;
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
    public class DisembarkService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService,
        IDisembarkService
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
        /// 查询离船申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PageResult<DepartureApplyVo>> DepartureApplyListAsync(DepartureApplyQuery query)
        {
            PageResult<DepartureApplyVo> result = new();
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            // 判断角色类型
            if (roleType == -1)
            {
                return new PageResult<DepartureApplyVo>();
            }

            #region 船舶相关

            //任职船舶
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .GroupBy(u => u.WorkShipId)
                .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
                .InnerJoin(crewWorkShip,
                    (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

            #endregion

            var departureApplyVoList =
                await _dbContext.Queryable<DepartureApply>()
                    .Where(da => da.IsDelete == 1)
                    .WhereIF(
                        !string.IsNullOrWhiteSpace(query.StartTime.ToString()) &&
                        !string.IsNullOrWhiteSpace(query.EndTime.ToString()),
                        (da) => da.Created >= Convert.ToDateTime(query.StartTime) &&
                                da.Created < Convert.ToDateTime(query.EndTime.Value.AddDays(1)))
                    // .WhereIF(!string.IsNullOrEmpty(query.ApproveStatus.ToString()), da => da.ApproveStatus == query.ApproveStatus)
                    .WhereIF(!string.IsNullOrEmpty(query.ShipId.ToString()), da => da.ShipId == query.ShipId)
                    .LeftJoin<User>((da, u) => da.UserId == u.BusinessId)
                    .LeftJoin(wShip, (da, u, ws) => ws.WorkShipId == da.UserId)
                    .LeftJoin<OwnerShip>((da, u, ws, ow) => ws.OnShip == ow.BusinessId.ToString())
                    // .WhereIF(roleType == 3, (da, u, ws, ow) => da.ApproveUserId == GlobalCurrentUser.UserBusinessId)
                    .OrderBy(da => da.ApproveStatus).OrderBy(da => SqlFunc.Desc(da.ApplyTime))
                    .Select((da, u, ws, ow) => new DepartureApplyVo
                    {
                        ApplyCode = da.ApplyCode,
                        UserId = da.UserId,
                        UserName = u.Name,
                        ShipType = ow.ShipType,
                        ShipName = ow.ShipName,
                        Company = ow.Company,
                        Country = ow.Country,
                        ShipId = ow.BusinessId,
                        ProjectName = ow.ProjectName,
                        ApproveStatus = da.ApproveStatus,
                        DisembarkDate = da.DisembarkDate,
                        ApplyTime = da.ApplyTime
                    })
                    .ToPageListAsync(query.PageIndex, query.PageSize, total);
            var countrys = departureApplyVoList.Select(x => x.Country).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrys.Contains(t.BusinessId))
                .ToListAsync();
            var ins = departureApplyVoList.Select(x => x.Company).ToList();
            var companys = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1 && ins.Contains(t.BusinessId))
                .ToListAsync();
            foreach (var item in departureApplyVoList)
            {
                item.ShipTypeName = EnumUtil.GetDescription(item.ShipType);
                item.CountryName = country.FirstOrDefault(x => x.BusinessId == item.Country)?.Name;
                item.CompanyName = companys.FirstOrDefault(x => x.BusinessId == item.Company)?.Name;
            }

            result.List = departureApplyVoList;
            result.TotalCount = total;
            return result;
        }

        /// <summary>
        /// 提交离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SubmitDepartureApplyAsync(DepartureApplyDto requestBody)
        {
            if (requestBody.DepartureApplyUserDtoList is not { Count: > 0 })
            {
                return Result.Fail("离船人员不能为空");
            }

            // 创建申请单据
            Guid applyCodeNo = Guid.NewGuid();
            await _dbContext.Insertable(new DepartureApply
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                ApplyCode = applyCodeNo,
                UserId = GlobalCurrentUser.UserBusinessId,
                ShipId = requestBody.ShipId,
                DisembarkDate = requestBody.DisembarkDate,
                ApproveUserId = requestBody.ApproveUserId,
                ApproveStatus = Models.Enums.ApproveStatus.PExamination,
                ApplyTime = DateTime.Now,
                BusinessId = applyCodeNo
            }).ExecuteCommandAsync();

            List<DepartureApplyUser> departureApplyUserList = new List<DepartureApplyUser>();

            foreach (var departureApplyUserDto in requestBody.DepartureApplyUserDtoList)
            {
                if (!string.IsNullOrWhiteSpace(departureApplyUserDto.DisembarkDate.ToString()) &&
                    departureApplyUserDto.DisembarkDate > departureApplyUserDto.ReturnShipDate)
                    throw new BusinessException("离船日期不能大于归船日期" + departureApplyUserDto.ReturnShipDate);
                if (!ValidateUtils.ValidatePhone(departureApplyUserDto.Phone))
                    throw new BusinessException("联系电话不正确" + departureApplyUserDto.Phone);
                if (!ValidateUtils.ValidatePhoneNumber(departureApplyUserDto.FiexdLine))
                    throw new BusinessException("家庭固话不正确" + departureApplyUserDto.FiexdLine);

                departureApplyUserList.Add(new DepartureApplyUser
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    ApplyCode = applyCodeNo,
                    UserId = departureApplyUserDto.UserId,
                    DisembarkDate = departureApplyUserDto.DisembarkDate,
                    ReturnShipDate = departureApplyUserDto.ReturnShipDate,
                    Phone = departureApplyUserDto.Phone,
                    FiexdLine = departureApplyUserDto.FiexdLine,
                    ReliefUserId = departureApplyUserDto.ReliefUserId,
                    Remark = departureApplyUserDto.Remark,
                    BusinessId = Guid.NewGuid()
                });
            }

            await _dbContext.Insertable<DepartureApplyUser>(departureApplyUserList).ExecuteCommandAsync();

            return Result.Success(true, "提交成功");
        }

        /// <summary>
        /// 查询离船申请单
        /// </summary>
        /// <param name="applyCode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DepartureApplyDetailVo> DepartureApplyDetail(Guid applyCode)
        {
            #region 船舶相关

            //任职船舶
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .GroupBy(u => u.WorkShipId)
                .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
                .InnerJoin(crewWorkShip,
                    (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

            #endregion

            var result =
                await _dbContext.Queryable<DepartureApply>()
                    .Where(da => da.IsDelete == 1 && da.ApplyCode == applyCode)
                    .LeftJoin<User>((da, u) => da.UserId == u.BusinessId)
                    .LeftJoin(wShip, (da, u, ws) => ws.WorkShipId == da.UserId)
                    .LeftJoin<OwnerShip>((da, u, ws, ow) => ws.OnShip == ow.BusinessId.ToString())
                    .Select((da, u, ws, ow) => new DepartureApplyDetailVo()
                    {
                        ApplyCode = da.ApplyCode,
                        UserId = da.UserId,
                        UserName = u.Name,
                        ShipType = ow.ShipType,
                        ShipName = ow.ShipName,
                        Company = ow.Company,
                        Country = ow.Country,
                        ShipId = ow.BusinessId,
                        ProjectName = ow.ProjectName,
                        ApproveUserId = da.ApproveUserId,
                        ApproveStatus = da.ApproveStatus,
                        ApproveTime = da.ApproveTime,
                        ApproveOpinion = da.ApproveOpinion,
                        DisembarkDate = da.DisembarkDate,
                        ApplyTime = da.ApplyTime
                    })
                    .FirstAsync();

            if (result == null)
            {
                throw new BusinessException("申请单不存在");
            }

            result.ShipTypeName = EnumUtil.GetDescription(result.ShipType);
            var countryRegion = await _dbContext.Queryable<CountryRegion>()
                .Where((t) => t.IsDelete == 1 && t.BusinessId == result.Country)
                .FirstAsync();
            result.CountryName = countryRegion?.Name;
            var institution = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1 && t.BusinessId == result.Company)
                .FirstAsync();
            result.CompanyName = institution?.Name;

            // 查询离船人员

            var departureApplyUserList = await _dbContext.Queryable<DepartureApplyUser>()
                .Where(dau => dau.IsDelete == 1)
                .LeftJoin<User>((dau, u) => dau.UserId == u.BusinessId)
                .LeftJoin(wShip, (dau, u, ws) => ws.WorkShipId == u.BusinessId)
                .LeftJoin<PositionOnBoard>((dau, u, ws, po) => po.BusinessId.ToString() == ws.Postition)
                .LeftJoin<User>((dau, u, ws, po, ru) => ru.BusinessId == dau.ReliefUserId)
                .Select((dau, u, ws, po, ru) => new DepartureApplyUserVo
                {
                    BusinessId = dau.BusinessId,
                    UserId = dau.UserId,
                    UserName = u.Name,
                    Position = ws.Postition,
                    PositionName = po.Name,
                    WorkNumber = u.WorkNumber,
                    DisembarkDate = dau.DisembarkDate,
                    ReturnShipDate = dau.ReturnShipDate,
                    RealDisembarkDate = dau.RealDisembarkDate,
                    RealReturnShipDate = dau.RealReturnShipDate,
                    Phone = dau.Phone,
                    FiexdLine = dau.FiexdLine,
                    ReliefUserId = dau.ReliefUserId,
                    ReliefUserName = ru.Name,
                    Remark = dau.Remark
                }).ToListAsync();

            result.DepartureApplyUserList = departureApplyUserList;

            // 审批节点
            List<Guid?> userIds = new() { result.UserId, result.ApproveUserId };
            var userInfos = await _dbContext.Queryable<User>()
                .Where(u => userIds.Contains(u.BusinessId))
                .LeftJoin<Institution>((u, i) => u.Oid == i.Oid)
                .Select((u, i) => new
                {
                    UserId = u.BusinessId,
                    UserName = u.Name,
                    Picture = u.Phone,
                    Oid = i.Oid,
                    DepartmentName = i.ShortName
                })
                .ToListAsync();

            List<DepartureApplyLog> departureApplyLogList = new();

            var initiateUser = userInfos.FirstOrDefault(x => x.UserId == result.UserId);

            departureApplyLogList.Add(new DepartureApplyLog
            {
                UserId = initiateUser?.UserId,
                UserName = initiateUser?.UserName,
                Picture = initiateUser?.Picture,
                Oid = initiateUser?.Oid,
                DepartmentName = initiateUser?.DepartmentName,
                Operate = ApproveOperateStatus.Initiate,
                OperateTime = result.ApplyTime
            });

            var approveUser = userInfos.FirstOrDefault(x => x.UserId == result.ApproveUserId);

            var operate = ApproveOperateStatus.Pending;

            if (result.ApproveStatus == ApproveStatus.Pass)
            {
                operate = ApproveOperateStatus.Pass;
            }
            else if (result.ApproveStatus == ApproveStatus.Reject)
            {
                operate = ApproveOperateStatus.Reject;
            }

            departureApplyLogList.Add(new DepartureApplyLog
            {
                UserId = approveUser?.UserId,
                UserName = approveUser?.UserName,
                Picture = approveUser?.Picture,
                Oid = approveUser?.Oid,
                DepartmentName = approveUser?.DepartmentName,
                Operate = operate,
                OperateTime = result.ApproveTime,
                ApproveOpinion = result.ApproveOpinion
            });

            result.DepartureApplyLogList = departureApplyLogList;

            return result;
        }


        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SubmitApprove(SubmitApproveRequestDto requestBody)
        {
            var departureApply = await _dbContext.Queryable<DepartureApply>()
                .Where(da => da.IsDelete == 1 && da.ApplyCode == requestBody.ApplyCode)
                .FirstAsync();

            if (departureApply == null)
            {
                return Result.Fail("申请单不存在");
            }

            // 判断是否是可审批
            if (departureApply.ApproveUserId != GlobalCurrentUser.UserBusinessId)
            {
                return Result.Fail("无权限审批");
            }

            if (departureApply.ApproveStatus != ApproveStatus.PExamination)
            {
                return Result.Fail("当前非待审批状态");
            }

            if (requestBody.ApproveStatus != ApproveStatus.Pass && departureApply.ApproveStatus != ApproveStatus.Reject)
            {
                return Result.Fail("审批状态错误");
            }

            departureApply.ApproveStatus = requestBody.ApproveStatus == ApproveStatus.Pass
                ? ApproveStatus.Pass
                : ApproveStatus.Reject;
            departureApply.ApproveTime = DateTime.Now;
            departureApply.ApproveOpinion = requestBody.ApproveOpinion;

            await _dbContext.Updateable(departureApply).UpdateColumns(da => new
            {
                da.ApproveStatus,
                da.ApproveTime,
                da.ApproveOpinion
            }).ExecuteCommandAsync();

            return Result.Success("审批成功");
        }

        /// <summary>
        /// 填报实际离船时间
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> RegisterActualTime(RegisterActualTimeDto requestBody)
        {
            if (requestBody.RegisterActualUserList is not { Count: > 0 })
            {
                return Result.Fail("未填报数据");
            }

            var departureApply = await _dbContext.Queryable<DepartureApply>()
                .Where(da => da.IsDelete == 1 && da.ApplyCode == requestBody.ApplyCode)
                .FirstAsync();

            if (departureApply == null)
            {
                return Result.Fail("申请单不存在");
            }

            if (departureApply.ApproveStatus != ApproveStatus.Pass)
            {
                return Result.Fail("非审批通过状态");
            }

            List<DepartureApplyUser> departureApplyUserList = new();

            foreach (var registerActualUser in requestBody.RegisterActualUserList)
            {
                departureApplyUserList.Add(new DepartureApplyUser
                {
                    BusinessId = registerActualUser.BusinessId,
                    RealDisembarkDate = registerActualUser.RealDisembarkDate,
                    RealReturnShipDate = registerActualUser.RealReturnShipDate,
                    ModifiedBy = GlobalCurrentUser.UserBusinessId.ToString()
                });
            }

            await _dbContext.Updateable(departureApplyUserList).UpdateColumns(dau => new
                {
                    dau.RealDisembarkDate,
                    dau.RealReturnShipDate,
                    dau.ModifiedBy
                }).WhereColumns(dau => new { dau.BusinessId }).Where(dau => dau.ApplyCode == requestBody.ApplyCode)
                .ExecuteCommandAsync();

            return Result.Success("提交成功");
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
                var upBIds = requestBody.DisembarkDtos.Where(x => !string.IsNullOrWhiteSpace(x.BId.ToString()))
                    .Select(x => x.BId).ToList();
                var uIds = requestBody.DisembarkDtos.Where(x => !string.IsNullOrWhiteSpace(x.BId.ToString()))
                    .Select(x => x.UserId).ToList();
                var workShips = await _dbContext.Queryable<WorkShip>()
                    .Where(x => x.IsDelete == 1 && uIds.Contains(x.WorkShipId)).ToListAsync();
                var updateTable = await _dbContext.Queryable<DepartureApplication>()
                    .Where(t => t.IsDelete == 1 && upBIds.Contains(t.BusinessId))
                    .ToListAsync();
                var insertTable = requestBody.DisembarkDtos.Where(x => string.IsNullOrWhiteSpace(x.BId.ToString()))
                    .ToList();
                foreach (var up in updateTable)
                {
                    if (!string.IsNullOrWhiteSpace(up.RealDisembarkTime.ToString()) &&
                        up.DisembarkTime < up.RealDisembarkTime)
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
                        var wShip = workShips.Where(x => x.WorkShipId == up.UserId)
                            .OrderByDescending(x => x.WorkShipStartTime).FirstOrDefault();
                        if (wShip != null)
                        {
                            wShip.WorkShipEndTime = up.RealDisembarkTime;
                            wsMod.Add(wShip);
                        }
                    }
                }

                foreach (var ins in insertTable)
                {
                    if (!string.IsNullOrWhiteSpace(ins.DisembarkTime.ToString()) &&
                        ins.DisembarkTime > ins.ReturnShipTime)
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
                if (wsMod.Any())
                    await _dbContext.Updateable(wsMod).UpdateColumns(x => new { x.WorkShipEndTime })
                        .ExecuteCommandAsync();
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
            if (roleType == -1)
            {
                return new PageResult<SearchDisembark>();
            }

            #region 船舶相关

            //任职船舶
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                .GroupBy(u => u.WorkShipId)
                .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
                .InnerJoin(crewWorkShip,
                    (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);

            #endregion

            var rr = await _dbContext.Queryable<DepartureApplication>()
                .Where(t => t.IsDelete == 1 && t.ApproveStatus == Models.Enums.ApproveStatus.PExamination)
                .LeftJoin<User>((t, u) => t.UserId == u.BusinessId)
                .LeftJoin(wShip, (t, u, ws) => ws.WorkShipId == t.UserId)
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
                    string.IsNullOrWhiteSpace(item.RealDisembarkTime.ToString())
                        ? item.DisembarkTime.Value
                        : item.RealDisembarkTime.Value,
                    string.IsNullOrWhiteSpace(item.RealDisembarkTime.ToString())
                        ? item.ReturnShipTime.Value
                        : item.RealReturnShipTime.Value
                ).Days + 1;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }

        /// <summary>
        /// 船舶用户列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> ShipUserListAsync(Guid shipId)
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
                .LeftJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId && shipId.ToString() == ws.OnShip)
                .LeftJoin<PositionOnBoard>((t, ws, po) => po.BusinessId.ToString() == ws.Postition)
                .Select((t, ws, po) => new SchedulingUser
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    Position = ws.Postition,
                    PositionName = po.Name,
                    RotaEnum = po.RotaType,
                    WorkNumber = t.WorkNumber
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
        /// 船舶排班审批用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<Result> ApproveUserListAsync(Guid shipId)
        {
            if (shipId == Guid.Empty || string.IsNullOrWhiteSpace(shipId.ToString()))
            {
                return Result.Fail("船舶ID不能为空");
            }

            var company = await _dbContext.Queryable<OwnerShip>()
                .Where(os => os.BusinessId == shipId)
                .Select(os => new
                    {
                        os.Company
                    }
                ).FirstAsync();

            var userInfos = await _dbContext.Queryable<User>()
                .Where(u => u.IsLoginUser == 0)
                .LeftJoin<InstitutionRole>((u, ir) => u.BusinessId == ir.UserBusinessId)
                .LeftJoin<HNKC.CrewManagePlatform.SqlSugars.Models.Role>(
                    (u, ir, r) => r.BusinessId == ir.RoleBusinessId)
                .Where((u, ir, r) => r.Type == 3 && ir.InstitutionBusinessId == company.Company && r.IsApprove)
                .Select((u, ir) => new ApproveUser
                {
                    UserId = u.BusinessId,
                    UserName = u.Name,
                })
                .Distinct()
                .ToListAsync();

            return Result.Success(userInfos);
        }

        #endregion
    }
}