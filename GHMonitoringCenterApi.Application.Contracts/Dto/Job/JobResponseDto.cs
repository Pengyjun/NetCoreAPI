using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务返回结果
    /// </summary>
    public  class JobResponseDto
    {

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public virtual BizModule BizModule { get; set; }

        /// <summary>
        /// 业务模块名称
        /// </summary>
        public  string? BizModuleName { get { return BizModule.ToDescription(); } }

        /// <summary>
        /// 任务状态 1:代办，2：已办
        /// </summary>
        public JobStatus JobStatus { get; set; }

        /// <summary>
        /// 审批状态  1：驳回，2：通过
        /// </summary>
        public JobApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// （当前）审批人Id
        /// </summary>
        public Guid? ApproverId { get; set; }

        /// <summary>
        /// （当前）审批层级（1：一级审批）
        /// </summary>
        public ApproveLevel ApproveLevel { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 业务处理信息
        /// </summary>
        public string? BizHandleMessage { get; set; }

        /// <summary>
        /// 任务是否结束
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 项目类型 id
        /// </summary>
        public Guid? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ProjectCode { get; set; }

        /// <summary>
        /// 施工地点 id
        /// </summary>
        public Guid? ProjectAreaId { get; set; }

        /// <summary>
        /// 项目所在地(城市)
        /// </summary>
        public string? ProjectCity { get; set; }

        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string? ProjectClassifyStandard { get; set; }

        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public Guid? ProjectCompanyId { get; set; }

        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int ProjectCategory { get; set; }

        /// <summary>
        /// 合同总价
        /// </summary>
        public decimal? ProjectAmount { get; set; }

        /// <summary>
        /// 币种 id
        /// </summary>
        public Guid? ProjectCurrencyId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 项目外文名称
        /// </summary>
        public string? ProjectForeign { get; set; }

        /// <summary>
        /// 项目曾用名
        /// </summary>
        public string? ProjectBeforeName { get; set; }

        /// <summary>
        /// 施工地点名称（省份名称）
        /// </summary>
        public string? ProjectAreaName { get; set; }

        /// <summary>
        /// 项目所在地(城市)
        /// </summary>
        public string? ProjectCityName { get; set; }

        /// <summary>
        /// 行业分类标准名称（中交项目业务分类）
        /// </summary>
        public string? ProjectClassifyStandardName { get; set; }

        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string? ProjectTypeName { get; set; }

        /// <summary>
        /// 国家/地区
        /// </summary>
        public string? CountryRegion { get; set; }

        /// <summary>
        /// 项目类别名称（境内，境外）
        /// </summary>
        public string? ProjectCategoryName { get; set; }

        /// <summary>
        /// 项目所属公司名称
        /// </summary>
        public string? ProjectCompanyName { get; set; }

        /// <summary>
        /// 币种 名称
        /// </summary>
        public string?  ProjectCurrencyName { get; set; }

        /// <summary>
        /// 项目税率
        /// </summary>
        public decimal? ProjectRate { get; set; }

        /// <summary>
        /// 审批状态（文本展示）
        /// </summary>
        public string? ApproveStatusText { get; set; }

        /// <summary>
        /// 提交人Id
        /// </summary>
        public Guid? SubmiterId { get; set; }

        /// <summary>
        /// 提交人姓名
        /// </summary>
        public string? SubmiterName { get; set; }
    }
}
