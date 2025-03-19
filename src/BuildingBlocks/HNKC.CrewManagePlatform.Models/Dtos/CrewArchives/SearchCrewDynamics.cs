using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 船员动态
    /// </summary>
    public class SearchCrewDynamics
    {
        /// <summary>
        /// 用户业务id
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public Guid? Country { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoardName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? OnStatus { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public CrewStatusEnum DeleteResonEnum { get; set; }
        /// <summary>
        /// 最近上船时间 列表展示
        /// </summary>
        public string BoardingTime { get; set; }
        /// <summary>
        /// 最近下船时间 列表展示
        /// </summary>
        public string? DisembarkTime { get; set; }
        /// <summary>
        /// 最近上船时间 逻辑应用
        /// </summary>
        public DateTime LBoardingTime { get; set; }
        /// <summary>
        /// 最近下船时间 逻辑应用
        /// </summary>
        public DateTime? LDisembarkTime { get; set; }
        /// <summary>
        /// 在船天数
        /// </summary>
        public int OnBoardDays { get; set; }
        /// <summary>
        /// 休假天数
        /// </summary>
        public int HolidayDays { get; set; }
        /// <summary>
        /// 上船次数
        /// </summary>
        public int BoardingNums { get; set; }
        /// <summary>
        /// 下船次数
        /// </summary>
        public int DisembarkNums { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string? CardId { get; set; }
    }
}
