using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 商机项目(不含境外商机项目)
    /// </summary>
    [SugarTable("t_businessnocpportunity", IsDisabledDelete = true)]
    public class BusinessNoCpportunity : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 商机项目主数据编码:新增项目由主数据系统生成并返回主数据编码，修改时必填
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "BPjectMDCode")]
        public string ZBOP { get; set; }
        /// <summary>
        /// 商机项目名称:商机项目的中文名称，该字段作为境内和港澳台商机项目的唯一标识
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "BPjecttName")]
        public string ZBOPN { get; set; }
        /// <summary>
        /// 商机项目外文名称:商机项目当地官方语言名称，该字段作为境外（不包括港澳台）商机项目的唯一标识
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "BPjectForeignName")]
        public string ZBOPN_EN { get; set; }
        /// <summary>
        /// 项目类型:按照字典表进行选择
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "PjectType")]
        public string ZPROJTYPE { get; set; }
        /// <summary>
        /// 中交项目业务分类:按照字典表进行选择，涉及多个业务分类的商机项目，其属性应优先归入合同额占比最大的业务分类，若合同额占比相同，应归入实施难度最大的业务分类
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "BTypeOfCCCCProjects")]
        public string ZCPBC { get; set; }
        /// <summary>
        /// 国家/地区:商机项目所在的国家/地区
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Country")]
        public string ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地:参照项目主数据标准要求填写，明确到市级地点，境内项目必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "PjectLocation")]
        public string ZPROJLOC { get; set; }
        /// <summary>
        /// 开始跟踪日期:填写首次跟踪的日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "StartTrackingDate")]
        public string ZSFOLDATE { get; set; }
        /// <summary>
        ///跟踪单位:填写跟踪单位的机构主数据编码
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "TrackingUnit")]
        public string ZORG { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "UnitSec")]
        public string Z2NDORG { get; set; }
        /// <summary>
        /// 状态: 数据是否有效的标识: 有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 资质单位
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "QualificationUnit")]
        public string ZORG_QUAL { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "TaxationMethod")]
        public string ZTAXMETHOD { get; set; }
        /// <summary>
        /// 参与单位:填写参与部门的行政机构主数据编码，可多值，用英文逗号隔开.
        /// </summary>
        [SugarColumn(Length = 300, ColumnName = "ParticipatingUnits")]
        public string? ZCY2NDORG { get; set; }
    }
}
