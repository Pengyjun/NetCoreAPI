using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 船舶定员标准
    /// </summary>
    [SugarTable("t_shippersonnelstandard", IsDisabledDelete = true, TableDescription = "船舶定员标准")]
    public class ShipPersonnelStandard : BaseEntity<long>
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "船舶id")]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "船舶名称")]
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶状态  1 施工  2 停工
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "船舶状态")]
        public int ShipStatus { get; set; }
    }
}
