using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.ShipDuty
{
    /// <summary>
    /// 船舶值班列表
    /// </summary>
    public class ShipDutyListResponseDto
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
        /// 所在国家
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 在岗人数
        /// </summary>
        public int OnDuty { get; set; }
        /// <summary>
        /// 休假人数
        /// </summary>
        public int Holiday { get; set; }
        /// <summary>
        /// 领导信息
        /// </summary>
        public LeaderInfo leaderInfo { get; set; } = new LeaderInfo();
        /// <summary>
        /// 甲板部与轮机部
        /// </summary>
        public List<DutyPerson> dutyPeople { get; set; } = new List<DutyPerson>();
        /// <summary>
        /// 非值班 甲板部与轮机部
        /// </summary>
        public List<OffDutyPerson> offDutyPeople { get; set; } = new List<OffDutyPerson>();
    }
    /// <summary>
    /// 领导信息
    /// </summary>
    public class LeaderInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? JobType { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
    }
    /// <summary>
    /// 值班人员
    /// </summary>
    public class DutyPerson
    {
        /// <summary>
        /// 人员1
        /// </summary>
        public string Person1 { get; set; }
        /// <summary>
        /// 人员2
        /// </summary>
        public string Person2 { get; set; }
        /// <summary>
        /// 休假人员
        /// </summary>
        public string HolidayPerson { get; set; }
        /// <summary>
        /// 其他人员
        /// </summary>
        public string OtherPerson { get; set; }
    }
    /// <summary>
    /// 非值班人员
    /// </summary>
    public class OffDutyPerson
    {
        /// <summary>
        /// 人员1
        /// </summary>
        public string Person1 { get; set; }
        /// <summary>
        /// 休假人员
        /// </summary>
        public string HolidayPerson { get; set; }
    }
}
