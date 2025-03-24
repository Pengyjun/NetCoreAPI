using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 休假人员用户信息表
    /// </summary>
    [SugarTable("t_leaveplanuser", IsDisabledDelete = true, TableDescription = "休假人员用户信息表")]
    public class LeavePlanUser : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid UserId { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string UserName { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid JobTypeId { get; set; }
        /// <summary>
        /// 第一持有适任证id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CertificateId { get; set; }
        /// <summary>
        /// 是否在船过年 上上年
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsOnShipLastYear { get; set; }
        /// <summary>
        /// 是否在船过年 上一年
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsOnShipCurrentYear { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remark { get; set; }
        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
    }
}
