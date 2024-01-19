using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    public class ProjectLonLatRequestDto : IValidatableObject
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        ///// <summary>
        ///// 经度
        ///// </summary>
        //public string Lon { get; set; }
        ///// <summary>
        ///// 纬度
        ///// </summary>
        //public string Lat { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
			if (string.IsNullOrWhiteSpace(Address))
			{
				yield return new ValidationResult("Address参数不能为空且类型是string类型", new string[] { nameof(Address) });
			}
			//if (string.IsNullOrWhiteSpace(Lon))
   //         {
   //             yield return new ValidationResult("Lon参数不能为空且类型是string类型", new string[] { nameof(Lon) });
   //         }
   //         if (string.IsNullOrWhiteSpace(Lat))
   //         {
   //             yield return new ValidationResult("Lat参数不能为空且类型是string类型", new string[] { nameof(Lat) });
   //         }
        }
    }
}
