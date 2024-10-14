using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
    /// </summary>
    [SugarTable("t_projectclassification", IsDisabledDelete = true)]
    public class ProjectClassification : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 中交业务分类三级分类代码
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "CCCCBTypeThirdCode")]
        public string? ZCPBC3ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类代码 
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "CCCCBTypeOneCode")]
        public string? ZCPBC1ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类名称 
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string? ZCPBC1NAME { get; set; }
        /// <summary>
        /// 中交业务分类二级分类代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "CCCCBTypeSecCode")]
        public string? ZCPBC2ID { get; set; }
        /// <summary>
        /// 中交业务分类二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBTypeSecName")]
        public string? ZCPBC2NAME { get; set; }
        /// <summary>
        /// 中交业务分类三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBTypeThirdName")]
        public string? ZCPBC3NAME { get; set; }
        /// <summary>
        /// 备注（业务分类）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BusinessRemark")]
        public string? ZZCPBREMARKS { get; set; }
        /// <summary>
        /// 产业分类一级分类代码
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "ChanYeOneCode")]
        public string? ZICSTD1ID { get; set; }
        /// <summary>
        /// 产业分类一级分类名称 
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ChanYeOneName")]
        public string? ZICSTD1NAME { get; set; }
        /// <summary>
        /// 产业分类二级分类代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "ChanYeSecCode")]
        public string? ZICSTD2ID { get; set; }
        /// <summary>
        /// 产业分类二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ChanYeSecName")]
        public string? ZICSTD2NAME { get; set; }
        /// <summary>
        /// 产业分类三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ChanYeThirdCode")]
        public string? ZICSTD3ID { get; set; }
        /// <summary>
        /// 产业分类三级分类代码
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ChanYeThirdName")]
        public string? ZICSTD3NAME { get; set; }
        /// <summary>
        /// 备注（产业分类）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ChanYeRemark")]
        public string? ZICSTDREMARKS { get; set; }
        /// <summary>
        /// 业务板块一级分类代码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "BSectorOneCode")]
        public string? ZBUSTD1ID { get; set; }
        /// <summary>
        /// 业务板块一级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BSectorOneName")]
        public string? ZBUSTD1NAME { get; set; }
        /// <summary>
        /// 业务板块二级分类代码
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "BSectorSecCode")]
        public string? ZBUSTD2ID { get; set; }
        /// <summary>
        /// 业务板块二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BSectorSecName")]
        public string? ZBUSTD2NAME { get; set; }
        /// <summary>
        /// 业务板块三级分类代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "BSectorThirdCode")]
        public string? ZBUSTD3ID { get; set; }
        /// <summary>
        /// 业务板块三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BSectorThirdName")]
        public string? ZBUSTD3NAME { get; set; }
        /// <summary>
        /// 备注（业务板块）
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "BSectorRemark")]
        public string? ZBUSTDREMARKS { get; set; }
        /// <summary>
        /// 中交十二大业务类型编码 
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CCCCBTypeCode")]
        public string? Z12TOPBID { get; set; }
        /// <summary>
        /// 中交十二大业务类型名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBTypeName")]
        public string? Z12TOPBNAME { get; set; }
        /// <summary>
        /// 中交江河湖海分类编码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CCCCRiverLakeAndSeaCode")]
        public string? ZRRLSID { get; set; }
        /// <summary>
        /// 中交江河湖海分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCRiverLakeAndSeaName")]
        public string? ZRRLSNAME { get; set; }
        /// <summary>
        /// 三新业务类型：取值：0：否、1：是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "ThirdNewBType")]
        public string? ZNEW3TOB { get; set; }
    }
}
