using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 中交业务分类三级分类代码
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string ZCPBC3ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类代码 
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZCPBC1ID { get; set; }
        /// <summary>
        /// 中交业务分类一级分类名称 
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZCPBC1NAME { get; set; }
        /// <summary>
        /// 中交业务分类二级分类代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZCPBC2ID { get; set; }
        /// <summary>
        /// 中交业务分类二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZCPBC2NAME { get; set; }
        /// <summary>
        /// 中交业务分类三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZCPBC3NAME { get; set; }
        /// <summary>
        /// 备注（业务分类）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZZCPBREMARKS { get; set; }
        /// <summary>
        /// 产业分类一级分类代码
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTD1ID { get; set; }
        /// <summary>
        /// 产业分类一级分类名称 
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTD1NAME { get; set; }
        /// <summary>
        /// 产业分类二级分类代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTD2ID { get; set; }
        /// <summary>
        /// 产业分类二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTD2NAME { get; set; }
        /// <summary>
        /// 产业分类三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTD3NAME { get; set; }
        /// <summary>
        /// 备注（产业分类）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZICSTDREMARKS { get; set; }
        /// <summary>
        /// 业务板块一级分类代码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD1ID { get; set; }
        /// <summary>
        /// 业务板块一级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD1NAME { get; set; }
        /// <summary>
        /// 业务板块二级分类代码
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD2ID { get; set; }
        /// <summary>
        /// 业务板块二级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD2NAME { get; set; }
        /// <summary>
        /// 业务板块三级分类代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD3ID { get; set; }
        /// <summary>
        /// 业务板块三级分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTD3NAME { get; set; }
        /// <summary>
        /// 备注（业务板块）
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZBUSTDREMARKS { get; set; }
        /// <summary>
        /// 中交十二大业务类型编码 
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? Z12TOPBID { get; set; }
        /// <summary>
        /// 中交十二大业务类型名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? Z12TOPBNAME { get; set; }
        /// <summary>
        /// 中交江河湖海分类编码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZRRLSID { get; set; }
        /// <summary>
        /// 中交江河湖海分类名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZRRLSNAME { get; set; }
        /// <summary>
        /// 三新业务类型：取值：0：否、1：是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "CCCCBusinessClassificationLevelThreeCode")]
        public string? ZNEW3TOB { get; set; }
    }
}
