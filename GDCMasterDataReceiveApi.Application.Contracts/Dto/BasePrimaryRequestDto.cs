using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 对所有表的主键请求进行封装
    /// </summary>
    public class BasePrimaryRequestDto :IValidatableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == null || !Id.HasValue)
            {
                yield return new ValidationResult("Id参数不能为空", new string[] { nameof(Id) });
            }
        }
    }
}
