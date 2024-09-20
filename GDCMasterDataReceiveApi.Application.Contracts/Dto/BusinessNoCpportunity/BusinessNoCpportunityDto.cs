using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity
{
    /// <summary>
    /// 商机项目(不含境外商机项目) 反显
    /// </summary>
    public class BusinessNoCpportunitySearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 商机项目主数据编码:新增项目由主数据系统生成并返回主数据编码，修改时必填
        /// </summary>
        public string? BPjectMDCode { get; set; }
        /// <summary>
        /// 商机项目名称:商机项目的中文名称，该字段作为境内和港澳台商机项目的唯一标识
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 商机项目外文名称:商机项目当地官方语言名称，该字段作为境外（不包括港澳台）商机项目的唯一标识
        /// </summary>
        public string? BPjectForeignName { get; set; }
        /// <summary>
        /// 项目类型:按照字典表进行选择
        /// </summary>
        public string? PjectType { get; set; }
        /// <summary>
        /// 国家/地区:商机项目所在的国家/地区
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 状态: 数据是否有效的标识: 有效：1无效：0
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 资质单位
        /// </summary>
        public string? QualificationUnit { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? TaxationMethod { get; set; }
    }
    /// <summary>
    /// 商机项目(不含境外商机项目) 详情
    /// </summary>
    public class BusinessNoCpportunityDetailsDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 商机项目主数据编码:新增项目由主数据系统生成并返回主数据编码，修改时必填
        /// </summary>
        [ExcelColumnName("商机项目主数据编码")]
        public string? BPjectMDCode { get; set; }
        /// <summary>
        /// 商机项目名称:商机项目的中文名称，该字段作为境内和港澳台商机项目的唯一标识
        /// </summary>
        [ExcelColumnName("商机项目名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 商机项目外文名称:商机项目当地官方语言名称，该字段作为境外（不包括港澳台）商机项目的唯一标识
        /// </summary>
        [ExcelColumnName("商机项目外文名称")]
        public string? BPjectForeignName { get; set; }
        /// <summary>
        /// 项目类型:按照字典表进行选择
        /// </summary>
        [ExcelColumnName("项目类型")]
        public string? PjectType { get; set; }
        /// <summary>
        /// 中交项目业务分类:按照字典表进行选择，涉及多个业务分类的商机项目，其属性应优先归入合同额占比最大的业务分类，若合同额占比相同，应归入实施难度最大的业务分类
        /// </summary>
        [ExcelColumnName("中交项目业务分类")]
        public string? BTypeOfCCCCProjects { get; set; }
        /// <summary>
        /// 国家/地区:商机项目所在的国家/地区
        /// </summary>
        [ExcelColumnName("国家/地区")]
        public string? Country { get; set; }
        /// <summary>
        /// 项目所在地:参照项目主数据标准要求填写，明确到市级地点，境内项目必填
        /// </summary>
        [ExcelColumnName("项目所在地")]
        public string? PjectLocation { get; set; }
        /// <summary>
        /// 开始跟踪日期:填写首次跟踪的日期
        /// </summary>
        [ExcelColumnName("开始跟踪日期")]
        public string? StartTrackingDate { get; set; }
        /// <summary>
        ///跟踪单位:填写跟踪单位的机构主数据编码
        /// </summary>
        [ExcelColumnName("跟踪单位")]
        public string? TrackingUnit { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [ExcelIgnore]
        public string? UnitSec { get; set; }
        /// <summary>
        /// 状态: 数据是否有效的标识: 有效：1无效：0
        /// </summary>
        [ExcelIgnore]
        public string? State { get; set; }
        /// <summary>
        /// 资质单位
        /// </summary>
        [ExcelColumnName("资质单位")]
        public string? QualificationUnit { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [ExcelColumnName("计税方式")]
        public string? TaxationMethod { get; set; }
        /// <summary>
        /// 参与单位:填写参与部门的行政机构主数据编码，可多值，用英文逗号隔开.
        /// </summary>
        [ExcelIgnore]
        public string? ParticipatingUnits { get; set; }
    }
    /// <summary>
    /// 商机项目(不含境外商机项目) 接收
    /// </summary>
    public class BusinessCpportunityItem
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
        /// 商机项目主数据编码:新增项目由主数据系统生成并返回主数据编码，修改时必填
        /// </summary>
        public string? ZBOP { get; set; }
        /// <summary>
        /// 商机项目名称:商机项目的中文名称，该字段作为境内和港澳台商机项目的唯一标识
        /// </summary>
        public string? ZBOPN { get; set; }
        /// <summary>
        /// 商机项目外文名称:商机项目当地官方语言名称，该字段作为境外（不包括港澳台）商机项目的唯一标识
        /// </summary>
        public string? ZBOPN_EN { get; set; }
        /// <summary>
        /// 项目类型:按照字典表进行选择
        /// </summary>
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 中交项目业务分类:按照字典表进行选择，涉及多个业务分类的商机项目，其属性应优先归入合同额占比最大的业务分类，若合同额占比相同，应归入实施难度最大的业务分类
        /// </summary>
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 国家/地区:商机项目所在的国家/地区
        /// </summary>
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地:参照项目主数据标准要求填写，明确到市级地点，境内项目必填
        /// </summary>
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 开始跟踪日期:填写首次跟踪的日期
        /// </summary>
        public string? ZSFOLDATE { get; set; }
        /// <summary>
        ///跟踪单位:填写跟踪单位的机构主数据编码
        /// </summary>
        public string? ZORG { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 状态: 数据是否有效的标识: 有效：1无效：0
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 资质单位
        /// </summary>
        public string? ZORG_QUAL { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 参与单位:填写参与部门的行政机构主数据编码，可多值，用英文逗号隔开.
        /// </summary>
        public string? ZCY2NDORG { get; set; }

        /// <summary>
        /// 中标交底项目表类型
        /// </summary>
        public TypeOfBidDisclosureProjectTableModels? ZAWARDP_LIST {  get; set; }
    }
}
