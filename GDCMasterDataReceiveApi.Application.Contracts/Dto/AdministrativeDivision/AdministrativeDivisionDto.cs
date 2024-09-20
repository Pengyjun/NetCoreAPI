using MiniExcelLibs.Attributes;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision
{
    /// <summary>
    /// 境内行政区划 反显dto
    /// </summary>
    public class AdministrativeDivisionSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 行政区划代码:业务主键
        /// </summary>
        public string? RegionalismCode { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 行政区域级别:总共3级，省、直辖市、自治区是第1级。
        /// </summary>
        public string? RegionalismLevel { get; set; }
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        public string? CodeOfCCCCRegional { get; set; }
        /// <summary>
        /// 状态：1是已启用，0是已停用
        /// </summary>
        public string? State { get; set; }
        ///// <summary>
        ///// 多语言描述表类型
        ///// </summary>
        //public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }
    }
    /// <summary>
    /// 境内行政区划 详细
    /// </summary>
    public class AdministrativeDivisionDetailsDto
    {
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 行政区划代码:业务主键
        /// </summary>
        [ExcelColumnName("行政区划代码")]
        public string? RegionalismCode { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [ExcelColumnName("行政区划名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 上级行政区划代码:第1级行政区划无上级代码。
        /// </summary>
        [ExcelColumnName("上级行政区划代码")]
        public string? SupRegionalismCode { get; set; }
        /// <summary>
        /// 行政区域级别:总共3级，省、直辖市、自治区是第1级。
        /// </summary>
        [ExcelColumnName("行政区域级别")]
        public string? RegionalismLevel { get; set; }
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        [ExcelColumnName("中交区域总部代码")]
        public string? CodeOfCCCCRegional { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 状态：1是已启用，0是已停用
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? DataIdentifier { get; set; }
        ///// <summary>
        ///// 多语言描述表类型
        ///// </summary>
        //public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }
    }
    ///境内行政区划 接收响应dto
    public class AdministrativeDivisionItem
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 行政区划代码:业务主键
        /// </summary>
        public string? ZADDVSCODE { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string? ZADDVSNAME { get; set; }
        /// <summary>
        /// 上级行政区划代码:第1级行政区划无上级代码。
        /// </summary>
        public string? ZADDVSUP { get; set; }
        /// <summary>
        /// 行政区域级别:总共3级，省、直辖市、自治区是第1级。
        /// </summary>
        public string? ZADDVSLEVEL { get; set; }
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        public string? ZCRHCODE { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：1是已启用，0是已停用
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
         public AdministrativeDivisionObj? ZLANG_LIST { get; set; }
    }




    public class AdministrativeDivisionObj
    {
        public List<AdministrativeDivisionLanguageItem> Item { get; set; }
    }
    public class AdministrativeDivisionLanguageItem
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZCODE { get; set; }
    }
}
