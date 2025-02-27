using GHMonitoringCenterApi.Application.Contracts.Dto.MenuDelete;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos
{
    /// <summary>
    /// 删除公司产值信息
    /// </summary>
    public class DeleteCompanyProductionValueInfoRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == null)
            {
                yield return new ValidationResult("ID不能为空", new string[] { nameof(DeleteMenuRequestDto.Id) });
            }
        }
    }

}
