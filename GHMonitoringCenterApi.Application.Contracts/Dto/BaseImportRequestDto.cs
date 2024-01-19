using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// 基本导出列请求DTO
    /// </summary>
    public class BaseImportRequestDto
    {
        /// <summary>
        /// 导出列的字段集合已逗号分割
        /// </summary>
        public string? ColumnsStr { get; set; }
    }
}
