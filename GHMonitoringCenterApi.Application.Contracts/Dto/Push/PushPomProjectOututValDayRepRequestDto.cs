using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// 项目日报请求对象
    /// </summary>
    public class ProjectDayRepRequestJsonDto
    {
        public string ProjectDayRepRequestJson { get; set; }
    }
    /// <summary>
    /// 产值日报对象
    /// </summary>
    public class ProjectOututValDayRepRequestDto
    {
        /// <summary>
        /// 项目日报id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 当月计划产值
        /// </summary>
        public decimal? PlannedOutputValue { get; set; }
        /// <summary>
        /// 项目已完成合同金额
        /// </summary>
        public decimal? CompleteAmount { get; set; }
        /// <summary>
        /// 当日实际产值
        /// </summary>
        public decimal? ActualOutputValueOfDay { get; set; }
        /// <summary>
        /// 当月实际产值
        /// </summary>
        public decimal? ActualOutputValue { get; set; }
        /// <summary>
        /// 年度实际产值
        /// </summary>
        public decimal YearActualOutputValue { get; set; }
        /// <summary>
        /// 累计实际产值
        /// </summary>
        public decimal CumulativeOutputValue { get; set; }
        /// <summary>
        /// 区分当前船舶是否要填写日报 默认false 需要填报  如果标记当前项目不需要填写船舶日报传true
        /// </summary>
        public bool IsFillShip { get; set; } = false;
        /// <summary>
        /// 接收日期
        /// </summary>
        public DateTime SubmitDate { get; set; }
    }
    /// <summary>
    /// 船舶日报请求对象
    /// </summary>
    public class OwnShipDayRepDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 自有船舶id
        /// </summary>
        public string OwnShipId { get; set; }
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string OwnShipName { get; set; }
        /// <summary>
        /// 项目日报Id
        /// </summary>
        public string ProjectDailyId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 船舶状态 0 施工 1调遣 2 厂休 3 待命  4 航修  取下拉列表名称
        /// </summary>
        public string ShipStatus { get; set; }
        /// <summary>
        /// 船舶日报填写日期
        /// </summary>
        public DateTime SubmitDate { get; set; }
        /// <summary>
        /// 判断装备是否填报 默认为0  1是装备已提交 2是装备未提交  
        /// </summary>
        public int? IsShipFilling { get; set; } = 2;
    }
    /// <summary>
    /// 安监日报请求对象
    /// </summary>
    public class ProjectSafeDayRepRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 地方政府许可复工日期是否明确  1 明确 0 不明确
        /// </summary>
        public int IsWork { get; set; }
        /// <summary>
        /// 地方政府许可复工日期
        /// </summary>
        public DateTime? WorkDate { get; set; }
        /// <summary>
        /// 是否完成隶属单位复工审批  是 1 否 0
        /// </summary>
        public int IsCompanyWork { get; set; }
        /// <summary>
        /// 项目复工状态
        /// </summary>
        public string WorkStatus { get; set; }
        /// <summary>
        /// 当天主要施工内容【未停工】
        /// </summary>
        public string ConstructionContent { get; set; }
        /// <summary>
        /// 当天安全生产情况【未停工】
        /// </summary>
        public string ProductionSituation { get; set; }
        /// <summary>
        /// 【未开复工】项目现计划复工日期
        /// </summary>
        public DateTime? PlanWorkDate { get; set; }
        /// <summary>
        /// 未开复工原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 项目未开复工原因具体情况
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 【已复工】项目实际复工日期
        /// </summary>
        public DateTime? ActualWorkDate { get; set; }
        /// <summary>
        /// 当天主要施工内容【已复工】
        /// </summary>
        public string ActualConstructionContent { get; set; }
        /// <summary>
        /// 当天安全生产情况【已复工】
        /// </summary>
        public string ActualSituation { get; set; }
        /// <summary>
        /// 【今日返回】项目管理人员数量
        /// </summary>
        public int? ManagementNumber { get; set; }
        /// <summary>
        /// 【今日返回】船舶人员数量
        /// </summary>
        public int? PersonNumber { get; set; }
        /// <summary>
        /// 【今日返回】产业工人数量
        /// </summary>
        public int? WorkerNumber { get; set; }
        /// <summary>
        /// 【今日返回】来自武汉人数  中高风险
        /// </summary>
        public int? FromWuHanNumber { get; set; }
        /// <summary>
        /// 【当前在场】项目管理人员数量
        /// </summary>
        public int? InManagementNumber { get; set; }
        /// <summary>
        /// 【当前在场】船舶人员数量
        /// </summary>
        public int? InPersonNumber { get; set; }
        /// <summary>
        /// 【当前在场】产业工人数量
        /// </summary>
        public int? InWorkerNumber { get; set; }
        /// <summary>
        /// 【当前在场】来自武汉人数 中高风险
        /// </summary>
        public int? InFromWuHanNumber { get; set; }
        /// <summary>
        /// 【当前隔离】项目管理人员数量
        /// </summary>
        public int? QuarantineManagementNum { get; set; }
        /// <summary>
        /// 【当前隔离】船舶人员数量
        /// </summary>
        public int? QuarantinePersonNum { get; set; }
        /// <summary>
        /// 【当前隔离】产业工人数量
        /// </summary>
        public int? QuarantineWorkerNum { get; set; }
        /// <summary>
        /// 隔离原因
        /// </summary>
        public string QuarantineReason { get; set; }
        /// <summary>
        /// 确诊人数
        /// </summary>
        public int? DiagnosisNum { get; set; }
        /// <summary>
        /// 疑似人数
        /// </summary>
        public int? SuspectsNum { get; set; }
        /// <summary>
        /// 【应在场】项目管理人员数量
        /// </summary>
        public int? ShouldManagementNum { get; set; }
        /// <summary>
        /// 【应在场】船舶人员数量
        /// </summary>
        public int? ShouldPersonNum { get; set; }
        /// <summary>
        /// 【应在场】产业工人数量
        /// </summary>
        public int? ShouldWorkerNum { get; set; }
        /// <summary>
        /// 口罩（个）
        /// </summary>
        public int? MaskNum { get; set; }
        /// <summary>
        /// 体温计（个）
        /// </summary>
        public int? ThermometerNum { get; set; }
        /// <summary>
        /// 消毒液（升）
        /// </summary>
        public int? DisinfectantNum { get; set; }
        /// <summary>
        /// 存储和消防安全措施（简要描述）
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 当日安全生产情况
        /// </summary>
        public string Situation { get; set; }
        /// <summary>
        /// 其他须说明的事项
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 是否接受过上级督查  是 1  否 0
        /// </summary>
        public int IsSuperiorSupervision { get; set; }
        /// <summary>
        /// 上级督查次数
        /// </summary>
        public int? SuperiorSupervisionCount { get; set; }
        /// <summary>
        /// 上级督查形式
        /// </summary>
        public string SuperiorSupervisionForm { get; set; }
        /// <summary>
        /// 上级督查时间
        /// </summary>
        public DateTime? SuperiorSupervisionDate { get; set; }
        /// <summary>
        /// 督查单位
        /// </summary>
        public string SupervisionUnit { get; set; }
        /// <summary>
        /// 督查领导
        /// </summary>
        public string SupervisionLeader { get; set; }
        /// <summary>
        /// 上级督查其他人员
        /// </summary>
        public string SupervisionOther { get; set; }
        /// <summary>
        /// <summary>
        /// 判断产值情况 或 复工情况是否填报 默认为2 产值与复工均未提交，0：产值情况未提交，1：复工情况未提交，3：全部提交
        /// </summary>
        public int? IsFilling { get; set; } = 2;
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime SubmitDate { get; set; }
    }
}
