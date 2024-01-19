using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{
    /// <summary>
    /// 首页菜单相关Dto
    /// </summary>
    public class SearchHomePageMenuResponseDto
    {
        public SearchHomePageMenuResponseDto()
        {
            this.OutputColumn = new List<ColumnInfo>();
            this.ShipColumn = new List<ColumnInfo>();
            this.SafetySupervision = new List<ColumnInfo>();

        }
        /// <summary>
        /// 产值列
        /// </summary>
        public List<ColumnInfo> OutputColumn { get; set; }
        /// <summary>
        /// 船舶列
        /// </summary>
        public List<ColumnInfo?> ShipColumn { get; set; }
        /// <summary>
        /// 安监列
        /// </summary>
        public List<ColumnInfo> SafetySupervision { get; set; }

    }
    /// <summary>
    /// 菜单Dto列中对象信息
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuName { get; set; }
        /// <summary>
        /// 填报状态
        /// </summary>
        public int? FillingStatus { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string? Prompt { get; set; }
        /// <summary>
        /// 项目类型提示   是非施工类类型为true
        /// </summary>
        public bool? TypePrompt { get; set; } = false;

    }
}
