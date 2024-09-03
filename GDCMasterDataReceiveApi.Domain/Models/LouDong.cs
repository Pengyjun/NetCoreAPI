using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 楼栋
    /// </summary>
    [SugarTable("t_loudong", IsDisabledDelete = true)]
    public class LouDong : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 楼栋编码：楼栋主数据编码由1位字母和4位数字组成，采用标识符+流水号的编码规则,如：B0001
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Name")]
        public string ZBLDG_NAME { get; set; }
        /// <summary>
        /// 楼栋名称：同一分期（子项目）下的楼栋唯一标识，同一个分期（子项目）下楼栋名称不允许重复
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "Code")]
        public string ZBLDG { get; set; }
        /// <summary>
        /// 业态信息 ：该楼栋的业态，可多值，需选择末级；通用类字典102接口下发 多值用英文逗号隔开
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BFormat")]
        public string ZFORMATINF { get; set; }
        /// <summary>
        /// 项目主数据编码 ：该楼栋所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "PjectMDCode")]
        public string ZPROJECT { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "SourceSystem")]
        public string ZSYSTEM { get; set; }
    }
}
