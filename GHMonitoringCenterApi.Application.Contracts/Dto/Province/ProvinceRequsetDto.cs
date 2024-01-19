using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Province
{
    /// <summary>
    /// 获取区域Id
    /// </summary>
     public class ProvinceRequsetDto: IValidatableObject
    {
        /// <summary>
        /// 返回id
        /// </summary>
        public Guid Regionid { get; set; }
        /// <summary>
        /// 模糊查询
        /// </summary>
        public string? Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Regionid == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Regionid) });
            }
        }
    }
}
