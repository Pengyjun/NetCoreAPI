using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    #region 船员值班列表
    /// <summary>
    /// 船员值班列表
    /// </summary>
    public class SearchCrewRota
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public Guid? Country { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public Guid? Company { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 船长
        /// </summary>
        public string? Captain { get; set; }
        /// <summary>
        /// 书记
        /// </summary>
        public string? Secretary { get; set; }
        /// <summary>
        /// 轮机长
        /// </summary>
        public string? ChiefEngineer { get; set; }
        /// <summary>
        /// 排班更新时间
        /// </summary>
        public DateTime? SchedulingTime { get; set; }
    }
    /// <summary>
    /// 船员值班列表
    /// </summary>
    public class SearchCrewRotaRequest : PageRequest
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
    }
    /// <summary>
    /// 值班详情
    /// </summary>
    public class CrewRotaDetails
    {
        /// <summary>
        /// 顶部
        /// </summary>
        public HeaderTtitle? TopTitle { get; set; }
        /// <summary>
        /// 甲板部
        /// </summary>
        public List<CrewRotaDetailsDto>? JiaBanDetails { get; set; }
        /// <summary>
        /// 轮机部
        /// </summary>
        public List<CrewRotaDetailsDto>? LunJiDetails { get; set; }
    }
    /// <summary>
    /// 头部
    /// </summary>
    public class HeaderTtitle
    {
        /// <summary>
        /// 船长 名称
        /// </summary>
        public string? CaptionName { get; set; }
        /// <summary>
        /// 船长
        /// </summary>
        public string? Caption { get; set; }
        /// <summary>
        /// 书记  名称
        /// </summary>
        public string? SecretaryName { get; set; }
        /// <summary>
        /// 轮机长 名称
        /// </summary>
        public string? ChiefEngineerName { get; set; }
        /// <summary>
        /// 轮机长
        /// </summary>
        public string? ChiefEngineer { get; set; }
        /// <summary>
        /// 书记
        /// </summary>
        public string? Secretary { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalNums { get; set; }
        /// <summary>
        /// 在岗数
        /// </summary>
        public int OnDutyNums { get; set; }
        /// <summary>
        /// 休假数
        /// </summary>
        public int HolidaysNums { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
    }
    /// <summary>
    /// 甲板部
    /// </summary>
    public class CrewRotaDetailsDto
    {
        /// <summary>
        /// 固定时间区间类型 1：0-4 2：4-8 3：8-12 / 班组1 2 3
        /// </summary>
        public string? TimeSlotTypeName { get; set; }
        /// <summary>
        /// 领导1
        /// </summary>
        public string? FLeaderUserName { get; set; }
        /// <summary>
        /// 领导2
        /// </summary>
        public string? SLeaderUserName { get; set; }
        /// <summary>
        /// 其他人员 ,拼接
        /// </summary>
        public string? OhterUserName { get; set; }
        /// <summary>
        ///非班人员
        /// </summary>
        public List<FeiBanDetails>? FeiBanUsers { get; set; }
    }
    /// <summary>
    /// 非班
    /// </summary>
    public class FeiBanDetails
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? PositionName { get; set; }
    }
    #endregion

    #region 排班列表
    /// <summary>
    /// 排班列表 入参
    /// </summary>
    public class SchedulingRequest
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 固定/非固定时间
        /// </summary>
        public TimeEnum TimeType { get; set; }
    }
    /// <summary>
    /// 排班列表回显
    /// </summary>
    public class SearchScheduling
    {
        /// <summary>
        ///甲板部
        /// </summary>
        public List<SearchSchedulingDto>? JiaBanSchedulings { get; set; }
        /// <summary>
        /// 轮机部
        /// </summary>
        public List<SearchSchedulingDto>? LunJichedulings { get; set; }
    }
    /// <summary>
    /// 排班列表
    /// </summary>
    public class SearchSchedulingDto
    {
        /// <summary>
        /// 固定时间区间
        /// </summary>
        public TimeSoltEnum TimeSlotType { get; set; }
        /// <summary>
        /// 固定时间区间
        /// </summary>
        public string? TimeSlotTypeName { get; set; }
        /// <summary>
        /// 班组名称 
        /// </summary>
        public string? TeamGroup { get; set; }
        /// <summary>
        /// 班组排序 
        /// </summary>
        public int TeamGroupDesc { get; set; }
        /// <summary>
        /// 领导1
        /// </summary>
        public Guid? FLeaderUserId { get; set; }
        /// <summary>
        /// 领导2
        /// </summary>
        public Guid? SLeaderUserId { get; set; }
        /// <summary>
        /// 其他人员 ,拼接
        /// </summary>
        public string? OhterUserId { get; set; }
    }

    /// <summary>
    /// 保存排班列表
    /// </summary>
    public class SaveSchedulingRequest
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 固定/非固定时间
        /// </summary>
        public TimeEnum TimeType { get; set; }
        /// <summary>
        /// 船舶排班列表
        /// </summary>
        public List<SaveSchedulingDto>? SaveScheduling { get; set; }
    }
    /// <summary>
    /// 排班列表
    /// </summary>
    public class SaveSchedulingDto
    {
        /// <summary>
        /// 部门类型
        /// </summary>
        public RotaEnum RotaType { get; set; }
        /// <summary>
        /// 固定时间区间类型 1：0-4 2：4-8 3：8-12
        /// </summary>
        public TimeSoltEnum TimeSlotType { get; set; }
        /// <summary>
        /// 班组名称  班组1  班组2  类推
        /// </summary>
        public string? TeamGroup { get; set; }
        /// <summary>
        /// 班组排序  如果是班组1 传1 班组2 传2  类推
        /// </summary>
        public int TeamGroupDesc { get; set; }
        /// <summary>
        /// 领导1
        /// </summary>
        public Guid? FLeaderUserId { get; set; }
        /// <summary>
        /// 领导2
        /// </summary>
        public Guid? SLeaderUserId { get; set; }
        /// <summary>
        /// 其他人员 ,拼接
        /// </summary>
        public string? OhterUserId { get; set; }
    }
    /// <summary>
    /// 排班用户列表
    /// </summary>
    public class SchedulingUser
    {
        /// <summary>
        /// 人员
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Position { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? PositionName { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public RotaEnum RotaEnum { get; set; }
    }
    #endregion
}
