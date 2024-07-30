using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics
{
    /// <summary>
    /// 统计当前模式所有表（当前指定数据库）请求dto
    /// </summary>
    public class MainTableOfStatisticsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 当前指定数据库(模式名)
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 日期(精确到小时)
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 入参校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Schema))
            {
                yield return new ValidationResult("模式名/数据库名不能为空", new string[] { nameof(Schema) });
            }
            if (string.IsNullOrEmpty(Date.ToString()) || DateTime.MinValue == Date)
            {
                yield return new ValidationResult("日期不能为空", new string[] { nameof(Date) });
            }
        }
    }
}
