using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData
{

    /// <summary>
    /// 各类主数据统计数量
    /// </summary>
    public class EachMainDataCountResponseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Day { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public int? MainPersonCount { get; set; }
        /// <summary>
        /// 机构
        /// </summary>
        public int? MainInstitutionCount { get; set; }
        /// <summary>
        /// 往来单位
        /// </summary>
        public int? MainCorresUnitCount { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public int? MainProjectCount { get; set; }
        /// <summary>
        /// 金融机构
        /// </summary>
        public int? MainFinancialInstitutionCount { get; set; }
        /// <summary>
        /// 物资设备分类
        /// </summary>
        public int? MainDeviceClassCodeCount { get; set; }
        /// <summary>
        /// 科研项目
        /// </summary>
        public int? MainScientifiCNoProjectCount { get; set; }
        /// <summary>
        /// 商机项目
        /// </summary>
        public int? MainBusinessNoCpportunityCount { get; set; }
    }
}
