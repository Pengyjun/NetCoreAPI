using GDCMasterDataReceiveApi.Domain.OtherModels;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 境内行政区划
    /// </summary>
    [SugarTable("t_administrativedivision", IsDisabledDelete = true)]
    public class AdministrativeDivision : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 行政区划代码:业务主键
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "RegionalismCode")]
        public string? ZADDVSCODE { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Name")]
        public string? ZADDVSNAME { get; set; }
        /// <summary>
        /// 上级行政区划代码:第1级行政区划无上级代码。
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "SupRegionalismCode")]
        public string? ZADDVSUP { get; set; }
        /// <summary>
        /// 行政区域级别:总共3级，省、直辖市、自治区是第1级。
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "RegionalismLevel")]
        public string? ZADDVSLEVEL { get; set; }
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CodeOfCCCCRegional")]
        public string? ZCRHCODE { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnName = "Version")]
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：1是已启用，0是已停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }
    }
}
