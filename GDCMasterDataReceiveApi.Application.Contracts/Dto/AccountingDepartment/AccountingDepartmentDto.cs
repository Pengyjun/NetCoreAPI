namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment
{
    /// <summary>
    /// 核算部门返显响应Dto
    /// </summary>
    public class AccountingDepartmentSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        public string? AccDepCode { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        public string? AccDepTCCName { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        public string? AccDepELName { get; set; }
        /// <summary>
        /// 核算部门停用标志：1:停用0:未停用
        /// </summary>
        public string? State { get; set; }
    }
    /// <summary>
    /// 核算部门详细
    /// </summary>
    public class AccountingDepartmentDetailsDto
    {
        public string? Id { get; set; }
        /// <summary>
        /// 核算组织编号:9月18日新加
        /// </summary>
        public string? AccOrgCode { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        public string? AccDepCode { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? AccOrgId { get; set; }
        /// <summary>
        /// 核算部门ID
        /// </summary>
        public string? AccDepId { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        public string? AccDepTCCName { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        public string? AccDepELName { get; set; }
        /// <summary>
        /// 上级核算部门ID
        /// </summary>
        public string? SupAccDepId { get; set; }
        /// <summary>
        /// 核算部门停用标志：1:停用0:未停用
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 数据删除标识：1:删除0：正常
        /// </summary>
        public string? DataIdentifier { get; set; }
    }
    /// <summary>
    /// 核算部门接收响应Dto
    /// </summary>
    public class AccountingDepartmentReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 核算组织编号:9月18日新加
        /// </summary>
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        public string? ZDCODE { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? ZACID { get; set; }
        /// <summary>
        /// 核算部门ID
        /// </summary>
        public string? ZDID { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        public string? ZDNAME_CHS { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        public string? ZDNAME_CHT { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        public string? ZDNAME_EN { get; set; }
        /// <summary>
        /// 上级核算部门ID
        /// </summary>
        public string? ZDPARENTID { get; set; }
        /// <summary>
        /// 核算部门停用标志：1:停用0:未停用
        /// </summary>
        public string? ZDATSTATE { get; set; }
        /// <summary>
        /// 数据删除标识：1:删除0：正常
        /// </summary>
        public string? ZDELETE { get; set; }
    }
}
