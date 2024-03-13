using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
    /// <summary>
    /// 项目月报导出Word响应Dto
    /// </summary>
    public class MonthReportProjectWordResponseDto
    {

        /// <summary>
        /// 月报Id
        /// </summary>
        public Guid? Id { get; set; }
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目归属
        /// </summary>
        public string? ProjectBelonging { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ProjectType { get; set; }
        /// <summary>
        /// 项目等级
        /// </summary>
        public string? ProjectGrade { get; set; }
        /// <summary>
        /// 工程位置
        /// </summary>
        public string? ProjectLocation { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string? ProjectState { get; set; }
        /// <summary>
        /// 项目内容
        /// </summary>
        public string? ProjectContent { get; set; }
        /// <summary>
        /// 汇率Id
        /// </summary>
        public Guid? CurrencyConverterId { get; set; }
        /// <summary>
        /// 原合同额
        /// </summary>
        public decimal? OriginalContractAmount { get; set; }
        /// <summary>
        /// 变更额
        /// </summary>
        public decimal? ChangeAmount { get; set; }
        /// <summary>
        /// 变更合同额
        /// </summary>
        public decimal? ActualContractAmount { get; set; }
        /// <summary>
        /// 变更合同信息
        /// </summary>
        public string? ContractChangeInformation { get; set; } = "暂无";
        /// <summary>
        /// 开工时间
        /// </summary>
        public string? CommencementTime { get; set; } 
        /// <summary>
        /// 工期信息
        /// </summary>
        public string? DurationInformation { get; set; }
        /// <summary>
        /// 当月项目进展
        /// </summary>
        public string? MonthProjectDebriefing { get; set; }

        #region 工程产值
        /// <summary>
        /// 工程产值 - 月产值
        /// </summary>
        public decimal? EngineeringMonthOutputValue { get; set; }
        /// <summary>
        /// 工程产值 - 年产值
        /// </summary>
        public decimal? EngineeringYeahOutputValue { get; set; }
        /// <summary>
        /// 工程产值 - 工程累计
        /// </summary>
        public decimal? EngineeringAccumulatedEngineering { get; set; }
        /// <summary>
        /// 工程产值 - 比例
        /// </summary>
        public decimal? EngineeringProportion { get; set; }
        /// <summary>
        /// 工程产值 - 备注
        /// </summary>
        public string? EngineeringRemarks { get; set; }
        #endregion

        #region 业主确认
        /// <summary>
        /// 业主 - 月产值
        /// </summary>
        public decimal? OwnerMonthOutputValue { get; set; }
        /// <summary>
        /// 业主 - 年产值
        /// </summary>
        public decimal? OwnerYeahOutputValue { get; set; }
        /// <summary>
        /// 业主 - 工程累计
        /// </summary>
        public decimal? OwnerAccumulatedEngineering { get; set; }
        /// <summary>
        /// 业主 - 比例
        /// </summary>
        public decimal? OwnerProportion { get; set; }
        /// <summary>
        /// 业主 - 备注
        /// </summary>
        public string? OwnerRemarks { get; set; }
        #endregion

        #region 工程收款
        /// <summary>
        /// 工程收款 - 月产值
        /// </summary>
        public decimal? ProjectMonthOutputValue { get; set; }
        /// <summary>
        /// 工程收款 - 年产值
        /// </summary>
        public decimal? ProjectYeahOutputValue { get; set; }
        /// <summary>
        /// 工程收款 - 工程累计
        /// </summary>
        public decimal? ProjectAccumulatedEngineering { get; set; }
        /// <summary>
        /// 工程收款 - 比例
        /// </summary>
        public decimal? ProjectProportion { get; set; }
        /// <summary>
        /// 工程收款 - 备注
        /// </summary>
        public string? ProjectRemarks { get; set; }
        #endregion

        /// <summary>
        /// 存在问题
        /// </summary>
        public string? ExistingProblems { get; set; }
        /// <summary>
        /// 采取措施
        /// </summary>
        public string? TakeSteps { get; set; }
        /// <summary>
        /// 需公司协调事项
        /// </summary>
        public string? CoordinationMatters { get; set; } = "暂无";
        /// <summary>
        /// 月报时间
        /// </summary>
        public int? MonthTime { get; set; }

    }
}
