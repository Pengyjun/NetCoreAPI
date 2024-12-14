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
        [Required(AllowEmptyStrings =false,ErrorMessage ="Id不能为空")]
        public string? Id { get; set; }
    }
}
