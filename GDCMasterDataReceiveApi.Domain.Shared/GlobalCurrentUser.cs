﻿namespace GDCMasterDataReceiveApi.Domain.Shared
{

    /// <summary>
    /// 全局用户信息对象
    /// </summary>
    public class GlobalCurrentUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        public string? InstitutionId { get; set; }
        /// <summary>
        /// 机构OID
        /// </summary>
        public string? InstitutionOid { get; set; }
        /// <summary>
        /// 机构Poid
        /// </summary>
        public string? InstitutionPOid { get; set; }
        /// <summary>
        /// 系统APPkey
        /// </summary>

        public string? AppKey { get; set; }
        /// <summary>
        /// 接口授权码
        /// </summary>
        public string? AppinterfaceCode { get; set; }
    }
}
