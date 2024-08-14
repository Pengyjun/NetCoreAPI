using System.ComponentModel;

namespace GHMonitoringCenterApi.Domain.Enums
{

    /// <summary>
    /// 日报进程状态(1:填写步骤中,2:已提交)
    /// </summary>
    public enum DayReportProcessStatus
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None = 0,

        /// <summary>
        /// 填写步骤中
        /// </summary>
        Steping = 1,

        /// <summary>
        /// 已提交
        /// </summary>
        Submited = 2
    }

    /// <summary>
    /// 施工-产值属性(1:自有，2：分包，4：分包-自有)
    /// </summary>
    public enum ConstructionOutPutType
    {
        /// <summary>
        /// 自有
        /// </summary>
        [Description("自有")]
        Self = 1,

        /// <summary>
        /// 分包
        /// </summary>
        [Description("分包")]
        SubPackage = 2,

        /// <summary>
        /// 分包-自有
        /// </summary>
        [Description("分包-自有")]
        SubOwner = 4
    }

    /// <summary>
    /// 船舶来源类型（1：自有船舶，2：分包船舶，3：往来单位，4：包-自有船舶）
    /// </summary>
    public enum ShipType
    {
        /// <summary>
        /// 自有船舶
        /// </summary>
        [Description("自有船舶")]
        OwnerShip = 1,

        /// <summary>
        /// 分包船舶
        /// </summary>
        [Description("分包船舶")]
        SubShip = 2,

        /// <summary>
        /// 往来单位
        /// </summary>
        [Description("往来单位")]
        SubBusinessUnit = 3,

        /// <summary>
        /// 分包-自有船舶
        /// </summary>
        [Description("分包-自有船舶")]
        SubOwe = 4,
    }

    /// <summary>
    /// 船舶动向状态(0:未进场，1:进场，2：退场)
    /// </summary>
    public enum ShipMovementStatus
    {
        /// <summary>
        /// 未进场
        /// </summary>
        [Description("未进场")]
        None = 0,

        /// <summary>
        /// 进场
        /// </summary>
        [Description("进场")]
        Enter = 1,

        /// <summary>
        /// 退场
        /// </summary>
        [Description("退场")]
        Quit = 2
    }

    /// <summary>
    /// 对象类型
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// 项目
        /// </summary>
        Project = 1,

        /// <summary>
        /// 月报
        /// </summary>
        MonthReport = 4,

        /// <summary>
        /// 分包船舶月报
        /// </summary>
        SubShipMonthReport = 5,

        /// <summary>
        /// 自有船舶月报
        /// </summary>
        OwnerShipMonthReport = 6,

        /// <summary>
        /// 项目日报
        /// </summary>
        DayReport = 7,

        /// <summary>
        /// 安监日报
        /// </summary>
        SafeDayReport = 8,

        /// <summary>
        /// 船舶日报
        /// </summary>
        ShipDayReport = 9,

        /// <summary>
        /// 月报特殊字段推送
        /// </summary>
        MonthReportPushSpecialFields = 10,

        /// <summary>
        /// 项目审批人
        /// </summary>
        ProjectApprover = 11,

        /// <summary>
        /// 分包船舶
        /// </summary>
        SubShip = 12
    }

    /// <summary>
    /// 复工状态（1：春节未复工，2：未开工或停工未复工，3：已复工，4：已完工）
    /// </summary>
    public enum SafeSupervisionWorkStatus
    {

        /// <summary>
        /// 春节未复工
        /// </summary>
        [Description("春节未复工")]
        UnWorkOfSpringFestival = 1,

        /// <summary>
        /// 未开工或停工未复工
        /// </summary>
        [Description("未开工或停工未复工")]
        UnWorkOfResuming = 2,

        /// <summary>
        /// 已复工
        /// </summary>
        [Description("已复工")]
        WorkOfResuming = 3,

        /// <summary>
        /// 已完工
        /// </summary>
        [Description("已完工")]
        WorkOfFinished = 4
    }

    /// <summary>
    /// 上级督察形式（1：远程督查，2：现场督查）
    /// </summary>
    public enum SuperiorSupervisionForm
    {

        /// <summary>
        /// 远程督查
        /// </summary>
        [Description("远程督查")]
        CheckedOfRemote = 1,

        /// <summary>
        ///现场督查
        /// </summary>
        [Description("现场督查")]
        CheckedOfSite = 2
    }

    /// <summary>
    /// 安全生产情况（1：安全，2：事故）
    /// </summary>
    public enum SafeMonitoringSituation
    {

        /// <summary>
        /// 安全
        /// </summary>
        [Description("平安")]
        Safe = 1,

        /// <summary>
        /// 事故
        /// </summary>
        [Description("事故")]
        Accident = 2
    }

    /// <summary>
    /// 任务状态（1：待办，2：已办）
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 待办
        /// </summary>
        UnHandle = 1,

        /// <summary>
        /// 已办
        /// </summary>
        Handled = 2

    }
    /// <summary>
    /// 审批状态（1：驳回，2：通过  3:撤回）
    /// </summary>
    public enum JobApproveStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        None = 0,

        /// <summary>
        /// 审核驳回
        /// </summary>
        [Description("驳回")]
        Reject = 1,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("通过")]
        Pass = 2,
        /// <summary>
        /// 一级审批撤回
        /// </summary>
        [Description("撤回")]
        Revoca = 3
    }

    /// <summary>
    /// 审批层级（0，无审批，1：一级审批，2：二级审批，3：三级审批）
    /// </summary>
    public enum ApproveLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("")]
        None = 0,

        /// <summary>
        /// 一级审批
        /// </summary>
        [Description("一级审批")]
        Level1 = 1,

        /// <summary>
        /// 二级审批
        /// </summary>
        [Description("二级审批")]
        Level2 = 2,

        /// <summary>
        /// 三级审批
        /// </summary>
        [Description("三级审批")]
        Level3 = 3,

        /// <summary>
        /// 四级审批
        /// </summary>
        [Description("四级审批")]
        Level4 = 4,

        /// <summary>
        /// 五级审批
        /// </summary>
        [Description("五级审批")]
        Level5 = 5

    }

    /// <summary>
    /// 业务模块（1：项目结构,2:项目月报）
    /// </summary>
    public enum BizModule
    {
        /// <summary>
        /// 项目结构
        /// </summary>
        [Description("项目结构")]
        ProjectWBS = 1,

        /// <summary>
        /// 项目月报
        /// </summary>
        [Description("项目月报")]
        MonthReport = 2
    }

    /// <summary>
    /// 日报-春节期间停工计划（1：春节期间不停工，2：春节期间停工）
    /// </summary>
    public enum WorkStatusOfSpringFestival
    {
        /// <summary>
        /// 无效值
        /// </summary>
        None = 0,

        /// <summary>
        /// 春节期间不停工
        /// </summary>
        Working = 1,

        /// <summary>
        /// 春节期间停工
        /// </summary>
        StopWork = 2
    }

    /// <summary>
    /// 项目-船舶状态(0:施工,1:调遣,2:厂修,3:待命)
    /// </summary>
    public enum ProjectShipState
    {
        /// <summary>
        /// 施工
        /// </summary>
        [Description("施工")]
        Construction = 0,

        /// <summary>
        /// 调遣
        /// </summary>
        [Description("调遣")]
        Dispatch = 1,

        /// <summary>
        /// 厂修
        /// </summary>
        [Description("厂修")]
        Repair = 2,

        /// <summary>
        /// 待命
        /// </summary>
        [Description("待命")]
        Standby = 3,
        /// <summary>
        /// 航修
        /// </summary>
        [Description("航修")]
        VoyageRepair = 4,

        /// <summary>
        /// 检修
        /// </summary>
        [Description("检修")]
        OverHaul = 5,

    }

    /// <summary>
    /// 动态船舶状态(1:调遣,2:厂修,3:待命,4:航修)
    /// </summary>
    public enum DynamicShipState
    {

        /// <summary>
        /// 调遣
        /// </summary>
        [Description("调遣")]
        Dispatch = 1,

        /// <summary>
        /// 厂修
        /// </summary>
        [Description("厂修")]
        ShopRepair = 2,

        /// <summary>
        /// 待命
        /// </summary>
        [Description("待命")]
        Standby = 3,

        /// <summary>
        /// 航修
        /// </summary>
        [Description("航修")]
        VoyageRepair = 4

    }

    /// <summary>
    /// 偏差原因(1:甲方原因,2:我方原因,3:不可抵抗,4:无)
    /// </summary>
    public enum DeviationReason
    {
        /// <summary>
        /// 甲方原因
        /// </summary>
        [Description("甲方原因")]
        PartAReason = 1,

        /// <summary>
        /// 我方原因
        /// </summary>
        [Description("我方原因")]
        MeReason = 2,

        /// <summary>
        /// 不可抵抗
        /// </summary>
        [Description("不可抵抗")]
        ForceMajeureReason = 3,

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        NoReason = 4
    }

    /// <summary>
    /// 船舶日报类型
    /// </summary>
    public enum ShipDayReportType
    {
        /// <summary>
        /// 项目-船舶日报
        /// </summary>
        ProjectShip = 1,

        /// <summary>
        /// 动态-船舶日报
        /// </summary>
        DynamicShip = 2
    }
    /// <summary>
    /// 日志系统模块类型
    /// </summary>
    public enum LogSystemModuleType
    {
        /// <summary>
        /// 项目信息清单
        /// </summary>
        t_project = 1,

        /// <summary>
        /// 船舶进出场
        /// </summary>
        t_shipmovement = 2,

        /// <summary>
        /// 项目组织结构
        /// </summary>
        t_projectwbs = 3,
        /// <summary>
        /// 在建产值日报
        /// </summary>
        t_dayreport = 4,
        /// <summary>
        /// 安监日报
        /// </summary>
        t_safesupervisiondayreport = 5,
        /// <summary>
        /// 船舶日报
        /// </summary>
        t_shipdayreport = 6,
        /// <summary>
        /// 项目月报
        /// </summary>
        t_monthreport = 7,
        /// <summary>
        /// 自有船舶月报
        /// </summary>
        t_ownershipmonthreport = 8,
        /// <summary>
        /// 分包船舶月报
        /// </summary>
        t_subshipmonthreport = 9,
        /// <summary>
        /// 分包船舶编辑
        /// </summary>
        t_subship = 10
    }

    /// <summary>
    /// 字典数据类型（4：天气，7：施工性质，8：客户自定义船舶，9：船舶-动态描述，10：合同清单）
    /// </summary>
    public enum DictionaryTypeNo
    {
        /// <summary>
        /// 天气
        /// </summary>
        Weather = 4,

        /// <summary>
        /// 客户自定义船舶
        /// </summary>
        CustomShip = 8,

        /// <summary>
        /// 施工性质
        /// </summary>
        ConstructionNature = 7,

        /// <summary>
        /// 船舶-动态描述
        /// </summary>
        ContractDetail = 9,

        /// <summary>
        /// 船舶-动态描述
        /// </summary>
        DynamicDescription = 10
    }

    /// <summary>
    /// 暂存业务类型
    /// </summary>
    public enum StagingBizType
    {
        /// <summary>
        /// 暂存月报
        /// </summary>
        SaveMonthReport = 1,
    }

    /// <summary>
    /// 推送状态
    /// </summary>
    public enum PushStatus
    {
        /// <summary>
        /// 未推送
        /// </summary>
        UnPush = 1,

        /// <summary>
        /// 推送失败
        /// </summary>
        PushFail = 2,

        /// <summary>
        /// 推送成功
        /// </summary>
        PushSucess = 3,

        /// <summary>
        ///推送 验证失败
        /// </summary>
        PushVerifyFail = 4
    }
    /// <summary>
    /// 船机成本类型枚举
    /// </summary>
    public enum ShipCostType
    {
        /// <summary>
        /// 2万方耙
        /// </summary>
        [Description("2万方耙")]
        TwoWanFangPa = 1,

        /// <summary>
        /// 万方耙
        /// </summary>
        [Description("万方耙")]
        WanFangPa = 2,

        /// <summary>
        /// 5000方耙
        /// </summary>
        [Description("5000方耙")]
        FiveThousand = 3,

        /// <summary>
        /// 8527
        /// </summary>
        [Description("8527")]
        EightFiveTwoSeven = 4,

        /// <summary>
        /// 7024
        /// </summary>
        [Description("7025")]
        SevenZeroTwoFive = 5,

        /// <summary>
        /// 抓斗船
        /// </summary>
        [Description("抓斗船")]
        ZhuaDou = 6,

        /// <summary>
        /// 200方抓斗船
        /// </summary>
        [Description("200方抓斗船")]
        TwoThousandFangZhuaDou = 7
    }

    /// <summary>
    /// 日期时间段类型
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// 本月
        /// </summary>
        thisMonth = 0,
        /// <summary>
        /// 上月
        /// </summary>
        lastMonth = 1
    }

    /// <summary>
    /// 项目月报状态
    /// </summary>
    public enum MonthReportStatus
    {
        /// <summary>
        /// 撤回
        /// </summary>
        Revoca = -1,
        /// <summary>
        /// 未知状态
        /// </summary>
        None = 0,

        /// <summary>
        /// 审批中
        /// </summary>
        Approveling = 1,

        /// <summary>
        /// 审批驳回
        /// </summary>
        ApproveReject = 2,


        /// <summary>
        /// 完成
        /// </summary>
        Finish = 3
    }

    /// <summary>
    /// 节假日类型（元旦：101，春节：121，清明：405，劳动：501，端午：505，中秋：815,国庆：1001）
    /// </summary>
    public enum HolidayType
    {

        /// <summary>
        /// 元旦
        /// </summary>
        YuanDan = 101,

        /// <summary>
        /// 春节
        /// </summary>
        ChunJie = 121,

        /// <summary>
        /// 清明
        /// </summary>
        QingMing = 405,

        /// <summary>
        /// 劳动
        /// </summary>
        LaoDong = 501,

        /// <summary>
        /// 端午
        /// </summary>
        DuanWu = 505,

        /// <summary>
        /// 中秋
        /// </summary>
        ZhongQiu = 815,

        /// <summary>
        /// 国庆
        /// </summary>
        GuoQing = 1001
    }



    /// <summary>
    /// 修理船舶类型（此枚举类型只有 装备管理下的修理备件管理模块使用）
    /// </summary>
    public enum RepairShipType
    {
        /// <summary>
        /// 临修
        /// </summary>
        [Description("临修")]
        TemRepair = 1,
        /// <summary>
        /// 航修
        /// </summary>
        [Description("航修")]
        VoyageRepair = 2,
        /// <summary>
        /// 小修
        /// </summary>
        [Description("小修")]
        MinorRepair = 3,
        /// <summary>
        /// 检修
        /// </summary>
        [Description("检修")]
        OverHaul = 4
    }

    /// <summary>
    /// 授权模式
    /// </summary>
    public enum AuthorizeMode
    {
        /// <summary>
        /// 授权一天
        /// </summary>
        [Description("授权一天")]
        AuthorizeOneDay = 1,


        /// <summary>
        /// 授权永不过期
        /// </summary>
        [Description("授权永不过期")]
        AuthorizeForever = 1000
    }
    /// <summary>
    /// 值类型
    /// </summary>
    public enum ValueEnumType
    {
        /// <summary>
        /// 初始值（初始wbs值）
        /// </summary>
        [Description("初始值")]
        None = 0,
        /// <summary>
        /// 当月
        /// </summary>
        [Description("当月")]
        NowMonth = 1,
        /// <summary>
        /// 当年
        /// </summary>
        [Description("当年")]
        NowYear = 2,
        /// <summary>
        /// 开累
        /// </summary>
        [Description("开累")]
        AccumulatedCommencement = 3
    }
}
