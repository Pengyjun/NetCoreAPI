using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 添加授权机构请求Dto
    /// </summary>
    public class AddAuthorizedInstitutionsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DivisionId { get; set; }     
        /// <summary>
        /// 下级公司ID
        /// </summary>
        public Guid? SubordinateCompaniesId { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(UserId) });
            }
            if (DivisionId == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(DivisionId) });
            }
            //if (SubordinateCompaniesId == Guid.Empty)
            //{
            //    yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(SubordinateCompaniesId) });
            //}
        }
    }
}
