using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Common
{
    /// <summary>
    /// 首页菜单Dto
    /// </summary>
    public class SearchHomeMenuRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

    }
}
