using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements
{

    /// <summary>
    ///  进场船舶请求
    /// </summary>
    public class EnterShipsRequestDto : BaseRequestDto, IValidatableObject
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 填报时间（不传或null默认昨天）
        /// </summary>
        public DateTime? DateDayTime { get; set; }
        /// <summary>
        /// 0全部信息 1关联项目 2不关联项目
        /// </summary>
        public int AssociationProject { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AssociationProject != 0 && AssociationProject != 1 && AssociationProject != 2)
            {
                yield return new ValidationResult("输入参数错误", new string[] { nameof(AssociationProject) });
            }
        }

    }
}
