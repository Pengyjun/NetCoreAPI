using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.POPManagOrg
{
    /// <summary>
    /// 生产经营管理组织列表
    /// </summary>
    public class POPManagOrgSearchDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树月度版本
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 快照组织树版本
        /// </summary>
        public string? ZSHOTTVER { get; set; }
        /// <summary>
        /// 组织树名称
        /// </summary>
        public string? ZTREENAME { get; set; }
        /// <summary>
        /// 组织树状态 0：审批中，1：审批通过，6：删除
        /// </summary>
        public string? ZTREESTAT { get; set; }
        /// <summary>
        /// 版本激活日期
        /// </summary>
        public string? ZACTIVE_DATE { get; set; }
        /// <summary>
        /// 删除标记 1:删除0：正常　
        /// </summary>
        public string? ZDELETE { get; set; }
    }
    /// <summary>
    /// 生产经营管理组织明细
    /// </summary>
    public class POPManagOrgDeatilsDto
    {
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树月度版本
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 快照组织树版本
        /// </summary>
        public string? ZSHOTTVER { get; set; }
        /// <summary>
        /// 组织树名称
        /// </summary>
        public string? ZTREENAME { get; set; }
        /// <summary>
        /// 组织树状态 0：审批中，1：审批通过，6：删除
        /// </summary>
        public string? ZTREESTAT { get; set; }
        /// <summary>
        /// 版本激活日期
        /// </summary>
        public string? ZACTIVE_DATE { get; set; }
        /// <summary>
        /// 删除标记 1:删除0：正常　
        /// </summary>
        public string? ZDELETE { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class POPMangOrgItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树月度版本
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 快照组织树版本
        /// </summary>
        public string? ZSHOTTVER { get; set; }
        /// <summary>
        /// 组织树名称
        /// </summary>
        public string? ZTREENAME { get; set; }
        /// <summary>
        /// 组织树状态 0：审批中，1：审批通过，6：删除
        /// </summary>
        public string? ZTREESTAT { get; set; }
        /// <summary>
        /// 版本激活日期
        /// </summary>
        public string? ZACTIVE_DATE { get; set; }
        /// <summary>
        /// 删除标记 1:删除0：正常　
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 组织树明细
        /// </summary>
        public POPManagOrgModels? ZACO_LIST { get; set; }
    }
}
