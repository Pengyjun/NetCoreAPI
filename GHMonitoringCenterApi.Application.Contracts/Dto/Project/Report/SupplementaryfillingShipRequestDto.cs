using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 船舶月报补填dto
    /// </summary>
    public class SupplementaryfillingShipRequestDto
    {
        /// <summary>
        /// 补填月份
        /// </summary>
        public DateTime DateMonth { get; set; }
        public SupplementaryfillingShipInfo supplementaryfillingShipInfo { get; set; }
    }

    public class SupplementaryfillingShipInfo:IValidatableObject
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipType ShipType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("ProjectId不能为空",new string[]{nameof(ProjectId) });
            }
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("ShipId不能为空", new string[] { nameof(ShipId) });
            }
        }
    }

}
