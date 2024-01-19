using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 获取人员授权机构请求 Dto
    /// </summary>
    public class SearchAuthorizedInstitutionsRequestDto
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 下级单位Id
        /// </summary>
        public Guid CompanyId { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Id) });
            }
            if (CompanyId == Guid.Empty)
            {
                yield return new ValidationResult("CompanyId参数不能为空且类型是GUID类型", new string[] { nameof(CompanyId) });
            }
        }
    }
}
