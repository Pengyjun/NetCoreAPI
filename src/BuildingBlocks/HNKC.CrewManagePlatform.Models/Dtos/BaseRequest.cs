using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos
{

    /// <summary>
    /// 基本请求DTO
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 业务ID
        /// </summary>
      
        public Guid? BId { get; set; }
    }
}
