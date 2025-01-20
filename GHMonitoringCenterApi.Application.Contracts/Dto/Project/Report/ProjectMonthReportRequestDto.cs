using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取月报请求
    /// </summary>
    public class ProjectMonthReportRequestDto : IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 开累数修改列表按钮开放  true是
        /// </summary>
        public bool ExhaustedBtn { get; set; } = false;
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
        #region 顶部行
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 顶部历史外包支出(人民币)
        /// </summary>
        public decimal TopHOutValue { get; set; } = 0M;
        /// <summary>
        /// 顶部历史外包支出(美元、欧元等)
        /// </summary>
        public decimal TopCurrencyHOutValue { get; set; } = 0M;
        /// <summary>
        /// 顶部历史工程量
        /// </summary>
        public decimal TopHQuantity { get; set; } = 0M;
        /// <summary>
        /// 顶部历史完成产值(人民币)
        /// </summary>
        public decimal TopHValue { get; set; } = 0M;
        /// <summary>
        /// 顶部历史完成产值(美元、欧元等)
        /// </summary>
        public decimal TopCurrencyHValue { get; set; } = 0M;
        /// <summary>
        /// 顶部实际人民币产值
        /// </summary>
        public decimal? TopRMBHValue { get; set; } = 0M;
        /// <summary>
        /// 顶部实际人民币外包支出
        /// </summary>
        public decimal? TopRMBHOutValue { get; set; } = 0M;
        #endregion
        /// <summary>
        /// 子项数据
        /// </summary>
        public List<ProjectHistory>? ProjectHistorys { get; set; }
    }
    /// <summary>
    /// 子项数据
    /// </summary>
    public class ProjectHistory
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 本月外包支出(实际)
        /// </summary>
        public decimal? ActualOutAmount { get; set; }
        /// <summary>
        /// 本月完成工程量(方)(实际)
        /// </summary>
        public decimal? ActualCompQuantity { get; set; }
        /// <summary>
        /// 本月完成产值(实际)
        /// </summary>
        public decimal? ActualCompAmount { get; set; }
        /// <summary>
        /// 实际人民币产值
        /// </summary>
        public decimal? RMBHValue { get; set; }
        /// <summary>
        /// 实际人民币外包支出
        /// </summary>
        public decimal? RMBHOutValue { get; set; }
    }
}
