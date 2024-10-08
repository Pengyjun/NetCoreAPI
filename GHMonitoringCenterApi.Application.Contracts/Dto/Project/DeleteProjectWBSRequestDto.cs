using GHMonitoringCenterApi.Application.Contracts.Dto.MenuDelete;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 删除项目WBS数据
    /// </summary>
    public class DeleteProjectWBSRequestDto
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
    /// <summary>
    /// 删除结构树  校验是否曾经写过月报
    /// </summary>
    public class DeleteProjectWBSValidatableDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// wbs  ids   
        /// </summary>
        public List<string>? Ids { get; set; }
    }
}
