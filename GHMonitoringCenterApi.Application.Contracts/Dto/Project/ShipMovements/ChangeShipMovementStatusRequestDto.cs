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
    /// 更改船舶动向状态
    /// </summary>
    public class ChangeShipMovementStatusRequestDto : IValidatableObject
    {
        /// <summary>
        /// 船舶动向Id
        /// </summary>
        public Guid ShipMovementId { get; set; }

        /// <summary>
        /// 船舶动向状态
        /// </summary>
        public ShipMovementStatus Status { get; set; }

        /// <summary>
        /// 进场/退场时间
        /// </summary>
        public DateTime EnterOrQuitTime { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string? Remarks { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShipMovementId == Guid.Empty)
            {
                yield return new ValidationResult("船舶动向Id不能为空", new string[] { nameof(ShipMovementId) });
            }
            if (Remarks!=null&&Remarks.Length>500)
            {
                yield return new ValidationResult("备注长度不能超过500", new string[] { nameof(Remarks) });
            }
        }
    }
}
