﻿using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH委托关系
    /// </summary>
    [SugarTable("t_dh_mdmmultorgagencyrelpage", IsDisabledDelete = true)]
    public class DHMdmMultOrgAgencyRelPage : BaseEntity<long>
    {
        /// <summary>
        /// 机构主数据编码  主键
        /// </summary>
        public string? MdmCode { get; set; }
        /// <summary>
        /// 委托单位明细行项目  主键
        /// </summary>
        public string? Znumc4x { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。主键
        /// </summary>
        public string? ZmviewFlag { get; set; }
        /// <summary>
        /// 委托单位编码
        /// </summary>
        public string? ZdelegateOrg { get; set; }
        /// <summary>
        /// 委托状态 1-启用0-停用
        /// </summary>
        public string? ZdelegateState { get; set; }
        /// <summary>
        /// 组织树编码 1-组织机构树，2-核算组织树，3-管理组织树
        /// </summary>
        public string? Ztreeid { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        public string? Ztreever { get; set; }
    }
}
