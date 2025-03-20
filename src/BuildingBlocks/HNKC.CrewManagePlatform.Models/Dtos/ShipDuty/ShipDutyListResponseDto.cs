using HNKC.CrewManagePlatform.Models.Enums;
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
        public List<LeaderInfo> leaderInfo { get; set; } = new List<LeaderInfo>();
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
        /// 用户id
        /// </summary>
        public string? UserId { get; set; }
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
        /// 休假人员
        /// </summary>
        public List<UserInfo> HolidayPerson { get; set; }
        /// <summary>
        /// 其他人员
        /// </summary>
        public List<string> OtherPerson { get; set; }
        /// <summary>
        /// 班组信息
        /// </summary>
        public List<TeamsGroup> teamsGroup { get; set; } = new List<TeamsGroup>();
    }
    /// <summary>
    /// 班组信息
    /// </summary>
    public class TeamsGroup
    {
        /// <summary>
        /// 班别  1：0-4 2：4-8 3：8-12  或 班组1 班组2  班组3
        /// </summary>
        public string? TimeslotType { get; set; }
        /// <summary>
        /// 图标1
        /// </summary>
        public string? Icon1 { get; set; }
        /// <summary>
        /// 图标2
        /// </summary>
        public string? Icon2 { get; set; }
        /// <summary>
        /// 用户id1
        /// </summary>
        public string? UserId1 { get; set; }
        /// <summary>
        /// 用户id2
        /// </summary>
        public string? UserId2 { get; set; }
        /// <summary>
        /// 人员1
        /// </summary>
        public string Person1 { get; set; }
        /// <summary>
        /// 人员2
        /// </summary>
        public string Person2 { get; set; }
    }

    /// <summary>
    /// 非值班人员
    /// </summary>
    public class OffDutyPerson
    {
        /// <summary>
        /// 人员1
        /// </summary>
        public List<UserInfo> Person1 { get; set; } = new List<UserInfo>();
        /// <summary>
        /// 休假人员
        /// </summary>
        public List<UserInfo> HolidayPerson { get; set; } = new List<UserInfo>();
    }
    public class UserInfo
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
