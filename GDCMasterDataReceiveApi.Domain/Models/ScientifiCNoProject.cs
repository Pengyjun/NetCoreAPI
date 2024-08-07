using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 科研项目
    /// </summary>
    [SugarTable("t_scientificnoproject", IsDisabledDelete = true)]
    public class ScientifiCNoProject : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 科研项目主数据编码:科研项目的唯一编码标识
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "MDCodeForScientificResearchProjects")]
        public string ZSRP { get; set; }
        /// <summary>
        /// 科研项目名称:科研项目名称适用于各类型科技研发项目，应写项目中文名称的全称，应按照有效法律文书（科研项目研发合同）中的名称为准详细填写。原则上同一分类中不可重复。
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ScientificResearchProjectName")]
        public string ZSRPN { get; set; }
        /// <summary>
        /// 科研项目外文名称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ForeignNameOfScientificResearchProject")]
        public string? ZSRPN_FN { get; set; }
        /// <summary>
        /// 是否高新项目:0 否 1 是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsHighTechProject")]
        public string? ZHITECH { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "State")]
        public string ZKPSTATE { get; set; }
        /// <summary>
        /// 是否委外项目:0 否 1 是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsOutsourcedProject")]
        public string ZOUTSOURCING { get; set; }
        /// <summary>
        /// 科研项目分类:代码
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "ScientificResearchProjectType")]
        public string ZSRPCLASS { get; set; }
        /// <summary>
        /// 所属二级单位:所属二级单位，按照承担单位、参与单位、委托单位的所属二级单位选择，属于集团内的应在符合Q/CCCC XX003—2021要求的机构主数据中选择。
        /// </summary>
        [NotMapped]
        public List<IT_AI>? IT_AI { get; set; }
        /// <summary>
        /// 承担单位:按科研项目合同填写，内部承担单位属于集团内的应在符合Q/CCCC XX003—2021要求的机构主数据中选择。
        /// 按科研项目合同填写，外部承担单位属于集团外的应在符合Q/CCCC XX007—2021要求的往来单位主数据中选择
        /// </summary>
        [NotMapped]
        public List<IT_AG> IT_AG { get; set; }
        /// <summary>
        /// 曾用名  
        /// </summary>
        [NotMapped]
        public List<IT_ONAME>? IT_ONAME { get; set; }
        /// <summary>
        /// 参与单位:按科研项目合同填写，内部参与单位属于集团内的应在符合Q/CCCC XX003—2021要求的机构主数据中选择。
        /// 按科研项目合同填写，外部参与单位属于集团外的应在符合Q/CCCC XX007—2021要求的往来单位主数据中选择
        /// </summary>
        [NotMapped]
        public List<IT_AH> IT_AH { get; set; }
        /// <summary>
        /// 委托单位:按科研项目合同填写，内部委托单位属于集团内的应在符合Q/CCCC XX003—2021要求的机构主数据中选择。
        /// 按科研项目合同填写，外部委托单位属于集团外的应在符合Q/CCCC XX007—2021要求的往来单位主数据中选择
        /// </summary>
        [NotMapped]
        public List<IT_AK> IT_AK { get; set; }
        /// <summary>
        /// 项目负责人：项目负责人按科研项目合同填写
        /// </summary>
        [NotMapped]
        public List<IT_AJ> IT_AJ { get; set; }
        /// <summary>
        /// 项目总费用（万元）：项目总费用按科研项目合同填写
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "TotalProjectCost")]
        public string ZPROJCOST { get; set; }
        /// <summary>
        /// 项目总费用币种
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "CurrencyOfTotalProjectCost")]
        public string ZPROJCOSTCUR { get; set; }
        /// <summary>
        /// 立项年份：立项年份应根据科研项目管理单位发布立项通知年份确定。若无立项通知，则与科研项目合同签订的年份一致
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "ProjectYear")]
        public string ZPROJYEA { get; set; }
        /// <summary>
        /// 计划开始日期：项目计划开始/完成日期适用于所有类型科研项目，应与科研项目合同保持一致，日期格式为：YYYYMMDD
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "PlanStartDate")]
        public string ZPSTARTDATE { get; set; }
        /// <summary>
        /// 计划结束日期：项目计划开始/完成日期适用于所有类型科研项目，应与科研项目合同保持一致，日期格式为：YYYYMMDD
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "PlanEndDate")]
        public string ZPFINDATE { get; set; }
        /// <summary>
        /// 专业类型：科研项目所属的专业类型，引用Q/CCCC GL005—2021要求的公司产业分类
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "ProfessionalType")]
        public string ZMAJORTYPE { get; set; }
        /// <summary>
        /// 状态：0停用 1启用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZPSTATE { get; set; }
        /// <summary>
        /// 上级科研项目主数据编码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "SupMDCodeForScientificResearchProjects")]
        public string? ZSRPUP { get; set; }
        /// <summary>
        /// 参与部门:按照承担单位、参与单位、委托单位的参与部门选择，可选多值。2022年6月2日新增加字段
        /// </summary>
        [NotMapped]
        public List<IT_DE> IT_DE { get; set; }
    }

    /// <summary>
    /// 所属二级单位
    /// </summary>
    public class IT_AI
    {
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string Z2NDORG { get; set; }
        /// <summary>
        /// 所属二级单位名称
        /// </summary>
        public string? Z2NDORGN { get; set; }
    }
    /// <summary>
    /// 承担单位
    /// </summary>
    public class IT_AG
    {
        /// <summary>
        /// 承担单位
        /// </summary>
        public string ZUDTK { get; set; }
        /// <summary>
        /// 承担单位名称
        /// </summary>
        public string? ZUDTKN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    /// 曾用名  
    /// </summary>
    public class IT_ONAME
    {
        /// <summary>
        /// 行项目编号
        /// </summary>
        public string ZITEM { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string ZOLDNAME { get; set; }
    }
    /// <summary>
    ///  参与单位 
    /// </summary>
    public class IT_AH
    {
        /// <summary>
        /// 参与单位
        /// </summary>
        public string ZPU { get; set; }
        /// <summary>
        /// 参与单位名称
        /// </summary>
        public string? ZPUN { get; set; }
        /// <summary>
        /// 内部/外部 :1 内部/2 外部
        /// </summary>
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    ///  委托单位
    /// </summary>
    public class IT_AK
    {
        /// <summary>
        /// 委托单位
        /// </summary>
        public string ZAUTHORISE { get; set; }
        /// <summary>
        /// 委托单位名称
        /// </summary>
        public string? ZAUTHORISEN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    ///  项目负责人
    /// </summary>
    public class IT_AJ
    {
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ZPRINCIPAL { get; set; }
        /// <summary>
        /// 项目负责人名称
        /// </summary>
        public string? ZPRINCIPALN { get; set; }
    }

    public class IT_DE
    {
        /// <summary>
        /// 参与部门编号
        /// </summary>
        public string ZKZDEPART { get; set; }
        /// <summary>
        /// 参与部门名称
        /// </summary>
        public string? ZKZDEPARTNM { get; set; }
    }
}
