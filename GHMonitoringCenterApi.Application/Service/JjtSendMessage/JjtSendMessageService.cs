using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.Timing;
using GHMonitoringCenterApi.Application.Service.File;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using SqlSugar.Extensions;
using UtilsSharp;
using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service.JjtSendMessage
{
    /// <summary>
    /// 交建通消息业务实现层
    /// </summary>
    public class JjtSendMessageService : IJjtSendMessageService
    {

        #region 依赖注入
        public ILogger<FileService> logger { get; set; }
        public ISqlSugarClient dbContext { get; set; }
        public IBaseService _baseService { get; set; }
        public ITimeService _timeService { get; set; }
        public IResourceManagementService _resourceManagementService { get; set; }
        public JjtSendMessageService(ILogger<FileService> logger, ISqlSugarClient dbContext, IBaseService baseService, ITimeService timeService, IResourceManagementService resourceManagementService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            _baseService = baseService;
            _timeService = timeService;
            _resourceManagementService = resourceManagementService;
        }
        #endregion
        /// <summary>
        /// 交建通消息单发
        /// </summary>
        /// <param name="singleMessageTemplateRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResponseAjaxResult<bool> MessageSending(SingleMessageTemplateRequestDto singleMessageTemplateRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var flag = JjtUtils.SinglePushMessage(singleMessageTemplateRequestDto);
            responseAjaxResult.Data = flag;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// jjt发送消息数值 卡片信息提示广航生产运营监控日报
        /// </summary>
        /// <param name="isFirst">是否是第一次发送消息</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextCardMsgForGHDayRep(bool isFirst)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            //响应结果 
            bool responseResult = false;
            //获取当日消息内容
            var isExist = await dbContext.Queryable<JjtWriteDataRecord>().Where(x => x.NowDay == DateTime.Now.ToDateDay()).FirstAsync();
            if (isExist != null)
            {
                await _timeService.GetServiceResponse<bool>(24);//修改船舶相关内容

                if (isExist.SendCount == 2)
                {
                    responseResult = false;
                    responseAjaxResult.FailResult(HttpStatusCode.PushFail, "推送次数超出规定次数，推送失败");
                    return responseAjaxResult;
                }
                string leadOrFollow = string.Empty;//超前还是滞后   （总产值-累计计划 ）/累计计划产值 
                var leadOrFollowRate = isExist.UptoNowDayPlanAccumulateOutputVal == 0 ? 0.00M : Math.Round((isExist.PAccumulateOutput - isExist.UptoNowDayPlanAccumulateOutputVal) / isExist.UptoNowDayPlanAccumulateOutputVal * 100, 2);
                if (leadOrFollowRate > 0) leadOrFollow = "超前" + leadOrFollowRate;
                else
                {
                    leadOrFollow = "滞后" + leadOrFollowRate;
                    leadOrFollow = leadOrFollow.Replace('-', ' ');
                }

                string shipleadOrFollwo = string.Empty;//船舶超前还是滞后
                var shipleadOrFollowRate = isExist.ShipUptoNowDayPlanAccumulateOutputVal == 0 ? 0.00M : Math.Round((isExist.OwnShipAccumulateOutputVal - isExist.ShipUptoNowDayPlanAccumulateOutputVal) / isExist.ShipUptoNowDayPlanAccumulateOutputVal * 100, 2);
                if (shipleadOrFollowRate > 0) shipleadOrFollwo = "超前" + shipleadOrFollowRate;
                else
                {
                    shipleadOrFollwo = "滞后" + shipleadOrFollowRate;
                    shipleadOrFollwo = shipleadOrFollwo.Replace('-', ' ');
                }

                if (isFirst)
                {
                    //第二次发送
                    if (string.IsNullOrWhiteSpace(isExist.ModifyTextMsg))
                    {
                        isExist.ModifyTextMsg = "广航局在建项目" + isExist.PInbuildNums + "个，正常施工" + isExist.PConstrucNums + "个，暂停" + isExist.pSuspendNums + "个；当日公司总产值" + Math.Round(isExist.POutputVal / 10000, 2) + "万元，本年项目开累产值" + Math.Round(isExist.PAccumulateOutput / 100000000, 2) + "亿元，较时间进度" + leadOrFollow + "%；*自有船施工船舶" + isExist.OwnShipNums + "艘，正常施工" + isExist.OwnShipConstrucNums + "艘，停置" + isExist.OwnShipStopNums + "艘；当日自有船产值" + Math.Round(isExist.OwnShipOutputVal / 10000, 2) + "万元，本年自有船开累产值" + Math.Round(isExist.OwnShipAccumulateOutputVal / 100000000, 2) + "亿元，较时间进度" + shipleadOrFollwo + "%";
                        await dbContext.Updateable(isExist).WhereColumns(x => new { x.Id }).ExecuteCommandAsync();
                    }
                }

                var userCodes = new List<string>();
                //获取发送的人员信息 获取当天分配的人员 如果没有获取最新的人员新增为当天分配人员
                var jjtMsgUsers = await dbContext.Queryable<JjtMessageUser>().Where(x => x.IsDelete == 1 && x.NowDay == DateTime.Now.ToDateDay()).ToListAsync();
                if (jjtMsgUsers.Count() > 1)
                {
                    //false 第一波人员
                    if (!isFirst)
                    {
                        userCodes = jjtMsgUsers.Where(x => x.UserType == 1).Select(x => x.UserGroupCode).ToList();
                    }
                    //第二波人员
                    else
                    {
                        userCodes = jjtMsgUsers.Where(x => x.UserType == 2).Select(x => x.UserGroupCode).ToList();
                    }
                }
                else
                {
                    //获取最新的人员
                    var maxNowDay = await dbContext.Queryable<JjtMessageUser>().MaxAsync(x => x.NowDay);
                    if (maxNowDay == 0)
                    {
                        responseResult = false;
                        responseAjaxResult.FailResult(HttpStatusCode.PushFail, "未分配人员，推送失败");
                        return responseAjaxResult;
                    }
                    var newnowDayjjtMsgUsers = await dbContext.Queryable<JjtMessageUser>().Where(x => x.NowDay == maxNowDay).ToListAsync();
                    //false 第一波人员
                    if (!isFirst)
                    {
                        userCodes = newnowDayjjtMsgUsers.Where(x => x.UserType == 1).Select(x => x.UserGroupCode).ToList();
                    }
                    //true 第二波人员
                    else
                    {
                        userCodes = newnowDayjjtMsgUsers.Where(x => x.UserType == 2).Select(x => x.UserGroupCode).ToList();
                    }
                    if (maxNowDay == DateTime.Now.ToDateDay())
                    {
                        //删除当日所有人员信息
                        await dbContext.Deleteable(newnowDayjjtMsgUsers).ExecuteCommandAsync();
                    }
                    //将最新数据作为今日数据新增
                    var data = new List<JjtMessageUser>();
                    newnowDayjjtMsgUsers.ForEach(x => data.Add(new JjtMessageUser
                    {
                        Id = GuidUtil.Next(),
                        NowDay = DateTime.Now.ToDateDay(),
                        UserName = x.UserName,
                        UserGroupCode = x.UserGroupCode,
                        UserType = x.UserType
                    }));
                    await dbContext.Insertable(data).ExecuteCommandAsync();
                }
                if (userCodes.Count() == 0)
                {
                    responseResult = false;
                    responseAjaxResult.FailResult(HttpStatusCode.PushFail, isFirst == true ? "未分配第二批人员，推送失败" : "未分配第一批人员，推送失败");
                    return responseAjaxResult;
                }
                //发送消息内容
                var msg = string.Empty;
                //如果第一次发送的内容为空 代表是当天的第一波用户发送消息
                if (string.IsNullOrWhiteSpace(isExist.FTextMsg))
                {
                    msg = "广航局在建项目" + isExist.PInbuildNums + "个，正常施工" + isExist.PConstrucNums + "个，暂停" + isExist.pSuspendNums + "个；当日公司总产值" + Math.Round(isExist.POutputVal / 10000, 2) + "万元，本年项目开累产值" + Math.Round(isExist.PAccumulateOutput / 100000000, 2) + "亿元，较时间进度" + leadOrFollow + "%；*自有船施工船舶" + isExist.OwnShipNums + "艘，正常施工" + isExist.OwnShipConstrucNums + "艘，停置" + isExist.OwnShipStopNums + "艘；当日自有船产值" + Math.Round(isExist.OwnShipOutputVal / 10000, 2) + "万元，本年自有船开累产值" + Math.Round(isExist.OwnShipAccumulateOutputVal / 100000000, 2) + "亿元，较时间进度" + shipleadOrFollwo + "%";
                    //消息内容写入
                    isExist.FTextMsg = msg;
                }
                //如果第一次发送的内容不为空且修改后的内容为空 代表是当天的第二波用户发送消息 且第二波消息内容数据为第一波消息内容
                else if (!string.IsNullOrWhiteSpace(isExist.FTextMsg) && string.IsNullOrWhiteSpace(isExist.ModifyTextMsg))
                {
                    isExist.ModifyTextMsg = isExist.FTextMsg;
                    msg = isExist.FTextMsg;
                }
                //两次或修改后的消息文本都不为空  取修改后的消息内容
                else
                {
                    msg = isExist.ModifyTextMsg;
                }
                isExist.SendCount++;
                await dbContext.Updateable(isExist).WhereColumns(x => new { x.Id }).ExecuteCommandAsync();

                //响应次数
                int responseCount = 0;
                //userCodes = new List<string>() { "2016146340" };
                var newMsg = msg.Split('*').ToArray();
                //循环次数
                var count = userCodes.Count() % 1000 == 0 ? userCodes.Count() / 1000 : Math.Floor(userCodes.Count() / 1000M) + 1;
                for (int i = 0; i < count; i++)
                {
                    var userCodePages = userCodes.Skip(i * 1000).Take(1000).ToList();
                    var msgResponse = new SingleMessageTemplateRequestDto
                    {
                        IsAll = false,
                        UserIds = userCodePages,
                        MessageType = JjtMessageType.TEXTCARD,
                        TextCardContent = new TextCardMessageContent
                        {
                            Description = "<div  class=\"highlight\">一、项目情况</div><div>" + newMsg[0] + "</div><div  class=\"highlight\">二、船舶情况</div><div>" + newMsg[1] + "</div>",
                            Title = "【" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广航局生产运营监控日报】",
                            Url = "https://www.baidu.com/"
                        }
                    };

                    var result = JjtUtils.SinglePushMessage(msgResponse);
                    //项目预警消息发送
                    await JjtSendTextMsgForGHDayRep(userCodePages);
                    if (result) responseCount++;
                }
                if (responseCount >= 1)
                {
                    responseResult = true;
                    responseAjaxResult.Data = responseResult;
                    responseAjaxResult.Success();
                }
                else
                {
                    responseResult = false;
                    responseAjaxResult.FailResult(HttpStatusCode.PushFail, "推送失败");
                }
            }
            else
            {
                responseResult = false;
                responseAjaxResult.FailResult(HttpStatusCode.PushFail, "推送数据日期不属今日，推送失败");
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取卡片消息详情
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JjtSendMsgDetailsResponse>> JjtSendTextCardMsgDetailsAsync()
        {
            ResponseAjaxResult<JjtSendMsgDetailsResponse> responseDto = new ResponseAjaxResult<JjtSendMsgDetailsResponse>();
            var jjtSendMsgDetailsResponse = new JjtSendMsgDetailsResponse();

            var result = await dbContext.Queryable<TempTable>().FirstAsync();
            if (result != null && !string.IsNullOrWhiteSpace(result.Value))
            {
                return JsonConvert.DeserializeObject<ResponseAjaxResult<JjtSendMsgDetailsResponse>>(result.Value);
            }
            //公司初始化
            List<string> oIds = new List<string>()
            {
                "101341960",//疏浚公司
                "101174265",//华南交建公司
                "101174254",//三公司
                "101332050",//四公司（北方分公司）
                "101305005",//五公司
                "101288132",//福建公司（中交广航（福建）交通建设有限公司（福建分公司））
                "101162350",//直营项目（广航局）
                //"101162702",//物流公司
                //"101172107",//测绘公司
                //"101162575",//设研院公司
            };
            //获取所有机构数据
            List<Institution> institions = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
            #region 项目相关
            //合同项目 
            var pInStatusIds = CommonData.PInStatusIds.Split(',').ToList();
            // 在建 
            var pConstruc = CommonData.PConstruc.Split(',').ToList();
            //暂停 
            var pSuspend = CommonData.PSuspend.Split(',').ToList();
            //获取所有项目信息  排除非施工类
            var projectsData = await dbContext.Queryable<Project>().Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid()).Select(x => new { x.Id, x.StatusId, x.PomId, x.CompanyId, x.Name, x.TypeId, x.CreateTime }).ToListAsync();
            //排除非施工项目的项目ids
            var notContainsProIds = projectsData.Select(x => x.Id).ToList();
            //项目总体生产项目信息
            var projects = projectsData.Where(x => pInStatusIds.Contains(x.StatusId.ToString())).Select(x => new { x.Id, x.StatusId, x.PomId, x.CompanyId, x.Name, x.CreateTime, x.TypeId }).ToList();
            //获取项目日报数据
            var proIds = projects.Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid() && pConstruc.Contains(x.StatusId.ToString())).Select(x => x.Id).ToList();
            //var proPomIds = projects.Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid()).Select(x => x.PomId.ToString()).ToList();
            //日期转换
            int nowDay = ConvertHelper.ToDateDay(DateTime.Now.AddDays(-1));
            //日报产值
            var dayRepsData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == nowDay && proIds.Contains(x.ProjectId)).GroupBy(x => x.ProjectId).Select(x => new { x.ProjectId, DayActualProductionAmount = SqlFunc.AggregateSum(x.DayActualProductionAmount) }).ToListAsync();
            //年度累计产值
            var yearAccumulateData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && proIds.Contains(x.ProjectId) && x.DateDay >= 20230726 && x.DateDay <= nowDay && Convert.ToDateTime(x.CreateTime).Year == DateTime.Now.AddDays(-1).Year).Select(x => new { x.ProjectId, x.DayActualProductionAmount }).ToListAsync();
            var institionsIds = institions.Where(x => x.Grule.Contains("101341960") || x.Grule.Contains("101174265") || x.Grule.Contains("101174254") || x.Grule.Contains("101332050") || x.Grule.Contains("101305005") || x.Grule.Contains("101288132") || x.Oid == "101162350").Select(x => x.PomId.Value).ToList();
            var yearProIds = projects.Where(x => institionsIds.Contains(x.CompanyId.Value) && proIds.Contains(x.Id) && x.TypeId != CommonData.PNonConstruType.ToGuid()).Select(x => x.Id).ToList();
            //已经过了多少天
            var dayss = 0;
            //当天
            var nowDateDay = DateTime.Now.AddDays(-1).Day;
            if (nowDateDay > 25)
            {
                dayss = nowDateDay - 25;
            }
            else
            {
                //获取上月自然月天数
                var lDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) - 25;//上月过了多少天
                //截至到今天过了多少天
                dayss = nowDateDay + lDays;
            }
            var yearAccumulateGH = yearAccumulateData.Where(x => yearProIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount) - 273928730.02M + 77245000M + 5933620000M + 999194900M / 30.5M * dayss;
            //年度计划产值
            var quarter = (DateTime.Now.AddDays(-1).Month + 2) / 3;//获取当前月份所属季度
            var inDate = new DateTime(DateTime.Now.AddDays(-1).Year, quarter == 1 ? 1 : quarter == 2 ? 3 : quarter == 3 ? 6 : 9, 1);
            decimal yearPlanGH = 0M;
            //title个数及占比
            int pTitleTotalNums = 0; int pTitleInBuildNums = 0; int pTitleStopNums = 0; decimal pTitleStopProp = 0M;
            int pNotFillTitleTotalNums = 0;
            var tableFirst = new List<TableProjectOverallProdNumsDto>();
            //title产值
            decimal leadOrLag = 0M; decimal titlePInBuildOutputVal = 0M;
            var tableSecond = new List<TableProjectOverallProdOpValDto>();
            //单位填报率情况
            var unitFillRepNums = new List<TableUnitFillRepDto>();
            for (int i = 0; i < 2; i++)
            {
                foreach (var item in oIds)
                {
                    int pNums = 0; //合同项目数
                    int pInBuildNums = 0;//在建数
                    int pStopNums = 0;//停工数
                    decimal dayOutputVal = 0M;//日产值（万元）
                    decimal yearProjectAccumulateOutputVal = 0M;//年累产值 万元
                    decimal yearPlanOutputVal = 0M;//广航年度计划产值
                    decimal unityearPlanOutputVal = 0M;//分公司年度计划产值
                    //公司数据
                    var institionsCompaynIds = institions.Where(x => x.Grule.Contains(item)).Select(x => x.PomId.Value).ToList();
                    //项目数据刷新
                    var projectsFlush = projects.Where(x => institionsCompaynIds.Contains(x.CompanyId.Value)).ToList();
                    if (i == 1)
                    {
                        projectsFlush = projectsFlush.Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid() && pConstruc.Contains(x.StatusId.ToString())).ToList();
                    }
                    //日报数据刷新
                    var projectIds = projectsFlush.Select(x => x.Id).ToList();
                    var dayRepsFlush = dayRepsData.Where(x => projectIds.Contains(x.ProjectId)).ToList();
                    var yearAccumulateDataFlush = yearAccumulateData.Where(x => projectIds.Contains(x.ProjectId)).ToList();
                    //应填报施工项目
                    //var fillData = projectsFlush.Where(x => pConstruc.Contains(x.StatusId.ToString())).Select(x => x.Id).ToList();
                    //年度计划产值刷新
                    var projectPomIds = new List<string>();//公司
                    if (item != "101162350")
                    {
                        projectPomIds = projectsFlush.Select(x => x.PomId.ToString()).ToList();
                    }
                    else if (item == "101162350")//直管项目
                    {
                        if (i == 1)
                        {
                            projectPomIds = projects.Where(x => x.CompanyId == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid()).Select(x => x.PomId.ToString()).ToList();
                        }
                        else
                        {
                            projectPomIds = projects.Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid() && pConstruc.Contains(x.StatusId.ToString()) && x.CompanyId == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid()).Select(x => x.PomId.ToString()).ToList();
                        }
                    }
                    else//整个广航
                    {
                        if (i == 1)
                        {
                            projectPomIds = projects.Where(x => x.TypeId != CommonData.PNonConstruType.ToGuid() && pConstruc.Contains(x.StatusId.ToString())).Select(x => x.PomId.ToString()).ToList();
                        }
                        else
                        {
                            projectPomIds = projects.Select(x => x.PomId.ToString()).ToList();
                        }
                    }
                    //产值初始化数值 需要累加0726日统计的初始数据  
                    var defaultOutputVal = 0M;
                    var defaultPlanOutputVal = 0M;
                    if (i == 1)
                    {
                        if (quarter == 1)//一季度获取数据1-3月计划产值
                        {

                        }
                        else if (quarter == 1)//二季度获取数据4-6月计划产值
                        {

                        }
                        else if (quarter == 3)//三季度获取数据7-9月计划产值
                        {
                            yearPlanOutputVal = 999194900M;

                        }
                        else //四季度获取数据10-12月计划产值
                        {

                        }
                        if (item == "101341960")//疏浚公司
                        {
                            unityearPlanOutputVal = 274441900M / 30.5M * dayss;//日均计划
                            defaultOutputVal = 981990000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;//计划
                        }
                        else if (item == "101174265")//华南交建公司
                        {
                            unityearPlanOutputVal = 245877300M / 30.5M * dayss;
                            defaultOutputVal = 1758410000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        else if (item == "101174254")//三公司
                        {
                            unityearPlanOutputVal = 25300000M / 30.5M * dayss;
                            defaultOutputVal = 174960000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        else if (item == "101305005")//五公司
                        {
                            unityearPlanOutputVal = 34450000M / 30.5M * dayss;
                            defaultOutputVal = 130960000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        else if (item == "101288132")//福建公司
                        {
                            unityearPlanOutputVal = 239134400M / 30.5M * dayss;
                            defaultOutputVal = 200010000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        else if (item == "101332050")//四公司
                        {
                            unityearPlanOutputVal = 20200500M / 30.5M * dayss;
                            defaultOutputVal = 7050000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        else if (item == "101162350")//直营项目
                        {
                            unityearPlanOutputVal = 361939500M / 30.5M * dayss;
                            defaultOutputVal = 2603000000M;
                            defaultPlanOutputVal = defaultOutputVal + unityearPlanOutputVal;
                        }
                        //年度计划
                        yearPlanGH = 5991230000M + yearPlanOutputVal / 30.5M * dayss;
                    }
                    if (item != "101162350")
                    {
                        if (i == 0)
                        {
                            //应填报施工项目
                            var fillData = projectsFlush.Where(x => pConstruc.Contains(x.StatusId.ToString())).ToList();
                            //未填报施工项目
                            var notFillData = dayRepsFlush.Count() == 0 ? fillData.Count() : fillData.Where(x => !dayRepsFlush.Select(y => y.ProjectId).Contains(x.Id)).Count();

                            pNums = projectsFlush.Count();
                            pInBuildNums = projectsFlush.Where(x => pConstruc.Contains(x.StatusId.ToString())).Count();
                            pStopNums = projectsFlush.Where(x => pSuspend.Contains(x.StatusId.ToString())).Count();
                            tableFirst.Add(new TableProjectOverallProdNumsDto
                            {
                                UnitName = institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "华南交建公司" ? "交建公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "北方分公司" ? "四公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname,
                                PInBuildNums = pInBuildNums,
                                PNums = pNums,
                                PStopNums = pStopNums,
                                UnitProp = pNums == 0 ? 0M : Math.Round(Convert.ToDecimal(pInBuildNums) / Convert.ToDecimal(pNums) * 100, 2)
                            });
                            //单位填报率情况
                            unitFillRepNums.Add(new TableUnitFillRepDto
                            {
                                UnitName = institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "华南交建公司" ? "交建公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "北方分公司" ? "四公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname,
                                PInBuildNums = pInBuildNums,
                                NotFillNums = notFillData,
                                FillProp = pInBuildNums == 0 ? 0M : Math.Round(Convert.ToDecimal(pInBuildNums - notFillData) / Convert.ToDecimal(pInBuildNums) * 100, 2)
                            });
                        }
                        else
                        {
                            dayOutputVal = dayRepsFlush.Where(x => proIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);
                            //累计产值
                            yearProjectAccumulateOutputVal = defaultOutputVal + dayOutputVal + yearAccumulateDataFlush.Sum(x => x.DayActualProductionAmount);
                            tableSecond.Add(new TableProjectOverallProdOpValDto
                            {
                                UnitName = institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "华南交建公司" ? "交建公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname == "北方分公司" ? "四公司" : institions.FirstOrDefault(x => x.Oid == item)?.Shortname,
                                DayOutputVal = Math.Round(dayOutputVal / 10000, 2),
                                YearProjectAccumulateOutputVal = Math.Round(yearProjectAccumulateOutputVal / 100000000, 2),
                                YearAccumulateOutputValProp = yearAccumulateGH == 0 ? 0M : Math.Round(yearProjectAccumulateOutputVal / yearAccumulateGH * 100, 2),
                                ComparedToAnnualPlanRate = defaultPlanOutputVal == 0 ? 0M : Math.Round((yearProjectAccumulateOutputVal - defaultPlanOutputVal) / defaultPlanOutputVal * 100, 2)
                            });
                        }
                    }
                    else// 直营项目
                    {
                        var zyProjectInfo = projects.Where(x => proIds.Contains(x.Id) && x.TypeId != CommonData.PNonConstruType.ToGuid() && x.CompanyId == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid()).ToList();
                        if (i == 0)
                        {
                            var zyProjecPconsTrucData = zyProjectInfo.Where(x => pConstruc.Contains(x.StatusId.ToString())).ToList();
                            pNums = zyProjectInfo.Count();
                            pInBuildNums = zyProjectInfo.Where(x => pConstruc.Contains(x.StatusId.ToString())).Count();
                            pStopNums = zyProjectInfo.Where(x => pSuspend.Contains(x.StatusId.ToString())).Count();
                            //未填报施工项目
                            var notFillData = zyProjecPconsTrucData.Count() == 0 ? pInBuildNums : zyProjecPconsTrucData.Where(x => !zyProjectInfo.Select(x => x.Id).Contains(x.Id)).Count();
                            tableFirst.Add(new TableProjectOverallProdNumsDto
                            {
                                UnitName = "直营项目",
                                PInBuildNums = pInBuildNums,
                                PNums = pNums,
                                PStopNums = pStopNums,
                                UnitProp = pNums == 0 ? 0M : Math.Round(Convert.ToDecimal(pInBuildNums) / Convert.ToDecimal(pNums) * 100, 2)
                            });
                            //单位填报率情况
                            unitFillRepNums.Add(new TableUnitFillRepDto
                            {
                                UnitName = "直营项目",
                                PInBuildNums = pInBuildNums,
                                NotFillNums = notFillData,
                                FillProp = pInBuildNums == 0 ? 0M : Math.Round(Convert.ToDecimal(zyProjecPconsTrucData.Count()) / Convert.ToDecimal(pInBuildNums) * 100, 2)
                            });
                        }
                        else
                        {
                            var zyProIds = zyProjectInfo.Select(x => x.Id).ToList();
                            dayOutputVal = dayRepsData.Where(x => zyProIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);
                            yearProjectAccumulateOutputVal = defaultOutputVal + dayOutputVal + yearAccumulateDataFlush.Where(x => zyProjectInfo.Select(y => y.Id).Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);
                            tableSecond.Add(new TableProjectOverallProdOpValDto
                            {
                                UnitName = "直营项目",
                                DayOutputVal = Math.Round(dayOutputVal / 10000, 2),
                                YearProjectAccumulateOutputVal = Math.Round(yearProjectAccumulateOutputVal / 100000000, 2),
                                YearAccumulateOutputValProp = yearAccumulateGH == 0M ? 0M : Math.Round(yearProjectAccumulateOutputVal / yearAccumulateGH * 100,
                                2),
                                ComparedToAnnualPlanRate = defaultPlanOutputVal == 0M ? 0M : Math.Round((yearProjectAccumulateOutputVal - defaultPlanOutputVal) / defaultPlanOutputVal * 100, 2)
                            });
                        }
                    }
                }
                if (i == 0)
                {
                    pTitleTotalNums = projects.Count();
                    pTitleInBuildNums = projects.Where(x => pConstruc.Contains(x.StatusId.ToString())).Count();
                    pTitleStopNums = projects.Where(x => pSuspend.Contains(x.StatusId.ToString())).Count();
                    var dayRepsFlush = dayRepsData.Where(x => proIds.Contains(x.ProjectId)).ToList();
                    pNotFillTitleTotalNums = dayRepsData.Count() == 0 ? pTitleInBuildNums : pTitleInBuildNums - dayRepsFlush.Count();
                    pTitleStopProp = projects.Count() == 0 ? 0M : Math.Round(Convert.ToDecimal(projects.Where(x => pConstruc.Contains(x.StatusId.ToString())).Count()) / Convert.ToDecimal(projects.Count()) * 100, 2);
                    tableFirst.Add(new TableProjectOverallProdNumsDto
                    {
                        UnitName = "广航局总体(个)",
                        PInBuildNums = pTitleInBuildNums,
                        PNums = pTitleTotalNums,
                        PStopNums = pTitleStopNums,
                        UnitProp = pTitleStopProp
                    });
                    //单位填报率情况
                    unitFillRepNums.Add(new TableUnitFillRepDto
                    {
                        UnitName = "广航局总体(个)",
                        PInBuildNums = pTitleInBuildNums,
                        NotFillNums = pNotFillTitleTotalNums,
                        FillProp = pTitleInBuildNums == 0 ? 0M : Math.Round(Convert.ToDecimal(dayRepsFlush.Count()) / Convert.ToDecimal(pTitleInBuildNums) * 100, 2)
                    });
                }
                else
                {
                    leadOrLag = yearPlanGH == 0 ? 0M : Math.Round((yearAccumulateGH - yearPlanGH) / yearPlanGH * 100, 2);
                    titlePInBuildOutputVal = dayRepsData.Where(x => proIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);
                    tableSecond.Add(new TableProjectOverallProdOpValDto
                    {
                        UnitName = "广航局总体",
                        DayOutputVal = Math.Round(titlePInBuildOutputVal / 10000, 2),
                        YearProjectAccumulateOutputVal = Math.Round(yearAccumulateGH / 100000000, 2),
                        YearAccumulateOutputValProp = 100,
                        ComparedToAnnualPlanRate = leadOrLag
                    });
                }
            }

            var ghj = unitFillRepNums.FirstOrDefault(x => x.UnitName.Contains("广航局"));
            unitFillRepNums = unitFillRepNums.Where(x => !x.UnitName.Contains("广航局")).OrderBy(x => x.FillProp).ToList();
            unitFillRepNums.Add(ghj);
            #endregion

            #region 船舶相关
            //title个数
            int shipTitleTotalNums = 0; int workTitleNums = 0; int overHaulTitleNums = 0; int dispatchTitleNums = 0; int standbyTitleNums = 0;
            var tableThird = new List<TableOwnShipWorkNumsDto>();
            //title产值
            decimal titleDayWorkOutputVal = 0M; decimal titleWorkHours = 0; decimal titleYearAccumulateOutputVal = 0M; decimal titleYearWorkHours = 0M;
            var tableFourth = new List<TableOwnShipWorkOpValDto>();
            var tableShipNotFill = new List<TableShipNotFillRepDto>();
            //船舶类型
            var shipTypes = CommonData.ShipType.Split(',').ToList();
            var shipTypeDetails = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1 && shipTypes.Contains(x.PomId.ToString())).Select(x => new { x.PomId, x.Name }).ToListAsync();
            //船舶日报
            var shipOpValDayRepData = await dbContext.Queryable<ShipDayReport>().Where(x => x.IsDelete == 1 && x.DateDay == nowDay).Select(x => new { x.ShipState, x.EstimatedOutputAmount, x.ShipId, WorkHours = SqlFunc.IsNull(x.Dredge, 0) + SqlFunc.IsNull(x.Sail, 0) + SqlFunc.IsNull(x.BlowingWater, 0) + SqlFunc.IsNull(x.BlowShore, 0) + SqlFunc.IsNull(x.SedimentDisposal, 0) }).ToListAsync();
            //自有船舶信息
            var ownShipIds = shipOpValDayRepData.Select(x => x.ShipId).ToList();
            var ownShipDetails = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1).Select(x => new { x.PomId, x.TypeId, x.Name }).ToListAsync();
            //筛选后的船舶
            var havingShips = ownShipDetails.Where(x => shipTypes.Contains(x.TypeId.ToString())).Select(x => new { x.PomId, x.TypeId, x.Name }).ToList();
            var havingShipIds = havingShips.Select(x => x.PomId).ToList();
            //年累船舶日报
            var yearAccumulateShipData = await dbContext.Queryable<ShipDayReport>().Where(x => x.IsDelete == 1 && x.DateDay <= nowDay && x.DateDay >= 20230723 && havingShipIds.Contains(x.ShipId)).Select(x => new { x.ShipState, x.EstimatedOutputAmount, x.ShipId, WorkHours = SqlFunc.IsNull(x.Dredge, 0) + SqlFunc.IsNull(x.Sail, 0) + SqlFunc.IsNull(x.BlowingWater, 0) + SqlFunc.IsNull(x.BlowShore, 0) + SqlFunc.IsNull(x.SedimentDisposal, 0) }).ToListAsync();
            titleYearWorkHours = Convert.ToDecimal(yearAccumulateShipData.Sum(x => x.WorkHours)) + 34209M + 3500M + 1800M + 700M;
            titleWorkHours = Convert.ToDecimal(shipOpValDayRepData.Where(x => havingShipIds.Contains(x.ShipId)).Sum(x => x.WorkHours));
            for (int j = 0; j < 2; j++)
            {
                foreach (var item in shipTypes)
                {
                    //船舶数量刷新
                    var ownShipDetailsFlush = ownShipDetails.Where(x => item == x.TypeId.ToString()).ToList();
                    //刷新后的船舶id
                    var shipIds = ownShipDetailsFlush.Select(x => x.PomId).ToList();
                    //船舶日报刷新
                    var shipOpValDayRepFlush = shipOpValDayRepData.Where(x => shipIds.Contains(x.ShipId)).ToList();
                    //筛选后的船舶刷新
                    var havingShipsFlush = havingShips.Where(x => x.TypeId.ToString() == item).ToList();
                    //年累船舶日报刷新
                    var yearAccumulateShipFlush = yearAccumulateShipData.Where(x => shipIds.Contains(x.ShipId)).ToList();
                    //默认时间初始化
                    var defaultHours = 0M;
                    int shipTotalNums = 0; int workNums = 0; int overHaulNums = 0; int dispatchNums = 0; int standbyNums = 0;
                    shipTotalNums = havingShipsFlush.Count();
                    workNums = shipOpValDayRepFlush.Where(x => x.ShipState == ProjectShipState.Construction).Count();
                    overHaulNums = shipOpValDayRepFlush.Where(x => x.ShipState == ProjectShipState.Repair).Count();
                    dispatchNums = shipOpValDayRepFlush.Where(x => x.ShipState == ProjectShipState.Dispatch).Count();
                    standbyNums = shipOpValDayRepFlush.Where(x => x.ShipState == ProjectShipState.Standby).Count();
                    if (j == 0)
                    {
                        tableThird.Add(new TableOwnShipWorkNumsDto
                        {
                            ShipTypeName = shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "耙吸挖泥船" ? "耙吸船" : shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "抓斗挖泥船" ? "抓斗船" : shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "绞吸挖泥船" ? "绞吸船" : "",
                            ShipTotalNums = shipTotalNums,
                            DispatchNums = dispatchNums,
                            OverHaulNums = overHaulNums,
                            StandbyNums = standbyNums,
                            WorkNums = workNums,
                            WorkRate = shipTotalNums == 0 ? 0M : Math.Round(Convert.ToDecimal(workNums) / Convert.ToDecimal(shipTotalNums) * 100, 2)
                        });
                        shipTitleTotalNums += shipTotalNums; workTitleNums += workNums; overHaulTitleNums += overHaulNums; dispatchTitleNums += dispatchNums; standbyTitleNums += standbyNums;
                    }
                    else
                    {
                        //产值初始化数值 需要累加
                        var defaultOutputVal = 0M;
                        if (item == "06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf")//耙吸
                        {
                            defaultOutputVal = 283000000M;
                            defaultHours = 22498M + 3500M;
                        }
                        else if (item == "f1718922-c213-4409-a59f-fdaf3d6c5e23")//绞吸
                        {
                            defaultOutputVal = 699000000M;
                            defaultHours = 8821M + 1800M;
                        }
                        else if (item == "6959792d-27a4-4f2b-8fa4-a44222f08cb2")//抓斗
                        {
                            defaultOutputVal = 50000000M;
                            defaultHours = 2891M + 700M;
                        }
                        decimal dayWorkOutputVal = 0M; decimal workHours = 0; decimal yearAccumulateOutputVal = 0M; decimal yearWorkHours = 0M;
                        dayWorkOutputVal = Convert.ToDecimal(shipOpValDayRepFlush.Sum(x => x.EstimatedOutputAmount));
                        workHours = Convert.ToDecimal(shipOpValDayRepFlush.Sum(x => x.WorkHours));
                        yearAccumulateOutputVal = dayWorkOutputVal + defaultOutputVal + Convert.ToDecimal(yearAccumulateShipFlush.Sum(x => x.EstimatedOutputAmount));
                        yearWorkHours = Convert.ToDecimal(yearAccumulateShipFlush.Sum(x => x.WorkHours));
                        tableFourth.Add(new TableOwnShipWorkOpValDto
                        {
                            ShipTypeName = shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "耙吸挖泥船" ? "耙吸船" : shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "抓斗挖泥船" ? "抓斗船" : shipTypeDetails.FirstOrDefault(x => x.PomId.ToString() == item)?.Name == "绞吸挖泥船" ? "绞吸船" : "",
                            DayWorkOutputVal = Math.Round(dayWorkOutputVal / 10000, 2),
                            WorkHours = workHours,
                            YearAccumulateOutputVal = Math.Round(yearAccumulateOutputVal / 100000000, 2),
                            YearWorkHours = yearWorkHours + defaultHours,
                            HoursRate = workNums == 0 ? 0M : Math.Round(workHours / (workNums * 24M) * 100, 2)
                        });
                        titleDayWorkOutputVal += dayWorkOutputVal;
                        titleYearAccumulateOutputVal += yearAccumulateOutputVal;
                    }
                }
                if (j == 0)//合计
                {
                    tableThird.Add(new TableOwnShipWorkNumsDto
                    {
                        ShipTypeName = "合计",
                        ShipTotalNums = shipTitleTotalNums,
                        DispatchNums = dispatchTitleNums,
                        OverHaulNums = overHaulTitleNums,
                        StandbyNums = standbyTitleNums,
                        WorkNums = workTitleNums,
                        WorkRate = shipTitleTotalNums == 0 ? 0M : Math.Round(Convert.ToDecimal(workTitleNums) / Convert.ToDecimal(shipTitleTotalNums) * 100, 2)
                    });
                }
                else
                {
                    tableFourth.Add(new TableOwnShipWorkOpValDto
                    {
                        ShipTypeName = "合计",
                        DayWorkOutputVal = Math.Round(titleDayWorkOutputVal / 10000, 2),
                        WorkHours = titleWorkHours,
                        YearAccumulateOutputVal = Math.Round(titleYearAccumulateOutputVal / 100000000, 2),
                        YearWorkHours = titleYearWorkHours,
                        HoursRate = workTitleNums == 0 ? 0M : Math.Round(titleWorkHours / (workTitleNums * 24M) * 100, 2)
                    });
                }
            }
            #endregion

            #region 关联项目的船舶未报数据
            //获取需要填报的进场船舶
            var enterShipData = await dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1 && x.Status == ShipMovementStatus.Enter && x.ShipType == ShipType.OwnerShip && x.ProjectId != Guid.Empty && havingShipIds.Contains(x.ShipId)).Select(x => new { x.ProjectId, x.ShipId }).ToListAsync();
            foreach (var item in enterShipData)
            {
                if (!ownShipIds.Contains(item.ShipId))
                {
                    tableShipNotFill.Add(new TableShipNotFillRepDto
                    {
                        ShipName = ownShipDetails.FirstOrDefault(x => x.PomId == item.ShipId)?.Name,
                        ProjectName = projects.FirstOrDefault(x => x.Id == item.ProjectId)?.Name
                    });
                }
            }
            tableShipNotFill = tableShipNotFill.Where(x => !string.IsNullOrWhiteSpace(x.ProjectName)).OrderBy(x => x.ProjectName.Count()).ToList();
            #endregion

            #region 特殊情况
            //获取当日特殊情况通报 嘉奖或预警
            var tableFifth = new List<TableSpecialDto>();
            var dayRepNoticeData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == nowDay && (x.IsHaveProductionWarning == 1 || x.IsHaveProductionWarning == 2)).Select(x => new { x.IsHaveProductionWarning, x.ProductionWarningContent }).OrderByDescending(x => x.IsHaveProductionWarning).ToListAsync();
            dayRepNoticeData.ForEach(x => tableFifth.Add(new TableSpecialDto
            {
                WorkType = x.IsHaveProductionWarning == 1 ? "异常预警" : x.IsHaveProductionWarning == 2 ? "嘉奖通报" : "",
                Describe = x.ProductionWarningContent
            }));
            #endregion

            #region 未报项目
            var tableSixth = new List<TableNotFillDto>();
            //日期转换
            ConvertHelper.TryConvertDateTimeFromDateDay(nowDay, out DateTime nowTime);
            //非长期停工  非施工
            var pInStatusIdsRemove = pInStatusIds.Where(x => x != "169c8afe-be81-4cbc-bfe7-61a40da0597b").ToList();
            //获取在建项目ids
            var inBuildProData = projects.Where(x => pInStatusIdsRemove.Contains(x.StatusId.ToString())).Select(x => new { x.Id, Date = Convert.ToDateTime(x.CreateTime).Date, StatusId = x.StatusId.ToString() }).ToList();
            var inBuildProIds = inBuildProData.Where(x => x.StatusId == CommonData.PConstruc).Select(x => x.Id).ToList();
            //计算开始时间写死 从上线日期开始  目前先写0715
            var startTime = Convert.ToDateTime("2023-07-27");
            var notFillProjects = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay > startTime.Date.ToDateDay() && x.DateDay <= nowDay && inBuildProIds.Contains(x.ProjectId)).ToListAsync();
            inBuildProData.ForEach(x =>
            {
                var newNotFillProjects = new List<DayReport>();
                //如果已填日报中不存在必填日报项目  且日期大于初始日期
                var pros = notFillProjects.Where(y => y.ProjectId == x.Id && x.Date.ToDateDay() > startTime.ToDateDay()).Select(y => y.ProjectId).ToList();
                if (pros.Count() == 0)
                {
                    newNotFillProjects = notFillProjects.Where(y => y.DateDay > x.Date.ToDateDay()).ToList();
                    startTime = (string.IsNullOrWhiteSpace(x.Date.ToString()) || x.Date == DateTime.MinValue) ? Convert.ToDateTime("2023-07-27") : x.Date;//日期为创建日期
                    startTime = x.Date < Convert.ToDateTime("2023-07-27") ? Convert.ToDateTime("2023-07-27") : x.Date;
                }
                else
                {
                    newNotFillProjects = notFillProjects.ToList();
                    startTime = Convert.ToDateTime("2023-07-27");
                }
                //获取天数
                int days = TimeHelper.GetTimeSpan(startTime, nowTime).Days;
                var fillCount = newNotFillProjects.Where(y => y.ProjectId == x.Id).Count();
                if ((days - fillCount) != 0 && fillCount <= days)
                {
                    tableSixth.Add(new TableNotFillDto
                    {
                        NotFillCount = days - fillCount,
                        ProjectName = projects.FirstOrDefault(y => y.Id == x.Id)?.Name
                    });
                }
            });
            tableSixth = tableSixth.OrderByDescending(x => x.NotFillCount).ToList();
            #endregion

            #region 数据质量
            //项目当日产值
            var dayProjectOpVal = Math.Round(titlePInBuildOutputVal / 10000, 2);
            //船舶当日产值
            var dayShipOpVal = Math.Round(titleDayWorkOutputVal / 10000, 2);
            //项目填报率 已填/在建
            var projectProp = pTitleInBuildNums == 0 ? 0M : Math.Round(Convert.ToDecimal(dayRepsData.Count()) / pTitleInBuildNums, 2);
            //船舶填报率 待命+调遣+修理+施工
            var shipProp = shipTitleTotalNums == 0 ? 0M : Math.Round(Convert.ToDecimal(dispatchTitleNums + overHaulTitleNums + standbyTitleNums + workTitleNums) / Convert.ToDecimal(shipTitleTotalNums), 2);
            var scoreVal = (dayProjectOpVal / 3300M * 0.5M + dayShipOpVal / 490M * 0.25M + projectProp * 0.2M + shipProp * 0.05M) * 100M;
            int score = 0;
            if (scoreVal >= 0 && scoreVal < 30) { score = 1; }
            if (scoreVal >= 30 && scoreVal < 60) { score = 2; }
            if (scoreVal >= 60 && scoreVal < 80) { score = 3; }
            if (scoreVal >= 80 && scoreVal < 90) { score = 4; }
            if (scoreVal >= 90) { score = 5; }
            #endregion

            unitFillRepNums.Clear();
            unitFillRepNums = await dbContext.Queryable<QualityTemporary>().Select(x => new TableUnitFillRepDto
            {
                UnitName = x.Name,
                PInBuildNums = x.ItemQuantity,
                NotFillNums = x.NotFillNums,
                FillProp = x.FillProp * 100
            }).ToListAsync();
            jjtSendMsgDetailsResponse = new JjtSendMsgDetailsResponse
            {
                PTotalNums = pTitleTotalNums,
                PInBuildNums = pTitleInBuildNums,
                PStopNums = pTitleStopNums,
                PStopProp = pTitleStopProp,
                PInBuildOutputVal = Math.Round(titlePInBuildOutputVal / 10000, 2),
                YearProjectAccumulateOutputVal = Math.Round(yearAccumulateGH / 100000000, 2),
                LeadOrLag = leadOrLag,
                WorkShipTotalNums = shipTitleTotalNums,
                WorkShipNums = workTitleNums,
                OverHaulNums = overHaulTitleNums,
                DispatchNums = dispatchTitleNums,
                StandbyNums = standbyTitleNums,
                WorkRate = shipTitleTotalNums == 0 ? 0M : Math.Round(Convert.ToDecimal(workTitleNums) / Convert.ToDecimal(shipTitleTotalNums) * 100, 2),
                DayWorkOutputVal = Math.Round(titleDayWorkOutputVal / 10000, 2),
                WorkHours = titleWorkHours,
                YearShipAccumulateOutputVal = Math.Round(titleYearAccumulateOutputVal / 100000000, 2),
                YearAccumulateWorkHours = titleYearWorkHours,
                SpecialNums = tableFifth.Count(),
                TableProjectOverallProdNumsDto = tableFirst,
                TableProjectOverallProdOpValDto = tableSecond,
                TableOwnShipWorkNumsDto = tableThird,
                TableOwnShipWorkOpValDto = tableFourth,
                TableSpecialDto = tableFifth,
                TableNotFillDto = tableSixth,
                TableUnitFillRepDto = unitFillRepNums,
                TableShipNotFillRepDto = tableShipNotFill,
                Score = score
            };
            responseDto.Data = jjtSendMsgDetailsResponse;
            responseDto.Success();
            return responseDto;

        }
        /// <summary>
        /// 发送图片
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UploadJjtSendTextDetailsImage(IFormFile file)
        {
            WebHelper web = new WebHelper();
            var responseDto = new ResponseAjaxResult<bool>();
            List<string> userCodes = new List<string>() { "2016146340" };
            //获取token
            var corpid = AppsettingsHelper.GetValue("JjtPushMesssage:Corpid");
            var token = AppsettingsHelper.GetValue("JjtPushMesssage:GetJjtTokenUrl").Replace("@corpid", corpid);
            var responseToken = await web.DoGetAsync(token);
            //获取media_Id对象

            var bytes = new byte[4096];
            string responseJsonDto = "https://jjt.ccccltd.cn/cgi-bin/media/upload?access_token=" + responseToken.Result + "&type=file";
            file.OpenReadStream().Read(bytes, 0, bytes.Length);
            HttpContent fileStreamContent = new StreamContent(file.OpenReadStream());

            using (var client = new HttpClient())
            {
                StreamContent content = new StreamContent(file.OpenReadStream());
                content.Headers.Add("Content-Type", "application/octet-stream");
                var response = await client.PostAsync(responseJsonDto, content);
            }


            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent, "media", file.FileName);

                //try
                //{
                //    client.DefaultRequestHeaders.Add("ContentType", "multipart/form-data");

                //    StreamContent content = new StreamContent(file.OpenReadStream());
                //    client.DefaultRequestHeaders["ContentType"] = "multipart/form-data";
                //    content.Headers["ContentType"]= "multipart/form-data"
                //    var response = await  client.PostAsync(responseJsonDto, content);

                //}
                //catch (Exception Error)
                //{
                //}
                //finally
                //{
                //    client.CancelPendingRequests();
                //}
            }

            var jjtSendMsgMediaDto = JsonConvert.DeserializeObject<JjtSendMsgMediaDto>(responseJsonDto);
            if (jjtSendMsgMediaDto.Errmsg != "ok")
            {
                responseDto.Data = false;
                responseDto.FailResult(HttpStatusCode.UploadFail, "上传文件失败");
                return responseDto;
            }
            //获取图片
            string getImage = "https://jjt.ccccltd.cn/cgi-bin/media/get?access_token=" + token + "&media_id=" + jjtSendMsgMediaDto.Media_id;
            //发送消息
            var sendUrl = "https://jjt.ccccltd.cn/cgi-bin/message/send?access_token=" + token;
            //var msgResponse = new SingleMessageTemplateRequestDto
            //{
            //    IsAll = false,
            //    UserIds = userCodes,
            //    MessageType = JjtMessageType.TEXTCARD,
            //    TextCardContent = new TextCardMessageContent
            //    {
            //        Description = "<div  class=\"highlight\">一、项目情况</div><div>" + newMsg[0] + "</div><div  class=\"highlight\">二、船舶情况</div><div>" + newMsg[1] + "</div>",
            //        Title = "【" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广航局生产运营监控日报】",
            //        Url = "https://www.baidu.com/"
            //    }
            //};
            responseDto.Success();
            return responseDto;
        }

        /// <summary>
        /// jjt发送消息  项目异常预警数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForGHDayRep(List<string> userCodes)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var warnDesc = string.Empty;
            int nowDay = DateTime.Now.AddDays(-1).ToDateDay();
            //获取项目异常预警数据
            var projectWarns = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((dr, p) => dr.ProjectId == p.Id)
                .Where((dr, p) => dr.IsHaveProductionWarning == 1 && dr.DateDay == nowDay)
                .Select((dr, p) => new { dr.ProjectId, p.Name, dr.ProductionWarningContent })
                .ToListAsync();
            if (projectWarns.Count() > 0)
            {
                warnDesc = "【" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广航局生产运营异常预警】  ";
                projectWarns.ForEach(x =>
                {
                    warnDesc += x.Name + x.ProductionWarningContent + ";";
                });
                warnDesc = warnDesc.Remove(warnDesc.Length - 1);
                var msgResponse = new SingleMessageTemplateRequestDto
                {
                    IsAll = false,
                    UserIds = userCodes,
                    MessageType = JjtMessageType.TEXT,
                    TextContent = warnDesc
                };
                var result = JjtUtils.SinglePushMessage(msgResponse);
                responseAjaxResult.Data = result;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取交建通提醒列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JjtSendMsgResponseDto>>> GetJjtSendMsgResponseSearchAsync(JjtSendMsgRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<JjtSendMsgResponseDto>>();
            var data = new List<JjtSendMsgResponseDto>();
            //RefAsync<int> total = 0;
            //获取所有已分配人员数据列表
            var assignedUsers = await dbContext.Queryable<JjtMessageUser>().Where(x => x.IsDelete == 1).Select(x => new { x.NowDay, x.UserGroupCode, x.UserName, x.UserType }).ToListAsync();
            //获取交建通所有数据
            var responseResult = await dbContext.Queryable<JjtWriteDataRecord>().ToListAsync();
            responseResult.ForEach(x => data.Add(new JjtSendMsgResponseDto
            {
                Id = x.Id,
                FTime = Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd 09:00"),
                STime = Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd HH:mm") == Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd 01:00") ? Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd 10:00") : Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd HH:mm"),
                ExpectUserName = assignedUsers.Where(y => y.NowDay == x.NowDay && y.UserType == 2).Select(y => y.UserName).ToList(),
                MsgContent = (string.IsNullOrWhiteSpace(x.ModifyTextMsg) && string.IsNullOrWhiteSpace(x.FTextMsg)) ? null : string.IsNullOrWhiteSpace(x.ModifyTextMsg) ? x.FTextMsg.Replace('*', ' ') : (!string.IsNullOrWhiteSpace(x.ModifyTextMsg) && !string.IsNullOrWhiteSpace(x.FTextMsg)) ? x.ModifyTextMsg.Replace('*', ' ') : x.FTextMsg.Replace('*', ' '),
                TextTypeContent = x.TextType == 1 ? "生产运营监控日报" : "",
                UserName = assignedUsers.Where(y => y.NowDay == x.NowDay && y.UserType == 1).Select(y => y.UserName).ToList(),
                NowDay = x.NowDay,
                SendCount = x.SendCount
            }));
            if (requestDto.Date != DateTime.MinValue && requestDto.Date != null)
            {
                data = data.Where(x => x.NowDay == Convert.ToDateTime(requestDto.Date).ToDateDay()).ToList();
            }
            if (!string.IsNullOrWhiteSpace(requestDto.KeyWord)) data = data.Where(x => x.MsgContent.Contains(requestDto.KeyWord)).ToList();
            int skipCount = (requestDto.PageIndex - 1) * requestDto.PageSize;
            var result = data.Skip(skipCount).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Data = result;
            responseAjaxResult.Count = data.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 编辑交建通发送内容
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ModifyJjtSendMsgContentAsync(JjtSendMsgDataRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var resultResponse = await dbContext.Queryable<JjtWriteDataRecord>().Where(x => x.Id == requestDto.Id).FirstAsync();
            if (resultResponse.NowDay != DateTime.Now.ToDateDay())
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.FailResult(HttpStatusCode.UpdateFail, "此数据不属今天，编辑失败");
            }
            else
            {
                //获取推送内容 “自有船施工船舶”
                var replaceMsg = requestDto.MsgContent.Replace("自有船施工船舶", "*自有船施工船舶");
                resultResponse.ModifyTextMsg = replaceMsg;
                resultResponse.CreateTime = DateTime.Now;
                await dbContext.Updateable(resultResponse).WhereColumns(x => new { x.Id }).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success("编辑成功", HttpStatusCode.Success);
                JjtSendTextCardMsgForGHDayRep(true);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取交建通分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtSendMsgUsersAsync(JjtSendMsgUsersRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>();
            var resultData = new List<JjtSendMsgUsersResponseDto>();
            //获取机构数据
            var orginzations = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).Select(x => new { x.PomId, x.Name, x.Grule }).ToListAsync();
            //获取传入公司id
            if (!string.IsNullOrEmpty(requestDto.CompanyId.ToString()))
            {
                var grule = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.PomId == requestDto.CompanyId).Select(x => new { x.Grule }).FirstAsync();
                var reverseGrule = grule.Grule.Split('-', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray()[1];
                orginzations = orginzations.Where(x => x.Grule.Contains(reverseGrule)).ToList();
            }
            //机构
            var orginzationIds = orginzations.Select(x => x.PomId.ToString()).ToList();
            var userInfoSearch = await dbContext.Queryable<JjtMessageUser>()
                                .LeftJoin<Model.User>((jjtuser, user) => jjtuser.UserGroupCode == user.GroupCode)
                                .Where((jjtuser, user) => jjtuser.IsDelete == 1 && jjtuser.NowDay == DateTime.Now.ToDateDay())
                                .Select((jjtuser, user) => new JjtSendMsgUsersResponseDto
                                {
                                    Id = jjtuser.Id,
                                    CompanyName = user.CompanyId.ToString(),
                                    DepartmentName = user.DepartmentId.ToString(),
                                    Phone = user.Phone,
                                    UserGroupCode = user.GroupCode,
                                    UserName = user.Name,
                                    UserType = jjtuser.UserType,
                                }).ToListAsync();
            userInfoSearch = userInfoSearch.Where(x => orginzationIds.Contains(x.CompanyName) || orginzationIds.Contains(x.DepartmentName)).ToList();
            userInfoSearch.ForEach(x => resultData.Add(new JjtSendMsgUsersResponseDto
            {
                Id = x.Id,
                CompanyName = orginzations.FirstOrDefault(y => x.CompanyName == y.PomId.ToString())?.Name,
                DepartmentName = orginzations.FirstOrDefault(y => x.DepartmentName == y.PomId.ToString())?.Name,
                Phone = x.Phone,
                UserGroupCode = x.UserGroupCode,
                UserName = x.UserName,
                UserType = x.UserType,
            }));
            resultData = resultData.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            if (!string.IsNullOrWhiteSpace(requestDto.KeyWords)) resultData = resultData.Where(x => x.UserName.Contains(requestDto.KeyWords)).ToList();

            int skipCount = (requestDto.PageIndex - 1) * requestDto.PageSize;
            var data = resultData.Skip(skipCount).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Data = data;
            responseAjaxResult.Count = resultData.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取交建通发消息未分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtNotSendMsgUsersAsync(JjtSendMsgUsersRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>();
            var resultData = new List<JjtSendMsgUsersResponseDto>();
            //获取机构数据
            var orginzations = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).Select(x => new { x.PomId, x.Name, x.Grule }).ToListAsync();
            //获取传入公司id
            if (!string.IsNullOrEmpty(requestDto.CompanyId.ToString()))
            {
                var grule = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.PomId == requestDto.CompanyId).Select(x => new { x.Grule }).FirstAsync();
                var reverseGrule = grule.Grule.Split('-', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray()[1];
                orginzations = orginzations.Where(x => x.Grule.Contains(reverseGrule)).ToList();
            }
            //获取已分配的用户 最新数据
            var assignedUserCodes = await dbContext.Queryable<JjtMessageUser>().Where(x => x.IsDelete == 1 && x.NowDay == DateTime.Now.ToDateDay() && (x.UserType == 1 || x.UserType == 2)).Select(x => x.UserGroupCode).ToListAsync();
            //获取未分配的用户
            //机构
            var orginzationIds = orginzations.Select(x => x.PomId).ToList();
            var undistributedUsers = await dbContext.Queryable<Model.User>().Where(x => x.IsDelete == 1 && !assignedUserCodes.Contains(x.GroupCode) && (orginzationIds.Contains(x.CompanyId) || orginzationIds.Contains(x.DepartmentId))).Select(x => new { x.PomId, x.Name, x.GroupCode, x.CompanyId, x.Phone, x.DepartmentId }).ToListAsync();
            undistributedUsers.ForEach(x => resultData.Add(new JjtSendMsgUsersResponseDto
            {
                Id = x.PomId,
                CompanyName = orginzations.FirstOrDefault(y => x.CompanyId == y.PomId)?.Name,
                DepartmentName = orginzations.FirstOrDefault(y => x.DepartmentId == y.PomId)?.Name,
                Phone = x.Phone,
                UserGroupCode = x.GroupCode,
                UserName = x.Name,
                UserType = 0,
            }));
            resultData = resultData.Where(x => !string.IsNullOrEmpty(x.CompanyName)).ToList();
            if (!string.IsNullOrWhiteSpace(requestDto.KeyWords)) resultData = resultData.Where(x => x.UserName.Contains(requestDto.KeyWords)).ToList();
            int skipCount = (requestDto.PageIndex - 1) * requestDto.PageSize;
            var data = resultData.Skip(skipCount).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Data = data;
            responseAjaxResult.Count = resultData.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 增改交建通发消息用户
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsOrModifyJjtSendMsgUsersAsync(JjtSendMsgModifyUsersRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (requestDto.UserInfos.Count() > 0)
            {
                //获取最新数据
                var jjtSengMsgUsers = await dbContext.Queryable<JjtMessageUser>().Where(x => x.NowDay == DateTime.Now.ToDateDay()).ToListAsync();
                var data = new List<JjtMessageUser>();
                //数据新增
                if (requestDto.InsOrModify)
                {
                    requestDto.UserInfos.ForEach(x => data.Add(new JjtMessageUser
                    {
                        Id = GuidUtil.Next(),
                        NowDay = DateTime.Now.ToDateDay(),
                        UserGroupCode = x.UserCode,
                        UserName = x.UserName,
                        UserType = x.UserType
                    }));
                    await dbContext.Insertable(data).ExecuteCommandAsync();
                }
                else
                {
                    //数据修改  如果存在删除的数据
                    var delCodes = requestDto.UserInfos.Where(x => x.UserType == 0).Select(x => x.UserCode).ToList();
                    if (delCodes.Count() > 0)
                    {
                        //获取最新数据 
                        var deldata = jjtSengMsgUsers.Where(x => delCodes.Contains(x.UserGroupCode)).ToList();
                        await dbContext.Deleteable(deldata).ExecuteCommandAsync();
                    }
                    //修改数据
                    var modifyData = requestDto.UserInfos.Where(x => x.UserType != 0).ToList();
                    if (modifyData.Count() > 0)
                    {
                        modifyData.ForEach(x => data.Add(new JjtMessageUser
                        {
                            Id = x.Id.Value,
                            NowDay = DateTime.Now.ToDateDay(),
                            UserGroupCode = x.UserCode,
                            UserName = x.UserName,
                            UserType = x.UserType
                        }));
                        await dbContext.Updateable(data).ExecuteCommandAsync();
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail("无数据，操作失败");
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 交建通发送项目日报填报通知消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectDayRep()
        {
            return await JjtSendTextMsgForProject(1);
        }
        /// <summary>
        /// 交建通发送项目月报填报通知消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectMonthRep()
        {
            return await JjtSendTextMsgForProject(2);
        }
        /// <summary>
        /// 交建通发送项目安监日报通知消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectSafeDayRep()
        {
            return await JjtSendTextMsgForProject(3);
        }
        /// <summary>
        /// 交建通发送项目船舶日报通知消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectShipDayRep()
        {
            return await JjtSendTextMsgForProject(4);
        }
        /// <summary>
        /// 交建通发送项目填报通知消息
        /// </summary>
        /// <param name="type">1 项目日报 2 项目月报 3安监日报</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProject(int type)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            TimeSpan rangeStart = new TimeSpan(9, 0, 0);
            TimeSpan rangeEnd = new TimeSpan(9, 20, 0);
            TimeSpan rangeEnd1 = new TimeSpan(9, 40, 0);
            var companyCodes = new List<string>()
            {
              "101162350",//广航局
              "101341960",//疏浚公司
              "101174265",//华南交建公司
              "101174254",//三公司
              "101332050",//四公司（北方分公司）
              "101305005",//五公司
              "101288132",//福建公司（中交广航（福建）交通建设有限公司（福建分公司））
              "101162575",//设研院公司
            };
            //定义默认设置的固定公司管理员
            var adminUsers = new List<string>()
            {
                //广航局陈翠
                //"2016146340",
                //疏浚公司朱智文
                //"2017028792",
                ////交建公司陈河元
                //"2018008722",
                ////三公司王琦
                //"2018009624",
                ////五公司孟浩
                //"2017005354",
                ////福建公司李倩
                //"2020012309",
                ////四公司杨加录
                //"2016042370",
                ////研究院赵锐
                //"L20080287"
            };
            //项目状态
            var projectData = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && CommonData.ConstructionStatus.Contains(p.StatusId) && p.TypeId != CommonData.PNonConstruType.ToGuid()).Select(project => new JjtSendMsgDayRepResponse
            {
                Id = project.Id,
                Name = project.Name,
                ReportForMertel = project.ReportForMertel,
                CompanyId = project.CompanyId
            }).ToListAsync();
            //项目ids
            var proIds = projectData.Select(x => x.Id).ToList();
            //关联项目的船舶ids
            var shipmovementsProjectIds = new List<Guid>();
            if (type == 4)
            {
                shipmovementsProjectIds = await dbContext.Queryable<ShipMovement>().Where(sm => sm.IsDelete == 1 && sm.Status == ShipMovementStatus.Enter && sm.ShipType == ShipType.OwnerShip && proIds.Contains(sm.ProjectId)).Select(ship => ship.ProjectId).Distinct().ToListAsync();
            }
            //关联项目的船舶
            var shipmovements = await dbContext.Queryable<ShipMovement>()
                .LeftJoin<Project>((sm, p) => sm.ProjectId == p.Id)
                .Where((sm, p) => sm.IsDelete == 1 && sm.Status == ShipMovementStatus.Enter && sm.ShipType == ShipType.OwnerShip && proIds.Contains(sm.ProjectId))
                .Select((sm, p) => new JjtSendMsgDayRepResponse { Id = sm.ProjectId, CompanyId = p.CompanyId })
                .ToListAsync();
            shipmovements = shipmovements.GroupBy(x => x).Select(x => x.First()).ToList();
            //机构数据
            var institutionData = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
            //当天
            int nowDay = DateTime.Now.AddDays(-1).ToDateDay();
            //非管理员通报内容
            string sendMsgContent = string.Empty;
            //已填报项目ids
            var fillProjectIds = new List<Guid>();
            if (type == 1)//项目日报
            {
                sendMsgContent = "项目日报";
                //获取当日已填报的项目
                fillProjectIds = await dbContext.Queryable<DayReport>().Where(dp => dp.IsDelete == 1 && dp.DateDay == nowDay && proIds.Contains(dp.ProjectId)).Select(fillPro => fillPro.ProjectId).ToListAsync();
            }
            else if (type == 2)//项目月报
            {
                sendMsgContent = "项目月报";
                //获取当月 
                int nowMonth = 0;
                if (DateTime.Now.AddDays(-1).Day >= 26)
                {
                    nowMonth = ConvertHelper.ToDateMonth(DateTime.Now.AddDays(-1));
                }
                else
                {
                    nowMonth = ConvertHelper.ToDateMonth(DateTime.Now.AddMonths(-1));
                }
                //获取已填报项目月报项目ids
                fillProjectIds = await dbContext.Queryable<MonthReport>().Where(project => project.DateMonth == nowMonth && project.IsDelete == 1 && proIds.Contains(project.ProjectId)).Select(pId => pId.ProjectId).ToListAsync();
            }
            else if (type == 3)//安监日报
            {
                sendMsgContent = "安监日报";
                //获取当日已填报的项目
                fillProjectIds = await dbContext.Queryable<SafeSupervisionDayReport>().Where(dp => dp.IsDelete == 1 && dp.DateDay == nowDay && proIds.Contains(dp.ProjectId)).Select(fillPro => fillPro.ProjectId).ToListAsync();
            }
            else if (type == 4)//船舶日报
            {
                sendMsgContent = "船舶日报";
                fillProjectIds = await dbContext.Queryable<ShipDayReport>().Where(dp => dp.IsDelete == 1 && dp.DateDay == nowDay && shipmovementsProjectIds.Contains(dp.ProjectId)).Select(fillPro => fillPro.ProjectId).ToListAsync();

            }
            #region 管理员接收消息
            //日期转换
            ConvertHelper.TryConvertDateTimeFromDateDay(nowDay, out DateTime dateTime);
            //给管理员发送次数
            int sendCountForAdmin = 0;
            foreach (var item in companyCodes)
            {
                //序号
                int seqAdmin = 1;
                string sendMsgContentForAdmin = string.Empty;
                //公司数据
                var institionsCompaynIds = institutionData.Where(x => x.Grule.Contains(item)).Select(x => x.PomId.Value).ToList();
                //当前公司需要填报的项目
                var companyProjectIds = new List<Guid>();
                if (type == 1 || type == 3 || type == 2)//项目日报、安监日报
                {
                    companyProjectIds = projectData.Where(x => institionsCompaynIds.Contains(x.CompanyId.Value)).Select(x => x.Id.Value).ToList();
                }
                else if (type == 4)//船舶日报
                {
                    companyProjectIds = shipmovements.Where(x => institionsCompaynIds.Contains(x.CompanyId.Value)).Select(x => x.Id.Value).ToList();
                }
                //当前公司已填报的项目
                var companyFillProjectIds = companyProjectIds.Where(y => fillProjectIds.Contains(y)).ToList();
                if (type == 4) { companyProjectIds = companyProjectIds.GroupBy(x => x).Select(x => x.First()).ToList(); }
                int i = 0;
                foreach (var x in companyProjectIds)
                {
                    if (!companyFillProjectIds.Contains(x))
                    {
                        i++;
                        if (i <= 20)
                        {
                            sendMsgContentForAdmin += seqAdmin + "、" + projectData.FirstOrDefault(y => y.Id == x).Name + "<br>";
                            seqAdmin++;
                        }
                        else
                        {
                            sendMsgContentForAdmin += "......";
                            seqAdmin++;
                            break;
                        }
                    }
                }
                //companyProjectIds.ForEach(x =>
                //{

                //    if (!companyFillProjectIds.Contains(x))
                //    {
                //        i++;
                //        if ( i <= 20 )
                //        {
                //            sendMsgContentForAdmin += seqAdmin + "、" + projectData.FirstOrDefault(y => y.Id == x).Name + "<br>";
                //            seqAdmin++;
                //        }
                //        else
                //        {
                //            sendMsgContentForAdmin += "......";
                //            seqAdmin++;

                //        }
                //    }
                //});
                string title = dateTime.Month + "月" + dateTime.Day + "日，未填报" + sendMsgContent + "项目数量为" + companyProjectIds.Count + ",未报项目如下：<br>";

                //内容不为空 发送消息
                if (!string.IsNullOrWhiteSpace(sendMsgContentForAdmin))
                {

                    var userCodesForAdmin = new List<string>();
                    switch (item)
                    {
                        case "101162350":
                            userCodesForAdmin.Add("2016146340");//广航局陈翠
                            break;
                        case "101341960":
                            userCodesForAdmin.Add("2017028792");//疏浚公司朱智文
                            break;
                        case "101174265":
                            userCodesForAdmin.Add("2018008722"); //交建公司陈河元
                            break;
                        case "101174254":
                            userCodesForAdmin.Add("2018009624");//三公司王琦
                            break;
                        case "101305005":
                            userCodesForAdmin.Add("2017005354");//五公司孟浩
                            break;
                        case "101288132":
                            userCodesForAdmin.Add("2020012309");//福建公司李倩
                            break;
                        case "101332050":
                            userCodesForAdmin.Add("2016042370");//四公司杨加录
                            break;
                        case "101162575":
                            userCodesForAdmin.Add("L20080287");//研究院赵锐
                            break;
                    }
                    if (DateTime.Now.TimeOfDay > rangeEnd && DateTime.Now.TimeOfDay < rangeEnd1)
                    {
                        var msgResponse = new SingleMessageTemplateRequestDto
                        {
                            IsAll = false,
                            UserIds = userCodesForAdmin,
                            MessageType = JjtMessageType.TEXT,
                            TextContent = title + sendMsgContentForAdmin
                        };
                        //var a = await dbContext.Queryable<Project>().Where(x => x.StatusId == "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid()).GroupBy(x => x.CompanyId).ToListAsync();
                        var result = JjtUtils.SinglePushMessage(msgResponse);
                        if (result) sendCountForAdmin++;
                    }
                    else
                    {
                        var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1).ToListAsync();
                        var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                        var obj = new SingleMessageTemplateRequestDto()
                        {
                            IsAll = false,
                            ChatId = pushUsers.GroupNumber,
                            MessageType = JjtMessageType.Test,
                            TextContent = title + sendMsgContentForAdmin
                        };
                        var pushResult = JjtUtils.SinglePushMessage(obj);
                        responseAjaxResult.Data = pushResult;
                        logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                    }
                }
            }
            #endregion

            #region 项目负责人接收消息
            //获取所有项目信息
            var projects = new List<JjtSendMsgDayRepResponse>();
            if (type == 1 || type == 3 || type == 2)
            {
                projects = projectData.Where(p => !string.IsNullOrWhiteSpace(p.ReportForMertel)).ToList();
            }
            else if (type == 4)
            {
                projects = shipmovements;
            }
            //获取需要发消息通知填报的项目
            var repProjects = projects.Where(project => !fillProjectIds.Contains(project.Id.Value)).ToList();
            //获取项目负责人手机号
            List<string> phones = repProjects.Select(phone => phone.ReportForMertel).ToList();
            //获取所有报表负责人信息
            List<Model.User> repUsers = await dbContext.Queryable<Model.User>().Where(u => phones.Contains(u.Phone)).ToListAsync();
            ////获取需要填报的项目ids
            //List<Guid> projectIds = repProjects.Select(p => p.Id).ToList();
            ////获取需要发送的项目负责人信息
            //var leaders = await dbContext.Queryable<ProjectLeader>()
            //    .LeftJoin<Model.User>((proLea, user) => proLea.AssistantManagerId == user.Id)
            //    .Where((proLea, user) => projectIds.Contains(proLea.ProjectId.Value) && proLea.IsDelete == 1 && proLea.Type == 8 && proLea.IsPresent == true)
            //    .Select((proLea, user) => new { proLea.ProjectId, user.GroupCode })
            //    .ToListAsync();
            //发送次数
            int sendCount = 0;
            //初始化需要发送的报表人、项目负责人人资编码
            List<string> userCodes = new List<string>();
            foreach (var item in repProjects)
            {
                userCodes.Clear();//重置人资编码
                string msgContent = string.Empty;//消息内容
                //追加报表人人资编码
                userCodes.Add(repUsers.Where(repuser => repuser.Phone == item.ReportForMertel).SingleOrDefault()?.GroupCode);
                ////追加项目经理人资编码
                //userCodes.Add(leaders.Where(leader => leader.ProjectId == item.Id).SingleOrDefault()?.GroupCode);
                msgContent = "您好，您所负责的（" + item.Name + "）" + sendMsgContent + "，当前还没有填报。请您尽快填报！！！";
                if (DateTime.Now.TimeOfDay >= rangeStart && DateTime.Now.TimeOfDay < rangeEnd)
                {
                    var msgResponse = new SingleMessageTemplateRequestDto
                    {
                        IsAll = false,
                        UserIds = userCodes,
                        MessageType = JjtMessageType.TEXT,
                        TextContent = msgContent
                    };
                    var result = JjtUtils.SinglePushMessage(msgResponse);
                    if (result) sendCount++;
                }
                else
                {
                    var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1).ToListAsync();
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        ChatId = pushUsers.GroupNumber,
                        MessageType = JjtMessageType.Test,
                        TextContent = msgContent
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj);
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                }
            }
            #endregion
            var responseMsg = sendCount > 0 && sendCountForAdmin > 0 ? "" : sendCountForAdmin > 0 && sendCount == 0 ? "未给项目负责人发送消息" : sendCountForAdmin == 0 && sendCount > 0 ? "未给管理员发送消息" : "未给管理员及项目负责人发送消息";
            responseAjaxResult.Data = responseMsg == "未给管理员及项目负责人发送消息" ? false : true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 新版交建通发消息 监控运营中心图片消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtTextCardMsgDetailsAsync(int dateDay = 0)
        {
            #region 111
            var responseAjaxResult = new ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>();
            var result = await dbContext.Queryable<TempTable>().FirstAsync();
            if (result != null && !string.IsNullOrWhiteSpace(result.Value))
            {
                return JsonConvert.DeserializeObject<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>>(result.Value);
            }
            //在建项目的IDs
            List<Guid> onBuildProjectIds = new List<Guid>();
            var jjtSendMessageMonitoringDayReportResponseDto = new JjtSendMessageMonitoringDayReportResponseDto()
            {
                DayTime = DateTime.Now.AddDays(-1).ToString("MM月dd日")
            };
            #region 查询条件相关

            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            //周期结束时间
            var endTime = Convert.ToDateTime(startTime).AddMonths(1).ToString("yyyy-MM-25 23:59:59");
            //统计周期 上个月的26号到本月的25号之间为一个周期
            //当前时间上限int类型
            var currentTimeIntUp = int.Parse(Convert.ToDateTime(startTime).ToString("yyyyMM26"));
            //当前时间下限int类型
            var currentTimeInt = DateTime.Now.AddDays(-1).ToDateDay();
            //本年的月份
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            //本年的年份 
            var yearStartTime = DateTime.Now.Year.ToString();
            //年累计开始时间（每年的开始时间）
            var startYearTimeInt = int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy") + "1226");//int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1226"));
            //年累计结束时间
            var endYearTimeInt = int.Parse(DateTime.Now.ToString("yyyyMMdd")) > 1226 && int.Parse(DateTime.Now.ToString("yyyyMMdd")) <= 31 ? int.Parse(DateTime.Now.AddYears(1).ToString("yyyy1225")) : int.Parse(DateTime.Now.ToString("yyyy1225")); //int.Parse(DateTime.Now.ToString("yyyy1225"));
                                                                                                                                                                                                                                                      //每月多少天
                                                                                                                                                                                                                                                      // int days = DateTime.DaysInMonth(int.Parse(endYearTimeInt.ToString().Substring(0, 4)), month);  //DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
            int days = TimeHelper.GetTimeSpan(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime)).Days + 1;
            //已过多少天
            var ofdays = DateTime.Now.Day <= 26 ? (DateTime.Now.Day + ((days - 26))) : DateTime.Now.Day - 26;
            //今年已过多少天
            var dayOfYear = 0;
            //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1231")))
            //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(yearStartTime + "1231"))
            //{
            //    dayOfYear = ofdays+ DateTime.Now.DayOfYear;
            //}
            //else
            //{
            //    //这个6天是上一年1226-1231之间的天数
            //    dayOfYear = DateTime.Now.DayOfYear + 5;
            //}

            if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > int.Parse(DateTime.Now.Year + "1231"))
            {
                var diffDay = TimeHelper.GetTimeSpan(DateTime.Now.ToString("yyyy-12-26").ObjToDate(), DateTime.Now);
                dayOfYear = diffDay.Days;
            }
            else
            {
                //这个6天是上一年1226-1231之间的天数
                dayOfYear = DateTime.Now.DayOfYear - 1 + 5;
            }
            #endregion

            #region 共用数据
            var commonDataList = await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1).ToListAsync();
            var comonDataProductionList = await dbContext.Queryable<CompanyProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.DateDay == DateTime.Now.Year).ToListAsync();
            var monthDiffProductionValue = await dbContext.Queryable<MonthDiffProductionValue>().Where(x => x.IsDelete == 1).ToListAsync();
            #endregion

            #region 项目总体生产情况
            //在建项目的所有IDS


            #region 广航局合同项目基本信息
            ProjectBasePoduction projectBasePoduction = null;
            List<CompanyProjectBasePoduction> companyProjectBasePoductions = new List<CompanyProjectBasePoduction>();
            //合同项目状态ids集合
            var contractProjectStatusIds = CommonData.BuildIds.SplitStr(",").Select(x => x.ToGuid()).ToList();
            //项目类型为其他非施工类业务 排除
            var noConstrutionProject = CommonData.NoConstrutionProjectType;
            //在建项目状态ID
            var buildProjectId = CommonData.PConstruc.ToGuid();
            //停缓建Ids
            var stopProjectIds = CommonData.PSuspend.Split(",").Select(x => x.ToGuid()).ToList();
            //未开工状态
            var notWorkIds = CommonData.NotWorkStatusIds.Split(",").Select(x => x.ToGuid()).ToList();
            //各个公司的项目信息
            var companyProjectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && contractProjectStatusIds.Contains(x.StatusId.Value)
            && x.TypeId != noConstrutionProject).ToListAsync();
            //取出相关日报信息(当天项目日报)
            var currentDayProjectList = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && x.ProcessStatus == DayReportProcessStatus.Submited)
                  .ToListAsync();
            //公共数据取出项目相关信息
            var companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //在建项目IDS
                var currentCompanyIds = companyProjectList.Where(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId)
                    .Select(x => x.Id).ToList();
                if (item.Collect == 0)
                {
                    onBuildProjectIds.AddRange(currentCompanyIds);
                }
                //合同项目数
                var currentCompanyCount = companyProjectList.Count(x => x.CompanyId == item.ItemId);
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //停缓建项目数
                var stopProjectCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && stopProjectIds.Contains(x.StatusId.Value));
                //未开工的项目数量
                var notWorkCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && notWorkIds.Contains(x.StatusId.Value));
                //当前合同项目的所有ids
                var dayIds = companyProjectList.Where(x => x.CompanyId == item.ItemId).Select(x => x.Id).ToList();
                //设备数量
                var facilityCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.ConstructionDeviceNum).Sum();
                //线程施工人数量
                var workerCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.SiteConstructionPersonNum).Sum();
                //危大工程项数量
                var riskWorkCountCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.HazardousConstructionNum).Sum();

                if (item.Collect == 0)
                {

                    companyProjectBasePoductions.Add(new CompanyProjectBasePoduction()
                    {
                        Name = item.Name,
                        OnContractProjectCount = currentCompanyCount,
                        OnBuildProjectCount = currentCompany,
                        StopBuildProjectCount = stopProjectCount,
                        BuildCountPercent = currentCompanyCount == 0M ? 0M : Math.Round((((decimal)(currentCompany)) / currentCompanyCount) * 100, 2),
                        FacilityCount = facilityCount,
                        WorkerCount = workerCount,
                        RiskWorkCount = riskWorkCountCount,
                        NotWorkCount = notWorkCount,
                    });
                }
                else
                {

                    var totalContractProjectCount = companyProjectBasePoductions.Sum(x => x.OnContractProjectCount);
                    var totalOnBuildProjectCount = companyProjectBasePoductions.Sum(x => x.OnBuildProjectCount);
                    var totalStopBuildProjectCount = companyProjectBasePoductions.Sum(x => x.StopBuildProjectCount);
                    var totalNotWorkProjectCount = companyProjectBasePoductions.Sum(x => x.NotWorkCount);
                    var totalContractProjectPercent = totalContractProjectCount == 0M ? 0M : Math.Round(((decimal)totalOnBuildProjectCount / totalContractProjectCount) * 100, 2);
                    //设备  施工   危大
                    var totalFacilityCount = companyProjectBasePoductions.Sum(x => x.FacilityCount);
                    var totalWorkerCount = companyProjectBasePoductions.Sum(x => x.WorkerCount);
                    var totalRiskWorkCount = companyProjectBasePoductions.Sum(x => x.RiskWorkCount);


                    //汇总项
                    companyProjectBasePoductions.Add(new CompanyProjectBasePoduction()
                    {
                        Name = item.Name,
                        OnContractProjectCount = companyProjectList.Count,
                        OnBuildProjectCount = totalOnBuildProjectCount,
                        StopBuildProjectCount = totalStopBuildProjectCount,
                        NotWorkCount = totalNotWorkProjectCount,
                        BuildCountPercent = totalContractProjectPercent,
                        FacilityCount = totalFacilityCount,
                        RiskWorkCount = totalRiskWorkCount,
                        WorkerCount = totalWorkerCount
                    });
                    companyProjectBasePoductions = companyProjectBasePoductions.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
                    projectBasePoduction = new ProjectBasePoduction()
                    {
                        TotalOnContractProjectCount = companyProjectList.Count,
                        //TotalOnContractProjectCount = totalContractProjectCount,
                        TotalStopBuildProjectCount = totalStopBuildProjectCount,
                        TotalBuildCountPercent = totalContractProjectPercent,
                        TotalOnBuildProjectCount = totalOnBuildProjectCount,
                        TotalFacilityCount = totalFacilityCount,
                        TotalRiskWorkCount = totalRiskWorkCount,
                        TotalWorkerCount = totalWorkerCount,
                        CompanyProjectBasePoductions = companyProjectBasePoductions,
                        CompanyBasePoductionValues = new List<CompanyBasePoductionValue>()
                    };

                    jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction = projectBasePoduction;
                }

            }



            #endregion

            #region 广航局在建项目产值信息
            List<CompanyBasePoductionValue> companyBasePoductionValues = new List<CompanyBasePoductionValue>();
            //统计当年所有的项目日报信息
            var dayProductionValueList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where(x => x.IsDelete == 1
             //&& onBuildProjectIds.Contains(x.ProjectId)
             && (x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt))
                .Select((x, y) => new JjtProjectDayReport
                {
                    CompanyId = y.CompanyId.Value,
                    ProjectId = x.ProjectId,
                    DateDay = x.DateDay,
                    CreateTime = x.CreateTime.Value,
                    UpdateTime = x.UpdateTime.Value,
                    DayActualProductionAmount = x.DayActualProductionAmount
                }).ToListAsync();
            //广航局年累计产值(基础数据累加+几个公司的所有日产值)
            // var yearProductionValue = companyList.Sum(x => x.YearProductionValue) + dayProductionValueList.Sum(x => x.DayActualProductionAmount);
            //var yearProductionValue = dayProductionValueList.Sum(x => x.DayActualProductionAmount);
            var companyValue = new ShareData().Init();
            var yearProductionValue = Math.Round(companyValue.Sum(x => x.Production * 100000000) + dayProductionValueList
                 .Where(x => x.DateDay >= 20240426 && x.DateDay <= currentTimeInt)
                    .Sum(x => x.DayActualProductionAmount), 3);
            //项目月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.DateMonth >= 202401).ToListAsync();
            //
            var projectIds = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            foreach (var item in companyList)
            {
                //if (item.ItemId != "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid())
                //    continue;
                //当前公司日产值 || x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime))
                decimal currentCompanyCount = dayProductionValueList.Where(x => x.CompanyId == item.ItemId
                  && x.DateDay == currentTimeInt
                  //&& x.CreateTime >= SqlFunc.ToDate(startTime) && x.CreateTime <= SqlFunc.ToDate(endTime)
                  //|| x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime)
                  ).Sum(x => x.DayActualProductionAmount);
                //当前公司的累计产值（前几个月报产值加上日产值）
                //var currentMonthCompanyCount =Math.Round( dayProductionValueList
                //    .Where(x => x.CompanyId == item.ItemId && x.DateDay >=20240326 && x.DateDay <= currentTimeInt)
                //    .Sum(x => x.DayActualProductionAmount),2)
                //    + GetCompanyProductuionValue(item.ItemId.Value, monthReport, projectIds, monthDiffProductionValue);

                var eachCompanyValue = companyValue.Where(x => x.CompanyId == item.ItemId.ToString()).FirstOrDefault()?.Production;
                //await Console.Out.WriteLineAsync(item.ItemId.ToString());
                var productionValue = eachCompanyValue == null ? 0 : eachCompanyValue.Value * 100000000;
                var currentMonthCompanyCount = Math.Round(dayProductionValueList
                    .Where(x => x.CompanyId == item.ItemId && x.DateDay >= 20240426 && x.DateDay <= currentTimeInt)
                    .Sum(x => x.DayActualProductionAmount), 3)
                    + productionValue;
                //年度产值占比 （广航局）
                //var yearProductionValuePercent = Math.Round(((decimal)(item.YearProductionValue + currentMonthCompanyCount) / yearProductionValue) * 100, 2);
                var yearProductionValuePercent = Math.Round(((decimal)(currentMonthCompanyCount) / yearProductionValue) * 100, 2);
                //较产值进度  计算公示如下
                //(年累计产值-（当前月-1）的计划产值+本月的计划产值/30.5*当前已过去多少天)/（当前月-1）的完成产值+本月的计划产值/30.5*当前已过去多少天)
                //累计完成产值
                var totalCompleteProductionValue = comonDataProductionList
                    .Where(x => x.CompanyId == item.ItemId)
                    .Sum(x => x.OnePlanProductionValue +
                 x.TwoPlanProductionValue +
                 x.ThreePlaProductionValue +
                 x.FourPlaProductionValue +
                 x.FivePlaProductionValue +
                 x.SixPlaProductionValue +
                 x.SevenPlaProductionValue +
                 x.EightPlaProductionValue +
                 x.NinePlaProductionValue +
                 x.TenPlaProductionValue +
                 x.ElevenPlaProductionValue +
                 x.TwelvePlaProductionValue);
                //本月计划产值
                var currentMonthPlanProductionValue = 0M;
                //本月计划产值汇总
                var currentTotalMonthPlanProductionValue = 0M;
                #region 查询当前月份计划产值
                //当前月份
                var currentMonth = DateTime.Now.Day <= 26 ? DateTime.Now.Month : DateTime.Now.AddMonths(1).Month;
                if (currentMonth == 1)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.OnePlanProductionValue.Value);
                };
                if (currentMonth == 2)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TwoPlanProductionValue.Value);
                };
                if (currentMonth == 3)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.ThreePlaProductionValue.Value);
                };
                if (currentMonth == 4)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.FourPlaProductionValue.Value);
                };
                if (currentMonth == 5)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.FivePlaProductionValue.Value);
                };
                if (currentMonth == 6)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.SixPlaProductionValue.Value);
                }
                if (currentMonth == 7)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.SevenPlaProductionValue.Value);
                };
                if (currentMonth == 8)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.EightPlaProductionValue.Value);
                };
                if (currentMonth == 9)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.NinePlaProductionValue.Value);
                };
                if (currentMonth == 10)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TenPlaProductionValue.Value);
                };
                if (currentMonth == 11)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.ElevenPlaProductionValue.Value);
                };
                if (currentMonth == 12)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TwelvePlaProductionValue.Value);
                };

                #endregion
                var baseCalc = totalCompleteProductionValue - currentMonthPlanProductionValue + currentMonthPlanProductionValue / 30.5M * ofdays;
                //较产值进度
                var productionValueProgressPercent = 0M;
                if (baseCalc.Value != 0)
                {
                    //var yearCompanyProductionValue = item.YearProductionValue + currentMonthCompanyCount;
                    var yearCompanyProductionValue = currentMonthCompanyCount;
                    //productionValueProgressPercent = Math.Round((yearCompanyProductionValue -baseCalc.Value) / baseCalc.Value * 100, 2);
                    productionValueProgressPercent = Math.Round((baseCalc.Value) / baseCalc.Value * 100, 2);
                }
                //每年公司完成指标
                var yearIndex = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).First().YearIndex;
                //序时进度
                var timeProgress = Math.Round((dayOfYear / 365M) * 100, 2);
                if (item.Collect == 0)
                {


                    //var totalYearProductionValue = Math.Round(((item.YearProductionValue + currentMonthCompanyCount) / 100000000), 2);
                    var totalYearProductionValue = Math.Round(((currentMonthCompanyCount) / 100000000), 2);
                    //超序时进度
                    var supersequenceProgress = yearIndex.Value != 0 ? (Math.Round((totalYearProductionValue / yearIndex.Value) * 100, 2) - timeProgress) : 0;
                    companyBasePoductionValues.Add(new CompanyBasePoductionValue()
                    {
                        Name = item.Name,
                        DayProductionValue = Math.Round(currentCompanyCount / 10000, 2),
                        TotalYearProductionValue = totalYearProductionValue,
                        YearProductionValueProgressPercent = yearProductionValuePercent,
                        ProductionValueProgressPercent = productionValueProgressPercent,
                        SupersequenceProgress = supersequenceProgress
                    });
                }
                else
                {
                    totalCompleteProductionValue = comonDataProductionList
                   .Sum(x => x.OnePlanProductionValue +
                x.TwoPlanProductionValue +
                x.ThreePlaProductionValue +
                x.FourPlaProductionValue +
                x.FivePlaProductionValue +
                x.SixPlaProductionValue +
                x.SevenPlaProductionValue +
                x.EightPlaProductionValue +
                x.NinePlaProductionValue +
                x.TenPlaProductionValue +
                x.ElevenPlaProductionValue +
                x.TwelvePlaProductionValue);
                    #region 查询汇总值
                    //查询汇总产值
                    if (currentMonth == 1)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.OnePlanProductionValue.Value);
                    };
                    if (currentMonth == 2)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TwoPlanProductionValue.Value);
                    };
                    if (currentMonth == 3)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.ThreePlaProductionValue.Value);
                    };
                    if (currentMonth == 4)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.FourPlaProductionValue.Value);
                    };
                    if (currentMonth == 5)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.FivePlaProductionValue.Value);
                    };
                    if (currentMonth == 6)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.SixPlaProductionValue.Value);
                    }
                    if (currentMonth == 7)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.SevenPlaProductionValue.Value);
                    };
                    if (currentMonth == 8)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.EightPlaProductionValue.Value);
                    };
                    if (currentMonth == 9)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.NinePlaProductionValue.Value);
                    };
                    if (currentMonth == 10)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TenPlaProductionValue.Value);
                    };
                    if (currentMonth == 11)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.ElevenPlaProductionValue.Value);
                    };
                    if (currentMonth == 12)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TwelvePlaProductionValue.Value);
                    };
                    #endregion

                    baseCalc = totalCompleteProductionValue - currentTotalMonthPlanProductionValue + currentTotalMonthPlanProductionValue / 30.5M * ofdays;
                    if (baseCalc.Value != 0)
                    {
                        var diffValue = ((yearProductionValue) - baseCalc);
                        productionValueProgressPercent = Math.Round(diffValue.Value / baseCalc.Value * 100, 2);
                    }

                    //当日产值
                    decimal dayTotalPoductionValues = companyBasePoductionValues.Sum(x => x.DayProductionValue);
                    //名称排除‘个’字
                    var filterStr = "(个)";
                    var name = item.Name.IndexOf(filterStr) >= 0 ? item.Name.Replace(filterStr, "").TrimAll() : item.Name;

                    #region 营业收入保障指数
                    //营业收入保障指数
                    var incomeSecurityLevel = (dayTotalPoductionValues / 3000) * 100;
                    var incomeStar = 0;
                    if (incomeSecurityLevel <= 30)
                    {
                        incomeStar = 1;
                    }
                    else if (incomeSecurityLevel > 30 && incomeSecurityLevel <= 60)
                    {
                        incomeStar = 2;
                    }
                    else if (incomeSecurityLevel > 60 && incomeSecurityLevel <= 80)
                    {
                        incomeStar = 3;
                    }
                    else if (incomeSecurityLevel > 80 && incomeSecurityLevel <= 90)
                    {
                        incomeStar = 4;
                    }
                    else if (incomeSecurityLevel > 90)
                    {
                        incomeStar = 5;
                    }
                    projectBasePoduction.IncomeSecurityLevel = incomeStar;
                    #endregion

                    projectBasePoduction.DayProductionValue = dayTotalPoductionValues;
                    projectBasePoduction.TotalYearProductionValue = companyBasePoductionValues.Sum(x => x.TotalYearProductionValue);
                    projectBasePoduction.ProductionValueProgressPercent = productionValueProgressPercent;
                    companyBasePoductionValues = companyBasePoductionValues.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
                    projectBasePoduction.CompanyBasePoductionValues = companyBasePoductionValues;
                    //广航局年度指标
                    yearIndex = comonDataProductionList.Sum(x => x.YearIndex.Value);
                    //超序时进度
                    projectBasePoduction.SupersequenceProgress = yearIndex.Value != 0 ? (Math.Round((projectBasePoduction.TotalYearProductionValue / yearIndex.Value) * 100, 2) - timeProgress) : 0;
                    companyBasePoductionValues.Add(new CompanyBasePoductionValue()
                    {
                        Name = name,
                        DayProductionValue = dayTotalPoductionValues,
                        // TotalYearProductionValue = Math.Round(yearProductionValue / 100000000, 2),
                        TotalYearProductionValue = companyBasePoductionValues.Sum(x => x.TotalYearProductionValue),

                        YearProductionValueProgressPercent = 100,
                        ProductionValueProgressPercent = productionValueProgressPercent,
                        SupersequenceProgress = projectBasePoduction.SupersequenceProgress
                    });

                }
            }
            #endregion

            #region 柱形图
            var companyProductionList = dbContext.Queryable<CompanyProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.DateDay.Value == SqlFunc.ToInt32(yearStartTime)).ToList();
            var companyMonthProductionValue = GetProductionValueInfo(month, companyProductionList);
            CompanyProductionCompare companyProductionCompares = new CompanyProductionCompare()
            {
                PlanCompleteRate = new List<decimal>(),
                TimeSchedule = new List<decimal>(),
                XAxisData = new List<string>(),
                CompleteProductuin = new List<decimal>(),
                PlanProductuin = new List<decimal>()
            };

            foreach (var item in companyList)
            {
                if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Contains("广航局"))
                {
                    continue;
                }

                companyProductionCompares.XAxisData.Add(item.Name);
                //获取各个公司本月的完成和计划产值
                var currentMonthCompanyProductionValue = companyMonthProductionValue.Where(x => x.Id == item.ItemId).FirstOrDefault();
                if (currentMonthCompanyProductionValue != null)
                {
                    var completeProductionValue = Math.Round(currentMonthCompanyProductionValue.CompleteProductionValue / 100000000M, 2);
                    var planProductionValue = Math.Round(currentMonthCompanyProductionValue.PlanProductionValue / 100000000M, 2);
                    companyProductionCompares.CompleteProductuin.Add(completeProductionValue);
                    companyProductionCompares.PlanProductuin.Add(planProductionValue);
                }

                //计划完成率
                if (currentMonthCompanyProductionValue != null && currentMonthCompanyProductionValue.PlanProductionValue != 0)
                {
                    var completeRate = Math.Round((((decimal)currentMonthCompanyProductionValue.CompleteProductionValue) / currentMonthCompanyProductionValue.PlanProductionValue) * 100, 0);
                    companyProductionCompares.PlanCompleteRate.Add(completeRate);
                }
                else
                {
                    companyProductionCompares.PlanCompleteRate.Add(0);
                }
                //时间进度
                var timeSchedult = Math.Round((ofdays / 31M) * 100, 0);
                companyProductionCompares.TimeSchedule.Add(timeSchedult);
                projectBasePoduction.CompanyProductionCompares = companyProductionCompares;
            }

            companyProductionCompares.YMax = companyProductionCompares.PlanProductuin.Count == 0 ? 0 : companyProductionCompares.PlanProductuin.Max();
            #endregion

            #region 项目产值完成排名 暂时不用
            //List<ProjectRank> projectRankList = new List<ProjectRank>();
            ////获取公司信息
            //var companyIds = await dbContext.Queryable<CompanyProductionValueInfo>()
            //    .Where(x => x.IsDelete == 1 && x.DateDay.Value == SqlFunc.ToInt32(yearStartTime)).Select(x => x.CompanyId).ToListAsync();
            //var planList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == Convert.ToInt32(yearStartTime)).ToListAsync();
            //var dayReport = dbContext.Queryable<DayReport>()
            //    .LeftJoin<Project>((d, p) => d.ProjectId == p.Id)
            //    .OrderByDescending((d, p) => p.Name)
            //    .Where((d, p) => companyIds.Contains(p.CompanyId.Value) && d.IsDelete == 1
            //    && d.DateDay == currentTimeInt)
            //    .GroupBy((d, p) => d.ProjectId)
            //    .Select((d, p) => new
            //    {
            //        ProjectId = p.Id,
            //        ProjectName = p.ShortName,
            //        CompanyId = p.CompanyId,
            //        MonthAmount = SqlFunc.AggregateSum(d.DayActualProductionAmount)
            //    })
            //    .ToList();
            //var dayReportData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1).ToListAsync();
            ////项目完成产值历史数据
            //var historyOutPut =await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //var projectPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1).ToListAsync();
            //foreach (var item in dayReport)
            //{
            //    ProjectRank model = new ProjectRank();
            //    var planValue = GetProjectPlanValue(month, planList.Where(x => x.ProjectId == item.ProjectId && x.CompanyId == item.CompanyId).FirstOrDefault());
            //    model.ProjectName = item.ProjectName;
            //    model.CurrentYearPlanProductionValue = Math.Round(GetRrojectProductionValue(projectPlanProductionData, item.ProjectId).Value, 2);
            //    model.CurrentYearCompleteProductionValue = Math.Round(GetRrojectCompletProductionValue(dayReportData, historyOutPut, item.ProjectId), 2);
            //    if (model.CurrentYearPlanProductionValue != 0)
            //    {
            //        model.CompleteRate = Math.Round(model.CurrentYearCompleteProductionValue / model.CurrentYearPlanProductionValue*100, 2);
            //    }
            //    model.DayActualValue = Math.Round(item.MonthAmount / 10000, 2);
            //    projectRankList.Add(model);
            //}
            //projectBasePoduction.ProjectRanks = projectRankList.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();
            ////合计
            //var totalYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            //var totalYearCompletProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            //decimal totalYearCompletRate = 0;
            //if (totalYearPlanProductionValue != 0)
            //{
            //    totalYearCompletRate = Math.Round((totalYearCompletProductionValue / totalYearPlanProductionValue)*100, 2);
            //}
            //projectBasePoduction.TotalCurrentYearPlanProductionValue = totalYearPlanProductionValue;
            //projectBasePoduction.TotalCurrentYearCompleteProductionValue = totalYearCompletProductionValue;
            //// projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            //if (projectBasePoduction.TotalYearProductionValue != 0)
            //{
            //    projectBasePoduction.TotalCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue)*100, 2) ;
            //}
            //if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
            //{
            //    projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            //}
            ////当日所有项目汇总
            //projectBasePoduction.SumProjectRanks = Math.Round(projectRankList.Sum(x => x.DayActualValue), 2);
            ////当日排名前10条汇总
            //projectBasePoduction.SumProjectRanksTen = Math.Round(projectBasePoduction.ProjectRanks.Sum(x => x.DayActualValue), 2);
            ////总产值占比
            //projectBasePoduction.TotalProportion = Math.Round(projectBasePoduction.SumProjectRanksTen == 0 || projectBasePoduction.SumProjectRanks == 0 ? 0 : projectBasePoduction.SumProjectRanksTen / projectBasePoduction.SumProjectRanks * 100, 2);


            #endregion

            #region 项目年度产值完成排名 暂时不用
            //List<ProjectRank> projectRankList = new List<ProjectRank>();
            //var projectLists = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
            //    .Select(x => new { x.Id, x.ShortName, x.CompanyId }).ToListAsync();
            //var projectSumDayProductionValue = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1
            //&& projectLists.Select(x => x.Id).ToList().Contains(x.ProjectId))
            //     .GroupBy(x => x.ProjectId)
            //     .Select(x => new { x.ProjectId, productionValue = SqlFunc.AggregateSum(x.DayActualProductionAmount) }).ToListAsync();
            //projectSumDayProductionValue = projectSumDayProductionValue.OrderByDescending(x => x.productionValue).Take(1000).ToList();
            //var planList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == Convert.ToInt32(yearStartTime)).ToListAsync();
            //var projectPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == DateTime.Now.Year).ToListAsync();
            //var dayReportData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay >= startYearTimeInt
            //&& x.DateDay <= endYearTimeInt).ToListAsync();
            ////项目完成产值历史数据
            //var historyOutPut = await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //var monthStartTime = int.Parse(startYearTimeInt.ToString().Substring(0, 6));
            //var monthEndTime = int.Parse(endYearTimeInt.ToString().Substring(0, 6));
            ////月报数据
            //var monthDataList = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1
            //&& x.DateMonth > monthStartTime && x.DateMonth <= monthEndTime).ToListAsync();
            //foreach (var item in projectSumDayProductionValue)
            //{
            //    ProjectRank model = new ProjectRank();
            //    //var planValue = GetProjectPlanValue(month, planList.Where(x => x.ProjectId == item.ProjectId && x.CompanyId == item.CompanyId).FirstOrDefault());
            //    var projectInfo = projectLists.Where(x => x.Id == item.ProjectId).SingleOrDefault();
            //    model.ProjectName = projectInfo == null ? string.Empty : projectInfo.ShortName;
            //    //if (model.ProjectName == "茂名港博贺项目")
            //    //{

            //    //}
            //    model.CurrentYearPlanProductionValue = Math.Round(GetRrojectProductionValue(projectPlanProductionData, item.ProjectId).Value, 2);
            //    model.CurrentYearCompleteProductionValue = Math.Round(GetRrojectCompletProductionValue(dayReportData, historyOutPut, monthDataList, currentTimeIntUp, currentTimeInt, item.ProjectId), 2);
            //    if (model.CurrentYearPlanProductionValue != 0)
            //    {
            //        model.CompleteRate = Math.Round(model.CurrentYearCompleteProductionValue / model.CurrentYearPlanProductionValue * 100, 2);
            //    }
            //    //当日产值
            //    var time = DateTime.Now.AddDays(-1).ToDateDay();
            //    var currentDayProduction = dayReportData.Where(x => x.ProjectId == item.ProjectId && x.DateDay == time).FirstOrDefault();
            //    //model.DayActualValue = Math.Round(item.productionValue / 10000, 2);
            //    if (currentDayProduction != null)
            //        model.DayActualValue = Math.Round(currentDayProduction.DayActualProductionAmount / 10000, 2);
            //    projectRankList.Add(model);
            //}
            //projectBasePoduction.ProjectRanks = projectRankList;
            //projectBasePoduction.ProjectRanks = projectBasePoduction.ProjectRanks.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();

            ////合计
            //var totalYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            //var totalYearCompletProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            //decimal totalYearCompletRate = 0;
            //if (totalYearPlanProductionValue != 0)
            //{
            //    totalYearCompletRate = Math.Round((totalYearCompletProductionValue / totalYearPlanProductionValue) * 100, 2);
            //}
            //projectBasePoduction.TotalCurrentYearPlanProductionValue = totalYearPlanProductionValue;
            //projectBasePoduction.TotalCurrentYearCompleteProductionValue = totalYearCompletProductionValue;
            //// projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            //if (projectBasePoduction.TotalYearProductionValue != 0)
            //{
            //    projectBasePoduction.TotalCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue) * 100, 2);
            //}
            //if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
            //{
            //    projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            //}
            //projectBasePoduction.SumProjectRanksTen = projectBasePoduction.ProjectRanks.Sum(x => x.DayActualValue);
            #endregion

            #region 项目年度产值完成排名新版
            List<ProjectRank> projectRankList = new List<ProjectRank>();
            var projectLists = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
                .Select(x => new { x.Id, x.ShortName, x.CompanyId }).ToListAsync();
            //当年完成产值
            var eachProjectProductionValue = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt).ToListAsync();
            //当年各个项目计划产值
            var projectYearPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == DateTime.Now.Year).ToListAsync();
            //查询历史数据
            var projectPlanProductionData = await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //项目月报数据
            var year = int.Parse(DateTime.Now.ToString("yyyy01"));
            var projectMonthData = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.DateMonth >= year).ToListAsync();
            foreach (var item in projectLists)
            {
                //if (item.Id != "08db3b35-fb38-4bd7-8c32-5423575bad59".ToGuid())
                //{
                //    continue;
                //}
                //当年项目完成产值
                var projectYearTotalProductionValue = eachProjectProductionValue.Where(x => x.ProjectId == item.Id && x.DateDay >= currentTimeIntUp && x.DateDay <= currentTimeInt).Sum(x => x.DayActualProductionAmount);
                //当年项目计划产值
                var projectPalnProduction = Math.Round(GetRrojectProductionValue(projectYearPlanProductionData, item.Id).Value, 2);
                //今日完成产值
                var day = DateTime.Now.AddDays(-1).ToDateDay();
                var dayProductionValue = eachProjectProductionValue.Where(x => x.ProjectId == item.Id && x.DateDay == day).SingleOrDefault();
                //计算历史计划产值
                //var projectHistoryProduciton= projectYearPlanProductionData.Where(x => x.ProjectId == item.Id).SingleOrDefault();
                //计算2023-06月之前的数据
                var proejctHistoty = projectPlanProductionData.Where(x => x.ProjectId == item.Id && x.OutputValue.HasValue == true).Select(x => x.OutputValue.Value).SingleOrDefault();
                //月份相加产值
                var monthValue = projectMonthData.Where(x => x.ProjectId == item.Id).Sum(x => x.CompleteProductionAmount);

                var dayValue = 0M;
                if (dayProductionValue != null)
                {
                    dayValue = Math.Round(dayProductionValue.DayActualProductionAmount / 10000, 2);
                }
                ProjectRank projectRank = new ProjectRank()
                {
                    ProjectName = item.ShortName,
                    //CurrentYearCompleteProductionValue = (Math.Round(projectYearTotalProductionValue / 100000000, 2) ),
                    CurrentYearCompleteProductionValue = Math.Round(monthValue / 100000000, 2) + Math.Round(projectYearTotalProductionValue / 100000000, 2),
                    CurrentYearPlanProductionValue = projectPalnProduction,
                    DayActualValue = dayValue,
                };
                if (projectPalnProduction != 0)
                {
                    projectRank.CompleteRate = Math.Round((projectRank.CurrentYearCompleteProductionValue / projectRank.CurrentYearPlanProductionValue) * 100, 2);
                }
                projectRankList.Add(projectRank);
            }
            projectBasePoduction.ProjectRanks = projectRankList.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();
            //总计
            projectBasePoduction.TotalCurrentYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            projectBasePoduction.TotalCurrentYearCompleteProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
                projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            var totalYearCompletRate = 0M;
            if (projectBasePoduction.TotalYearProductionValue != 0)
            {
                totalYearCompletRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue) * 100, 2);
            }
            projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            projectBasePoduction.SumProjectRanksTen = projectBasePoduction.ProjectRanks.Sum(x => x.DayActualValue.Value);
            #endregion

            #region 项目产值强度表格
            List<ProjectIntensity> projectIntensities = new List<ProjectIntensity>();
            //获取只需要在建的项目
            var onBuildProjectList = companyProjectList.Where(x => onBuildProjectIds.Contains(x.Id)).ToList();
            var onBuildIds = onBuildProjectList.Select(x => x.Id).ToList();
            var planValueList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && onBuildIds.Contains(x.ProjectId)).ToListAsync();
            if (onBuildProjectList.Any())
            {
                foreach (var item in onBuildProjectList)
                {
                    //项目当日实际产值
                    var currentDayProjectPrduction = currentDayProjectList.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                    //项目当日计划
                    var planValueFirst = planValueList.Where(x => x.ProjectId == item.Id && x.Year == Convert.ToInt32(yearStartTime)).FirstOrDefault();
                    var planValue = GetProjectPlanValue(month, planValueFirst);
                    var rate = currentDayProjectPrduction == null || planValue == 0 ? 0 : Math.Round(((currentDayProjectPrduction.DayActualProductionAmount / 10000) / (planValue / 10000) * 100), 0);
                    if (rate < 80)
                    {
                        projectIntensities.Add(new ProjectIntensity()
                        {
                            Id = item.Id,
                            Name = item.ShortName,
                            PlanDayProduciton = Math.Round(planValue / 10000, 0),
                            DayProduciton = currentDayProjectPrduction == null ? 0 : Math.Round(currentDayProjectPrduction.DayActualProductionAmount / 10000, 0),
                            CompleteDayProducitonRate = rate,
                            DayProductionIntensityDesc = currentDayProjectPrduction == null ? null : currentDayProjectPrduction.LowProductionReason
                        });
                    }
                }
            }
            projectBasePoduction.ProjectIntensities = projectIntensities.Where(x => x.PlanDayProduciton > 0).OrderBy(x => x.CompleteDayProducitonRate).ToList();
            #endregion

            #endregion

            #region 自有船施工情况  自有船运转以及产值情况
            //三种船舶的shiid集合
            List<Guid> allShipIds = new List<Guid>();
            //计算船舶填报率和船舶未填报统计使用 其他无使用此集合（此集合就是已填报的船舶会记录shiid）
            List<Guid> shipIds = new List<Guid>();
            //需要更新在场天数的一个合计
            List<ShipOnDay> keyValuePairs = new List<ShipOnDay>();
            var ownerShipBuildInfos = new List<CompanyShipBuildInfo>();
            var companyShipProductionValueInfo = new List<CompanyShipProductionValueInfo>();
            //三类船舶类型集合
            var shipTypeIds = CommonData.ShipType.SplitStr(",").Select(x => x.ToGuid()).ToList();
            //三类船舶的数据
            var shipList = await dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1
                  && shipTypeIds.Contains(x.TypeId.Value)).ToListAsync();
            //船舶日报相关(施工  调遣  待命  检修)
            var shipDayList = await dbContext.Queryable<ShipDayReport>()
               .Where(x => x.IsDelete == 1
               && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
               ).ToListAsync();
            OwnerShipBuildInfo ownerShipBuildInfo = null;
            if (shipList != null && shipList.Any())
            {
                allShipIds = shipList.Select(x => x.PomId).ToList();
                companyList = commonDataList.Where(x => x.Type == 2).OrderBy(x => x.Sort).ToList();
                foreach (var item in companyList)
                {
                    //当前船舶的类型数量
                    var currentCompanyShipCount = shipList.Where(x => x.TypeId == item.ItemId).Count();
                    //当前船舶施工数量
                    var constructionShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Construction).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(constructionShipIds);
                    var constructionShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Construction).Count(); //shipList.Where(x => constructionShipIds.Contains(x.PomId)&&x.TypeId == item.ItemId.Value).Count();
                    //当前船舶修理数量
                    var repairShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Repair).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(repairShipIds);
                    var repairShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Repair).Count(); //shipList.Where(x => repairShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前船舶调遣数量
                    var dispatchShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Dispatch).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(dispatchShipIds);
                    var dispatchShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Dispatch).Count();//shipList.Where(x => dispatchShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前船舶待命数量
                    var standbyShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Standby).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(standbyShipIds);
                    var standbyShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Standby).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前舶航修数量
                    var voyageRepairIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.VoyageRepair).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(voyageRepairIds);
                    var voyageRepairCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.VoyageRepair).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前检修数量
                    var overHaulIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.OverHaul).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(overHaulIds);
                    var overHaulCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.OverHaul).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //开工率
                    var buildPercent = 0M;
                    if (currentCompanyShipCount != 0)
                    {
                        //明天的在场天数
                        var nextDayCount = item.OnDayCount + constructionShipCount;
                        //记录需要更新的id和值
                        keyValuePairs.Add(new ShipOnDay() { Id = item.Id, OnDayCount = nextDayCount });
                        buildPercent = Math.Round((((decimal)(constructionShipCount)) / currentCompanyShipCount) * 100, 2);
                    }

                    #region 自有船施工情况  自有船运转产值情况

                    var allShipDayReportIds = shipList.Where(x => x.TypeId == item.ItemId).Select(x => x.PomId).ToList();
                    //当日产值
                    var dayConstructionShipPrudctionValue = shipDayList.Where(x => x.DateDay == currentTimeInt
                    && allShipDayReportIds.Contains(x.ShipId)).Select(x => x.EstimatedOutputAmount).Sum();
                    //当日运转小时
                    foreach (var key in shipDayList)
                    {
                        key.Dredge = key.Dredge.HasValue == false ? 0M : key.Dredge.Value;
                        key.Sail = key.Sail == null ? 0M : key.Sail.Value;
                        key.BlowingWater = key.BlowingWater == null ? 0M : key.BlowingWater.Value;
                        key.BlowShore = key.BlowShore == null ? 0M : key.BlowShore.Value;
                        key.SedimentDisposal = key.SedimentDisposal == null ? 0M : key.SedimentDisposal.Value;
                    }
                    var dayShipTurnHours = shipDayList.Where(x => x.DateDay == currentTimeInt
                    && allShipDayReportIds.Contains(x.ShipId))
                       .Sum(x => x.Dredge.Value + x.Sail.Value + x.BlowingWater.Value + x.BlowShore.Value + x.SedimentDisposal.Value);
                    //年累计产值 (含基础数据)
                    var yearConstructionShipPrudctionValue = shipDayList.Where(x =>
                   allShipDayReportIds.Contains(x.ShipId)).Select(x => x.EstimatedOutputAmount).Sum(); //+item.YearProductionValue;
                    //当年累计运转小时
                    var yearShipTurnHours = shipDayList.Where(x =>
                   allShipDayReportIds.Contains(x.ShipId))
                        .Sum(x => x.Dredge.Value + x.Sail.Value + x.BlowingWater.Value + x.BlowShore.Value + x.SedimentDisposal.Value);
                    //+ item.TurnHours;
                    //时间利用率年累计运转小时/在场天数累计/24
                    var TimePercent = 0M;
                    if (currentCompanyShipCount != 0)
                    {
                        var ondayCount = item.OnDayCount + constructionShipCount;


                        TimePercent = Math.Round(yearShipTurnHours / ondayCount / 24M * 100, 2);
                    }
                    #endregion

                    if (item.Collect == 0)
                    {
                        ownerShipBuildInfos.Add(new CompanyShipBuildInfo()
                        {
                            Name = item.Name,
                            AssignCount = dispatchShipCount,
                            AwaitCount = standbyShipCount,
                            BuildCount = constructionShipCount,
                            ReconditionCount = repairShipCount + voyageRepairCount + overHaulCount,
                            BuildPercent = buildPercent,
                            Count = currentCompanyShipCount

                        });
                        companyShipProductionValueInfo.Add(new CompanyShipProductionValueInfo()
                        {
                            DayTurnHours = dayShipTurnHours,
                            Name = item.Name,
                            YearTotalProductionValue = Math.Round((((decimal)yearConstructionShipPrudctionValue.Value) / 100000000), 2),
                            YearTotalTurnHours = yearShipTurnHours,
                            DayProductionValue = Math.Round(((decimal)dayConstructionShipPrudctionValue.Value) / 10000, 2),
                            TimePercent = TimePercent

                        });
                    }
                    else
                    {
                        //合计
                        var totalShipCount = ownerShipBuildInfos.Sum(x => x.Count);
                        var totalBuildShipCount = ownerShipBuildInfos.Sum(x => x.BuildCount);
                        var totalAwaitShipCount = ownerShipBuildInfos.Sum(x => x.AwaitCount);
                        var totalAssignShipCount = ownerShipBuildInfos.Sum(x => x.AssignCount);
                        var totalReconditShipCount = ownerShipBuildInfos.Sum(x => x.ReconditionCount);
                        var totalBuildShipPercent = ownerShipBuildInfos.Sum(x => x.BuildPercent);
                        //产值合计
                        var totalDayShipProductionValue = companyShipProductionValueInfo.Sum(x => x.DayProductionValue);
                        var totalDayShipTurnHours = companyShipProductionValueInfo.Sum(x => x.DayTurnHours);
                        //var totalDayTimePercent = companyShipProductionValueInfo.Sum(x => x.TimePercent);
                        var totalYearProductionValue = companyShipProductionValueInfo.Sum(x => x.YearTotalProductionValue);
                        var totalYearTurnHours = companyShipProductionValueInfo.Sum(x => x.YearTotalTurnHours);
                        //合计时间利用率
                        var totalDayTimePercent = 0M;
                        if (item.OnDayCount != 0)
                        {
                            var totalOnDayCount = keyValuePairs.Sum(x => x.OnDayCount);
                            totalDayTimePercent = Math.Round(totalYearTurnHours / totalOnDayCount / 24M * 100, 2);
                        }

                        //开工率
                        var BuildPercent = Math.Round(((decimal)totalBuildShipCount) / totalShipCount * 100, 2);
                        ownerShipBuildInfos.Add(new CompanyShipBuildInfo()
                        {
                            Name = item.Name,
                            AssignCount = totalAssignShipCount,
                            AwaitCount = totalAwaitShipCount,
                            BuildCount = totalBuildShipCount,
                            ReconditionCount = totalReconditShipCount,
                            BuildPercent = BuildPercent,
                            Count = totalShipCount,
                        });
                        companyShipProductionValueInfo.Add(new CompanyShipProductionValueInfo()
                        {
                            DayTurnHours = totalDayShipTurnHours,
                            Name = item.Name,
                            YearTotalProductionValue = totalYearProductionValue,
                            YearTotalTurnHours = totalYearTurnHours,
                            DayProductionValue = totalDayShipProductionValue,
                            TimePercent = totalDayTimePercent

                        });
                        ownerShipBuildInfo = new OwnerShipBuildInfo()
                        {
                            BuildCount = totalBuildShipCount,
                            AwaitCount = totalAwaitShipCount,
                            AssignCount = totalAssignShipCount,
                            ReconditionCount = totalReconditShipCount,
                            TotalCount = totalShipCount,
                            BulidProductionValue = totalDayShipProductionValue,
                            DayTurnHours = totalDayShipTurnHours,
                            YearTotalProductionValue = totalYearProductionValue,
                            YearTotalTurnHours = totalYearTurnHours,
                            BuildPercent = BuildPercent,
                            companyShipBuildInfos = ownerShipBuildInfos,
                            companyShipProductionValueInfos = companyShipProductionValueInfo
                        };
                        jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo = ownerShipBuildInfo;
                    }
                }
                #region 更新船舶在场天数
                if (keyValuePairs.Any())
                {
                    var ids = keyValuePairs.Select(x => x.Id).ToList();
                    var updateStartDay = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    var updateEndDay = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                    //判断需要更新不 
                    var updateData = commonDataList.Where(x => ids.Contains(x.Id)
                     && x.UpdateTime >= SqlFunc.ToDate(updateStartDay)
                     && x.UpdateTime <= SqlFunc.ToDate(updateEndDay)).ToList();
                    if (updateData.Count == 0)
                    {
                        var entitysChange = commonDataList.Where(x => ids.Contains(x.Id)).ToList();
                        foreach (var item in entitysChange)
                        {
                            var singleEntity = keyValuePairs.SingleOrDefault(x => x.Id == item.Id);
                            if (singleEntity != null)
                            {
                                item.OnDayCount = singleEntity.OnDayCount;
                            }
                        }
                        await dbContext.Updateable<ProductionMonitoringOperationDayReport>(entitysChange).ExecuteCommandAsync();
                    }
                }
                #endregion
            }
            #endregion

            #region  施工船舶产值强度低于80%


            #endregion

            #region 自有船舶排行前五的产值
            List<ShipProductionValue> shipProductionValue = new List<ShipProductionValue>();
            //获取船舶ids  EstimatedUnitPrice
            var ownShipIds = shipList.Select(x => x.PomId).ToList();
            //  前五船舶 -年产值(todo 加入历史数据)
            var yearQuery = await dbContext.Queryable<ShipDayReport>()
                  .Where(x => x.IsDelete == 1
                  //&& x.ShipDayReportType == ShipDayReportType.ProjectShip
                  && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
                  ).ToListAsync();
            //
            //var shipDaysList=
            //    await dbContext.Queryable<ShipDayReport>().Where(x => x.IsDelete == 1
            //    && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
            //    ).ToListAsync();
            //自有船舶历史数据
            var histotyShipList = await dbContext.Queryable<OwnerShipHistory>()
                 .Where(x => x.IsDelete == 1 && x.Year == 2024).OrderBy(x => x.Sort)
                  .ToListAsync();
            foreach (var item in histotyShipList)
            {
                //当日产值
                var onDayCount = yearQuery.Where(x => x.ShipId == item.Id
                ).Count();

                //当日产值
                var dayValue = yearQuery.Where(x => x.ShipId == item.Id && x.DateDay == currentTimeInt).Sum(x => x.EstimatedOutputAmount.Value);
                //当年产值
                var yearValue = yearQuery.Where(x => x.ShipId == item.Id).Sum(x => x.EstimatedOutputAmount.Value);
                //当年运转小时
                var yearHoursValue = yearQuery.Where(x => x.ShipId == item.Id)
                .Select(t => new
                {
                    a = t.Dredge ?? 0,
                    b = t.Sail ?? 0,
                    c = t.BlowingWater ?? 0,
                    d = t.SedimentDisposal ?? 0,
                    e = t.BlowShore ?? 0
                }).ToList();
                var totalHoursValue = yearHoursValue.Select(x => x.a + x.b + x.c + x.d + x.e).ToList().Sum();
                var obj = new ShipProductionValue()
                {
                    ShipName = item.Name,
                    ShipDayOutput = Math.Round(dayValue / 10000, 2),
                    //ShipYearOutput = Math.Round(yearValue/100000000,2) + item.YearShipHistory,
                    ShipYearOutput = Math.Round(yearValue / 100000000, 2),// + item.YearShipHistory,
                    WorkingHours = totalHoursValue,// + item.WorkingHours.Value,
                    //ConstructionDays = onDayCount+ item.OnDay.Value,
                    ConstructionDays = onDayCount,

                };
                if (obj.ConstructionDays != 0)
                    obj.TimePercent = Math.Round((obj.WorkingHours / obj.ConstructionDays / 24) * 100, 2);
                shipProductionValue.Add(obj);

            }
            projectBasePoduction.YearTopFiveTotalOutput = shipProductionValue.Sum(x => x.ShipDayOutput.Value);
            projectBasePoduction.YearFiveTotalOutput = shipProductionValue.Sum(x => x.ShipYearOutput.Value);
            var totalOnDay = shipProductionValue.Sum(x => x.ConstructionDays);
            if (totalOnDay != 0)
            {
                projectBasePoduction.YearTotalTopFiveOutputPercent = Math.Round((shipProductionValue.Sum(x => x.WorkingHours) / totalOnDay / 24) * 100, 2);
            }
            if (ownerShipBuildInfo.YearTotalProductionValue != 0)
            {
                projectBasePoduction.YearFiveTimeRate = Math.Round(
                    (projectBasePoduction.YearFiveTotalOutput /
                    ownerShipBuildInfo.YearTotalProductionValue) * 100, 2);

            }
            jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.companyShipTopFiveInfoList = shipProductionValue;

            #endregion

            #region 特殊情况
            var specialProjectList = new List<SpecialProjectInfo>();
            var dayRepNoticeData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && (x.IsHaveProductionWarning == 1 || x.IsHaveProductionWarning == 2 || x.IsHaveProductionWarning == 3))
                .Select(x => new { x.IsHaveProductionWarning, x.ProductionWarningContent, x.ProjectId }).OrderByDescending(x => x.IsHaveProductionWarning).ToListAsync();
            dayRepNoticeData.ForEach(x => specialProjectList.Add(new SpecialProjectInfo
            {
                ProjectId = x.ProjectId,
                Type = x.IsHaveProductionWarning,
                Description = x.ProductionWarningContent
            }));
            var pIds = dayRepNoticeData.Select(x => x.ProjectId).ToList();
            var sourceProjectList = companyProjectList.Where(x => pIds.Contains(x.Id)).ToList();
            foreach (var item in specialProjectList)
            {
                var projectInfo = sourceProjectList.Where(x => x.Id == item.ProjectId).FirstOrDefault();
                item.SourceMatter = projectInfo?.ShortName;
            }
            jjtSendMessageMonitoringDayReportResponseDto.SpecialProjectInfo = specialProjectList;
            #endregion

            #region 各单位填报情况(数据质量)
            //未填报项目的IDS
            List<Guid> unWriteReportIds = new List<Guid>();

            #region 各单位产值日报填报率情况
            List<CompanyWriteReportInfo> companyWriteReportInfos = new List<CompanyWriteReportInfo>();
            //所有已填报的项目
            var writeReportList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where((x, y) => x.IsDelete == 1 && onBuildProjectIds.Contains(x.ProjectId) && x.DateDay == currentTimeInt)
                .Select((x, y) => new JjtProjectDayReport() { ProjectId = x.Id, CompanyId = y.CompanyId.Value, DateDay = x.DateDay })
                .ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //当前公司已填报的数量
                var currentDayUnReportCount = writeReportList.Where(x => x.CompanyId == item.ItemId).Count();

                //填报率
                var writeReportPercent = 0M;
                if (currentCompany != 0)
                {
                    writeReportPercent = Math.Round(((decimal)currentDayUnReportCount / currentCompany) * 100, 2);
                }

                if (item.Collect == 0)
                {
                    companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    {
                        Name = item.Name,
                        OnBulidCount = currentCompany,
                        UnReportCount = currentCompany - currentDayUnReportCount,
                        WritePercent = writeReportPercent,
                        QualityLevel = 0,
                        ProjectId = item.Id
                    });
                }
                else
                {
                    //在建项目合计
                    var totalBuildCount = companyWriteReportInfos.Sum(x => x.OnBulidCount);
                    var totalUnReportCount = companyWriteReportInfos.Sum(x => x.UnReportCount);
                    var totalWritePercent = 0M;
                    if (totalBuildCount != 0)
                    {
                        totalWritePercent = Math.Round(((decimal)(totalBuildCount - totalUnReportCount)) / totalBuildCount * 100, 2);
                    }

                    companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    {
                        Name = item.Name,
                        OnBulidCount = totalBuildCount,
                        UnReportCount = totalUnReportCount,
                        WritePercent = totalWritePercent,
                    });
                }
            }
            //数据质量程度 几颗星（//船舶填报率 待命填报率+调遣填报率+修理填报率+施工填报率）
            //评分 1：一颗星[0 - 30) 2:两颗星[30 - 60) 3:三颗星[60 - 80) 4:四颗星[80 - 90) 5:五颗星[90 - 100)
            /// 计算公式：（项目当日产值/3300*50%+船舶当日产值/490*25%+项目填报率*20%+船舶填报率*5%）*100
            /// 计算船舶填报率
            var shipPercent = 0M;
            var reportShipCount = shipDayList.Where(x => x.DateDay == currentTimeInt && shipIds.Contains(x.ShipId)).Count();
            var tatalShipCount = jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.TotalCount;
            if (tatalShipCount != 0)
            {
                shipPercent = ((decimal)reportShipCount) / tatalShipCount;
            }
            //计算星星的数据质量程度
            var qualityLevel = ((jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.DayProductionValue / 3300M) * 50 / 100 +
            (jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.BulidProductionValue / 490M) * 25 / 100 +
            (companyWriteReportInfos[8].WritePercent / 100M * 20 / 100) +
            shipPercent * 5 / 100M) * 100;

            var star = 0;
            if (qualityLevel <= 30)
            {
                star = 1;
            }
            else if (qualityLevel > 30 && qualityLevel <= 60)
            {
                star = 2;
            }
            else if (qualityLevel > 60 && qualityLevel <= 80)
            {
                star = 3;
            }
            else if (qualityLevel > 80 && qualityLevel <= 90)
            {
                star = 4;
            }
            else if (qualityLevel > 90)
            {
                star = 5;
            }
            jjtSendMessageMonitoringDayReportResponseDto.QualityLevel = star;
            companyWriteReportInfos = companyWriteReportInfos.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
            jjtSendMessageMonitoringDayReportResponseDto.CompanyWriteReportInfos = companyWriteReportInfos;

            #endregion

            #region 说明：项目生产数据存在不完整部分主要是以下项目未填报
            List<CompanyUnWriteReportInfo> companyUnWriteReportInfos = new List<CompanyUnWriteReportInfo>();
            //统计本周期内已填报的日报
            var endTimes = endTime.ObjToDate().AddDays(1);
            var writeCompanyReportList = await dbContext.Queryable<DayReport>()
             .Where(x => x.IsDelete == 1
              && x.CreateTime >= SqlFunc.ToDate(startTime) && x.CreateTime <= SqlFunc.ToDate(endTimes)
              && x.DateDay >= currentTimeIntUp && x.DateDay <= currentTimeInt
              && (x.UpdateTime == null || x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTimes)))
             .ToListAsync();
            //查询项目信息
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && onBuildProjectIds.Contains(x.Id)).ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            var distinctOnBuildProjects = onBuildProjectIds.Distinct();
            //查询符合范围内的数据
            var projectStatusChangeRecordList = await dbContext.Queryable<ProjectStatusChangeRecord>()
                .Where(x => x.NewStatus == buildProjectId && (x.ChangeTime >= SqlFunc.ToDate(startTime) && x.ChangeTime <= SqlFunc.ToDate(endTime)))
                .ToListAsync();
            //特殊项
            //var sIds = projectStatusChangeRecordList.Where(x => x.IsValid == 0).Select(x => x.Id).Distinct().ToList();
            //排除掉不满足的条件 得到满足的条件
            var satisfyIds = projectStatusChangeRecordList.Where(x => x.IsValid == 1).Select(x => x.Id).ToList();
            onBuildProjectIds = onBuildProjectIds.Where(x => satisfyIds.Contains(x)).ToList();
            foreach (var item in onBuildProjectIds)
            {
                //if (item != "08dbc93a-4536-4ee3-83f2-d63866bbdd1e".ToGuid())
                //    continue;

                //查询当前项目什么时间变更状态的(变更时间就是当前填写日报的时间)
                var currentProjectStatusChangeTime = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                     .Select(x => x.ChangeTime)
                     .FirstOrDefault();
                //当前项目在本周期范围内停了多少天
                var projectStopDay = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                .Select(x => x.StopDay)
                .FirstOrDefault();
                //当前项目本周期需要填写的数量
                var changeTimeInt = int.Parse(currentProjectStatusChangeTime.ToString("dd"));
                //计算当前项目需要填写的日报的数量
                var currentWriteReportCount = 0;
                if (changeTimeInt >= 26)
                {
                    currentWriteReportCount = days - (changeTimeInt - 1) + 26;
                }
                else
                {
                    //currentWriteReportCount = days - (((days - 26)) + changeTimeInt - 1);
                    currentWriteReportCount = days - (((days - 26)) + changeTimeInt);
                }
                //未过天数
                var unDays = days - ofdays;
                //已填报数量
                var dayReportCount = writeCompanyReportList.Where(x => x.ProjectId == item).Count();
                //未填报数量
                var unReportCount = (days - projectStopDay.Value - unDays) - dayReportCount;
                if (unReportCount <= 0)
                {
                    unReportCount = 0;
                }
                //ofdays - dayReportCount<= 0 ? 0 : ofdays - dayReportCount- passedTime;
                //当前项目信息
                var currentProjectInfo = projectList.SingleOrDefault(x => x.Id == item);
                //业主单位
                var companyInfo = companyList.SingleOrDefault(x => x.ItemId == currentProjectInfo.CompanyId && x.Collect == 0);

                if (unReportCount != 0)
                {
                    companyUnWriteReportInfos.Add(new CompanyUnWriteReportInfo()
                    {
                        ProjectName = currentProjectInfo.Name,
                        Name = companyInfo.Name,
                        Count = unReportCount
                    });
                }
            }
            if (jjtSendMessageMonitoringDayReportResponseDto != null)
            {

                jjtSendMessageMonitoringDayReportResponseDto.CompanyUnWriteReportInfos = companyUnWriteReportInfos
                    .OrderByDescending(x => x.Count).ToList();
            }
            #endregion

            #region 说明：船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            List<CompanyShipUnWriteReportInfo> companyShipUnWriteReportInfos = new List<CompanyShipUnWriteReportInfo>();
            //未填写船舶日报的ids集合
            var unReportShipIds = allShipIds.Where(x => !shipIds.Contains(x)).ToList();
            if (unReportShipIds != null && unReportShipIds.Any())
            {
                //查询每个项目上的船舶信息
                var writeReportShipList = await dbContext.Queryable<ShipMovement>()
                   .Where(x => x.IsDelete == 1
                           && unReportShipIds.Contains(x.ShipId)
                           && x.Status == ShipMovementStatus.Enter
                           && x.ShipType == ShipType.OwnerShip
                        )
                   .ToListAsync();
                if (writeReportShipList != null && writeReportShipList.Any())
                {
                    foreach (var item in unReportShipIds)
                    {
                        var singleProject = writeReportShipList.FirstOrDefault(x => x.ShipId == item);
                        if (singleProject != null)
                        {
                            //船舶信息
                            var shipInfo = shipList.FirstOrDefault(x => x.PomId == item);
                            if (shipInfo != null)
                            {
                                //项目信息
                                var projectInfo = companyProjectList.FirstOrDefault(x => x.Id == singleProject.ProjectId);
                                if (projectInfo != null)
                                {
                                    companyShipUnWriteReportInfos.Add(new CompanyShipUnWriteReportInfo()
                                    {
                                        ShipName = shipInfo?.Name,
                                        OnProjectName = projectInfo?.Name,
                                    });
                                }
                            }
                            else
                            {
                                companyShipUnWriteReportInfos.Add(new CompanyShipUnWriteReportInfo()
                                {
                                    ShipName = shipInfo?.Name,
                                });
                            }
                        }
                    }
                }
            }
            if (jjtSendMessageMonitoringDayReportResponseDto != null)
            {
                jjtSendMessageMonitoringDayReportResponseDto.CompanyShipUnWriteReportInfos = companyShipUnWriteReportInfos;
            }
            #endregion


            #endregion

            #region 各单位计划、完成产值对比

            #region 全局维度
            List<EachCompanyProductionValue> eachCompanyProductionValues = new List<EachCompanyProductionValue>();
            var length = 15;//查询半月的时间
            var currentNowTimeInt = 0;
            #region 
            //获取项目产值计划表数据
            var monthPlanRepData = await dbContext.Queryable<CompanyProductionValueInfo>().Where(x => x.IsDelete == 1).ToListAsync();
            //获取项目日报产值数据
            var dayRepData = await dbContext.Queryable<DayReport>().Where(t => t.IsDelete == 1).ToListAsync();
            #endregion
            for (int i = 1; i <= length; i++)
            {
                //currentNowTimeInt = int.Parse(DateTime.Now.ToString("yyyyMMdd")) - (i - 1);
                currentNowTimeInt = int.Parse(DateTime.Now.AddDays(-i).ToString("yyyyMMdd"));
                //判断月份
                var monthInt = Utils.GetMonth(currentNowTimeInt);
                var yearInt = Utils.GetYear(currentNowTimeInt);
                //var dayActualProductionAmount = dayProductionValueList.Where(x => x.DateDay == (currentNowTimeInt - 1)).Sum(x => x.DayActualProductionAmount);
                var dayActualProductionAmount = dayRepData.Where(x => x.DateDay == currentNowTimeInt).Sum(x => x.DayActualProductionAmount);
                var dayPlanProAmount = GetProjectPlanAmount(monthPlanRepData, yearInt, monthInt);
                eachCompanyProductionValues.Add(new EachCompanyProductionValue()
                {
                    XAxle = currentNowTimeInt,
                    YAxlePlanValue = Math.Round(dayPlanProAmount / 3000000000M, 2),
                    YAxleCompleteValue = Math.Round(dayActualProductionAmount / 100000000M, 2)
                    //YAxlePlanValue = Math.Round((GetProductionValueInfo(monthInt, companyProductionList).Sum(x => x.PlanProductionValue) / 300000M), 2),
                    //YAxleCompleteValue = Math.Round(dayActualProductionAmount / 100000000M, 2)
                });
            }
            #endregion

            #region 已公司维度
            //var companyInfoList = companyList.Where(x =>!SqlFunc.IsNullOrEmpty(x.Name) &&x.Name != "广航局总体").ToList();
            //List<EachCompanyProductionValue> companyEachCompanyProductionValues = new List<EachCompanyProductionValue>();
            //for (int i = 1; i <= length; i++)
            //{
            //    foreach (var item in companyInfoList)
            //    {
            //        var currentMonthCompanyProductionValue = companyMonthProductionValue.Where(x => x.Id == item.ItemId).FirstOrDefault();
            //        currentNowTimeInt = int.Parse(DateTime.Now.ToString("yyyyMMdd")) - (i - 1);
            //        //判断月份
            //        var monthInt = Utils.GetMonth(currentNowTimeInt);
            //        var dayActualProductionAmount = dayProductionValueList.Where(x => x.DateDay == (currentNowTimeInt - 1)).Sum(x => x.DayActualProductionAmount);
            //        companyEachCompanyProductionValues.Add(new EachCompanyProductionValue()
            //        {
            //            ConpanyName = item.Name,
            //            XAxle = currentNowTimeInt,
            //            YAxlePlanValue = Math.Round((GetProductionValueInfo(monthInt, companyProductionList).Where(x => x.Id
            //            == item.ItemId).Sum(x => x.PlanProductionValue) / 30M), 2),
            //            YAxleCompleteValue = Math.Round(dayActualProductionAmount, 2)
            //        });
            //    }


            //}
            //eachCompanyProductionValues.AddRange(companyEachCompanyProductionValues);
            #endregion

            jjtSendMessageMonitoringDayReportResponseDto.EachCompanyProductionValue = eachCompanyProductionValues;
            #endregion

            jjtSendMessageMonitoringDayReportResponseDto.Month = month;
            jjtSendMessageMonitoringDayReportResponseDto.Year = int.Parse(yearStartTime);
            responseAjaxResult.Data = jjtSendMessageMonitoringDayReportResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion
        }
        /// <summary>
        /// 获取计划产值
        /// </summary>
        /// <param name="pPlanPro"></param>
        /// <param name="yearInt"></param>
        /// <param name="monthInt"></param>
        /// <returns></returns>
        public decimal GetProjectPlanAmount(List<CompanyProductionValueInfo> pPlanPro, int yearInt, int monthInt)
        {
            pPlanPro = pPlanPro.Where(x => x.DateDay == yearInt).ToList();
            decimal planProAmount = 0M;
            switch (monthInt)
            {
                case 1:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.OnePlanProductionValue));
                    break;
                case 2:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.TwoPlanProductionValue));
                    break;
                case 3:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.ThreePlaProductionValue));
                    break;
                case 4:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.FourPlaProductionValue));
                    break;
                case 5:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.FivePlaProductionValue));
                    break;
                case 6:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.SixPlaProductionValue));
                    break;
                case 7:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.SevenPlaProductionValue));
                    break;
                case 8:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.EightPlaProductionValue));
                    break;
                case 9:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.NinePlaProductionValue));
                    break;
                case 10:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.TenPlaProductionValue));
                    break;
                case 11:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.ElevenPlaProductionValue));
                    break;
                case 12:
                    planProAmount = pPlanPro.Sum(x => Convert.ToDecimal(x.TwelvePlaProductionValue));
                    break;
            }
            return planProAmount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtTextCardMsgDetailsAsync(DateTime date)
        {
            #region 111
            var responseAjaxResult = new ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>();
            var result = await dbContext.Queryable<TempTable>().FirstAsync();
            if (result != null && !string.IsNullOrWhiteSpace(result.Value))
            {
                return JsonConvert.DeserializeObject<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>>(result.Value);
            }
            //在建项目的IDs
            List<Guid> onBuildProjectIds = new List<Guid>();
            var jjtSendMessageMonitoringDayReportResponseDto = new JjtSendMessageMonitoringDayReportResponseDto()
            {
                DayTime = date.AddDays(-1).ToString("MM月dd日")
            };
            #region 查询条件相关

            //周期开始时间
            var startTime = string.Empty;
            if (date.Day >= 27)
            {
                startTime = date.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = date.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            //周期结束时间
            var endTime = Convert.ToDateTime(startTime).AddMonths(1).ToString("yyyy-MM-25 23:59:59");
            //统计周期 上个月的26号到本月的25号之间为一个周期
            //当前时间上限int类型
            var currentTimeIntUp = int.Parse(Convert.ToDateTime(startTime).ToString("yyyyMM26"));
            //当前时间下限int类型
            var currentTimeInt = date.AddDays(-1).ToDateDay();
            //本年的月份
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            //本年的年份 
            var yearStartTime = date.Year.ToString();
            //年累计开始时间（每年的开始时间）
            var startYearTimeInt = int.Parse(date.AddYears(-1).ToString("yyyy") + "1226");//int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1226"));
            //年累计结束时间
            var endYearTimeInt = int.Parse(date.ToString("yyyyMMdd")) > 1226 && int.Parse(date.ToString("yyyyMMdd")) <= 31 ? int.Parse(date.AddYears(1).ToString("yyyy1225")) : int.Parse(date.ToString("yyyy1225")); //int.Parse(DateTime.Now.ToString("yyyy1225"));
                                                                                                                                                                                                                      //每月多少天
                                                                                                                                                                                                                      // int days = DateTime.DaysInMonth(int.Parse(endYearTimeInt.ToString().Substring(0, 4)), month);  //DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
            int days = TimeHelper.GetTimeSpan(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime)).Days + 1;
            //已过多少天
            var ofdays = date.Day <= 26 ? (date.Day + ((days - 26))) : date.Day - 26;
            //今年已过多少天
            var dayOfYear = 0;
            //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1231")))
            if (int.Parse(date.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(date.ToString("yyyyMMdd")) < int.Parse(yearStartTime + "1231"))
            {
                dayOfYear = ofdays;
            }
            else
            {
                //这个6天是上一年1226-1231之间的天数
                dayOfYear = date.DayOfYear + 6;
            }
            #endregion

            #region 共用数据
            var commonDataList = await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1).ToListAsync();
            var comonDataProductionList = await dbContext.Queryable<CompanyProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.DateDay == date.Year).ToListAsync();
            #endregion

            #region 项目总体生产情况
            //在建项目的所有IDS


            #region 广航局合同项目基本信息
            ProjectBasePoduction projectBasePoduction = null;
            List<CompanyProjectBasePoduction> companyProjectBasePoductions = new List<CompanyProjectBasePoduction>();
            //合同项目状态ids集合
            var contractProjectStatusIds = CommonData.BuildIds.SplitStr(",").Select(x => x.ToGuid()).ToList();
            //项目类型为其他非施工类业务 排除
            var noConstrutionProject = CommonData.NoConstrutionProjectType;
            //在建项目状态ID
            var buildProjectId = CommonData.PConstruc.ToGuid();
            //停缓建Ids
            var stopProjectIds = CommonData.PSuspend.Split(",").Select(x => x.ToGuid()).ToList();
            //未开工状态
            var notWorkIds = CommonData.NotWorkStatusIds.Split(",").Select(x => x.ToGuid()).ToList();
            //各个公司的项目信息
            var companyProjectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && contractProjectStatusIds.Contains(x.StatusId.Value)
            && x.TypeId != noConstrutionProject).ToListAsync();
            //取出相关日报信息(当天项目日报)
            var currentDayProjectList = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && x.ProcessStatus == DayReportProcessStatus.Submited)
                  .ToListAsync();
            //公共数据取出项目相关信息
            var companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //在建项目IDS
                var currentCompanyIds = companyProjectList.Where(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId)
                    .Select(x => x.Id).ToList();
                if (item.Collect == 0)
                {
                    onBuildProjectIds.AddRange(currentCompanyIds);
                }
                //合同项目数
                var currentCompanyCount = companyProjectList.Count(x => x.CompanyId == item.ItemId);
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //停缓建项目数
                var stopProjectCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && stopProjectIds.Contains(x.StatusId.Value));
                //未开工的项目数量
                var notWorkCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && notWorkIds.Contains(x.StatusId.Value));
                //当前合同项目的所有ids
                var dayIds = companyProjectList.Where(x => x.CompanyId == item.ItemId).Select(x => x.Id).ToList();
                //设备数量
                var facilityCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.ConstructionDeviceNum).Sum();
                //线程施工人数量
                var workerCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.SiteConstructionPersonNum).Sum();
                //危大工程项数量
                var riskWorkCountCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.HazardousConstructionNum).Sum();

                if (item.Collect == 0)
                {

                    companyProjectBasePoductions.Add(new CompanyProjectBasePoduction()
                    {
                        Name = item.Name,
                        OnContractProjectCount = currentCompanyCount,
                        OnBuildProjectCount = currentCompany,
                        StopBuildProjectCount = stopProjectCount,
                        BuildCountPercent = currentCompanyCount == 0M ? 0M : Math.Round((((decimal)(currentCompany)) / currentCompanyCount) * 100, 2),
                        FacilityCount = facilityCount,
                        WorkerCount = workerCount,
                        RiskWorkCount = riskWorkCountCount,
                        NotWorkCount = notWorkCount,
                    });
                }
                else
                {

                    var totalContractProjectCount = companyProjectBasePoductions.Sum(x => x.OnContractProjectCount);
                    var totalOnBuildProjectCount = companyProjectBasePoductions.Sum(x => x.OnBuildProjectCount);
                    var totalStopBuildProjectCount = companyProjectBasePoductions.Sum(x => x.StopBuildProjectCount);
                    var totalNotWorkProjectCount = companyProjectBasePoductions.Sum(x => x.NotWorkCount);
                    var totalContractProjectPercent = totalContractProjectCount == 0M ? 0M : Math.Round(((decimal)totalOnBuildProjectCount / totalContractProjectCount) * 100, 2);
                    //设备  施工   危大
                    var totalFacilityCount = companyProjectBasePoductions.Sum(x => x.FacilityCount);
                    var totalWorkerCount = companyProjectBasePoductions.Sum(x => x.WorkerCount);
                    var totalRiskWorkCount = companyProjectBasePoductions.Sum(x => x.RiskWorkCount);


                    //汇总项
                    companyProjectBasePoductions.Add(new CompanyProjectBasePoduction()
                    {
                        Name = item.Name,
                        OnContractProjectCount = companyProjectList.Count,
                        OnBuildProjectCount = totalOnBuildProjectCount,
                        StopBuildProjectCount = totalStopBuildProjectCount,
                        NotWorkCount = totalNotWorkProjectCount,
                        BuildCountPercent = totalContractProjectPercent,
                        FacilityCount = totalFacilityCount,
                        RiskWorkCount = totalRiskWorkCount,
                        WorkerCount = totalWorkerCount
                    });
                    companyProjectBasePoductions = companyProjectBasePoductions.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
                    projectBasePoduction = new ProjectBasePoduction()
                    {
                        TotalOnContractProjectCount = companyProjectList.Count,
                        //TotalOnContractProjectCount = totalContractProjectCount,
                        TotalStopBuildProjectCount = totalStopBuildProjectCount,
                        TotalBuildCountPercent = totalContractProjectPercent,
                        TotalOnBuildProjectCount = totalOnBuildProjectCount,
                        TotalFacilityCount = totalFacilityCount,
                        TotalRiskWorkCount = totalRiskWorkCount,
                        TotalWorkerCount = totalWorkerCount,
                        CompanyProjectBasePoductions = companyProjectBasePoductions,
                        CompanyBasePoductionValues = new List<CompanyBasePoductionValue>()
                    };

                    jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction = projectBasePoduction;
                }

            }



            #endregion

            #region 广航局在建项目产值信息
            List<CompanyBasePoductionValue> companyBasePoductionValues = new List<CompanyBasePoductionValue>();
            //统计当年所有的项目日报信息
            var dayProductionValueList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where(x => x.IsDelete == 1
             //&& onBuildProjectIds.Contains(x.ProjectId)
             && (x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt))
                .Select((x, y) => new JjtProjectDayReport
                {
                    CompanyId = y.CompanyId.Value,
                    ProjectId = x.ProjectId,
                    DateDay = x.DateDay,
                    CreateTime = x.CreateTime.Value,
                    UpdateTime = x.UpdateTime.Value,
                    DayActualProductionAmount = x.DayActualProductionAmount
                }).ToListAsync();
            //广航局年累计产值(基础数据累加+几个公司的所有日产值)
            // var yearProductionValue = companyList.Sum(x => x.YearProductionValue) + dayProductionValueList.Sum(x => x.DayActualProductionAmount);
            var yearProductionValue = dayProductionValueList.Sum(x => x.DayActualProductionAmount);
            foreach (var item in companyList)
            {
                //当前公司日产值 || x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime))
                decimal currentCompanyCount = dayProductionValueList.Where(x => x.CompanyId == item.ItemId
                  && x.DateDay == currentTimeInt
                  //&& x.CreateTime >= SqlFunc.ToDate(startTime) && x.CreateTime <= SqlFunc.ToDate(endTime)
                  //|| x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime)
                  ).Sum(x => x.DayActualProductionAmount);
                //当前公司的累计产值
                var currentMonthCompanyCount = dayProductionValueList.Where(x => x.CompanyId == item.ItemId).Sum(x => x.DayActualProductionAmount);
                //年度产值占比 （广航局）
                //var yearProductionValuePercent = Math.Round(((decimal)(item.YearProductionValue + currentMonthCompanyCount) / yearProductionValue) * 100, 2);
                var yearProductionValuePercent = Math.Round(((decimal)(currentMonthCompanyCount) / yearProductionValue) * 100, 2);
                //较产值进度  计算公示如下
                //(年累计产值-（当前月-1）的计划产值+本月的计划产值/30.5*当前已过去多少天)/（当前月-1）的完成产值+本月的计划产值/30.5*当前已过去多少天)
                //累计完成产值
                var totalCompleteProductionValue = comonDataProductionList
                    .Where(x => x.CompanyId == item.ItemId)
                    .Sum(x => x.OnePlanProductionValue +
                 x.TwoPlanProductionValue +
                 x.ThreePlaProductionValue +
                 x.FourPlaProductionValue +
                 x.FivePlaProductionValue +
                 x.SixPlaProductionValue +
                 x.SevenPlaProductionValue +
                 x.EightPlaProductionValue +
                 x.NinePlaProductionValue +
                 x.TenPlaProductionValue +
                 x.ElevenPlaProductionValue +
                 x.TwelvePlaProductionValue);
                //本月计划产值
                var currentMonthPlanProductionValue = 0M;
                //本月计划产值汇总
                var currentTotalMonthPlanProductionValue = 0M;
                #region 查询当前月份计划产值
                //当前月份
                var currentMonth = date.Day <= 26 ? date.Month : date.AddMonths(1).Month;
                if (currentMonth == 1)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.OnePlanProductionValue.Value);
                };
                if (currentMonth == 2)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TwoPlanProductionValue.Value);
                };
                if (currentMonth == 3)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.ThreePlaProductionValue.Value);
                };
                if (currentMonth == 4)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.FourPlaProductionValue.Value);
                };
                if (currentMonth == 5)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.FivePlaProductionValue.Value);
                };
                if (currentMonth == 6)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.SixPlaProductionValue.Value);
                }
                if (currentMonth == 7)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.SevenPlaProductionValue.Value);
                };
                if (currentMonth == 8)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.EightPlaProductionValue.Value);
                };
                if (currentMonth == 9)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.NinePlaProductionValue.Value);
                };
                if (currentMonth == 10)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TenPlaProductionValue.Value);
                };
                if (currentMonth == 11)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.ElevenPlaProductionValue.Value);
                };
                if (currentMonth == 12)
                {
                    currentMonthPlanProductionValue = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).Sum(x => x.TwelvePlaProductionValue.Value);
                };

                #endregion
                var baseCalc = totalCompleteProductionValue - currentMonthPlanProductionValue + currentMonthPlanProductionValue / 30.5M * ofdays;
                //较产值进度
                var productionValueProgressPercent = 0M;
                if (baseCalc.Value != 0)
                {
                    //var yearCompanyProductionValue = item.YearProductionValue + currentMonthCompanyCount;
                    var yearCompanyProductionValue = currentMonthCompanyCount;
                    //productionValueProgressPercent = Math.Round((yearCompanyProductionValue -baseCalc.Value) / baseCalc.Value * 100, 2);
                    productionValueProgressPercent = Math.Round((baseCalc.Value) / baseCalc.Value * 100, 2);
                }
                //每年公司完成指标
                //var yearIndex = comonDataProductionList.Where(x => x.CompanyId == item.ItemId.Value).First().YearIndex;
                //序时进度
                var timeProgress = Math.Round((dayOfYear / 365M) * 100, 2);
                if (item.Collect == 0)
                {
                    //var totalYearProductionValue = Math.Round(((item.YearProductionValue + currentMonthCompanyCount) / 100000000), 2);
                    var totalYearProductionValue = Math.Round(((currentMonthCompanyCount) / 100000000), 2);
                    //超序时进度
                    //var supersequenceProgress = yearIndex.Value != 0 ? (Math.Round(totalYearProductionValue / yearIndex.Value, 2) * 100 - timeProgress) : 0;
                    companyBasePoductionValues.Add(new CompanyBasePoductionValue()
                    {
                        Name = item.Name,
                        DayProductionValue = Math.Round(currentCompanyCount / 10000, 2),
                        TotalYearProductionValue = totalYearProductionValue,
                        YearProductionValueProgressPercent = yearProductionValuePercent,
                        ProductionValueProgressPercent = productionValueProgressPercent,
                        //SupersequenceProgress = supersequenceProgress
                    });
                }
                else
                {
                    totalCompleteProductionValue = comonDataProductionList
                   .Sum(x => x.OnePlanProductionValue +
                x.TwoPlanProductionValue +
                x.ThreePlaProductionValue +
                x.FourPlaProductionValue +
                x.FivePlaProductionValue +
                x.SixPlaProductionValue +
                x.SevenPlaProductionValue +
                x.EightPlaProductionValue +
                x.NinePlaProductionValue +
                x.TenPlaProductionValue +
                x.ElevenPlaProductionValue +
                x.TwelvePlaProductionValue);
                    #region 查询汇总值
                    //查询汇总产值
                    if (currentMonth == 1)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.OnePlanProductionValue.Value);
                    };
                    if (currentMonth == 2)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TwoPlanProductionValue.Value);
                    };
                    if (currentMonth == 3)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.ThreePlaProductionValue.Value);
                    };
                    if (currentMonth == 4)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.FourPlaProductionValue.Value);
                    };
                    if (currentMonth == 5)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.FivePlaProductionValue.Value);
                    };
                    if (currentMonth == 6)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.SixPlaProductionValue.Value);
                    }
                    if (currentMonth == 7)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.SevenPlaProductionValue.Value);
                    };
                    if (currentMonth == 8)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.EightPlaProductionValue.Value);
                    };
                    if (currentMonth == 9)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.NinePlaProductionValue.Value);
                    };
                    if (currentMonth == 10)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TenPlaProductionValue.Value);
                    };
                    if (currentMonth == 11)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.ElevenPlaProductionValue.Value);
                    };
                    if (currentMonth == 12)
                    {
                        currentTotalMonthPlanProductionValue = comonDataProductionList.Sum(x => x.TwelvePlaProductionValue.Value);
                    };
                    #endregion

                    baseCalc = totalCompleteProductionValue - currentTotalMonthPlanProductionValue + currentTotalMonthPlanProductionValue / 30.5M * ofdays;
                    if (baseCalc.Value != 0)
                    {
                        var diffValue = ((yearProductionValue) - baseCalc);
                        productionValueProgressPercent = Math.Round(diffValue.Value / baseCalc.Value * 100, 2);
                    }

                    //当日产值
                    decimal dayTotalPoductionValues = companyBasePoductionValues.Sum(x => x.DayProductionValue);
                    //名称排除‘个’字
                    var filterStr = "(个)";
                    var name = item.Name.IndexOf(filterStr) >= 0 ? item.Name.Replace(filterStr, "").TrimAll() : item.Name;

                    #region 营业收入保障指数
                    //营业收入保障指数
                    var incomeSecurityLevel = (dayTotalPoductionValues / 3000) * 100;
                    var incomeStar = 0;
                    if (incomeSecurityLevel <= 30)
                    {
                        incomeStar = 1;
                    }
                    else if (incomeSecurityLevel > 30 && incomeSecurityLevel <= 60)
                    {
                        incomeStar = 2;
                    }
                    else if (incomeSecurityLevel > 60 && incomeSecurityLevel <= 80)
                    {
                        incomeStar = 3;
                    }
                    else if (incomeSecurityLevel > 80 && incomeSecurityLevel <= 90)
                    {
                        incomeStar = 4;
                    }
                    else if (incomeSecurityLevel > 90)
                    {
                        incomeStar = 5;
                    }
                    projectBasePoduction.IncomeSecurityLevel = incomeStar;
                    #endregion

                    projectBasePoduction.DayProductionValue = dayTotalPoductionValues;
                    projectBasePoduction.TotalYearProductionValue = Math.Round(yearProductionValue / 100000000, 2);
                    projectBasePoduction.ProductionValueProgressPercent = productionValueProgressPercent;
                    companyBasePoductionValues = companyBasePoductionValues.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
                    projectBasePoduction.CompanyBasePoductionValues = companyBasePoductionValues;
                    //广航局年度指标
                    //yearIndex = comonDataProductionList.Sum(x => x.YearIndex.Value);
                    //超序时进度
                    //projectBasePoduction.SupersequenceProgress = yearIndex.Value != 0 ? (Math.Round(projectBasePoduction.TotalYearProductionValue / yearIndex.Value, 2) * 100 - timeProgress) : 0;
                    companyBasePoductionValues.Add(new CompanyBasePoductionValue()
                    {
                        Name = name,
                        DayProductionValue = dayTotalPoductionValues,
                        TotalYearProductionValue = Math.Round(yearProductionValue / 100000000, 2),
                        YearProductionValueProgressPercent = 100,
                        ProductionValueProgressPercent = productionValueProgressPercent,
                        SupersequenceProgress = projectBasePoduction.SupersequenceProgress
                    });

                }
            }
            #endregion

            #region 柱形图
            var companyProductionList = dbContext.Queryable<CompanyProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.DateDay.Value == SqlFunc.ToInt32(yearStartTime)).ToList();
            var companyMonthProductionValue = GetProductionValueInfo(month, companyProductionList);
            CompanyProductionCompare companyProductionCompares = new CompanyProductionCompare()
            {
                PlanCompleteRate = new List<decimal>(),
                TimeSchedule = new List<decimal>(),
                XAxisData = new List<string>(),
                CompleteProductuin = new List<decimal>(),
                PlanProductuin = new List<decimal>()
            };

            foreach (var item in companyList)
            {
                if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Contains("广航局"))
                {
                    continue;
                }

                companyProductionCompares.XAxisData.Add(item.Name);
                //获取各个公司本月的完成和计划产值
                var currentMonthCompanyProductionValue = companyMonthProductionValue.Where(x => x.Id == item.ItemId).FirstOrDefault();
                if (currentMonthCompanyProductionValue != null)
                {
                    var completeProductionValue = Math.Round(currentMonthCompanyProductionValue.CompleteProductionValue / 100000000M, 2);
                    var planProductionValue = Math.Round(currentMonthCompanyProductionValue.PlanProductionValue / 100000000M, 2);
                    companyProductionCompares.CompleteProductuin.Add(completeProductionValue);
                    companyProductionCompares.PlanProductuin.Add(planProductionValue);
                }

                //计划完成率
                if (currentMonthCompanyProductionValue != null && currentMonthCompanyProductionValue.PlanProductionValue != 0)
                {
                    var completeRate = Math.Round((((decimal)currentMonthCompanyProductionValue.CompleteProductionValue) / currentMonthCompanyProductionValue.PlanProductionValue) * 100, 0);
                    companyProductionCompares.PlanCompleteRate.Add(completeRate);
                }
                //时间进度
                var timeSchedult = Math.Round((ofdays / 31M) * 100, 0);
                companyProductionCompares.TimeSchedule.Add(timeSchedult);
                projectBasePoduction.CompanyProductionCompares = companyProductionCompares;
            }

            companyProductionCompares.YMax = companyProductionCompares.PlanProductuin.Count == 0 ? 0 : companyProductionCompares.PlanProductuin.Max();
            #endregion

            #region 项目产值完成排名 暂时不用
            //List<ProjectRank> projectRankList = new List<ProjectRank>();
            ////获取公司信息
            //var companyIds = await dbContext.Queryable<CompanyProductionValueInfo>()
            //    .Where(x => x.IsDelete == 1 && x.DateDay.Value == SqlFunc.ToInt32(yearStartTime)).Select(x => x.CompanyId).ToListAsync();
            //var planList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == Convert.ToInt32(yearStartTime)).ToListAsync();
            //var dayReport = dbContext.Queryable<DayReport>()
            //    .LeftJoin<Project>((d, p) => d.ProjectId == p.Id)
            //    .OrderByDescending((d, p) => p.Name)
            //    .Where((d, p) => companyIds.Contains(p.CompanyId.Value) && d.IsDelete == 1
            //    && d.DateDay == currentTimeInt)
            //    .GroupBy((d, p) => d.ProjectId)
            //    .Select((d, p) => new
            //    {
            //        ProjectId = p.Id,
            //        ProjectName = p.ShortName,
            //        CompanyId = p.CompanyId,
            //        MonthAmount = SqlFunc.AggregateSum(d.DayActualProductionAmount)
            //    })
            //    .ToList();
            //var dayReportData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1).ToListAsync();
            ////项目完成产值历史数据
            //var historyOutPut =await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //var projectPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1).ToListAsync();
            //foreach (var item in dayReport)
            //{
            //    ProjectRank model = new ProjectRank();
            //    var planValue = GetProjectPlanValue(month, planList.Where(x => x.ProjectId == item.ProjectId && x.CompanyId == item.CompanyId).FirstOrDefault());
            //    model.ProjectName = item.ProjectName;
            //    model.CurrentYearPlanProductionValue = Math.Round(GetRrojectProductionValue(projectPlanProductionData, item.ProjectId).Value, 2);
            //    model.CurrentYearCompleteProductionValue = Math.Round(GetRrojectCompletProductionValue(dayReportData, historyOutPut, item.ProjectId), 2);
            //    if (model.CurrentYearPlanProductionValue != 0)
            //    {
            //        model.CompleteRate = Math.Round(model.CurrentYearCompleteProductionValue / model.CurrentYearPlanProductionValue*100, 2);
            //    }
            //    model.DayActualValue = Math.Round(item.MonthAmount / 10000, 2);
            //    projectRankList.Add(model);
            //}
            //projectBasePoduction.ProjectRanks = projectRankList.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();
            ////合计
            //var totalYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            //var totalYearCompletProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            //decimal totalYearCompletRate = 0;
            //if (totalYearPlanProductionValue != 0)
            //{
            //    totalYearCompletRate = Math.Round((totalYearCompletProductionValue / totalYearPlanProductionValue)*100, 2);
            //}
            //projectBasePoduction.TotalCurrentYearPlanProductionValue = totalYearPlanProductionValue;
            //projectBasePoduction.TotalCurrentYearCompleteProductionValue = totalYearCompletProductionValue;
            //// projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            //if (projectBasePoduction.TotalYearProductionValue != 0)
            //{
            //    projectBasePoduction.TotalCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue)*100, 2) ;
            //}
            //if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
            //{
            //    projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            //}
            ////当日所有项目汇总
            //projectBasePoduction.SumProjectRanks = Math.Round(projectRankList.Sum(x => x.DayActualValue), 2);
            ////当日排名前10条汇总
            //projectBasePoduction.SumProjectRanksTen = Math.Round(projectBasePoduction.ProjectRanks.Sum(x => x.DayActualValue), 2);
            ////总产值占比
            //projectBasePoduction.TotalProportion = Math.Round(projectBasePoduction.SumProjectRanksTen == 0 || projectBasePoduction.SumProjectRanks == 0 ? 0 : projectBasePoduction.SumProjectRanksTen / projectBasePoduction.SumProjectRanks * 100, 2);


            #endregion

            #region 项目年度产值完成排名
            List<ProjectRank> projectRankList = new List<ProjectRank>();
            var projectLists = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
                .Select(x => new { x.Id, x.ShortName, x.CompanyId }).ToListAsync();
            var projectSumDayProductionValue = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1
            && projectLists.Select(x => x.Id).ToList().Contains(x.ProjectId))
                 .GroupBy(x => x.ProjectId)
                 .Select(x => new { x.ProjectId, productionValue = SqlFunc.AggregateSum(x.DayActualProductionAmount) }).ToListAsync();
            //projectSumDayProductionValue = projectSumDayProductionValue.OrderByDescending(x => x.productionValue).Take(10).ToList();
            var planList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == Convert.ToInt32(yearStartTime)).ToListAsync();
            var projectPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1).ToListAsync();
            var dayReportData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1).ToListAsync();
            //项目完成产值历史数据
            var historyOutPut = await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            var monthStartTime = int.Parse(startYearTimeInt.ToString().Substring(0, 6));
            var monthEndTime = int.Parse(endYearTimeInt.ToString().Substring(0, 6));
            //月报数据
            var monthDataList = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1
            && x.DateMonth >= monthStartTime && x.DateMonth <= monthEndTime).ToListAsync();
            foreach (var item in projectSumDayProductionValue)
            {
                ProjectRank model = new ProjectRank();
                //var planValue = GetProjectPlanValue(month, planList.Where(x => x.ProjectId == item.ProjectId && x.CompanyId == item.CompanyId).FirstOrDefault());
                var projectInfo = projectLists.Where(x => x.Id == item.ProjectId).SingleOrDefault();
                model.ProjectName = projectInfo == null ? string.Empty : projectInfo.ShortName;
                //if (model.ProjectName == "茂名港博贺项目")
                //{

                //}
                model.CurrentYearPlanProductionValue = Math.Round(GetRrojectProductionValue(projectPlanProductionData, item.ProjectId).Value, 2);
                model.CurrentYearCompleteProductionValue = Math.Round(GetRrojectCompletProductionValue(dayReportData, historyOutPut, monthDataList, currentTimeIntUp, currentTimeInt, item.ProjectId), 2);
                if (model.CurrentYearPlanProductionValue != 0)
                {
                    model.CompleteRate = Math.Round(model.CurrentYearCompleteProductionValue / model.CurrentYearPlanProductionValue * 100, 2);
                }
                //model.DayActualValue = Math.Round(item.MonthAmount / 10000, 2);
                projectRankList.Add(model);
            }
            projectBasePoduction.ProjectRanks = projectRankList;
            projectBasePoduction.ProjectRanks = projectBasePoduction.ProjectRanks.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();

            //合计
            var totalYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            var totalYearCompletProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            decimal totalYearCompletRate = 0;
            if (totalYearPlanProductionValue != 0)
            {
                totalYearCompletRate = Math.Round((totalYearCompletProductionValue / totalYearPlanProductionValue) * 100, 2);
            }
            projectBasePoduction.TotalCurrentYearPlanProductionValue = totalYearPlanProductionValue;
            projectBasePoduction.TotalCurrentYearCompleteProductionValue = totalYearCompletProductionValue;
            // projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            if (projectBasePoduction.TotalYearProductionValue != 0)
            {
                projectBasePoduction.TotalCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue) * 100, 2);
            }
            if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
            {
                projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            }


            #endregion

            #region 项目产值强度表格
            List<ProjectIntensity> projectIntensities = new List<ProjectIntensity>();
            //获取只需要在建的项目
            var onBuildProjectList = companyProjectList.Where(x => onBuildProjectIds.Contains(x.Id)).ToList();
            var onBuildIds = onBuildProjectList.Select(x => x.Id).ToList();
            var planValueList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && onBuildIds.Contains(x.ProjectId)).ToListAsync();
            if (onBuildProjectList.Any())
            {
                foreach (var item in onBuildProjectList)
                {
                    //项目当日实际产值
                    var currentDayProjectPrduction = currentDayProjectList.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                    //项目当日计划
                    var planValueFirst = planValueList.Where(x => x.ProjectId == item.Id && x.Year == Convert.ToInt32(yearStartTime)).FirstOrDefault();
                    var planValue = GetProjectPlanValue(month, planValueFirst);
                    var rate = currentDayProjectPrduction == null || planValue == 0 ? 0 : Math.Round(((currentDayProjectPrduction.DayActualProductionAmount / 10000) / (planValue / 10000) * 100), 0);
                    if (rate < 80)
                    {
                        projectIntensities.Add(new ProjectIntensity()
                        {
                            Id = item.Id,
                            Name = item.ShortName,
                            PlanDayProduciton = Math.Round(planValue / 10000, 0),
                            DayProduciton = currentDayProjectPrduction == null ? 0 : Math.Round(currentDayProjectPrduction.DayActualProductionAmount / 10000, 0),
                            CompleteDayProducitonRate = rate,
                            DayProductionIntensityDesc = currentDayProjectPrduction == null ? null : currentDayProjectPrduction.LowProductionReason
                        });
                    }
                }
            }
            projectBasePoduction.ProjectIntensities = projectIntensities.Where(x => x.PlanDayProduciton > 0).OrderBy(x => x.CompleteDayProducitonRate).ToList();
            #endregion

            #endregion

            #region 自有船施工情况  自有船运转以及产值情况
            //三种船舶的shiid集合
            List<Guid> allShipIds = new List<Guid>();
            //计算船舶填报率和船舶未填报统计使用 其他无使用此集合（此集合就是已填报的船舶会记录shiid）
            List<Guid> shipIds = new List<Guid>();
            //需要更新在场天数的一个合计
            List<ShipOnDay> keyValuePairs = new List<ShipOnDay>();
            var ownerShipBuildInfos = new List<CompanyShipBuildInfo>();
            var companyShipProductionValueInfo = new List<CompanyShipProductionValueInfo>();
            //三类船舶类型集合
            var shipTypeIds = CommonData.ShipType.SplitStr(",").Select(x => x.ToGuid()).ToList();
            //三类船舶的数据
            var shipList = await dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1
                  && shipTypeIds.Contains(x.TypeId.Value)).ToListAsync();
            //船舶日报相关(施工  调遣  待命  检修)
            var shipDayList = await dbContext.Queryable<ShipDayReport>()
               .Where(x => x.IsDelete == 1
               && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
               ).ToListAsync();
            OwnerShipBuildInfo ownerShipBuildInfo = null;
            if (shipList != null && shipList.Any())
            {
                allShipIds = shipList.Select(x => x.PomId).ToList();
                companyList = commonDataList.Where(x => x.Type == 2).OrderBy(x => x.Sort).ToList();
                foreach (var item in companyList)
                {
                    //当前船舶的类型数量
                    var currentCompanyShipCount = shipList.Where(x => x.TypeId == item.ItemId).Count();
                    //当前船舶施工数量
                    var constructionShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Construction).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(constructionShipIds);
                    var constructionShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Construction).Count(); //shipList.Where(x => constructionShipIds.Contains(x.PomId)&&x.TypeId == item.ItemId.Value).Count();
                    //当前船舶修理数量
                    var repairShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Repair).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(repairShipIds);
                    var repairShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Repair).Count(); //shipList.Where(x => repairShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前船舶调遣数量
                    var dispatchShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Dispatch).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(dispatchShipIds);
                    var dispatchShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Dispatch).Count();//shipList.Where(x => dispatchShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前船舶待命数量
                    var standbyShipIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.Standby).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(standbyShipIds);
                    var standbyShipCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.Standby).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前舶航修数量
                    var voyageRepairIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.VoyageRepair).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(voyageRepairIds);
                    var voyageRepairCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.VoyageRepair).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //当前检修数量
                    var overHaulIds = shipDayList.Where(x => x.DateDay == currentTimeInt && x.ShipState == ProjectShipState.OverHaul).Select(x => x.ShipId).ToList();
                    shipIds.AddRange(overHaulIds);
                    var overHaulCount = shipList.Where(x => x.TypeId == item.ItemId && x.ShipState == ProjectShipState.OverHaul).Count(); //shipList.Where(x => standbyShipIds.Contains(x.PomId) && x.TypeId == item.ItemId.Value).Count();
                    //开工率
                    var buildPercent = 0M;
                    if (currentCompanyShipCount != 0)
                    {
                        //明天的在场天数
                        var nextDayCount = item.OnDayCount + constructionShipCount;
                        //记录需要更新的id和值
                        keyValuePairs.Add(new ShipOnDay() { Id = item.Id, OnDayCount = nextDayCount });
                        buildPercent = Math.Round((((decimal)(constructionShipCount)) / currentCompanyShipCount) * 100, 2);
                    }

                    #region 自有船施工情况  自有船运转产值情况

                    var allShipDayReportIds = shipList.Where(x => x.TypeId == item.ItemId).Select(x => x.PomId).ToList();
                    //当日产值
                    var dayConstructionShipPrudctionValue = shipDayList.Where(x => x.DateDay == currentTimeInt
                    && allShipDayReportIds.Contains(x.ShipId)).Select(x => x.EstimatedOutputAmount).Sum();
                    //当日运转小时
                    foreach (var key in shipDayList)
                    {
                        key.Dredge = key.Dredge.HasValue == false ? 0M : key.Dredge.Value;
                        key.Sail = key.Sail == null ? 0M : key.Sail.Value;
                        key.BlowingWater = key.BlowingWater == null ? 0M : key.BlowingWater.Value;
                        key.BlowShore = key.BlowShore == null ? 0M : key.BlowShore.Value;
                        key.SedimentDisposal = key.SedimentDisposal == null ? 0M : key.SedimentDisposal.Value;
                    }
                    var dayShipTurnHours = shipDayList.Where(x => x.DateDay == currentTimeInt
                    && allShipDayReportIds.Contains(x.ShipId))
                       .Sum(x => x.Dredge.Value + x.Sail.Value + x.BlowingWater.Value + x.BlowShore.Value + x.SedimentDisposal.Value);
                    //年累计产值 (含基础数据)
                    var yearConstructionShipPrudctionValue = shipDayList.Where(x =>
                   allShipDayReportIds.Contains(x.ShipId)).Select(x => x.EstimatedOutputAmount).Sum(); //+item.YearProductionValue;
                    //当年累计运转小时
                    var yearShipTurnHours = shipDayList.Where(x =>
                   allShipDayReportIds.Contains(x.ShipId))
                        .Sum(x => x.Dredge.Value + x.Sail.Value + x.BlowingWater.Value + x.BlowShore.Value + x.SedimentDisposal.Value);
                    //+ item.TurnHours;
                    //时间利用率年累计运转小时/在场天数累计/24
                    var TimePercent = 0M;
                    if (currentCompanyShipCount != 0)
                    {
                        var ondayCount = item.OnDayCount + constructionShipCount;


                        TimePercent = Math.Round(yearShipTurnHours / ondayCount / 24M * 100, 2);
                    }
                    #endregion

                    if (item.Collect == 0)
                    {
                        ownerShipBuildInfos.Add(new CompanyShipBuildInfo()
                        {
                            Name = item.Name,
                            AssignCount = dispatchShipCount,
                            AwaitCount = standbyShipCount,
                            BuildCount = constructionShipCount,
                            ReconditionCount = repairShipCount + voyageRepairCount + overHaulCount,
                            BuildPercent = buildPercent,
                            Count = currentCompanyShipCount

                        });
                        companyShipProductionValueInfo.Add(new CompanyShipProductionValueInfo()
                        {
                            DayTurnHours = dayShipTurnHours,
                            Name = item.Name,
                            YearTotalProductionValue = Math.Round((((decimal)yearConstructionShipPrudctionValue.Value) / 100000000), 2),
                            YearTotalTurnHours = yearShipTurnHours,
                            DayProductionValue = Math.Round(((decimal)dayConstructionShipPrudctionValue.Value) / 10000, 2),
                            TimePercent = TimePercent

                        });
                    }
                    else
                    {
                        //合计
                        var totalShipCount = ownerShipBuildInfos.Sum(x => x.Count);
                        var totalBuildShipCount = ownerShipBuildInfos.Sum(x => x.BuildCount);
                        var totalAwaitShipCount = ownerShipBuildInfos.Sum(x => x.AwaitCount);
                        var totalAssignShipCount = ownerShipBuildInfos.Sum(x => x.AssignCount);
                        var totalReconditShipCount = ownerShipBuildInfos.Sum(x => x.ReconditionCount);
                        var totalBuildShipPercent = ownerShipBuildInfos.Sum(x => x.BuildPercent);
                        //产值合计
                        var totalDayShipProductionValue = companyShipProductionValueInfo.Sum(x => x.DayProductionValue);
                        var totalDayShipTurnHours = companyShipProductionValueInfo.Sum(x => x.DayTurnHours);
                        //var totalDayTimePercent = companyShipProductionValueInfo.Sum(x => x.TimePercent);
                        var totalYearProductionValue = companyShipProductionValueInfo.Sum(x => x.YearTotalProductionValue);
                        var totalYearTurnHours = companyShipProductionValueInfo.Sum(x => x.YearTotalTurnHours);
                        //合计时间利用率
                        var totalDayTimePercent = 0M;
                        if (item.OnDayCount != 0)
                        {
                            var totalOnDayCount = keyValuePairs.Sum(x => x.OnDayCount);
                            totalDayTimePercent = Math.Round(totalYearTurnHours / totalOnDayCount / 24M * 100, 2);
                        }

                        //开工率
                        var BuildPercent = Math.Round(((decimal)totalBuildShipCount) / totalShipCount * 100, 2);
                        ownerShipBuildInfos.Add(new CompanyShipBuildInfo()
                        {
                            Name = item.Name,
                            AssignCount = totalAssignShipCount,
                            AwaitCount = totalAwaitShipCount,
                            BuildCount = totalBuildShipCount,
                            ReconditionCount = totalReconditShipCount,
                            BuildPercent = BuildPercent,
                            Count = totalShipCount,
                        });
                        companyShipProductionValueInfo.Add(new CompanyShipProductionValueInfo()
                        {
                            DayTurnHours = totalDayShipTurnHours,
                            Name = item.Name,
                            YearTotalProductionValue = totalYearProductionValue,
                            YearTotalTurnHours = totalYearTurnHours,
                            DayProductionValue = totalDayShipProductionValue,
                            TimePercent = totalDayTimePercent

                        });
                        ownerShipBuildInfo = new OwnerShipBuildInfo()
                        {
                            BuildCount = totalBuildShipCount,
                            AwaitCount = totalAwaitShipCount,
                            AssignCount = totalAssignShipCount,
                            ReconditionCount = totalReconditShipCount,
                            TotalCount = totalShipCount,
                            BulidProductionValue = totalDayShipProductionValue,
                            DayTurnHours = totalDayShipTurnHours,
                            YearTotalProductionValue = totalYearProductionValue,
                            YearTotalTurnHours = totalYearTurnHours,
                            BuildPercent = BuildPercent,
                            companyShipBuildInfos = ownerShipBuildInfos,
                            companyShipProductionValueInfos = companyShipProductionValueInfo
                        };
                        jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo = ownerShipBuildInfo;
                    }
                }
                #region 更新船舶在场天数
                if (keyValuePairs.Any())
                {
                    var ids = keyValuePairs.Select(x => x.Id).ToList();
                    var updateStartDay = date.ToString("yyyy-MM-dd 00:00:00");
                    var updateEndDay = date.ToString("yyyy-MM-dd 23:59:59");
                    //判断需要更新不 
                    var updateData = commonDataList.Where(x => ids.Contains(x.Id)
                     && x.UpdateTime >= SqlFunc.ToDate(updateStartDay)
                     && x.UpdateTime <= SqlFunc.ToDate(updateEndDay)).ToList();
                    if (updateData.Count == 0)
                    {
                        var entitysChange = commonDataList.Where(x => ids.Contains(x.Id)).ToList();
                        foreach (var item in entitysChange)
                        {
                            var singleEntity = keyValuePairs.SingleOrDefault(x => x.Id == item.Id);
                            if (singleEntity != null)
                            {
                                item.OnDayCount = singleEntity.OnDayCount;
                            }
                        }
                        await dbContext.Updateable<ProductionMonitoringOperationDayReport>(entitysChange).ExecuteCommandAsync();
                    }
                }
                #endregion
            }
            #endregion

            #region  施工船舶产值强度低于80%


            #endregion

            #region 自有船舶排行前五的产值
            List<ShipProductionValue> shipProductionValue = new List<ShipProductionValue>();
            //获取船舶ids  EstimatedUnitPrice
            var ownShipIds = shipList.Select(x => x.PomId).ToList();
            //  前五船舶 -年产值(todo 加入历史数据)
            var yearQuery = await dbContext.Queryable<ShipDayReport>()
                  .Where(x => x.IsDelete == 1
                  //&& x.ShipDayReportType == ShipDayReportType.ProjectShip
                  && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
                  ).ToListAsync();
            //
            //var shipDaysList=
            //    await dbContext.Queryable<ShipDayReport>().Where(x => x.IsDelete == 1
            //    && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt
            //    ).ToListAsync();
            //自有船舶历史数据
            var histotyShipList = await dbContext.Queryable<OwnerShipHistory>()
                 .Where(x => x.IsDelete == 1 && x.Year == 2023).OrderBy(x => x.Sort)
                  .ToListAsync();
            foreach (var item in histotyShipList)
            {
                //当日产值
                var onDayCount = yearQuery.Where(x => x.ShipId == item.Id
                ).Count();

                //当日产值
                var dayValue = yearQuery.Where(x => x.ShipId == item.Id && x.DateDay == currentTimeInt).Sum(x => x.EstimatedOutputAmount.Value);
                //当年产值
                var yearValue = yearQuery.Where(x => x.ShipId == item.Id).Sum(x => x.EstimatedOutputAmount.Value);
                //当年运转小时
                var yearHoursValue = yearQuery.Where(x => x.ShipId == item.Id)
                .Select(t => new
                {
                    a = t.Dredge ?? 0,
                    b = t.Sail ?? 0,
                    c = t.BlowingWater ?? 0,
                    d = t.SedimentDisposal ?? 0,
                    e = t.BlowShore ?? 0
                }).ToList();
                var totalHoursValue = yearHoursValue.Select(x => x.a + x.b + x.c + x.d + x.e).ToList().Sum();
                var obj = new ShipProductionValue()
                {
                    ShipName = item.Name,
                    ShipDayOutput = Math.Round(dayValue / 10000, 2),
                    //ShipYearOutput = Math.Round(yearValue/100000000,2) + item.YearShipHistory,
                    ShipYearOutput = Math.Round(yearValue / 100000000, 2),// + item.YearShipHistory,
                    WorkingHours = totalHoursValue,// + item.WorkingHours.Value,
                    //ConstructionDays = onDayCount+ item.OnDay.Value,
                    ConstructionDays = onDayCount,

                };
                if (obj.ConstructionDays != 0)
                    obj.TimePercent = Math.Round((obj.WorkingHours / obj.ConstructionDays / 24) * 100, 2);
                shipProductionValue.Add(obj);

            }
            projectBasePoduction.YearTopFiveTotalOutput = shipProductionValue.Sum(x => x.ShipDayOutput.Value);
            projectBasePoduction.YearFiveTotalOutput = shipProductionValue.Sum(x => x.ShipYearOutput.Value);
            var totalOnDay = shipProductionValue.Sum(x => x.ConstructionDays);
            if (totalOnDay != 0)
            {
                projectBasePoduction.YearTotalTopFiveOutputPercent = Math.Round((shipProductionValue.Sum(x => x.WorkingHours) / totalOnDay / 24) * 100, 2);
            }
            if (ownerShipBuildInfo.YearTotalProductionValue != 0)
            {
                projectBasePoduction.YearFiveTimeRate = Math.Round(
                    (projectBasePoduction.YearFiveTotalOutput /
                    ownerShipBuildInfo.YearTotalProductionValue) * 100, 2);

            }
            jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.companyShipTopFiveInfoList = shipProductionValue;

            #endregion















            #region 特殊情况
            var specialProjectList = new List<SpecialProjectInfo>();
            var dayRepNoticeData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && (x.IsHaveProductionWarning == 1 || x.IsHaveProductionWarning == 2 || x.IsHaveProductionWarning == 3))
                .Select(x => new { x.IsHaveProductionWarning, x.ProductionWarningContent, x.ProjectId }).OrderByDescending(x => x.IsHaveProductionWarning).ToListAsync();
            dayRepNoticeData.ForEach(x => specialProjectList.Add(new SpecialProjectInfo
            {
                ProjectId = x.ProjectId,
                Type = x.IsHaveProductionWarning,
                Description = x.ProductionWarningContent
            }));
            var pIds = dayRepNoticeData.Select(x => x.ProjectId).ToList();
            var sourceProjectList = companyProjectList.Where(x => pIds.Contains(x.Id)).ToList();
            foreach (var item in specialProjectList)
            {
                var projectInfo = sourceProjectList.Where(x => x.Id == item.ProjectId).FirstOrDefault();
                item.SourceMatter = projectInfo?.ShortName;
            }
            jjtSendMessageMonitoringDayReportResponseDto.SpecialProjectInfo = specialProjectList;
            #endregion

            #region 各单位填报情况(数据质量)
            //未填报项目的IDS
            List<Guid> unWriteReportIds = new List<Guid>();

            #region 各单位产值日报填报率情况
            List<CompanyWriteReportInfo> companyWriteReportInfos = new List<CompanyWriteReportInfo>();
            //所有已填报的项目
            var writeReportList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where((x, y) => x.IsDelete == 1 && onBuildProjectIds.Contains(x.ProjectId) && x.DateDay == currentTimeInt)
                .Select((x, y) => new JjtProjectDayReport() { ProjectId = x.Id, CompanyId = y.CompanyId.Value, DateDay = x.DateDay })
                .ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //当前公司已填报的数量
                var currentDayUnReportCount = writeReportList.Where(x => x.CompanyId == item.ItemId).Count();

                //填报率
                var writeReportPercent = 0M;
                if (currentCompany != 0)
                {
                    writeReportPercent = Math.Round(((decimal)currentDayUnReportCount / currentCompany) * 100, 2);
                }

                if (item.Collect == 0)
                {
                    companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    {
                        Name = item.Name,
                        OnBulidCount = currentCompany,
                        UnReportCount = currentCompany - currentDayUnReportCount,
                        WritePercent = writeReportPercent,
                        QualityLevel = 0,
                        ProjectId = item.Id
                    });
                }
                else
                {
                    //在建项目合计
                    var totalBuildCount = companyWriteReportInfos.Sum(x => x.OnBulidCount);
                    var totalUnReportCount = companyWriteReportInfos.Sum(x => x.UnReportCount);
                    var totalWritePercent = 0M;
                    if (totalBuildCount != 0)
                    {
                        totalWritePercent = Math.Round(((decimal)(totalBuildCount - totalUnReportCount)) / totalBuildCount * 100, 2);
                    }

                    companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    {
                        Name = item.Name,
                        OnBulidCount = totalBuildCount,
                        UnReportCount = totalUnReportCount,
                        WritePercent = totalWritePercent,
                    });
                }
            }
            //数据质量程度 几颗星（//船舶填报率 待命填报率+调遣填报率+修理填报率+施工填报率）
            //评分 1：一颗星[0 - 30) 2:两颗星[30 - 60) 3:三颗星[60 - 80) 4:四颗星[80 - 90) 5:五颗星[90 - 100)
            /// 计算公式：（项目当日产值/3300*50%+船舶当日产值/490*25%+项目填报率*20%+船舶填报率*5%）*100
            /// 计算船舶填报率
            var shipPercent = 0M;
            var reportShipCount = shipDayList.Where(x => x.DateDay == currentTimeInt && shipIds.Contains(x.ShipId)).Count();
            var tatalShipCount = jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.TotalCount;
            if (tatalShipCount != 0)
            {
                shipPercent = ((decimal)reportShipCount) / tatalShipCount;
            }
            //计算星星的数据质量程度
            var qualityLevel = ((jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.DayProductionValue / 3300M) * 50 / 100 +
            (jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.BulidProductionValue / 490M) * 25 / 100 +
            (companyWriteReportInfos[8].WritePercent / 100M * 20 / 100) +
            shipPercent * 5 / 100M) * 100;

            var star = 0;
            if (qualityLevel <= 30)
            {
                star = 1;
            }
            else if (qualityLevel > 30 && qualityLevel <= 60)
            {
                star = 2;
            }
            else if (qualityLevel > 60 && qualityLevel <= 80)
            {
                star = 3;
            }
            else if (qualityLevel > 80 && qualityLevel <= 90)
            {
                star = 4;
            }
            else if (qualityLevel > 90)
            {
                star = 5;
            }
            jjtSendMessageMonitoringDayReportResponseDto.QualityLevel = star;
            companyWriteReportInfos = companyWriteReportInfos.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
            jjtSendMessageMonitoringDayReportResponseDto.CompanyWriteReportInfos = companyWriteReportInfos;

            #endregion

            #region 说明：项目生产数据存在不完整部分主要是以下项目未填报
            List<CompanyUnWriteReportInfo> companyUnWriteReportInfos = new List<CompanyUnWriteReportInfo>();
            //统计本周期内已填报的日报
            var writeCompanyReportList = await dbContext.Queryable<DayReport>()
             .Where(x => x.IsDelete == 1
              && x.CreateTime >= SqlFunc.ToDate(startTime) && x.CreateTime <= SqlFunc.ToDate(endTime)
              && x.DateDay >= currentTimeIntUp && x.DateDay <= currentTimeInt
              && (x.UpdateTime == null || x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime)))
             .ToListAsync();
            //查询项目信息
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && onBuildProjectIds.Contains(x.Id)).ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            var distinctOnBuildProjects = onBuildProjectIds.Distinct();
            //查询符合范围内的数据
            var projectStatusChangeRecordList = await dbContext.Queryable<ProjectStatusChangeRecord>()
                .Where(x => x.NewStatus == buildProjectId && (x.ChangeTime >= SqlFunc.ToDate(startTime) && x.ChangeTime <= SqlFunc.ToDate(endTime)))
                .ToListAsync();
            //特殊项
            //var sIds = projectStatusChangeRecordList.Where(x => x.IsValid == 0).Select(x => x.Id).Distinct().ToList();
            //排除掉不满足的条件 得到满足的条件
            var satisfyIds = projectStatusChangeRecordList.Where(x => x.IsValid == 1).Select(x => x.Id).ToList();
            onBuildProjectIds = onBuildProjectIds.Where(x => satisfyIds.Contains(x)).ToList();
            foreach (var item in onBuildProjectIds)
            {
                //if (item != "08db3b35-fb38-4be0-8ad8-5a0a29a2d73f".ToGuid())
                //    continue;

                //查询当前项目什么时间变更状态的(变更时间就是当前填写日报的时间)
                var currentProjectStatusChangeTime = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                     .Select(x => x.ChangeTime)
                     .FirstOrDefault();
                //当前项目在本周期范围内停了多少天
                var projectStopDay = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                .Select(x => x.StopDay)
                .FirstOrDefault();
                //当前项目本周期需要填写的数量
                var changeTimeInt = int.Parse(currentProjectStatusChangeTime.ToString("dd"));
                //计算当前项目需要填写的日报的数量
                var currentWriteReportCount = 0;
                if (changeTimeInt >= 26)
                {
                    currentWriteReportCount = days - changeTimeInt + 26;
                }
                else
                {
                    currentWriteReportCount = days - (((days - 26)) + changeTimeInt - 1);
                }
                //未过天数
                var unDays = days - ofdays;
                //已填报数量
                var dayReportCount = writeCompanyReportList.Where(x => x.ProjectId == item).Count();
                //未填报数量
                var unReportCount = (days - projectStopDay.Value - unDays) - dayReportCount;
                if (unReportCount <= 0)
                {
                    unReportCount = 0;
                }
                //ofdays - dayReportCount<= 0 ? 0 : ofdays - dayReportCount- passedTime;
                //当前项目信息
                var currentProjectInfo = projectList.SingleOrDefault(x => x.Id == item);
                //业主单位
                var companyInfo = companyList.SingleOrDefault(x => x.ItemId == currentProjectInfo.CompanyId && x.Collect == 0);

                if (unReportCount != 0)
                {
                    companyUnWriteReportInfos.Add(new CompanyUnWriteReportInfo()
                    {
                        ProjectName = currentProjectInfo.Name,
                        Name = companyInfo.Name,
                        Count = unReportCount
                    });
                }
            }
            if (jjtSendMessageMonitoringDayReportResponseDto != null)
            {

                jjtSendMessageMonitoringDayReportResponseDto.CompanyUnWriteReportInfos = companyUnWriteReportInfos
                    .OrderByDescending(x => x.Count).ToList();
            }
            #endregion

            #region 说明：船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            List<CompanyShipUnWriteReportInfo> companyShipUnWriteReportInfos = new List<CompanyShipUnWriteReportInfo>();
            //未填写船舶日报的ids集合
            var unReportShipIds = allShipIds.Where(x => !shipIds.Contains(x)).ToList();
            if (unReportShipIds != null && unReportShipIds.Any())
            {
                //查询每个项目上的船舶信息
                var writeReportShipList = await dbContext.Queryable<ShipMovement>()
                   .Where(x => x.IsDelete == 1
                           && unReportShipIds.Contains(x.ShipId)
                           && x.Status == ShipMovementStatus.Enter
                           && x.ShipType == ShipType.OwnerShip
                        )
                   .ToListAsync();
                if (writeReportShipList != null && writeReportShipList.Any())
                {
                    foreach (var item in unReportShipIds)
                    {
                        var singleProject = writeReportShipList.FirstOrDefault(x => x.ShipId == item);
                        if (singleProject != null)
                        {
                            //船舶信息
                            var shipInfo = shipList.FirstOrDefault(x => x.PomId == item);
                            if (shipInfo != null)
                            {
                                //项目信息
                                var projectInfo = companyProjectList.FirstOrDefault(x => x.Id == singleProject.ProjectId);
                                if (projectInfo != null)
                                {
                                    companyShipUnWriteReportInfos.Add(new CompanyShipUnWriteReportInfo()
                                    {
                                        ShipName = shipInfo?.Name,
                                        OnProjectName = projectInfo?.Name,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            if (jjtSendMessageMonitoringDayReportResponseDto != null)
            {
                jjtSendMessageMonitoringDayReportResponseDto.CompanyShipUnWriteReportInfos = companyShipUnWriteReportInfos;
            }
            #endregion


            #endregion

            jjtSendMessageMonitoringDayReportResponseDto.Month = month;
            jjtSendMessageMonitoringDayReportResponseDto.Year = int.Parse(yearStartTime);
            responseAjaxResult.Data = jjtSendMessageMonitoringDayReportResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion


        }




        #region 根据不同月份获取不同的月计划产值 和完成产值

        #region 根据不同月份获取不同的月计划产值 和完成产值
        /// <summary>
        /// 根据不同月份获取不同的月计划产值 和完成产值
        /// </summary>
        /// <param name="month"></param>
        /// <param name="companyProductionList"></param>
        /// <returns></returns>
        private List<CompanyPlanCompleteProductionDto> GetProductionValueInfo(int month, List<CompanyProductionValueInfo> companyProductionList)
        {
            var year = DateTime.Now.Year;
            List<CompanyPlanCompleteProductionDto> companyPlanCompleteProductionDtos = new List<CompanyPlanCompleteProductionDto>();
            if (companyProductionList.Any())
            {
                #region 获取每月计划和完成产值


                if (month == 1)
                {
                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.OneCompleteProductionValue.Value,
                        PlanProductionValue = x.OnePlanProductionValue.Value
                    }).ToList();
                }

                if (month == 2)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.TwoCompleteProductionValue.Value,
                        PlanProductionValue = x.TwoPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 3)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.ThreeCompleteProductionValue.Value,
                        PlanProductionValue = x.ThreePlaProductionValue.Value
                    }).ToList();
                }

                if (month == 4)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.FourCompleteProductionValue.Value,
                        PlanProductionValue = x.FourPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 5)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.FiveCompleteProductionValue.Value,
                        PlanProductionValue = x.FivePlaProductionValue.Value
                    }).ToList();
                }

                if (month == 6)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.SixCompleteProductionValue.Value,
                        PlanProductionValue = x.SixPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 7)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.SevenCompleteProductionValue.Value,
                        PlanProductionValue = x.SevenPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 8)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.EightCompleteProductionValue.Value,
                        PlanProductionValue = x.EightPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 9)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.NineCompleteProductionValue.Value,
                        PlanProductionValue = x.NinePlaProductionValue.Value
                    }).ToList();
                }

                if (month == 10)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.TenCompleteProductionValue.Value,
                        PlanProductionValue = x.TenPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 11)
                {
                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.ElevenCompleteProductionValue.Value,
                        PlanProductionValue = x.ElevenPlaProductionValue.Value
                    }).ToList();
                }

                if (month == 12)
                {
                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.CompanyId,
                        CompleteProductionValue = x.TwelveCompleteProductionValue.Value,
                        PlanProductionValue = x.TwelvePlaProductionValue.Value
                    }).ToList();
                }
                #endregion
            }
            return companyPlanCompleteProductionDtos;
        }
        #endregion

        #region 根据不同月份获取交建公司计划产值和完成产值


        /// <summary>
        /// 根据不同月份获取交建公司计划产值和完成产值
        /// </summary>
        /// <param name="month"></param>
        /// <param name="companyProductionList"></param>
        /// <returns></returns>
        private List<CompanyPlanCompleteProductionDto> GetJJCompanyProductionValueInfo(int month, List<ProjectPlanProduction> companyProductionList, List<JjtProjectDayReport> jjtDayReport)
        {
            var year = DateTime.Now.Year;
            var dataDayStr = int.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMM26"));
            var dataDayEnd = int.Parse(DateTime.Now.ToString("yyyyMM25"));
            List<CompanyPlanCompleteProductionDto> companyPlanCompleteProductionDtos = new List<CompanyPlanCompleteProductionDto>();
            if (companyProductionList.Any())
            {
                var dayProjectValue = 0M;

                #region 获取每月计划和完成产值
                if (month == 1)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.OnePlanProductionValue.Value
                    }).ToList();

                }

                if (month == 2)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.TwoPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 3)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.ThreePlanProductionValue.Value
                    }).ToList();
                }

                if (month == 4)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.FourPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 5)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.FivePlanProductionValue.Value
                    }).ToList();
                }

                if (month == 6)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.SixPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 7)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.SevenPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 8)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.EigPlannedQuantities.Value
                    }).ToList();
                }

                if (month == 9)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.NinePlanProductionValue.Value
                    }).ToList();
                }

                if (month == 10)
                {

                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.TenPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 11)
                {
                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.ElevenPlanProductionValue.Value
                    }).ToList();
                }

                if (month == 12)
                {
                    companyPlanCompleteProductionDtos = companyProductionList.Select(x => new CompanyPlanCompleteProductionDto()
                    {
                        Id = x.ProjectId,
                        CompleteProductionValue = dayProjectValue,
                        PlanProductionValue = x.TwelvePlanProductionValue.Value
                    }).ToList();
                }

                foreach (var item in companyPlanCompleteProductionDtos)
                {
                    dayProjectValue = jjtDayReport.Where(x => x.DateDay >= dataDayStr && x.DateDay <= dataDayEnd
                    && x.ProjectId == item.Id).Sum(x => x.DayActualProductionAmount);
                    item.CompleteProductionValue = dayProjectValue;
                }
                #endregion
            }
            return companyPlanCompleteProductionDtos;
        }
        #endregion
        #endregion

        #region 船舶日报图片信息
        /// <summary>
        /// 获取自有船舶日报卡片消息详情
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JjtOwnShipDayRepDto>> JjtOwnShipTextCardMsgDetailsAsync(int dateDay = 0)
        {
            var responseAjaxResult = new ResponseAjaxResult<JjtOwnShipDayRepDto>();
            //日期
            var currentTimeInt = dateDay == 0 ? DateTime.Now.AddDays(-1).ToDateDay() : dateDay;
            #region
            //获取所有自有船舶(耙吸、绞吸、抓斗)
            var ownShips = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && CommonData.ShipTypes.Contains(x.TypeId)).Select(x => new { x.PomId, x.TypeClass, x.Name }).ToListAsync();
            var oids = ownShips.Select(x => x.PomId).ToList();
            //固定船机成本
            int year = Convert.ToInt32(currentTimeInt.ToString().Substring(0, 4));
            var shipMachineryCostData = await dbContext.Queryable<ShipMonthCost>().Where(x => x.IsDelete == 1 && x.DateYear == year).ToListAsync();
            //int month = DateTime.Now.Day >= 26 ? DateTime.Now.Month + 1 : DateTime.Now.Month;
            //船舶相关计算值
            var shipResultValue = await dbContext.Queryable<ShipDayReport>().Where(x => oids.Contains(x.ShipId) && x.DateDay == currentTimeInt).Select(x => new
            {
                OwnShipId = x.ShipId,
                ShipDayRepId = x.Id,//船舶日报id
                x.ProjectId,//项目id
                ConstructionMode = x.ConstructionmMode,//施工模式
                BlowingDistance = x.BlowTorque,//吹距
                //DayGrossMargin = x.ShipState == ProjectShipState.Construction ? ShipGrossMargin(x.shipid,) : 0M,
                WorkHours = SqlFunc.IsNull(x.Dredge, 0) + SqlFunc.IsNull(x.Sail, 0) + SqlFunc.IsNull(x.BlowingWater, 0) + SqlFunc.IsNull(x.SedimentDisposal, 0) + SqlFunc.IsNull(x.BlowShore, 0),//生产运转时间
                WorkShips = x.ForwardNumber,//施工船数（前进距/舱/驳数）
                WanfangOilConsumption = x.ShipReportedProduction == 0 ? 0M : x.OilConsumption / (x.ShipReportedProduction / 10000),//万方油耗=油耗/(船报产量/10000)
                x.ShipboardFuel,//船存燃油
                WorkHoursRate = (SqlFunc.IsNull(x.Dredge, 0) + SqlFunc.IsNull(x.Sail, 0) + SqlFunc.IsNull(x.BlowingWater, 0) + SqlFunc.IsNull(x.SedimentDisposal, 0) + SqlFunc.IsNull(x.BlowShore, 0) == 0 ? 0M : SqlFunc.IsNull(x.Dredge, 0) + SqlFunc.IsNull(x.Sail, 0) + SqlFunc.IsNull(x.BlowingWater, 0) + SqlFunc.IsNull(x.SedimentDisposal, 0) + SqlFunc.IsNull(x.BlowShore, 0)) / 24M//时间利用率=运转时间/24
            }).ToListAsync();
            //土质
            var soilQuailyData = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 5).Select(x => new { x.Name, x.Type }).ToListAsync();
            #endregion
            #region 基本数据查询
            //基本数据查询
            var data = await dbContext.Queryable<OwnerShip>()
                .LeftJoin<ShipDayReport>((ownship, shipdayrep) => ownship.PomId == shipdayrep.ShipId)
                .Where((ownship, shipdayrep) => shipdayrep.DateDay == currentTimeInt && ownship.IsDelete == 1 && shipdayrep.IsDelete == 1)
                .Select((ownship, shipdayrep) => new JjtOwnShipDayRepResponseDto
                {
                    OwnShipId = ownship.PomId,
                    OwnShipName = ownship.Name,
                    ProjectId = shipdayrep.ProjectId,
                    Production = shipdayrep.ShipReportedProduction,
                    OilConsumption = shipdayrep.OilConsumption,
                    FuelSupply = shipdayrep.FuelSupply,
                    SoilQuality = shipdayrep.SoilQuality,//土质
                    OwnShipType = ownship.TypeClass,
                    ShipDynamic = shipdayrep.ShipState,
                    Category = 0,
                    FuelUnitPrice = shipdayrep.FuelUnitPrice,
                    EstimatedOutputAmount = shipdayrep.EstimatedOutputAmount,
                    Remark = shipdayrep.Remarks
                }).ToListAsync();
            #endregion
            //获取项目信息
            var projects = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            foreach (var item in data)
            {
                item.ProjectName = projects.Where(x => x.Id == item.ProjectId).FirstOrDefault() == null ? "" : projects.Where(x => x.Id == item.ProjectId).FirstOrDefault().ShortName;
                item.Category = projects.Where(x => x.Id == item.ProjectId).FirstOrDefault() == null ? 0 : projects.Where(x => x.Id == item.ProjectId).FirstOrDefault().Category;
                item.DayGrossMargin = item.ShipDynamic == ProjectShipState.Construction ? ShipGrossMargin(item.OwnShipId, item.FuelUnitPrice == null ? 0M : item.OilConsumption * Convert.ToDecimal(item.FuelUnitPrice), item.EstimatedOutputAmount == null ? 0M : Convert.ToDecimal(item.EstimatedOutputAmount), item.Category, shipMachineryCostData) : 0M;
            }

            #region 数据赋值
            var result = new List<JjtOwnShipDayRepResponseDto>();
            foreach (var item in ownShips)
            {
                var isShipDayRepData = data.FirstOrDefault(x => x.OwnShipId == item.PomId);
                //项目名称
                string pName = string.Empty;
                //土质
                string sqilQuailtyName = string.Empty;
                //船舶类型
                string OwnShipType = item.TypeClass.Replace("挖泥", "");
                //船舶状态
                string shipDynamicName = string.Empty;
                //船舶产量
                decimal? production = decimal.Zero;
                //油耗
                decimal? oilConsumption = decimal.Zero;
                //补给
                decimal? fuelSupply = decimal.Zero;
                //船存燃油
                decimal? shipboardFuel = decimal.Zero;
                //毛利率
                decimal? dayGrossMargin = decimal.Zero;
                //备注
                string? remark = string.Empty;
                if (isShipDayRepData != null)
                {
                    pName = isShipDayRepData.ProjectName;
                    var soilQuailty = isShipDayRepData.SoilQuality == null || isShipDayRepData.SoilQuality == "" ? null : isShipDayRepData.SoilQuality.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                    if (soilQuailty != null)
                    {
                        foreach (var item2 in soilQuailty)
                        {
                            sqilQuailtyName += soilQuailyData.FirstOrDefault(x => item2 == x.Type)?.Name + ",";
                        }
                        sqilQuailtyName = sqilQuailtyName.Substring(0, sqilQuailtyName.Length - 1);
                    }
                    shipDynamicName = isShipDayRepData.ShipDynamic.ToDescription();
                    production = isShipDayRepData.Production;
                    oilConsumption = isShipDayRepData.OilConsumption;
                    fuelSupply = isShipDayRepData.FuelSupply;
                    dayGrossMargin = isShipDayRepData.DayGrossMargin;
                    remark = isShipDayRepData.Remark;
                    //shipboardFuel = isShipDayRepData.ShipboardFuel;

                }
                var isShipResultValue = shipResultValue.FirstOrDefault(x => x.OwnShipId == item.PomId);
                //施工模式
                string constructionMode = string.Empty;
                //吹距
                decimal? blowingDistance = decimal.Zero;
                //施工船数
                decimal workShips = decimal.Zero;
                //运转时间
                decimal? workHours = decimal.Zero;
                //时间利用率
                decimal? workHoursRate = decimal.Zero;
                //万方油耗
                decimal? wanfangOilConsumption = decimal.Zero;
                if (isShipResultValue != null)
                {
                    blowingDistance = isShipResultValue.BlowingDistance;
                    workHours = isShipResultValue.WorkHours;
                    workHoursRate = isShipResultValue.WorkHoursRate * 100;
                    wanfangOilConsumption = isShipResultValue.WanfangOilConsumption;
                    constructionMode = isShipResultValue.ConstructionMode;
                    workShips = isShipResultValue.WorkShips;
                    shipboardFuel = isShipResultValue.ShipboardFuel;
                }
                //生产率=产量/运转时间
                decimal? workRate = workHours == 0M ? 0M : Convert.ToDecimal(production) / Convert.ToDecimal(workHours);
                result.Add(new JjtOwnShipDayRepResponseDto
                {
                    OwnShipId = item.PomId,
                    OwnShipType = OwnShipType,
                    OwnShipName = item.Name == "东祥" ? "金广" : item.Name,
                    ProjectName = pName,
                    SoilQuality = sqilQuailtyName,
                    ShipDynamicName = shipDynamicName,
                    ConstructionMode = constructionMode == "1" ? "挖抛" : constructionMode == "2" ? "挖吹" : "",
                    BlowingDistance = Math.Round(Convert.ToDecimal(blowingDistance), 1, MidpointRounding.AwayFromZero),
                    Production = Math.Round(Convert.ToDecimal(production), 1, MidpointRounding.AwayFromZero),
                    WorkShips = workShips,
                    WorkRate = Convert.ToInt32(workRate),
                    WorkHours = Math.Round(Convert.ToDecimal(workHours), 1, MidpointRounding.AwayFromZero),
                    HoursRate = Math.Round(Convert.ToDecimal(workHoursRate), 1, MidpointRounding.AwayFromZero),
                    OilConsumption = Math.Round(Convert.ToDecimal(oilConsumption), 1, MidpointRounding.AwayFromZero),
                    WanfangOilConsumption = Math.Round(Convert.ToDecimal(wanfangOilConsumption), 1, MidpointRounding.AwayFromZero),
                    FuelSupply = Math.Round(Convert.ToDecimal(fuelSupply), 1, MidpointRounding.AwayFromZero),
                    ShipboardFuel = Math.Round(Convert.ToDecimal(shipboardFuel), 1, MidpointRounding.AwayFromZero),
                    Sort = _resourceManagementService.GetOwnShipSort(item.PomId),
                    DayGrossMargin = dayGrossMargin,
                    Remark = remark
                });
            }
            var responseData = await _resourceManagementService.SearchShipRepairRolling();
            //船舶修理滚动计划与执行
            var resourceManagementData = responseData.Data.ToList();
            foreach (var item in resourceManagementData)
            {
                item.ShipTypeName = item.ShipTypeName.Replace("挖泥", "");
            }
            ConvertHelper.TryConvertDateTimeFromDateDay(currentTimeInt, out DateTime currentTime);
            #endregion
            responseAjaxResult.Data = new JjtOwnShipDayRepDto
            {
                JjtOwnShipDayRepResponse = result.OrderBy(x => x.Sort).ToList(),
                resourceManagementSearch = resourceManagementData.OrderBy(x => x.Sort).ToList(),
                PushTime = currentTime.ToString("yyyy年MM月dd日")
            };
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 船舶毛利率计算 
        /// <summary>
        /// 计算船舶毛利率
        /// ①先判断是否施工船；只统计施工船的毛利率。
        /// ②判断施工船所在项目为国内还是国外，套用不同的日成本。
        /// ③该施工船所在项目当天油耗×燃油单价。
        /// ④当日船舶总成本=固定成本+燃油成本。
        /// ⑤毛利率=毛利额/产值=（当日船舶方量* 估算单价-当日船舶总成本）/（当日船舶方量* 估算单价） 
        /// </summary>
        /// <param name="category">境内境外</param>
        /// <param name="shipId">船舶id</param>
        /// <param name="shipMachineryCostData">船舶月度成本</param>
        /// <param name="fuelcost">燃油成本</param>
        /// <param name="dayOutputValue">当日完成产值</param>
        /// <returns></returns>
        public decimal ShipGrossMargin(Guid? shipId, decimal fuelcost, decimal dayOutputValue, int category, List<ShipMonthCost> shipMachineryCostData)
        {
            var grossMargin = decimal.Zero;
            if (dayOutputValue == 0) { return 0M; }
            var isExistCostData = shipMachineryCostData.Where(x => x.Category == category && x.ShipId == shipId).FirstOrDefault();
            if (isExistCostData != null)
            {
                if (category == 1) //境外
                {
                    if (isExistCostData.DayShipCostOverseas == null) { isExistCostData.DayShipCostOverseas = 0M; }
                    grossMargin = Math.Round((dayOutputValue - (fuelcost + Convert.ToDecimal(isExistCostData.DayShipCostOverseas))) / dayOutputValue * 100, 2);
                }
                else
                {
                    if (isExistCostData.DayShipCost == null) { isExistCostData.DayShipCost = 0M; }
                    grossMargin = Math.Round((dayOutputValue - (fuelcost + Convert.ToDecimal(isExistCostData.DayShipCost))) / dayOutputValue * 100, 2);
                }
            }
            return grossMargin;

        }
        #endregion


        #region 获取项目当月日均计划
        /// <summary>
        /// 获取项目当月日均计划
        /// </summary>
        /// <param name="month"></param>
        /// <param name="projectPlans"></param>
        /// <returns></returns>
        public decimal GetProjectPlanValue(int month, ProjectPlanProduction projectPlans)
        {
            decimal? value = 0.0M;
            if (projectPlans != null)
            {
                switch (month)
                {
                    case 1:
                        value = projectPlans.OnePlanProductionValue;
                        break;
                    case 2:
                        value = projectPlans.TwoPlanProductionValue;
                        break;
                    case 3:
                        value = projectPlans.ThreePlanProductionValue;
                        break;
                    case 4:
                        value = projectPlans.FourPlanProductionValue;
                        break;
                    case 5:
                        value = projectPlans.FivePlanProductionValue;
                        break;
                    case 6:
                        value = projectPlans.SixPlanProductionValue;
                        break;
                    case 7:
                        value = projectPlans.SevenPlanProductionValue;
                        break;
                    case 8:
                        value = projectPlans.EightPlanProductionValue;
                        break;
                    case 9:
                        value = projectPlans.NinePlanProductionValue;
                        break;
                    case 10:
                        value = projectPlans.TenPlanProductionValue;
                        break;
                    case 11:
                        value = projectPlans.ElevenPlanProductionValue;
                        break;
                    case 12:
                        value = projectPlans.TwelvePlanProductionValue;
                        break;
                    default:
                        break;
                }
            }
            return (value ?? 0) / 30.5M;
        }
        #endregion


        #region 获取项目的当年计划产值和完成产值
        /// <summary>
        /// 获取项目的当年计划产值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private decimal? GetRrojectProductionValue(List<ProjectPlanProduction> data, Guid id)
        {
            decimal? result = 0;
            try
            {
                result = data.Where(x => x.ProjectId == id)
                 .Sum(x => (x.OnePlanProductionValue ?? 0) + (x.TwoPlanProductionValue ?? 0) + (x.ThreePlanProductionValue ?? 0) +
                          (x.FourPlanProductionValue ?? 0) + (x.FivePlanProductionValue ?? 0) + (x.SixPlanProductionValue ?? 0) +
                          (x.SevenPlanProductionValue ?? 0) + (x.EightPlanProductionValue ?? 0) + (x.NinePlanProductionValue ?? 0) +
                          (x.TenPlanProductionValue ?? 0) + (x.ElevenPlanProductionValue ?? 0) + (x.TwelvePlanProductionValue ?? 0));
            }
            catch (Exception)
            {


            }
            return result / 100000000;
        }

        /// <summary>
        /// 获取项目的完成产值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="datahistoty">历史数据</param>
        /// <param name="monthData">当前月报数据</param>
        /// <param name="currentMonth">当前月份</param>
        /// <param name="id"></param>
        /// <returns></returns>
        private decimal GetRrojectCompletProductionValue(List<DayReport> data, List<ProjectHistoryData> datahistoty, List<MonthReport> monthData, int startTime, int endTime, Guid id)
        {
            decimal result = 0;
            decimal historyOutPut = 0;
            try
            {
                var month = monthData.Where(x => x.ProjectId == id).Sum(x => x.CompleteProductionAmount);
                //month = Math.Round(month/ 100000000, 2);
                if (DateTime.Now.Year == 2023)
                {
                    var outPut = datahistoty.Where(x => x.ProjectId == id).Select(x => x.OutputValue.Value).FirstOrDefault();
                    if (outPut != null)
                    {
                        historyOutPut = outPut;
                    }
                }
                result = data.Where(x => x.ProjectId == id
                && x.DateDay >= startTime && x.DateDay <= endTime)
                .Sum(x => x.DayActualProductionAmount);
                result += historyOutPut + month;
            }
            catch (Exception)
            {


            }
            return result / 100000000;
        }


        #endregion


        #region 项目日报未填报通知
        /// <summary>
        /// 项目日报未填报通知
        /// </summary>
        /// <param name="isTimingTask">是否是定时任务调用</param>
        /// <param name="isFirst">是否是第一次执行</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ProjectUnDayReportNotifAsync(bool isTimingTask, bool isFirst)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!isTimingTask)
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.DATA_NOTTIMINGTASK_FAIL, HttpStatusCode.NotTimingTask);
                return responseAjaxResult;
            }
            #region 基础信息
            //分隔符
            var separatorChar = "%%%";
            //当天日报 
            var day = DateTime.Now.AddDays(-1).ToDateDay();
            //查询四种必填项目日报的状态
            var statusIds = CommonData.DayReportIds.Split(",").Select(x => x.ToGuid()).ToList();
            //类型为非施工的
            var pNonConstruType = CommonData.PNonConstruType.ToGuid();
            //获取要填写日报的项目信息
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && statusIds.Contains(x.StatusId.Value)
            && x.TypeId != pNonConstruType).ToListAsync();
            //用户信息
            var userList = await dbContext.Queryable<Model.User>().Where(x => x.IsDelete == 1).ToListAsync();
            //项目领导班子信息
            List<ProjectLeader> projectLeaderList = new List<ProjectLeader>();
            //取此日报最晚填写的人员 如果找不到就找这个项目的报表负责人
            List<string> userPushMessage = new List<string>();
            //存放用户对应的项目
            Dictionary<string, string> dir = new Dictionary<string, string>();
            #endregion

            if (projectList.Any())
            {
                //需要填写日报的项目集合
                var projectIds = projectList.Select(x => x.Id).ToList();
                //用户信息
                if (!isFirst)
                {
                    projectLeaderList = await dbContext.Queryable<ProjectLeader>().Where(x => x.IsDelete == 1).ToListAsync();
                }
                //已填报的项目集合
                var dayReportList = await dbContext.Queryable<DayReport>()
                        .Where(x => x.IsDelete == 1
                        && projectIds.Contains(x.ProjectId)
                        ).Select(x => new { x.Id, x.ProjectId, x.DateDay, x.CreateId }).ToListAsync();
                //求两个集合的差集 算出没有填写日报的ID集合
                var dayReportIds = dayReportList.Where(x => x.DateDay == day).Select(x => x.ProjectId).ToList();
                var unDayReportIds = projectIds.Except(dayReportIds);
                foreach (var item in unDayReportIds)
                {
                    #region 项目填报人
                    if (isFirst)
                    {
                        var lastReportPerson = dayReportList.Where(x => x.ProjectId == item).OrderByDescending(x => x.DateDay).FirstOrDefault();
                        if (lastReportPerson != null)
                        {
                            var userAccount = userList.Where(x => x.Id == lastReportPerson.CreateId).Select(x => x.LoginAccount).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(userAccount))
                            {
                                userPushMessage.Add(userAccount);
                                var project = projectList.Where(x => x.Id == item).FirstOrDefault();
                                if (project != null)
                                {
                                    if (dir.Keys.Contains(userAccount))
                                    {
                                        var newValue = dir[userAccount] + separatorChar + project.ShortName;
                                        dir.Remove(userAccount);
                                        dir.Add(userAccount, newValue);
                                    }
                                    else
                                    {
                                        dir.Add(userAccount, project.ShortName);
                                    }
                                }

                            }
                        }
                        else
                        {
                            var projectReport = projectList.Where(x => x.Id == item).Select(x => x.ReportForMertel).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(projectReport))
                            {
                                var userAccount = userList.Where(x => x.Phone == projectReport).Select(x => x.LoginAccount).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(userAccount))
                                {
                                    userPushMessage.Add(userAccount);
                                }
                                var project = projectList.Where(x => x.Id == item).FirstOrDefault();
                                if (project != null)
                                {
                                    if (dir.Keys.Contains(userAccount))
                                    {
                                        var newValue = dir[userAccount] + separatorChar + project.ShortName;
                                        dir.Remove(userAccount);
                                        dir.Add(userAccount, newValue);
                                    }
                                    else
                                    {
                                        dir.Add(userAccount, project.ShortName);
                                    }
                                }
                            }

                        }
                    }

                    #endregion

                    #region 项目经理
                    if (!isFirst)
                    {
                        var assistantManagerId = projectLeaderList.Where(x => x.ProjectId == item).Select(x => x.AssistantManagerId).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(assistantManagerId.ToString()) && assistantManagerId != Guid.Empty)
                        {
                            var userAccount = userList.Where(x => x.PomId == assistantManagerId).Select(x => x.LoginAccount).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(userAccount))
                            {
                                userPushMessage.Add(userAccount);
                            }
                            var project = projectList.Where(x => x.Id == item).FirstOrDefault();
                            if (project != null)
                            {
                                if (dir.Keys.Contains(userAccount))
                                {
                                    var newValue = dir[userAccount] + separatorChar + project.ShortName;
                                    dir.Remove(userAccount);
                                    dir.Add(userAccount, newValue);
                                }
                                else
                                {
                                    dir.Add(userAccount, project.ShortName);
                                }
                            }
                        }
                    }
                    #endregion

                }
            }

            #region 发消息通知
            if (userPushMessage.Any() && userPushMessage.Count > 0)
            {
                foreach (var item in userPushMessage)
                {
                    List<string> pushAccount = new List<string>();
                    pushAccount.Add(item);
                    var projectMessage = string.Empty;
                    List<string> projectNameList = new List<string>();
                    #region 构造项目名称
                    if (!string.IsNullOrWhiteSpace(dir[item].TrimAll()))
                    {
                        //索引
                        var index = 1;
                        projectNameList = dir[item].TrimAll().Split(separatorChar, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (var project in projectNameList)
                        {
                            projectMessage += $"{index}、{project}<br>";
                            index += 1;
                        }
                    }
                    #endregion

                    #region 构造消息模版发消息
                    var tempMessage = string.Empty;
                    if (isFirst)
                    {
                        tempMessage = $"{DateTime.Now.Month}月{DateTime.Now.AddDays(-1).Day}日,未填报项目数量为{projectNameList.Count},未填报项目如下:<br>";
                    }
                    else
                    {
                        tempMessage = $"您好，您所负责的（{projectMessage}）项目日报，当前还没有填报。请您尽快填报！！！";
                    }
                    var msgResponse = new SingleMessageTemplateRequestDto
                    {
                        IsAll = false,
                        UserIds = pushAccount,
                        //ChatId = "zxcv",
                        MessageType = JjtMessageType.TEXT,
                        TextContent = tempMessage + projectMessage
                    };
                    if (!isFirst)
                    {
                        msgResponse.TextContent = tempMessage;
                    }
                    await Task.Factory.StartNew(async () =>
                    {
                        JjtUtils.SinglePushMessage(msgResponse);
                    });
                    #endregion

                }
            }
            #endregion
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        public static decimal GetCompanyProductuionValue(Guid companyId, List<MonthReport> data, List<Project> projects,
            List<MonthDiffProductionValue> monthDiffProductionValues)
        {
            //此事件段是填写月报的时间  此时间段不加入 当前月报产值
            //判断是否加入当前月的月报产值
            //var day = DateTime.Now.AddDays(-1).Day;
            //if (day >= 27 || day <=15)
            //{
            //    var singleCompany = monthDiffProductionValues.SingleOrDefault(x => x.IsDelete == 1 && x.CompanyId == companyId);
            //    if(singleCompany!=null)
            //    return singleCompany.ProductionValue.Value*100000000;
            //    return 0M;
            //}
            //else {
            var ids = projects.Where(x => x.CompanyId.Value == companyId).Select(x => x.Id).ToList();
            //已完成月报的产值
            var res = data.Where(x => ids.Contains(x.ProjectId)).Sum(x => x.CompleteProductionAmount);
            return res;

            // }

        }







        #region 交建公司生产日报推送

        /// <summary>
        /// 新版交建通发消息 监控运营中心图片消息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtDayReportPushAsync(int dateDay = 0)
        {


            #region 初始化基本业务参数
            var responseAjaxResult = new ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>();
            var result = await dbContext.Queryable<TempTable>().FirstAsync();
            if (result != null && !string.IsNullOrWhiteSpace(result.Value))
            {
                return JsonConvert.DeserializeObject<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>>(result.Value);
            }
            //在建项目的IDs
            List<Guid> onBuildProjectIds = new List<Guid>();
            var jjtSendMessageMonitoringDayReportResponseDto = new JjtSendMessageMonitoringDayReportResponseDto()
            {
                DayTime = DateTime.Now.AddDays(-1).ToString("MM月dd日")
            };
            #region 查询条件相关

            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            //周期结束时间
            var endTime = Convert.ToDateTime(startTime).AddMonths(1).ToString("yyyy-MM-25 23:59:59");
            //统计周期 上个月的26号到本月的25号之间为一个周期
            //当前时间上限int类型
            var currentTimeIntUp = int.Parse(Convert.ToDateTime(startTime).ToString("yyyyMM26"));
            //当前时间下限int类型
            var currentTimeInt = DateTime.Now.AddDays(-1).ToDateDay();
            //本年的月份
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            //本年的年份 
            var yearStartTime = DateTime.Now.Year.ToString();
            //年累计开始时间（每年的开始时间）
            var startYearTimeInt = int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy") + "1226");//int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1226"));
            //年累计结束时间
            var endYearTimeInt = int.Parse(DateTime.Now.ToString("yyyyMMdd")) > 1226 && int.Parse(DateTime.Now.ToString("yyyyMMdd")) <= 31 ? int.Parse(DateTime.Now.AddYears(1).ToString("yyyy1225")) : int.Parse(DateTime.Now.ToString("yyyy1225")); //int.Parse(DateTime.Now.ToString("yyyy1225"));
                                                                                                                                                                                                                                                      //每月多少天
                                                                                                                                                                                                                                                      // int days = DateTime.DaysInMonth(int.Parse(endYearTimeInt.ToString().Substring(0, 4)), month);  //DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
            int days = TimeHelper.GetTimeSpan(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime)).Days + 1;
            //已过多少天
            var ofdays = DateTime.Now.Day <= 26 ? (DateTime.Now.Day + ((days - 26))) : DateTime.Now.Day - 26;
            //今年已过多少天
            var dayOfYear = 0;
            //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy1231")))
            //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > startYearTimeInt && int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(yearStartTime + "1231"))
            //{
            //    dayOfYear = ofdays+ DateTime.Now.DayOfYear;
            //}
            //else
            //{
            //    //这个6天是上一年1226-1231之间的天数
            //    dayOfYear = DateTime.Now.DayOfYear + 5;
            //}

            if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > int.Parse(DateTime.Now.Year + "1231"))
            {
                var diffDay = TimeHelper.GetTimeSpan(DateTime.Now.ToString("yyyy-12-26").ObjToDate(), DateTime.Now);
                dayOfYear = diffDay.Days;
            }
            else
            {
                //这个6天是上一年1226-1231之间的天数
                dayOfYear = DateTime.Now.DayOfYear - 1 + 5;
            }
            #endregion

            #region 共用数据
            var companyId = "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid();//交建公司
            var commonDataList = await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1
            && x.ItemId == companyId).ToListAsync();
            var comonDataProductionList = await dbContext.Queryable<CompanyProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.DateDay == DateTime.Now.Year && x.CompanyId == companyId).ToListAsync();
            var monthDiffProductionValue = await dbContext.Queryable<MonthDiffProductionValue>().Where(x => x.IsDelete == 1).ToListAsync();
            #endregion

            #endregion

            #region 项目总体生产情况
            #region 交建公司合同项目基本信息
            ProjectBasePoduction projectBasePoduction = null;
            List<CompanyProjectBasePoduction> companyProjectBasePoductions = new List<CompanyProjectBasePoduction>();
            //合同项目状态ids集合
            var contractProjectStatusIds = CommonData.BuildIds.SplitStr(",").Select(x => x.ToGuid()).ToList();
            //项目类型为其他非施工类业务 排除
            var noConstrutionProject = CommonData.NoConstrutionProjectType;
            //在建项目状态ID
            var buildProjectId = CommonData.PConstruc.ToGuid();
            //停缓建Ids
            var stopProjectIds = CommonData.PSuspend.Split(",").Select(x => x.ToGuid()).ToList();
            //未开工状态
            var notWorkIds = CommonData.NotWorkStatusIds.Split(",").Select(x => x.ToGuid()).ToList();
            //各个公司的项目信息
            var companyProjectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && contractProjectStatusIds.Contains(x.StatusId.Value)
            && x.TypeId != noConstrutionProject).ToListAsync();
            //取出相关日报信息(当天项目日报)
            var currentDayProjectList = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && x.ProcessStatus == DayReportProcessStatus.Submited)
                  .ToListAsync();
            //公共数据取出项目相关信息
            var companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //在建项目IDS
                var currentCompanyIds = companyProjectList.Where(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId)
                    .Select(x => x.Id).ToList();
                if (item.Collect == 0)
                {
                    onBuildProjectIds.AddRange(currentCompanyIds);
                }
                //合同项目数
                var currentCompanyCount = companyProjectList.Count(x => x.CompanyId == item.ItemId);
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //停缓建项目数
                var stopProjectCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && stopProjectIds.Contains(x.StatusId.Value));
                //未开工的项目数量
                var notWorkCount = companyProjectList.Count(x => x.CompanyId == item.ItemId && notWorkIds.Contains(x.StatusId.Value));
                //当前合同项目的所有ids
                var dayIds = companyProjectList.Where(x => x.CompanyId == item.ItemId).Select(x => x.Id).ToList();
                //设备数量
                var facilityCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.ConstructionDeviceNum).Sum();
                //线程施工人数量
                var workerCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.SiteConstructionPersonNum).Sum();
                //危大工程项数量
                var riskWorkCountCount = currentDayProjectList.Where(x => dayIds.Contains(x.ProjectId)).Select(x => x.HazardousConstructionNum).Sum();

                if (item.Collect == 0)
                {

                    companyProjectBasePoductions.Add(new CompanyProjectBasePoduction()
                    {
                        Name = item.Name,
                        OnContractProjectCount = currentCompanyCount,
                        OnBuildProjectCount = currentCompany,
                        StopBuildProjectCount = stopProjectCount,
                        BuildCountPercent = currentCompanyCount == 0M ? 0M : Math.Round((((decimal)(currentCompany)) / currentCompanyCount) * 100, 2),
                        FacilityCount = facilityCount,
                        WorkerCount = workerCount,
                        RiskWorkCount = riskWorkCountCount,
                        NotWorkCount = notWorkCount,
                    });
                    projectBasePoduction = new ProjectBasePoduction()
                    {
                        TotalOnContractProjectCount = currentCompanyCount,
                        TotalStopBuildProjectCount = stopProjectCount,
                        TotalOnBuildProjectCount = currentCompany,
                        TotalFacilityCount = facilityCount,
                        TotalRiskWorkCount = workerCount,
                        TotalWorkerCount = riskWorkCountCount,
                        CompanyProjectBasePoductions = companyProjectBasePoductions,
                        TotalBuildCountPercent = Math.Round(((currentCompany.ObjToDecimal() / currentCompanyCount) * 100), 2),
                        CompanyBasePoductionValues = new List<CompanyBasePoductionValue>()
                    };
                    jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction = projectBasePoduction;
                }
            }



            #endregion

            #region 交建公司在建项目产值信息
            List<CompanyBasePoductionValue> companyBasePoductionValues = new List<CompanyBasePoductionValue>();
            //统计当年所有的项目日报信息
            var dayProductionValueList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where((x, y) => x.IsDelete == 1 && y.CompanyId == companyId
             && (x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt))
                .Select((x, y) => new JjtProjectDayReport
                {
                    CompanyId = y.CompanyId.Value,
                    ProjectId = x.ProjectId,
                    DateDay = x.DateDay,
                    CreateTime = x.CreateTime.Value,
                    UpdateTime = x.UpdateTime.Value,
                    DayActualProductionAmount = x.DayActualProductionAmount
                }).ToListAsync();
            //广航局年累计产值(基础数据累加+几个公司的所有日产值)
            var companyValue = new ShareData().Init().Where(x => x.CompanyId == companyId.ToString()).FirstOrDefault();//历史数据
            var yearTotalProductionValue = Math.Round((companyValue.Production * 100000000 + dayProductionValueList
                 .Where(x => x.DateDay >= 20240426 && x.DateDay <= currentTimeInt)
                    .Sum(x => x.DayActualProductionAmount)) / 100000000M, 2);
            //项目
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.CompanyId == companyId && x.StatusId.Value == buildProjectId).ToListAsync();
            //项目月报数据
            var projectIds = projectList.Select(x => x.Id).ToList();
            var monthReport = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.DateMonth >= 202401
            && projectIds.Contains(x.ProjectId)).ToListAsync();
            //计算总体:
            var totalJJProdutionValue = dayProductionValueList.Where(x => x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt).Sum(x => x.DayActualProductionAmount);//年累计总产值
            foreach (var item in projectList)
            {
                //当日产值
                var dayProductionValue = dayProductionValueList.Where(x => x.ProjectId == item.Id && x.DateDay == currentTimeInt).Sum(x => x.DayActualProductionAmount);
                //当年产值
                var yearProductionValue = dayProductionValueList.Where(x => x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt && x.ProjectId == item.Id).Sum(x => x.DayActualProductionAmount);
                companyBasePoductionValues.Add(new CompanyBasePoductionValue()
                {
                    Name = item.ShortName,
                    DayProductionValue = Math.Round((dayProductionValue / 10000M), 2),
                    TotalYearProductionValue = Math.Round((yearProductionValue / 100000000M), 2),
                    YearProductionValueProgressPercent = Math.Round((yearProductionValue / totalJJProdutionValue) * 100, 2),
                });
            }
            #region 计算总体
            var jjTotalDayProductionValue = dayProductionValueList.Where(x => x.DateDay == currentTimeInt).Sum(x => x.DayActualProductionAmount);
            var totalDayProductionValue = Math.Round((companyBasePoductionValues.Sum(x => x.DayProductionValue)), 2);
            //var totalYearProductionValue = Math.Round(companyBasePoductionValues.Sum(x => x.TotalYearProductionValue), 2);

            companyBasePoductionValues.Add(new CompanyBasePoductionValue()
            {
                Name = "交建公司总体",
                DayProductionValue = totalDayProductionValue,
                TotalYearProductionValue = yearTotalProductionValue,
                YearProductionValueProgressPercent = Math.Round(((totalDayProductionValue / 10000M / yearTotalProductionValue) * 100), 2),

            });

            #endregion
            jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.DayProductionValue = totalDayProductionValue;
            jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.TotalYearProductionValue = yearTotalProductionValue;
            jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.CompanyBasePoductionValues = companyBasePoductionValues;

            #endregion

            #region 柱形图
            //重点项目
            var jjProjectIds = CommonData.jjCompanyProjectids.Split(",").Select(x => x.ToGuid()).ToList();

            var jjProjectList = projectList.Where(x => jjProjectIds.Contains(x.Id)).ToList();
            var companyProductionList = dbContext.Queryable<ProjectPlanProduction>()
                .Where(x => x.IsDelete == 1).ToList();
            var companyMonthProductionValue = GetJJCompanyProductionValueInfo(month, companyProductionList, dayProductionValueList);
            CompanyProductionCompare companyProductionCompares = new CompanyProductionCompare()
            {
                PlanCompleteRate = new List<decimal>(),
                TimeSchedule = new List<decimal>(),
                XAxisData = new List<string>(),
                CompleteProductuin = new List<decimal>(),
                PlanProductuin = new List<decimal>()
            };

            foreach (var item in jjProjectList)
            {
                if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Contains("广航局"))
                {
                    continue;
                }

                companyProductionCompares.XAxisData.Add(item.ShortName);
                //获取各个公司本月的完成和计划产值
                var currentMonthCompanyProductionValue = companyMonthProductionValue.Where(x => x.Id == item.Id).FirstOrDefault();
                if (currentMonthCompanyProductionValue != null)
                {
                    var completeProductionValue = Math.Round(currentMonthCompanyProductionValue.CompleteProductionValue / 100000000M, 2);
                    var planProductionValue = Math.Round(currentMonthCompanyProductionValue.PlanProductionValue / 100000000M, 2);
                    companyProductionCompares.CompleteProductuin.Add(completeProductionValue);
                    companyProductionCompares.PlanProductuin.Add(planProductionValue);
                }

                //计划完成率
                if (currentMonthCompanyProductionValue != null && currentMonthCompanyProductionValue.PlanProductionValue != 0)
                {
                    var completeRate = Math.Round((((decimal)currentMonthCompanyProductionValue.CompleteProductionValue) / currentMonthCompanyProductionValue.PlanProductionValue) * 100, 0);
                    companyProductionCompares.PlanCompleteRate.Add(completeRate);
                }
                else
                {
                    companyProductionCompares.PlanCompleteRate.Add(0);
                }
                //时间进度
                var timeSchedult = Math.Round((ofdays / 31M) * 100, 0);
                companyProductionCompares.TimeSchedule.Add(timeSchedult);
                projectBasePoduction.CompanyProductionCompares = companyProductionCompares;
            }

            companyProductionCompares.YMax = companyProductionCompares.PlanProductuin.Count == 0 ? 0 : companyProductionCompares.PlanProductuin.Max();
            #endregion



            #region 项目年度产值完成排名新版
            List<ProjectRank> projectRankList = new List<ProjectRank>();
            var projectLists = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.CompanyId == companyId)
                .Select(x => new { x.Id, x.ShortName, x.CompanyId }).ToListAsync();
            //当年完成产值
            var eachProjectProductionValue = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay >= startYearTimeInt && x.DateDay <= endYearTimeInt).ToListAsync();
            //当年各个项目计划产值
            var projectYearPlanProductionData = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.Year == DateTime.Now.Year).ToListAsync();
            //查询历史数据
            var projectPlanProductionData = await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //项目月报数据
            var year = int.Parse(DateTime.Now.ToString("yyyy01"));
            var projectMonthData = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.DateMonth >= year).ToListAsync();
            foreach (var item in projectLists)
            {
                //if (item.Id != "08db3b35-fb38-4bd7-8c32-5423575bad59".ToGuid())
                //{
                //    continue;
                //}
                //当年项目完成产值
                var projectYearTotalProductionValue = eachProjectProductionValue.Where(x => x.ProjectId == item.Id && x.DateDay >= currentTimeIntUp && x.DateDay <= currentTimeInt).Sum(x => x.DayActualProductionAmount);
                //当年项目计划产值
                var projectPalnProduction = Math.Round(GetRrojectProductionValue(projectYearPlanProductionData, item.Id).Value, 2);
                //今日完成产值
                var day = DateTime.Now.AddDays(-1).ToDateDay();
                var dayProductionValue = eachProjectProductionValue.Where(x => x.ProjectId == item.Id && x.DateDay == day).SingleOrDefault();
                //计算历史计划产值
                //var projectHistoryProduciton= projectYearPlanProductionData.Where(x => x.ProjectId == item.Id).SingleOrDefault();
                //计算2023-06月之前的数据
                var proejctHistoty = projectPlanProductionData.Where(x => x.ProjectId == item.Id && x.OutputValue.HasValue == true).Select(x => x.OutputValue.Value).SingleOrDefault();
                //月份相加产值
                var monthValue = projectMonthData.Where(x => x.ProjectId == item.Id).Sum(x => x.CompleteProductionAmount);

                var dayValue = 0M;
                if (dayProductionValue != null)
                {
                    dayValue = Math.Round(dayProductionValue.DayActualProductionAmount / 10000, 2);
                }
                ProjectRank projectRank = new ProjectRank()
                {
                    ProjectName = item.ShortName,
                    //CurrentYearCompleteProductionValue = (Math.Round(projectYearTotalProductionValue / 100000000, 2) ),
                    CurrentYearCompleteProductionValue = Math.Round(monthValue / 100000000, 2) + Math.Round(projectYearTotalProductionValue / 100000000, 2),
                    CurrentYearPlanProductionValue = projectPalnProduction,
                    DayActualValue = dayValue,
                };
                if (projectPalnProduction != 0)
                {
                    projectRank.CompleteRate = Math.Round((projectRank.CurrentYearCompleteProductionValue / projectRank.CurrentYearPlanProductionValue) * 100, 2);
                }
                projectRankList.Add(projectRank);
            }
            projectBasePoduction.ProjectRanks = projectRankList.OrderByDescending(x => x.CurrentYearCompleteProductionValue).Take(10).ToList();
            //总计
            projectBasePoduction.TotalCurrentYearPlanProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearPlanProductionValue);
            projectBasePoduction.TotalCurrentYearCompleteProductionValue = projectBasePoduction.ProjectRanks.Sum(x => x.CurrentYearCompleteProductionValue);
            if (projectBasePoduction.TotalCurrentYearPlanProductionValue != 0)
                projectBasePoduction.SumCompleteRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalCurrentYearPlanProductionValue) * 100, 2);
            var totalYearCompletRate = 0M;
            if (projectBasePoduction.TotalYearProductionValue != 0)
            {
                totalYearCompletRate = Math.Round((projectBasePoduction.TotalCurrentYearCompleteProductionValue / projectBasePoduction.TotalYearProductionValue) * 100, 2);
            }
            projectBasePoduction.TotalCompleteRate = totalYearCompletRate;
            projectBasePoduction.SumProjectRanksTen = projectBasePoduction.ProjectRanks.Sum(x => x.DayActualValue.Value);
            #endregion

            #region 项目产值强度表格
            List<ProjectIntensity> projectIntensities = new List<ProjectIntensity>();
            //获取只需要在建的项目
            var onBuildProjectList = companyProjectList.Where(x => onBuildProjectIds.Contains(x.Id) && x.CompanyId == companyId).ToList();
            var onBuildIds = onBuildProjectList.Select(x => x.Id).ToList();
            var planValueList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && onBuildIds.Contains(x.ProjectId)).ToListAsync();
            if (onBuildProjectList.Any())
            {
                foreach (var item in onBuildProjectList)
                {
                    //项目当日实际产值
                    var currentDayProjectPrduction = currentDayProjectList.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                    //项目当日计划
                    var planValueFirst = planValueList.Where(x => x.ProjectId == item.Id && x.Year == Convert.ToInt32(yearStartTime)).FirstOrDefault();
                    var planValue = GetProjectPlanValue(month, planValueFirst);
                    var rate = currentDayProjectPrduction == null || planValue == 0 ? 0 : Math.Round(((currentDayProjectPrduction.DayActualProductionAmount / 10000) / (planValue / 10000) * 100), 0);
                    if (rate < 80)
                    {
                        projectIntensities.Add(new ProjectIntensity()
                        {
                            Id = item.Id,
                            Name = item.ShortName,
                            PlanDayProduciton = Math.Round(planValue / 10000, 0),
                            DayProduciton = currentDayProjectPrduction == null ? 0 : Math.Round(currentDayProjectPrduction.DayActualProductionAmount / 10000, 0),
                            CompleteDayProducitonRate = rate,
                            DayProductionIntensityDesc = currentDayProjectPrduction == null ? null : currentDayProjectPrduction.LowProductionReason
                        });
                    }
                }
            }
            //projectBasePoduction.ProjectIntensities = projectIntensities.Where(x => x.PlanDayProduciton > 0).OrderBy(x => x.CompleteDayProducitonRate).ToList();
            #endregion

            #endregion

            #region 特殊情况
            var specialProjectList = new List<SpecialProjectInfo>();
            var dayRepNoticeData = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay == currentTimeInt && (x.IsHaveProductionWarning == 1 || x.IsHaveProductionWarning == 2 || x.IsHaveProductionWarning == 3))
                .Select(x => new { x.IsHaveProductionWarning, x.ProductionWarningContent, x.ProjectId }).OrderByDescending(x => x.IsHaveProductionWarning).ToListAsync();
            dayRepNoticeData.ForEach(x => specialProjectList.Add(new SpecialProjectInfo
            {
                ProjectId = x.ProjectId,
                Type = x.IsHaveProductionWarning,
                Description = x.ProductionWarningContent
            }));
            var pIds = dayRepNoticeData.Select(x => x.ProjectId).ToList();
            var sourceProjectList = companyProjectList.Where(x => pIds.Contains(x.Id)).ToList();
            foreach (var item in specialProjectList)
            {
                var projectInfo = sourceProjectList.Where(x => x.Id == item.ProjectId && x.CompanyId == companyId).FirstOrDefault();
                item.SourceMatter = projectInfo?.ShortName;
            }
            jjtSendMessageMonitoringDayReportResponseDto.SpecialProjectInfo = specialProjectList;
            #endregion

            #region 各单位填报情况(数据质量)
            ////未填报项目的IDS
            //List<Guid> unWriteReportIds = new List<Guid>();
            //#region 各单位产值日报填报率情况
            //List<CompanyWriteReportInfo> companyWriteReportInfos = new List<CompanyWriteReportInfo>();
            ////所有已填报的项目
            //var writeReportList = await dbContext.Queryable<DayReport>()
            //    .LeftJoin<Project>((x, y) => x.ProjectId == y.Id&&y.CompanyId==companyId)
            //    .Where((x, y) => x.IsDelete == 1 && onBuildProjectIds.Contains(x.ProjectId) && x.DateDay == currentTimeInt)
            //    .Select((x, y) => new JjtProjectDayReport() { ProjectId = x.Id, CompanyId = y.CompanyId.Value, DateDay = x.DateDay })
            //    .ToListAsync();

            //companyWriteReportInfos.Add(new CompanyWriteReportInfo()
            //{
            //    Name = "交建公司",
            //    OnBulidCount = companyProjectBasePoductions[0].OnBuildProjectCount,
            //    UnReportCount = companyProjectBasePoductions[0].OnBuildProjectCount - writeReportList.Count,
            //     WritePercent=Math.Round((writeReportList.Count)/Convert.ToDecimal(companyProjectBasePoductions[0].OnBuildProjectCount)*100,2)

            //}) ;
            ////#endregion


            //if (jjtSendMessageMonitoringDayReportResponseDto != null)
            //{
            //    jjtSendMessageMonitoringDayReportResponseDto.CompanyWriteReportInfos = companyWriteReportInfos;
            //}
            #endregion


            //未填报项目的IDS
            List<Guid> unWriteReportIds = new List<Guid>();

            #region 各单位产值日报填报率情况
            List<CompanyWriteReportInfo> companyWriteReportInfos = new List<CompanyWriteReportInfo>();
            //所有已填报的项目
            var writeReportList = await dbContext.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where((x, y) => x.IsDelete == 1 && onBuildProjectIds.Contains(x.ProjectId) && x.DateDay == currentTimeInt)
                .Select((x, y) => new JjtProjectDayReport() { ProjectId = x.Id, CompanyId = y.CompanyId.Value, DateDay = x.DateDay })
                .ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            foreach (var item in companyList)
            {
                //当前公司在建合同项数
                var currentCompany = companyProjectList.Count(x => x.CompanyId == item.ItemId && x.StatusId == buildProjectId);
                //当前公司已填报的数量
                var currentDayUnReportCount = writeReportList.Where(x => x.CompanyId == item.ItemId).Count();

                //填报率
                var writeReportPercent = 0M;
                if (currentCompany != 0)
                {
                    writeReportPercent = Math.Round(((decimal)currentDayUnReportCount / currentCompany) * 100, 2);
                }

                if (item.Collect == 0)
                {
                    companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    {
                        Name = item.Name,
                        OnBulidCount = currentCompany,
                        UnReportCount = currentCompany - currentDayUnReportCount,
                        WritePercent = writeReportPercent,
                        QualityLevel = 0,
                        ProjectId = item.Id
                    });
                    //在建项目合计
                    var totalBuildCount = companyWriteReportInfos.Sum(x => x.OnBulidCount);
                    var totalUnReportCount = companyWriteReportInfos.Sum(x => x.UnReportCount);
                    var totalWritePercent = 0M;
                    if (totalBuildCount != 0)
                    {
                        totalWritePercent = Math.Round(((decimal)(totalBuildCount - totalUnReportCount)) / totalBuildCount * 100, 2);
                    }

                    //companyWriteReportInfos.Add(new CompanyWriteReportInfo()
                    //{
                    //    Name = item.Name,
                    //    OnBulidCount = totalBuildCount,
                    //    UnReportCount = totalUnReportCount,
                    //    WritePercent = totalWritePercent,
                    //});
                }

            }
            //数据质量程度 几颗星（//船舶填报率 待命填报率+调遣填报率+修理填报率+施工填报率）
            //评分 1：一颗星[0 - 30) 2:两颗星[30 - 60) 3:三颗星[60 - 80) 4:四颗星[80 - 90) 5:五颗星[90 - 100)
            /// 计算公式：（项目当日产值/3300*50%+船舶当日产值/490*25%+项目填报率*20%+船舶填报率*5%）*100
            /// 计算船舶填报率
            var shipPercent = 0M;
            //var tatalShipCount = jjtSendMessageMonitoringDayReportResponseDto.OwnerShipBuildInfo.TotalCount;
            //if (tatalShipCount != 0)
            //{
            //    shipPercent = ((decimal)reportShipCount) / tatalShipCount;
            //}
            //计算星星的数据质量程度
            var qualityLevel = ((jjtSendMessageMonitoringDayReportResponseDto.projectBasePoduction.DayProductionValue / 3300M) * 50 / 100 +
            (companyWriteReportInfos[0].WritePercent / 100M * 20 / 100) +
            shipPercent * 5 / 100M) * 100;

            var star = 0;
            if (qualityLevel <= 30)
            {
                star = 1;
            }
            else if (qualityLevel > 30 && qualityLevel <= 60)
            {
                star = 2;
            }
            else if (qualityLevel > 60 && qualityLevel <= 80)
            {
                star = 3;
            }
            else if (qualityLevel > 80 && qualityLevel <= 90)
            {
                star = 4;
            }
            else if (qualityLevel > 90)
            {
                star = 5;
            }
            jjtSendMessageMonitoringDayReportResponseDto.QualityLevel = star;
            companyWriteReportInfos = companyWriteReportInfos.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToList();
            jjtSendMessageMonitoringDayReportResponseDto.CompanyWriteReportInfos = companyWriteReportInfos;

            #endregion

            #region 说明：项目生产数据存在不完整部分主要是以下项目未填报
            List<CompanyUnWriteReportInfo> companyUnWriteReportInfos = new List<CompanyUnWriteReportInfo>();
            //统计本周期内已填报的日报
            var writeCompanyReportList = await dbContext.Queryable<DayReport>()
             .Where(x => x.IsDelete == 1
              && x.CreateTime >= SqlFunc.ToDate(startTime) && x.CreateTime <= SqlFunc.ToDate(endTime)
              && x.DateDay >= currentTimeIntUp && x.DateDay <= currentTimeInt
              && (x.UpdateTime == null || x.UpdateTime >= SqlFunc.ToDate(startTime) && x.UpdateTime <= SqlFunc.ToDate(endTime)))
             .ToListAsync();
            //查询项目信息
            var jJProjectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && onBuildProjectIds.Contains(x.Id)).ToListAsync();
            companyList = commonDataList.Where(x => x.Type == 1).OrderBy(x => x.Sort).ToList();
            var distinctOnBuildProjects = onBuildProjectIds.Distinct();
            //查询符合范围内的数据
            var projectStatusChangeRecordList = await dbContext.Queryable<ProjectStatusChangeRecord>()
                .Where(x => x.NewStatus == buildProjectId && (x.ChangeTime >= SqlFunc.ToDate(startTime) && x.ChangeTime <= SqlFunc.ToDate(endTime)))
                .ToListAsync();
            //特殊项
            //var sIds = projectStatusChangeRecordList.Where(x => x.IsValid == 0).Select(x => x.Id).Distinct().ToList();
            //排除掉不满足的条件 得到满足的条件
            var satisfyIds = projectStatusChangeRecordList.Where(x => x.IsValid == 1).Select(x => x.Id).ToList();
            onBuildProjectIds = onBuildProjectIds.Where(x => satisfyIds.Contains(x)).ToList();
            foreach (var item in onBuildProjectIds)
            {
                //if (item != "08db3b35-fb38-4be0-8ad8-5a0a29a2d73f".ToGuid())
                //    continue;

                //查询当前项目什么时间变更状态的(变更时间就是当前填写日报的时间)
                var currentProjectStatusChangeTime = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                     .Select(x => x.ChangeTime)
                     .FirstOrDefault();
                //当前项目在本周期范围内停了多少天
                var projectStopDay = projectStatusChangeRecordList.Where(x => x.Id == item && x.IsValid == 1)
                .Select(x => x.StopDay)
                .FirstOrDefault();
                //当前项目本周期需要填写的数量
                var changeTimeInt = int.Parse(currentProjectStatusChangeTime.ToString("dd"));
                //计算当前项目需要填写的日报的数量
                var currentWriteReportCount = 0;
                if (changeTimeInt >= 26)
                {
                    currentWriteReportCount = days - changeTimeInt + 26;
                }
                else
                {
                    currentWriteReportCount = days - (((days - 26)) + changeTimeInt - 1);
                }
                //未过天数
                var unDays = days - ofdays;
                //已填报数量
                var dayReportCount = writeCompanyReportList.Where(x => x.ProjectId == item).Count();
                //未填报数量
                var unReportCount = (days - projectStopDay.Value - unDays) - dayReportCount;
                if (unReportCount <= 0)
                {
                    unReportCount = 0;
                }
                //ofdays - dayReportCount<= 0 ? 0 : ofdays - dayReportCount- passedTime;
                //当前项目信息
                var currentProjectInfo = jJProjectList.SingleOrDefault(x => x.Id == item);
                //业主单位
                var companyInfo = companyList.SingleOrDefault(x => x.ItemId == currentProjectInfo.CompanyId && x.Collect == 0);

                if (unReportCount != 0)
                {
                    companyUnWriteReportInfos.Add(new CompanyUnWriteReportInfo()
                    {
                        ProjectName = currentProjectInfo.Name,
                        Name = companyInfo.Name,
                        Count = unReportCount
                    });
                }
            }
            if (jjtSendMessageMonitoringDayReportResponseDto != null)
            {

                jjtSendMessageMonitoringDayReportResponseDto.CompanyUnWriteReportInfos = companyUnWriteReportInfos
                    .OrderByDescending(x => x.Count).ToList();
            }
            #endregion


            jjtSendMessageMonitoringDayReportResponseDto.Month = month;
            jjtSendMessageMonitoringDayReportResponseDto.Year = int.Parse(yearStartTime);
            responseAjaxResult.Data = jjtSendMessageMonitoringDayReportResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion




        }

    }
}
