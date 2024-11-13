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
    public class OperationExecutionRequestDto:IValidatableObject
    {
        /// <summary>
        /// 增还是改
        /// </summary>
        public OperateType OperateType { get; set; }
        /// <summary>
        /// 1 用户 2 项目 3 往来单位
        /// </summary>
        public int Table { get; set; }
        /// <summary>
        /// 操作实体对象Json
        /// </summary>
        public string? EntityJson { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EntityJson))
            {
                yield return new ValidationResult("操作实体对象不能为空", new string[] { nameof(EntityJson) });
            }
        }
    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 增
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 改
        /// </summary>
        Update = 2
    }
}
