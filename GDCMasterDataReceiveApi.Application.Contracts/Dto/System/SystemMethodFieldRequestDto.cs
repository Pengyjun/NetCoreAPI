using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.System
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMethodFieldRequestDto:IValidatableObject
        
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(InterfaceName))
            {
                yield return new ValidationResult("接口名称不能为空", new string[] { nameof(InterfaceName) });
            }
        }
    }
}
