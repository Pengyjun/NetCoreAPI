using System.ComponentModel.DataAnnotations;
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Common
{
    /// <summary>
    /// excel请求对象
    /// </summary>
    public class ExcelRequestDto<T> : IValidatableObject where T : class
    {
        /// <summary>
        /// excel表头名称
        /// </summary>
        public string ExcelTabHeadName { get; set; }
        /// <summary>
        /// 生成的Excel模版路径
        /// </summary>
        public string ExcelTemplatePath { get; set; }
        /// <summary>
        /// Excel数据
        /// </summary>
        public T? ExcelData { get; set; }
        /// <summary>
        /// 参数验证
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ExcelTabHeadName))
            {
                yield return new ValidationResult("导出的Excel表头名称不能为空!!!", new string[] { nameof(ExcelTabHeadName) });
            }
            if (string.IsNullOrWhiteSpace(ExcelTemplatePath))
            {
                yield return new ValidationResult("导出的Excel模版路径不能为空!!!", new string[] { nameof(ExcelTemplatePath) });
            }
        }
    }

}

