using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 暂存数据表
    /// </summary>
    /// 
	[SugarTable("t_stagingdata", IsDisabledDelete = true)]
    public class StagingData : BaseEntity<Guid>
    {
        /// <summary>
        /// 暂存业务类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public StagingBizType BizType { get; set; }

        /// <summary>
        /// 暂存业务数据
        /// </summary>
        [SugarColumn(ColumnDataType = "longtext")]
        public string? BizData { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        ///暂存日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        ///暂存月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        ///暂存年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        ///是否生效
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsEffectStaging { get; set; }
    }
}
