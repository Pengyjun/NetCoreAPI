using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 分包船舶
    /// </summary>
    [SugarTable("t_subship", IsDisabledDelete = true)]
    public class SubShip : BaseEntity<Guid>
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
        /// 船舶英文名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? EnglishName { get; set; }

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
        [SugarColumn(Length = 100)]
        public string? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 管理Id
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]

        public bool Belong { get; set; }
        /// <summary>
        /// 管理Id
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶状态GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? StatusId { get; set; }

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
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? LengthOverall { get; set; }

        /// <summary>
        /// 两柱间长
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? LengthBpp { get; set; }

        /// <summary>
        /// 船舶型宽
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? Breadth { get; set; }

        /// <summary>
        /// 船舶型深
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? Depth { get; set; }

        /// <summary>
        /// 满载吃水
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? LoadedDraft { get; set; }

        /// <summary>
        /// 空载吃水
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? LightDraft { get; set; }

        /// <summary>
        /// 满载排水量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? LoadedDisplacement { get; set; }

        /// <summary>
        /// 空载排水量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? LightDisplacement { get; set; }

        /// <summary>
        /// 总吨
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? GrossTonnage { get; set; }

        /// <summary>
        /// 净吨
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? NetTonnage { get; set; }

        /// <summary>
        /// 全船总功率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal")]
        public decimal? TotalPower { get; set; }

        /// <summary>
        /// 续航力
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Endurance { get; set; }
        /// <summary>
        /// 自由航速
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? Speed { get; set; }

        /// <summary>
        /// 建筑物距水面高度
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Height { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FinishDate { get; set; }

        /// <summary>
        /// 船舶描述
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Remarks { get; set; }

        /// <summary>
        /// 船舶经营单位
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? BusinessUnit { get; set; }
    }
}
