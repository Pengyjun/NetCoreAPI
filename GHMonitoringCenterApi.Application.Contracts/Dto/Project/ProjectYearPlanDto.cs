
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 年度计划响应dto
    /// </summary>
    public class ProjectYearPlanDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 计划产值 按季度导入  第一月
        /// </summary>
        public decimal OnePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值   第二月
        /// </summary>
        public decimal TwoPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第三月
        /// </summary>
        public decimal ThreePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第四月
        /// </summary>
        public decimal FourPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第五月
        /// </summary>
        public decimal FivPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第六月
        /// </summary>
        public decimal SixPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第七月
        /// </summary>
        public decimal SevPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第八月
        /// </summary>
        public decimal EigPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第九月
        /// </summary>
        public decimal NinPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十月
        /// </summary>
        public decimal TenPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十一月
        /// </summary>
        public decimal ElePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十二月
        /// </summary>
        public decimal TwePlannedOutputValue { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime SubmitYear { get; set; }

        /// <summary>
        /// 计划工程量 按季度导入  第一月
        /// </summary>
        public decimal OnePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量   第二月
        /// </summary>
        public decimal TwoPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第三月
        /// </summary>
        public decimal ThreePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第四月
        /// </summary>
        public decimal FourPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第五月
        /// </summary>
        public decimal FivPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第六月
        /// </summary>
        public decimal SixPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第七月
        /// </summary>
        public decimal SevPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第八月
        /// </summary>
        public decimal EigPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第九月
        /// </summary>
        public decimal NinPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十月
        /// </summary>
        public decimal TenPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十一月
        /// </summary>
        public decimal ElePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十二月
        /// </summary>
        public decimal TwePlannedQuantities { get; set; }
    }
}
