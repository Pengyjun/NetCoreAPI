using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{

    /// <summary>
    /// 保存请求DTO
    /// </summary>
    public class SaveShipPlanRequestDto:IValidatableObject
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public bool ReuqestType { get; set; }

        /// <summary>
        ///船舶状态类型
        /// </summary>
        public int? ShipStatusType { get; set; }
        /// <summary>
        /// 船舶状态名称
        /// </summary>
        public string? ShipStatusName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>

        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>

        public string? ProjectName { get; set; }

        /// <summary>
        /// 间隔天数
        /// </summary>

        //public int Days { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>

        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>

        public DateTime EndTime { get; set; }


        /// <summary>
        /// 工程量
        /// </summary>

        public decimal QuantityWork { get; set; }

        /// <summary>
        /// 单价
        /// </summary>

        public decimal Price { get; set; }

        /// <summary>
        /// 年份
        /// </summary>

        //public int? Year { get; set; }

        /// <summary>
        /// 计划产值 第一月   
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
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReuqestType && (Id==null||  !Id.HasValue || Id.Value == Guid.Empty))
            {
                yield return new ValidationResult("编辑时Id不能为空", new string[]{nameof(Id) }); 
            }

            if (ShipStatusType == null || !ShipStatusType.HasValue || ShipStatusType.Value < 0 || ShipStatusType.Value > 5)
            {
                yield return new ValidationResult("船舶状态类型不合法", new string[] { nameof(ShipStatusType) });
            }

            if (ShipStatusType != null && ShipStatusType.HasValue && ShipStatusType==0&& (ProjectId==null|| !ProjectId.HasValue))
            {
                yield return new ValidationResult("船舶为施工状态时项目Id必须要传", new string[] { nameof(ProjectId) });
            }

            if (ShipStatusType != null && ShipStatusType.HasValue && ShipStatusType > 1 && ShipStatusType <= 5 && ProjectId!=null&&ProjectId.HasValue)
            {
                yield return new ValidationResult("船舶为非施工状态时项目Id不能传", new string[] { nameof(ProjectId) });
            }
        }
    }
}
