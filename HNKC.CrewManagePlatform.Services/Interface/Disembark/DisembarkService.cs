using AutoMapper;
using System.Runtime.InteropServices.JavaScript;
using HNKC.CrewManagePlatform.Exceptions;
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
    public class DisembarkService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService,
        IDisembarkService
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

            // 获取当前登录用户
            var userBusinessId = GlobalCurrentUser.UserBusinessId;

            var departureApplyVoList =
                await _dbContext.Queryable<DepartureApply>()
                    .Where(da => da.IsDelete == 1)
                    .Where(da => da.UserId == userBusinessId || da.ApproveUserId == userBusinessId)
                    .WhereIF(
                        !string.IsNullOrWhiteSpace(query.StartTime.ToString()) &&
                        !string.IsNullOrWhiteSpace(query.EndTime.ToString()),
                        (da) => da.Created >= Convert.ToDateTime(query.StartTime) &&
                                da.Created < Convert.ToDateTime(query.EndTime.Value.AddDays(1)))
                    .WhereIF(query.Status == 1,da => da.ApproveStatus == ApproveStatus.PExamination)
                    .WhereIF(query.Status == 2,da => da.ApproveStatus == ApproveStatus.Pass)
                    .WhereIF(query.Status == 3,da => da.ApproveStatus == ApproveStatus.Reject)
                    .WhereIF(!string.IsNullOrEmpty(query.ShipId.ToString()), da => da.ShipId == query.ShipId)
                    .WhereIF(!string.IsNullOrEmpty(query.UserId.ToString()), da => da.UserId == query.UserId)
                    .LeftJoin<User>((da, u) => da.UserId == u.BusinessId)
                    .LeftJoin(wShip, (da, u, ws) => ws.WorkShipId == da.UserId)
                    .LeftJoin<OwnerShip>((da, u, ws, ow) => ws.OnShip == ow.BusinessId.ToString())
                    .WhereIF(!string.IsNullOrEmpty(query.UserName),(da, u)=>u.Name.Contains(query.UserName))
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

            var roleType = await _baseService.CurRoleType();
            // 判断角色类型
            if (roleType == 3)
            {
                return Result.Fail("非船长角色无法提交申请");
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

            if (shipId == Guid.Empty || string.IsNullOrWhiteSpace(shipId.ToString()))
            {
                return Result.Fail("船舶ID不能为空");
            }

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
                .Where((u, ir, r) => r.Type == 4 && ir.InstitutionBusinessId == company.Company && r.IsApprove)
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
            //RefAsync<int> total = 0;
            if (requestDto.Year <= 0)
            {
                requestDto.Year = DateTime.Now.Year;
            }
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
                    SubTime = !string.IsNullOrWhiteSpace(y.Modified.ToString()) ? y.Modified.Value.ToString("yyyy-MM-dd HH:mm:ss") : y.Created.Value.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToListAsync();

            var pageResult = data.Select(x => new AnnualLeavePlanResponseDto
            {
                ShipId = x.ShipId,
                LeaveYear = requestDto.Year <= 0 ? DateTime.Now.Year : requestDto.Year,
                ShipName = x.ShipName,
                ShipType = x.ShipType,
                CountryId = x.CompanyId,
                CompanyId = x.CompanyId,
                SubStatus = x.SubStatus,
                SubUser = x.SubUser,
                SubTime = x.SubTime,
                ShipNamePinyin = Utils.Utils.GetPinyinInitials(x.ShipName ?? "")
            })
            .OrderBy(x => x.SubStatus).ThenBy(x => x.ShipNamePinyin)
            .Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();


            //获取国家
            var countryInfo = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1 && pageResult.Select(t => t.CountryId).Contains(t.BusinessId)).ToListAsync();
            //获取公司
            var institutionInfo = await _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && pageResult.Select(t => t.CompanyId).Contains(t.BusinessId)).ToListAsync();
            //获取船舶关联项目
            var shipProjectInfo = await _dbContext.Queryable<ShipProjectRelation>().Where(t => t.IsDelete == 1 && pageResult.Select(t => t.ShipId).Contains(t.RelationShipId)).ToListAsync();

            var shipIds = pageResult.Select(t => t.ShipId.ToString()).Distinct().ToArray();
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
            foreach (var item in pageResult)
            {
                item.ShipTypeName = GetEnumDescription(item.ShipType);
                item.CountryName = countryInfo.FirstOrDefault(t => t.BusinessId == item.CountryId)?.Name ?? "";
                item.ProjectName = shipProjectInfo.FirstOrDefault(t => t.BusinessId == item.ShipId)?.ProjectName ?? "";
                item.CompanyName = institutionInfo.FirstOrDefault(t => t.BusinessId == item.CompanyId)?.Name ?? "";
                item.Captain = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "93f80b81-cf29-11ef-82f9-ecd68ace58a2")?.UserName ?? "";
                item.Secretary = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "")?.UserName ?? "";
                item.ChiefEngineer = userInfos.FirstOrDefault(t => t.ShipId == item.ShipId.ToString() && t.JobTypeId == "93f86f23-cf29-11ef-82f9-ecd68ace58a2")?.UserName ?? "";
            }
            result.List = pageResult;
            result.TotalCount = data.Count;
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
                .WhereIF(requestDto.ShipId != null && requestDto.ShipId.Length > 0, t => requestDto.ShipId.Contains(t.OnShip))
              .GroupBy(u => u.WorkShipId)
              .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>().WhereIF(requestDto.ShipId != null && requestDto.ShipId.Length > 0, t => requestDto.ShipId.Contains(t.OnShip))
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion

            var userInfos = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .InnerJoin(wShip, (t, ws) => ws.WorkShipId == t.BusinessId)
                .LeftJoin(_dbContext.Queryable<LeavePlanUser>().Where(t => t.IsDelete == 1), (t, ws, le) => t.BusinessId == le.UserId && ws.OnShip == le.ShipId.ToString())
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), (t, ws) => SqlFunc.Contains(t.Name, requestDto.KeyWords) || SqlFunc.Contains(t.Phone, requestDto.KeyWords) || SqlFunc.Contains(t.CardId, requestDto.KeyWords) || SqlFunc.Contains(t.WorkNumber, requestDto.KeyWords))
                .WhereIF(requestDto.Year == 1, (t, ws, le) => le.IsOnShipCurrentYear && le.Year == DateTime.Now.Year)
                .WhereIF(requestDto.Year == 2, (t, ws, le) => le.IsOnShipLastYear && le.Year == DateTime.Now.Year - 1)
                .Select((t, ws, le) => new SearchLeavePlanUserResponseDto
                {
                    UserId = t.BusinessId,
                    UserName = t.Name,
                    JobTypeId = ws.Postition,
                    ShipId = ws.OnShip,
                    IsOnShipCurrentYear = le.IsOnShipCurrentYear,
                    IsOnShipLastYear = le.IsOnShipLastYear
                })
                .ToListAsync();
            var userIds = userInfos.Select(t => t.UserId).ToArray();
            var jobTypeId = userInfos.Select(t => t.JobTypeId).ToArray();

            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            //文件
            var fileAll = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && userIds.Contains(t.UserId)).ToListAsync();
            var certificateInfo = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => t.Type == CertificatesEnum.FCertificate && userIds.Contains(t.CertificateId)).ToListAsync();
            var positionInfo = await _dbContext.Queryable<PositionOnBoard>().Where(t => jobTypeId.Contains(t.BusinessId.ToString())).ToListAsync();
            var areaIds = certificateInfo.Select(t => t.FNavigationArea).Distinct().ToArray();
            var navigationInfo = await _dbContext.Queryable<NavigationArea>().Where(t => areaIds.Contains(t.BusinessId.ToString())).ToListAsync();
            foreach (var item in userInfos)
            {
                //第一适任证书职务
                var position = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FPosition;
                var area = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FNavigationArea;
                //第一适任扫描件
                var scans = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.FScans;
                item.JobTypeName = positionInfo.FirstOrDefault(x => x.BusinessId.ToString() == position)?.Name ?? "";
                item.CertificateId = certificateInfo.FirstOrDefault(x => x.CertificateId == item.UserId)?.BusinessId;
                item.CertificateName = item.JobTypeName + navigationInfo.FirstOrDefault(t => t.BusinessId.ToString() == area)?.Name;
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
                List<LeavePlanUser> addleavePlans = new List<LeavePlanUser>();
                List<LeavePlanUser> updateleavePlans = new List<LeavePlanUser>();
                List<LeavePlanUserVacation> addLeavePlanUsers = new List<LeavePlanUserVacation>();
                List<LeavePlanUserVacation> updateLeavePlanUsers = new List<LeavePlanUserVacation>();

                //获取船舶年休假填报信息
                var leaveSubmitInfo = await _dbContext.Queryable<LeavePlanSubmitInfo>().Where(t => t.IsDelete == 1 && t.ShipId == requestDto.ShipId).FirstAsync();
                //获取船舶年休假 人员信息
                var leaveUserInfo = await _dbContext.Queryable<LeavePlanUser>().Where(t => t.IsDelete == 1 && requestDto.vacationVBases.Select(t => t.UserId).Contains(t.UserId) && t.ShipId == requestDto.ShipId).ToListAsync();
                //获取对应船舶年休数据
                var vacationList = await _dbContext.Queryable<LeavePlanUserVacation>().Where(t => t.ShipId == requestDto.ShipId && t.Year == requestDto.Year).ToListAsync();
                LeavePlanSubmitInfo leavePlanBaseInfo = new LeavePlanSubmitInfo();
                if (leaveSubmitInfo == null)
                {
                    leavePlanBaseInfo.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    leavePlanBaseInfo.BusinessId = GuidUtil.Next();
                    leavePlanBaseInfo.ShipId = requestDto.ShipId;
                    leavePlanBaseInfo.Year = requestDto.Year;
                    leavePlanBaseInfo.IsSubmit = true;
                    leavePlanBaseInfo.SubUserId = GlobalCurrentUser.UserBusinessId;
                    leavePlanBaseInfo.SubUser = GlobalCurrentUser.Name ?? "";
                }
                foreach (var item in requestDto.vacationVBases)
                {
                    LeavePlanUser leavePlanUser = new LeavePlanUser();
                    var user = leaveUserInfo.FirstOrDefault(t => t.UserId == item.UserId);
                    if (user == null)
                    {
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
                        addleavePlans.Add(leavePlanUser);
                    }
                    else
                    {
                        updateleavePlans.Add(user);
                    }

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
                            model.IsAbnormal = item2.IsAbnormal;
                            addLeavePlanUsers.Add(model);
                        }
                        else
                        {
                            //若存在 则只修改删除状态 防止重复添加无用数据
                            if (data.IsDelete == 0)
                            {
                                data.IsDelete = 1;
                            }
                            else
                            {
                                data.IsDelete = 0;
                            }
                            updateLeavePlanUsers.Add(data);
                        }
                    }
                }

                if (leaveSubmitInfo == null) await _dbContext.Insertable(leavePlanBaseInfo).ExecuteCommandAsync();
                else await _dbContext.Updateable(leaveSubmitInfo).ExecuteCommandAsync();

                if (addleavePlans.Any())
                {
                    await _dbContext.Insertable(addleavePlans).ExecuteCommandAsync();
                }
                if (updateleavePlans.Any())
                {
                    await _dbContext.Updateable(updateleavePlans).ExecuteCommandAsync();
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
            if (requestDto.ShipId == null)
            {
                requestDto.ShipId = new string[] { };
            }
            var data = await SearchLeavePlanUserAsync(requestDto);
            var list = data.Data == null ? new List<SearchLeavePlanUserResponseDto>() : (List<SearchLeavePlanUserResponseDto>)data.Data;
            var userIds = list.Select(t => t.UserId).Distinct().ToArray();
            //获取休假日期明细
            var leaveInfo = await _dbContext.Queryable<LeavePlanUserVacation>().Where(t => t.IsDelete == 1 && userIds.Contains(t.UserId) && requestDto.ShipId.Contains(t.ShipId.ToString()) && t.Year == requestDto.Year).ToListAsync();
            var shipInfo = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1 && requestDto.ShipId.Contains(t.BusinessId.ToString())).Select(t => new { ShipId = t.BusinessId.ToString(), t.ShipName }).ToListAsync();
            var leaveUser = await _dbContext.Queryable<LeavePlanUser>().Where(t => t.IsDelete == 1 && userIds.Contains(t.UserId) && requestDto.ShipId.Contains(t.ShipId.ToString())).ToListAsync();
            if (requestDto.ShipId != null && requestDto.ShipId.Length > 0)
            {
                foreach (var ship in requestDto.ShipId)
                {
                    SearchLeaveDetailResponseDto detail = new SearchLeaveDetailResponseDto();
                    detail.ShipId = ship;
                    detail.ShipName = shipInfo.FirstOrDefault(t => t.ShipId == ship)?.ShipName ?? "";
                    foreach (var item in list.Where(t => t.ShipId == ship))
                    {
                        LeaveUserInfo model = new LeaveUserInfo();
                        model = _mapper.Map<SearchLeavePlanUserResponseDto, LeaveUserInfo>(item);
                        model.Year = requestDto.Year;
                        for (int i = 1; i <= 12; i++)
                        {
                            VacationList model2 = new VacationList();
                            var leaveFirst = leaveInfo.Where(t => t.UserId == item.UserId && t.ShipId.ToString() == item.ShipId && t.Month == i);
                            //查询对应月份上半是否休假
                            var firstHalf = leaveFirst.FirstOrDefault(t => t.VacationHalfMonth == 1);
                            model2.Month = i;
                            if (firstHalf != null) model2.FirstHalf = true;
                            //查询对应月份下半是否休假
                            var lowerHalf = leaveFirst.FirstOrDefault(t => t.VacationHalfMonth == 2);
                            if (lowerHalf != null) model2.LowerHalf = true;
                            model2.FirstHalfAbnormal = leaveFirst.FirstOrDefault(t => t.VacationHalfMonth == 1)?.IsAbnormal ?? false;
                            model2.LowerHalfAbnormal = leaveFirst.FirstOrDefault(t => t.VacationHalfMonth == 2)?.IsAbnormal ?? false;
                            model.vacationLists.Add(model2);
                        }
                        //统计当前人员总休假月数
                        model.LeaveMonth = leaveInfo.Where(t => t.UserId == item.UserId && t.ShipId.ToString() == item.ShipId).Select(t => t.VacationMonth).Sum();
                        model.OnShipMonth = 12 - model.LeaveMonth;
                        var leaveUserInfo = leaveUser.Where(t => t.UserId == item.UserId && t.ShipId.ToString() == item.ShipId && t.Year == requestDto.Year).FirstOrDefault();
                        model.IsOnShipLastYear = leaveUserInfo?.IsOnShipLastYear ?? false;
                        model.IsOnShipCurrentYear = leaveUserInfo?.IsOnShipCurrentYear ?? false;
                        model.Remark = leaveUserInfo?.Remark ?? "";
                        detail.leaveUsers.Add(model);
                    }
                    searchLeave.Add(detail);
                }
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
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.OnShip == requestDto.ShipId.ToString())
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