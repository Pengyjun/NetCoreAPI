using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 获取年度休假详情
    /// </summary>
    public class SearchLeaveDetailResponseDto
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public string? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 船舶对应人员信息
        /// </summary>
        public List<LeaveUserInfo> leaveUsers { get; set; } = new List<LeaveUserInfo>();
    }

    public class LeaveUserInfo
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 职务Id
        /// </summary>
        public string? JobTypeId { get; set; }
        /// <summary>
        /// 职务名称
        /// </summary>
        public string JobTypeName { get; set; }
        /// <summary>
        /// 第一适任证书Id
        /// </summary>
        public Guid? CertificateId { get; set; }
        /// <summary>
        /// 第一适任证书名称
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 第一适任证书扫描件
        /// </summary>
        public string CertificateScans { get; set; }
        /// <summary>
        /// 是否在船过年 去年
        /// </summary>
        public bool IsOnShipLastYear { get; set; }
        /// <summary>
        /// 是否在船过年 当前年
        /// </summary>
        public bool IsOnShipCurrentYear { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 休假日期详情
        /// </summary>
        public List<VacationList> vacationLists { get; set; } = new List<VacationList>();
        /// <summary>
        /// 在船月数
        /// </summary>
        public double OnShipMonth { get; set; }
        /// <summary>
        /// 休假月数
        /// </summary>
        public double LeaveMonth { get; set; }
    }

    /// <summary>
    /// 休假日期详情
    /// </summary>
    public class VacationList
    {
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 上半月  true 休假
        /// </summary>
        public bool FirstHalf { get; set; }
        /// <summary>
        /// 下半月  true 休假
        /// </summary>
        public bool LowerHalf { get; set; }
        /// <summary>
        /// 是否异常  上半
        /// </summary>
        public bool FirstHalfAbnormal { get; set; }
        /// <summary>
        /// 是否异常 下半
        /// </summary>
        public bool LowerHalfAbnormal { get; set; }
    }
}
