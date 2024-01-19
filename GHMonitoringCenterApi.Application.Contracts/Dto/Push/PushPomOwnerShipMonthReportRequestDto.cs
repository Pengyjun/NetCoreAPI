using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// 推送到Pom的分包船舶月报
    /// </summary>
    public class PushPomOwnerShipMonthReportRequestDto
    {
        public string ProjectOwnShipMonthRepJson { get; set; }
    }

    /// <summary>
    /// 接收 增改项目船舶月报请求model 
    /// </summary>
    public class PomProjectOwnShipMonthRepRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 自有船舶id
        /// </summary>
        public string OwnShipId { get; set; }
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string OwnShipName { get; set; }
        /// <summary>
        /// 自有船舶进场日期
        /// </summary>
        public string ApproachDate { get; set; }
        /// <summary>
        /// 自有船舶退场日期
        /// </summary>
        public string ExitDate { get; set; }
        /// <summary>
        /// 本月工程量（万方）
        /// </summary>
        public decimal Quantities { get; set; }
        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal OutputValue { get; set; }
        /// <summary>
        /// 自有船舶运转(h)
        /// </summary>
        public decimal WorkHours { get; set; }
        /// <summary>
        /// 本月施工天数
        /// </summary>
        public decimal ConstructionDays { get; set; }
        /// <summary>
        /// 工艺方式Id
        /// </summary>
        public string WorkModeId { get; set; }
        /// <summary>
        /// 疏浚吹填分类Id
        /// </summary>
        public string WorkTypeId { get; set; }
        /// <summary>
        /// 清单类型
        /// </summary>
        public string ContractType { get; set; }
        /// <summary>
        /// 工况级别Id
        /// </summary>
        public string ConditionGradeId { get; set; }
        /// <summary>
        /// 运距
        /// </summary>
        public decimal HaulDistance { get; set; }
        /// <summary>
        /// 挖深(M)
        /// </summary>
        public decimal DigDeep { get; set; }
        /// <summary>
        /// 吹距(KM)
        /// </summary>
        public decimal BlowingDistance { get; set; }
        /// <summary>
        /// 填报月份
        /// </summary>
        public DateTime StatementMonth { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 自有船舶月报施工土质
        /// </summary>
        public List<PomShipMonthRepSoil> ShipMonthRepSoils { get; set; }
    }

    /// <summary>
    /// 自有船舶月报施工土质
    /// </summary>
    public class PomShipMonthRepSoil
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// z自有船舶Id
        /// </summary>
        public string OwnShipId { get; set; }
        /// <summary>
        /// 施工土质id
        /// </summary>
        public string SoilId { get; set; }
        /// <summary>
        /// 土质分级id
        /// </summary>
        public string SoilGradeId { get; set; }
        /// <summary>
        /// 所在比重 比重不能为0
        /// </summary>
        public decimal Proportion { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime StatementMonth { get; set; }
    }
}
