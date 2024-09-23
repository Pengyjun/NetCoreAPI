using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.System
{
    public class SystemInterfaceRequestDto:IValidatableObject
    {
        /// <summary>
        /// 接口标识
        /// </summary>
        public string? SystemIdentity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(SystemIdentity))
            {
                yield return new ValidationResult("系统标识不能为空不能为空", new string[] { nameof(SystemIdentity) });
            }
        }
    }
}
