using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationExecution
    {
    }
    /// <summary>
    /// 操作响应Dto
    /// </summary>
    public class OperationExecutionRequestDto : IValidatableObject
    {
        /// <summary>
        /// 增还是改 1增 2改 3删
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 1 用户 2 项目 3 往来单位
        /// </summary>
        public int Table { get; set; }
        /// <summary>
        /// 操作实体对象Json
        /// </summary>
        public string? EntityJson { get; set; }
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EntityJson))
            {
                yield return new ValidationResult("操作实体对象不能为空", new string[] { nameof(EntityJson) });
            }
        }
    }
}
