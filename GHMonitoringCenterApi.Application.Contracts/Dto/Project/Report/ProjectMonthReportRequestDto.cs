using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取月报请求
    /// </summary>
    public class ProjectMonthReportRequestDto : IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份(例：202304 固定6位数)
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报月份（时间格式）
        /// </summary>
        public DateTime? DateMonthTime { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (DateMonth == null && DateMonthTime != null)
            {
                DateMonth = DateMonthTime.Value.ToDateMonth();
            }
        }
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
        }

    }
    /// <summary>
    /// 修改开累数
    /// </summary>
    public class SaveMonthReportForProjectHistoryDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectHistory>? ProjectHistorys { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ProjectHistory
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DetailsId { get; set; }
        /// <summary>
        /// 本月外包支出(偏差元)
        /// </summary>
        public decimal? DeviationOutAmount { get; set; }
        /// <summary>
        /// 本月外包支出(偏差外币)
        /// </summary>
        public decimal? CurrencyDeviationOutAmount { get; set; }
        /// <summary>
        /// 本月外包支出(实际元)
        /// </summary>
        public decimal? ActualOutAmount { get; set; }
        /// <summary>
        /// 本月外包支出(实际外币)
        /// </summary>
        public decimal? CurrencyActualOutAmount { get; set; }
        /// <summary>
        /// 本月完成工程量(方)(偏差)
        /// </summary>
        public decimal? DeviationCompQuantity { get; set; }
        /// <summary>
        /// 本月完成工程量(方)(实际)
        /// </summary>
        public decimal? ActualCompQuantity { get; set; }
        /// <summary>
        /// 本月完成产值(偏差元)
        /// </summary>
        public decimal? DeviationCompAmount { get; set; }
        /// <summary>
        /// 本月完成产值(偏差外币)
        /// </summary>
        public decimal? CurrencyDeviationCompAmount { get; set; }
        /// <summary>
        /// 本月完成产值(实际元)
        /// </summary>
        public decimal? ActualCompAmount { get; set; }
        /// <summary>
        /// 本月完成产值(实际外币)
        /// </summary>
        public decimal? CurrencyActualCompAmount { get; set; }
    }
}
