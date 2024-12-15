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
           /// ID
           /// </summary>
        public string? Id { get; set; }
    }
}
