using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取船舶动向集合请求
    /// </summary>
    public class ShipMovementsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( ( ProjectId == Guid.Empty))
            {
                yield return new ValidationResult("项目Id不存在", new string[] { nameof(ProjectId)});
            }
        }

    }
}
