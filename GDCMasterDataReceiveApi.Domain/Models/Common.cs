using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 通用类字典数据
    /// </summary>
    [SugarTable("t_common", IsDisabledDelete = true)]
    public class Common : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "SystemCode")]
        public string ZCODE { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "SystemName")]
        public string ZSYSNAME { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "AffiliatedUnit")]
        public string ZNDORGN { get; set; }
        /// <summary>
        /// 所属业务部门
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BUnit")]
        public string ZNDEPART { get; set; }
        /// <summary>
        /// 业务对接人
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BLiaisonPerson")]
        public string ZLKPERSON { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "PjectManager")]
        public string ZPMANAGER { get; set; }
        /// <summary>
        /// 数字化管理部门
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "DigitalManagementDep")]
        public string ZIADMDEPART { get; set; }
        /// <summary>
        /// 管理部门负责人
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "HeadOfManagementDept")]
        public string ZIDEPERSON { get; set; }
        /// <summary>
        /// 系统概述
        /// </summary>
        [SugarColumn(Length = 5000, ColumnName = "SystemOverview")]
        public string ZSYSDESC { get; set; }
        /// <summary>
        /// 是否有效:有效：1 无效：2
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "ValidOrNot")]
        public string ZVALID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CreateDate")]
        public string? ZCRDATE { get; set; }
        /// <summary>
        /// 最后修改时间 
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "LastModified")]
        public string? ZCHDATE { get; set; }
    }
}
