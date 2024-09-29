using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject
{
    /// <summary>
    /// wbs转换响应dto
    /// </summary>
    public class ProjectWBSDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? EngQuantity { get; set; }
        /// <summary>
        /// Pid
        /// </summary>
        public string? Pid { get; set; }
        /// <summary>
        /// 子集id
        /// </summary>
        public string? KeyId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 施工分类名称
        /// </summary>
        public string? ProjectWBSName { get; set; }
        /// <summary>
        /// 做业务区分字段
        /// month
        /// </summary>
        public ValueEnumType ValueType { get; set; }
        /// <summary>
        /// 列表行允许删除 true允许删除  false不允许删除
        /// </summary>
        public bool IsAllowDelete { get; set; }
        /// <summary>
        /// 填报月份
        /// </summary>
        public int DateMonth { get; set; }
        /// <summary>
        /// 填报年份
        /// </summary>
        public int DateYear { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ProjectWBSDto> Children { get; set; } = new List<ProjectWBSDto> { };
        /// <summary>
        /// 最后资源节点
        /// </summary>
        public List<ProjectWBSDto> ReportDetails { get; set; } = new List<ProjectWBSDto> { };

        #region 详细节点字段
        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal CompleteProductionAmount { get; set; }
        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        public decimal CompletedQuantity { get; set; }
        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNature { get; set; }
        /// <summary>
        /// 施工性质名称
        /// </summary>
        public string? ConstructionNatureName { get; set; }
        /// <summary>
        /// 合同产值
        /// </summary>
        public decimal? ContractAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ContractQuantity { get; set; }
        /// <summary>
        /// 详细月报主键id
        /// </summary>
        public Guid? DetailId { get; set; }
        /// <summary>
        /// 产值属性
        /// </summary>
        public ConstructionOutPutType OutPutType { get; set; }
        /// <summary>
        /// 产值属性名称
        /// </summary>
        public string? OutPutTypeName { get; set; }
        /// <summary>
        /// 本月外包支出
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }
        /// <summary>
        /// 项目WBSId
        /// </summary>
        public Guid ProjectWBSId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 资源：船舶Id(PomId)
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 累计完成产值
        /// </summary>
        public decimal TotalCompleteProductionAmount { get; set; }
        /// <summary>
        /// 累计完成工程量
        /// </summary>
        public decimal TotalCompletedQuantity { get; set; }
        /// <summary>
        /// 累计外包支出
        /// </summary>
        public decimal TotalOutsourcingExpensesAmount { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 本年完成产值
        /// </summary>
        public decimal YearCompleteProductionAmount { get; set; }
        /// <summary>
        /// 本年完成工程量
        /// </summary>
        public decimal YearCompletedQuantity { get; set; }
        /// <summary>
        /// 本年外包支出
        /// </summary>
        public decimal YearOutsourcingExpensesAmount { get; set; }

        #endregion
        /// <summary>
        /// wbs使用
        /// </summary>
        public int IsDelete {  get; set; }
    }
}
