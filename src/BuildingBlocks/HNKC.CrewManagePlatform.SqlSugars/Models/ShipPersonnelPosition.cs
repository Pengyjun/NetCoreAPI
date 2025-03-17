using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 船舶定员标准 对应职位人数
    /// </summary>
    [SugarTable("t_shippersonnelposition", IsDisabledDelete = true, TableDescription = "船舶定员标准 对应职位人数")]
    public class ShipPersonnelPosition : BaseEntity<long>
    {
        /// <summary>
        /// 关联id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联id")]
        public Guid? ConnectionId { get; set; }
        /// <summary>
        /// 职务Id
        /// </summary>
        [SugarColumn(Length = 30, ColumnDescription = "职务Id")]
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        [SugarColumn(Length = 30, ColumnDescription = "职务")]
        public string? Position { get; set; }
        /// <summary>
        /// 对应最低标准数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "对应最低标准数量")]
        public int? Num { get; set; }
    }
}
