using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos
{

    /// <summary>
    /// 基本响应体
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public Guid? BId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseResponseDto
    {

    }
}
