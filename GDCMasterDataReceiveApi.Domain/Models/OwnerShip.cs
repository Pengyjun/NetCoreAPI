using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 自有船舶
    /// </summary>
    [SugarTable("t_ownership", IsDisabledDelete = true)]
    public class OwnerShip : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }

        /// <summary>
        /// 船舶代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }

        /// <summary>
        /// 船舶英文名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? EnglishName { get; set; }

        /// <summary>
        /// 船舶中文名称全拼
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Spell { get; set; }

        /// <summary>
        /// 船舶中文名称全拼
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? SpellAbbreviate { get; set; }

        /// <summary>
        /// 船舶类型GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? TypeId { get; set; }

        /// <summary>
        /// 船级社GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ClassicId { get; set; }

        /// <summary>
        /// 船舶呼号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CallSign { get; set; }

        /// <summary>
        /// 船舶MMSI号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Mmsi { get; set; }

        /// <summary>
        /// 船舶IMO号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Imo { get; set; }

        /// <summary>
        /// 船舶登记号码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? RegisterNumber { get; set; }

        /// <summary>
        /// 船舶初始登记号码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FisrtRegisterNumber { get; set; }

        /// <summary>
        /// 船舶识别号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? IdentNumber { get; set; }

        /// <summary>
        /// 船舶卫星无线电号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? RadioNumber { get; set; }

        /// <summary>
        /// 船检登记号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InspectNumber { get; set; }

        /// <summary>
        /// 船检证书类别
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InspectType { get; set; }

        /// <summary>
        /// 国籍证书类别
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? NationType { get; set; }

        /// <summary>
        /// 自由船舶/ 外籍船舶
        /// </summary>
        [SugarColumn(ColumnDataType ="bit")]
        public bool Belong { get; set; }

        /// <summary>
        /// 船舶的船籍港
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? RegistryPort { get; set; }

        /// <summary>
        /// 航行区域
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? NavigateArea { get; set; }

        /// <summary>
        /// 施工区域
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ConstructionArea { get; set; }

        /// <summary>
        /// 所属公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 所属公司名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CompanyName { get; set; }

        /// <summary>
        /// 船舶状态GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? StatusId { get; set; }

        /// <summary>
        ///  船舶状态(0:施工,1:调遣,2:厂修,3:待命)
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public ProjectShipState? ShipState { get; set; }

        /// <summary>
        /// 设计单位
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Designer { get; set; }

        /// <summary>
        /// 建造单位
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Builder { get; set; }
        /// <summary>
        /// 船舶总长
        /// </summary>
        
        [Column(TypeName = "decimal")]
        public decimal? LengthOverall { get; set; }
        /// <summary>
        /// 两柱间长
        /// </summary>
        [Column(TypeName = "decimal")]

        public decimal? LengthBpp { get; set; }
        /// <summary>
        /// 船舶型宽
        /// </summary>
        [Column(TypeName = "decimal")]

        public decimal? Breadth { get; set; }
        /// <summary>
        /// 船舶型深
        /// </summary>
        [Column(TypeName = "decimal")]

        public decimal? Depth { get; set; }
        /// <summary>
        /// 满载吃水
        /// </summary>
        [Column(TypeName = "decimal")]

        public decimal? LoadedDraft { get; set; }
        /// <summary>
        /// 空载吃水
        /// </summary>
        [Column(TypeName = "decimal")]

        public decimal? LightDraft { get; set; }
        /// <summary>
        /// 满载排水量
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? LoadedDisplacement { get; set; }
        /// <summary>
        /// 空载排水量
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? LightDisplacement { get; set; }
        /// <summary>
        /// 总吨
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? GrossTonnage { get; set; }
        /// <summary>
        /// 净吨
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? NetTonnage { get; set; }
        /// <summary>
        /// 全船总功率
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? TotalPower { get; set; }

        /// <summary>
        /// 续航力
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Endurance { get; set; }
        /// <summary>
        /// 自由航速
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? Speed { get; set; }

        /// <summary>
        /// 建筑物距水面高度
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Height { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        [SugarColumn(ColumnDataType ="datetime")]

        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 退役日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? RetireDate { get; set; }
        /// <summary>
        /// 资产原值
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? OriginalValue { get; set; }
        /// <summary>
        /// 资产净值
        /// </summary>
        [Column(TypeName = "decimal")]
        public decimal? NetValue { get; set; }

        /// <summary>
        /// 建造合同开始日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractStartDate { get; set; }

        /// <summary>
        /// 建造合同结束日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractEndDate { get; set; }

        /// <summary>
        /// 批准总投资
        /// </summary>
        [SugarColumn(ColumnDataType ="decimal")]
        public decimal? ApprovalInvestment { get; set; }

        /// <summary>
        /// 已完成投资（冗余）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? CompleteInvestment { get; set; }

        /// <summary>
        /// 船舶残值（变化量）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? Residual { get; set; }

        /// <summary>
        /// 船舶描述
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Remarks { get; set; }

        /// <summary>
        /// 公司编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CompanyCode { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TypeCode { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TypeName { get; set; }

        /// <summary>
        /// 状态编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? StatusCode { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? StatusName { get; set; }

        /// <summary>
        /// 船级社名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ClassicName { get; set; }
        /// <summary>
        /// 船舶类型分类
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TypeClass { get; set; }

        /// <summary>
        /// 船舶经营单位
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ShipOperationUnit { get; set; }
    }
}
