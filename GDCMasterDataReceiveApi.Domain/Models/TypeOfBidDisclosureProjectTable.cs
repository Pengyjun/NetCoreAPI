using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中标交底项目表类型
    /// </summary>
    [SugarTable("t_typeofbiddisclosureprojecttable", IsDisabledDelete = true)]
    public class TypeOfBidDisclosureProjectTable : BaseEntity<long>
    {
        /// <summary>
        /// 中标交底项目编码:格式：BO000001-01
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "Code")]
        public string ZAWARDP { get; set; }
        /// <summary>
        /// 交底项目二级单位
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "UnitSec")]
        public string ZAP2NDORG { get; set; }
        /// <summary>
        /// 中标资质单位
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "WinBidUnit")]
        public string ZAWARDORG { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string ZAWARDPN { get; set; }
        /// <summary>
        /// 项目外文名称:境外项目必填
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ForeignName")]
        public string ZAWPN_EN { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "BTypeOfCCCC")]
        public string ZSCPBC { get; set; }
        /// <summary>
        /// 项目所在地:境内项目必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "Location")]
        public string ZSPROJLOC { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "Type")]
        public string ZSPROTYPE { get; set; }
        /// <summary>
        /// 是否联合体总项目:0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsItAJoint")]
        public string ZSZCONTPRO { get; set; }
        /// <summary>
        /// 数据状态:0-停用，1-启用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataStatus")]
        public string ZSSTATE { get; set; }
    }
}
