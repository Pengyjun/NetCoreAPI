namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Common
{
    /// <summary>
    /// 通用类字典数据 反显
    /// </summary>
    public class CommonDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        public string AffiliatedUnit { get; set; }
        /// <summary>
        /// 所属业务部门
        /// </summary>
        public string BUnit { get; set; }
        /// <summary>
        /// 业务对接人
        /// </summary>
        public string BLiaisonPerson { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        public string PjectManager { get; set; }
        /// <summary>
        /// 数字化管理部门
        /// </summary>
        public string DigitalManagementDep { get; set; }
        /// <summary>
        /// 管理部门负责人
        /// </summary>
        public string HeadOfManagementDept { get; set; }
        /// <summary>
        /// 系统概述
        /// </summary>
        public string SystemOverview { get; set; }
        /// <summary>
        /// 是否有效:有效：1 无效：2
        /// </summary>
        public string ValidOrNot { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string? CreateDate { get; set; }
        /// <summary>
        /// 最后修改时间 
        /// </summary>
        public string? LastModified { get; set; }
    }
    /// <summary>
    /// 通用类字典数据 接收
    /// </summary>
    public class CommonReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        public string ZCODE { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string ZSYSNAME { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        public string ZNDORGN { get; set; }
        /// <summary>
        /// 所属业务部门
        /// </summary>
        public string ZNDEPART { get; set; }
        /// <summary>
        /// 业务对接人
        /// </summary>
        public string ZLKPERSON { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        public string ZPMANAGER { get; set; }
        /// <summary>
        /// 数字化管理部门
        /// </summary>
        public string ZIADMDEPART { get; set; }
        /// <summary>
        /// 管理部门负责人
        /// </summary>
        public string ZIDEPERSON { get; set; }
        /// <summary>
        /// 系统概述
        /// </summary>
        public string ZSYSDESC { get; set; }
        /// <summary>
        /// 是否有效:有效：1 无效：2
        /// </summary>
        public string ZVALID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string? ZCRDATE { get; set; }
        /// <summary>
        /// 最后修改时间 
        /// </summary>
        public string? ZCHDATE { get; set; }
    }
}
