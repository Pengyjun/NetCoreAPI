using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Common
{
    /// <summary>
    /// 机构添加请求类
    /// </summary>
    public class AddInstutionRequestDto
    {
        /// <summary>
        /// 机构id
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 父级机构id
        /// </summary>
        public string Poid { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string Name { get; set; }
    }
}
