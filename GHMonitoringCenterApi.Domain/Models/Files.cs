using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 文件表
    /// </summary>
    [SugarTable("t_files",IsDisabledDelete =true)]
    public class Files:BaseEntity<Guid>
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [SugarColumn(Length =100)]
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? OriginName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? SuffixName { get; set; }
    }
}
