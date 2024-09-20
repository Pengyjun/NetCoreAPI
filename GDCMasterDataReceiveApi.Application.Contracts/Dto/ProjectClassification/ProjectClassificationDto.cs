using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification
{
    /// <summary>
    /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 反显
    /// </summary>
    public class ProjectClassificationSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 中交业务分类一级分类名称 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 中交业务分类二级分类名称
        /// </summary>
        public string? CCCCBTypeSecName { get; set; }
        /// <summary>
        /// 中交业务分类三级分类名称
        /// </summary>
        public string? CCCCBTypeThirdName { get; set; }
        /// <summary>
        /// 备注（业务分类）
        /// </summary>
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 产业分类一级分类名称 
        /// </summary>
        public string? ChanYeOneName { get; set; }
        /// <summary>
        /// 产业分类二级分类名称
        /// </summary>
        public string? ChanYeSecName { get; set; }
        /// <summary>
        /// 产业分类三级分类名称
        /// </summary>
        public string? ChanYeThirdName { get; set; }
        /// <summary>
        /// 备注（产业分类）
        /// </summary>
        public string? ChanYeRemark { get; set; }
        /// <summary>
        /// 业务板块一级分类名称
        /// </summary>
        public string? BSectorOneName { get; set; }
        /// <summary>
        /// 业务板块二级分类名称
        /// </summary>
        public string? BSectorSecName { get; set; }
        /// <summary>
        /// 业务板块三级分类名称
        /// </summary>
        public string? BSectorThirdName { get; set; }
        /// <summary>
        /// 备注（业务板块）
        /// </summary>
        public string? BSectorRemark { get; set; }
        /// <summary>
        /// 中交十二大业务类型名称
        /// </summary>
        public string? CCCCBTypeName { get; set; }
        /// <summary>
        /// 中交江河湖海分类名称
        /// </summary>
        public string? CCCCRiverLakeAndSeaName { get; set; }
    }
    /// <summary>
    /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 详细
    /// </summary>
    public class ProjectClassificationDetailsDto
    {
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 中交业务分类三级分类代码
        /// </summary>
        [ExcelColumnName("中交业务分类三级分类代码")]
        public string? CCCCBTypeThirdCode { get; set; }
        /// <summary>
        /// 中交业务分类一级分类代码 
        /// </summary>
        [ExcelColumnName("中交业务分类一级分类代码")]
        public string? CCCCBTypeOneCode { get; set; }
        /// <summary>
        /// 中交业务分类一级分类名称 
        /// </summary>
        [ExcelColumnName("中交业务分类一级分类名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 中交业务分类二级分类代码
        /// </summary>
        [ExcelColumnName("中交业务分类二级分类代码")]
        public string? CCCCBTypeSecCode { get; set; }
        /// <summary>
        /// 中交业务分类二级分类名称
        /// </summary>
        [ExcelColumnName("中交业务分类二级分类名称")]
        public string? CCCCBTypeSecName { get; set; }
        /// <summary>
        /// 中交业务分类三级分类名称
        /// </summary>
        [ExcelColumnName("中交业务分类三级分类名称")]
        public string? CCCCBTypeThirdName { get; set; }
        /// <summary>
        /// 备注（业务分类）
        /// </summary>
        [ExcelColumnName("备注（业务分类）")]
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 产业分类一级分类代码
        /// </summary>
        [ExcelColumnName("产业分类一级分类代码")]
        public string? ChanYeOneCode { get; set; }
        /// <summary>
        /// 产业分类一级分类名称 
        /// </summary>
        [ExcelColumnName("产业分类一级分类名称")]
        public string? ChanYeOneName { get; set; }
        /// <summary>
        /// 产业分类二级分类代码
        /// </summary>
        [ExcelColumnName("产业分类二级分类代码")]
        public string? ChanYeSecCode { get; set; }
        /// <summary>
        /// 产业分类二级分类名称
        /// </summary>
        [ExcelColumnName("产业分类二级分类名称")]
        public string? ChanYeSecName { get; set; }
        /// <summary>
        /// 产业分类三级分类名称
        /// </summary>
        [ExcelColumnName("产业分类三级分类名称")]
        public string? ChanYeThirdName { get; set; }
        /// <summary>
        /// 备注（产业分类）
        /// </summary>
        [ExcelColumnName("备注（产业分类）")]
        public string? ChanYeRemark { get; set; }
        /// <summary>
        /// 业务板块一级分类代码
        /// </summary>
        [ExcelColumnName("业务板块一级分类代码")]
        public string? BSectorOneCode { get; set; }
        /// <summary>
        /// 业务板块一级分类名称
        /// </summary>
        [ExcelColumnName("业务板块一级分类名称")]
        public string? BSectorOneName { get; set; }
        /// <summary>
        /// 业务板块二级分类代码
        /// </summary>
        [ExcelColumnName("业务板块二级分类代码")]
        public string? BSectorSecCode { get; set; }
        /// <summary>
        /// 业务板块二级分类名称
        /// </summary>
        [ExcelColumnName("业务板块二级分类名称")]
        public string? BSectorSecName { get; set; }
        /// <summary>
        /// 业务板块三级分类代码
        /// </summary>
        [ExcelColumnName("业务板块三级分类代码")]
        public string? BSectorThirdCode { get; set; }
        /// <summary>
        /// 业务板块三级分类名称
        /// </summary>
        [ExcelColumnName("业务板块三级分类名称")]
        public string? BSectorThirdName { get; set; }
        /// <summary>
        /// 备注（业务板块）
        /// </summary>
        [ExcelColumnName("备注（业务板块）")]
        public string? BSectorRemark { get; set; }
        /// <summary>
        /// 中交十二大业务类型编码 
        /// </summary>
        [ExcelColumnName("中交十二大业务类型编码")]
        public string? CCCCBTypeCode { get; set; }
        /// <summary>
        /// 中交十二大业务类型名称
        /// </summary>
        [ExcelColumnName("中交十二大业务类型名称")]
        public string? CCCCBTypeName { get; set; }
        /// <summary>
        /// 中交江河湖海分类编码
        /// </summary>
        [ExcelColumnName("中交江河湖海分类编码")]
        public string? CCCCRiverLakeAndSeaCode { get; set; }
        /// <summary>
        /// 中交江河湖海分类名称
        /// </summary>
        [ExcelColumnName("中交江河湖海分类名称")]
        public string? CCCCRiverLakeAndSeaName { get; set; }
        /// <summary>
        /// 三新业务类型：取值：0：否、1：是
        /// </summary>
        [ExcelIgnore]
        public string? ThirdNewBType { get; set; }
    }
    /// <summary>
    /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系  接收
    /// </summary>
    public class ProjectClassificationItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 中交业务分类三级分类代码
        /// </summary>
        public string? ZCPBC3ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类代码 
        /// </summary>
        public string? ZCPBC1ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类名称 
        /// </summary>
        public string? ZCPBC1NAME { get; set; }
        /// <summary>
        /// 中交业务分类二级分类代码
        /// </summary>
        public string? ZCPBC2ID { get; set; }
        /// <summary>
        /// 中交业务分类二级分类名称
        /// </summary>
        public string? ZCPBC2NAME { get; set; }
        /// <summary>
        /// 中交业务分类三级分类名称
        /// </summary>
        public string? ZCPBC3NAME { get; set; }
        /// <summary>
        /// 备注（业务分类）
        /// </summary>
        public string? ZZCPBREMARKS { get; set; }
        /// <summary>
        /// 产业分类一级分类代码
        /// </summary>
        public string? ZICSTD1ID { get; set; }
        /// <summary>
        /// 产业分类一级分类名称 
        /// </summary>
        public string? ZICSTD1NAME { get; set; }
        /// <summary>
        /// 产业分类二级分类代码
        /// </summary>
        public string? ZICSTD2ID { get; set; }
        /// <summary>
        /// 产业分类二级分类名称
        /// </summary>
        public string? ZICSTD2NAME { get; set; }
        /// <summary>
        /// 产业分类三级分类名称
        /// </summary>
        public string? ZICSTD3NAME { get; set; }
        /// <summary>
        /// 备注（产业分类）
        /// </summary>
        public string? ZICSTDREMARKS { get; set; }
        /// <summary>
        /// 业务板块一级分类代码
        /// </summary>
        public string? ZBUSTD1ID { get; set; }
        /// <summary>
        /// 业务板块一级分类名称
        /// </summary>
        public string? ZBUSTD1NAME { get; set; }
        /// <summary>
        /// 业务板块二级分类代码
        /// </summary>
        public string? ZBUSTD2ID { get; set; }
        /// <summary>
        /// 业务板块二级分类名称
        /// </summary>
        public string? ZBUSTD2NAME { get; set; }
        /// <summary>
        /// 业务板块三级分类代码
        /// </summary>
        public string? ZBUSTD3ID { get; set; }
        /// <summary>
        /// 业务板块三级分类名称
        /// </summary>
        public string? ZBUSTD3NAME { get; set; }
        /// <summary>
        /// 备注（业务板块）
        /// </summary>
        public string? ZBUSTDREMARKS { get; set; }
        /// <summary>
        /// 中交十二大业务类型编码 
        /// </summary>
        public string? Z12TOPBID { get; set; }
        /// <summary>
        /// 中交十二大业务类型名称
        /// </summary>
        public string? Z12TOPBNAME { get; set; }
        /// <summary>
        /// 中交江河湖海分类编码
        /// </summary>
        public string? ZRRLSID { get; set; }
        /// <summary>
        /// 中交江河湖海分类名称
        /// </summary>
        public string? ZRRLSNAME { get; set; }
        /// <summary>
        /// 三新业务类型：取值：0：否、1：是
        /// </summary>
        public string? ZNEW3TOB { get; set; }
    }
}
