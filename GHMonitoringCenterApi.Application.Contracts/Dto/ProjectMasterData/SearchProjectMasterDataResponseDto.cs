using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectMasterData
{
    /// <summary>
    /// 获取项目主数据列表返回Dto
    /// </summary>
    public class SearchProjectMasterDataResponseDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ProjectType { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? ProjectMasterCode { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        public string? Foreign { get; set; }
        /// <summary>
        /// 项目曾用名
        /// </summary>
        public string? BeforeName { get; set; }
        /// <summary>
        /// 项目所在国家/地区
        /// </summary>
        public string? ProjectCountry { get; set; }
        /// <summary>
        /// 项目所在地(城市)
        /// </summary>
        public string? ProjectCity { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? BusinessClassification { get; set; }
    }
}
