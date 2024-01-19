using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements
{
    /// <summary>
    /// 移除船舶进出场
    /// </summary>
    public class RemoveShipMovementRequestDto : IValidatableObject
    {
        /// <summary>
        /// 船舶动向Id
        /// </summary>
        public Guid ShipMovementId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShipMovementId == Guid.Empty)
            {
                yield return new ValidationResult("船舶进出场Id不能为空", new string[] { nameof(ShipMovementId) });
            }
        }
    }
}
