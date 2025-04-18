﻿using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper
{
    /// <summary>
    /// 行政机构和核算机构映射关系 反显响应dto
    /// </summary>
    public class AdministrativeAccountingMapperSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? AccOrgId { get; set; }
        /// <summary>
        /// 核算组织编码
        /// </summary>
        public string? AccOrgCode { get; set; }
        /// <summary>
        /// 行政组织编码
        /// </summary>
        public string? AdministrativeOrgCode { get; set; }
    }
    /// <summary>
    /// 行政机构和核算机构映射关系 明细
    /// </summary>
    public class AdministrativeAccountingMapperDetailsDto
    {
        public string? Id { get; set; }
        /// <summary>
        /// 必须唯一
        /// </summary>
        public string? KeyId { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        [DisplayName("核算组织ID")]
        public string? AccOrgId { get; set; }
        /// <summary>
        /// 核算组织编码
        /// </summary>
        [DisplayName("核算组织编码")]
        public string? AccOrgCode { get; set; }
        /// <summary>
        /// 行政组织ID
        /// </summary>
        [DisplayName("行政组织ID")]
        public string? AdministrativeOrgId { get; set; }
        /// <summary>
        /// 行政组织编码
        /// </summary>
        [DisplayName("行政组织编码")]
        public string? AdministrativeOrgCode { get; set; }
        /// <summary>
        /// 是否删除: 数据是否有效的标识:   有效：1无效：0
        /// </summary>
        [DisplayName("是否删除")]
        public string? DataIdentifier { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 行政机构和核算机构映射关系接收
    /// </summary>
    public class AdministrativeAccountingMapperItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 主键ID，必须唯一
        /// </summary>
        public string? ZID { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? ZAID { get; set; }
        /// <summary>
        /// 核算组织编码
        /// </summary>
        public string? ZAORGNO { get; set; }
        /// <summary>
        /// 行政组织ID
        /// </summary>
        public string? ZORGID { get; set; }
        /// <summary>
        /// 行政组织编码
        /// </summary>
        public string? ZORGCODE { get; set; }
        /// <summary>
        /// 是否删除: 数据是否有效的标识:   有效：1无效：0
        /// </summary>
        public string? ZDELETE { get; set; }
    }
}
