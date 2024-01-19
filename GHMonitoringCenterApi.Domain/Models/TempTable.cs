using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    [SugarTable("t_temptable", IsDisabledDelete = true)]
    public class TempTable
    {
        [SugarColumn(ColumnDataType="text")]
        public string? Value { get; set; }
    }
}
