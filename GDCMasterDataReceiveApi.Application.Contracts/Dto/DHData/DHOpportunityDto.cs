using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// 商机项目
    /// </summary>
    public class DHOpportunityDto
    {
    }
    /// <summary>
    /// 商机项目关联的中标交底项目
    /// </summary>
    public class DHAwardpList
    {
        /// <summary>
        /// 项目所在地 境内项目必填
        /// </summary>
        public string? ZSPROJLOC { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ZAWARDPN { get; set; }
        /// <summary>
        /// 中标交底项目编码
        /// </summary>
        public string? ZAWARDP { get; set; }
        /// <summary>
        /// 是否联合体总项目 0-否，1-是
        /// </summary>
        public string? ZSZCONTPRO { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZSPROTYPE { get; set; }
        /// <summary>
        /// 交底项目二级单位
        /// </summary>
        public string? ZAP2NDORG { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZSCPBC { get; set; }
        /// <summary>
        /// 中标资质单位
        /// </summary>
        public string? ZAWARDORG { get; set; }
        /// <summary>
        /// 状态 有效：1无效：0
        /// </summary>
        public string? ZSSTATE { get; set; }
    }
}
