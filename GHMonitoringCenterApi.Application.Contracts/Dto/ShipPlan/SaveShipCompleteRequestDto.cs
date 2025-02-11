using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{
    public class SaveShipCompleteRequestDto:IValidatableObject
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public bool ReuqestType { get; set; }
        public Guid? Id { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>

        public string ShipName { get; set; }

        /// <summary>
        /// 工程量  单位 万m³
        /// </summary>

        public decimal QuantityWork { get; set; }

        /// <summary>
        /// 单价  单位元m³
        /// </summary>

        public decimal Price { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int? DateDay { get; set; }
        /// <summary>
        /// 完成产值 
        /// </summary>

        public decimal CompleteOutputValue { get; set; }
        /// <summary>
        /// 计划产值 
        /// </summary>

        public decimal PlanOutputValue { get; set; }
        ///// <summary>
        ///// 产值偏差  完成产值-计划产值  单位（万元）
        ///// </summary>
        //public decimal DiffProductionValue { get; set; }
        /// <summary>
        /// 偏差原因
        /// </summary>
        public string? Reason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReuqestType && (Id == null || !Id.HasValue || Id.Value == Guid.Empty))
            {
                yield return new ValidationResult("编辑时Id不能为空", new string[] { nameof(Id) });
            }
        }
    }
}
