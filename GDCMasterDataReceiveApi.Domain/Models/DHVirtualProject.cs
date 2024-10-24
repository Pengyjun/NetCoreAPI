using SqlSugar;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH虚拟项目
    /// </summary>
    [SugarTable("t_dh_virtualproject", IsDisabledDelete = true)]
    public class DHVirtualProject : BaseEntity<long>
    {
        /// <summary>
        /// 虚拟项目主数据编码
        /// </summary>
        [DisplayName("虚拟项目主数据编码")]
        public string? ZVTPROJ { get; set; }
        /// <summary>
        /// 虚拟项目名称
        /// </summary>
        [DisplayName("虚拟项目名称")]
        public string? ZVTPROJN { get; set; }
        /// <summary>
        /// 所属核算组织
        /// </summary>
        [DisplayName("所属核算组织")]
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [DisplayName("所属二级单位")]
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [DisplayName("版本")]
        public string? Zversion { get; set; }
        /// <summary>
        /// 状态 0:停用;1:启用
        /// </summary>
        [DisplayName("状态")]
        public string? ZPSTATE { get; set; }
        /// <summary>
        /// 是否删除 0:已删除;1:未删除
        /// </summary>
        [DisplayName("是否删除")]
        public string? Zdelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string? CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string? UpdatedAt { get; set; }
    }
}
