
using GHMonitoringCenterApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 船舶成本维护增改请求dto
    /// </summary>
    public class AddOrModifyShipCostMaintenaceRequestDto : IValidatableObject
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 船舶分类
        /// </summary>
        public ShipCostType ShipCostType { get; set; }

        /// <summary>
        /// 自有船舶id
        /// </summary>
        public Guid OwnShipId { get; set; }

        /// <summary>
        /// 境内境外  0境内  1境外
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 增或改  日期
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 不含油成本
        /// </summary>
        public decimal NoOilCost { get; set; }

        /// <summary>
        /// 燃油成本
        /// </summary>
        public decimal? OilCost { get; set; }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(OwnShipId.ToString()) || OwnShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id参数不能为空且类型是GUID类型", new string[] { nameof(OwnShipId) });
            }

            if (string.IsNullOrWhiteSpace(NoOilCost.ToString()) || NoOilCost == 0)
            {
                yield return new ValidationResult("不含油成本不能为空且大于0", new string[] { nameof(NoOilCost) });
            }

            if (string.IsNullOrWhiteSpace(Date.ToString()) || Date == DateTime.MinValue)
            {
                yield return new ValidationResult("增或改日期不能为空", new string[] { nameof(Date) });
            }
        }
    }

    /// <summary>
    /// 船舶成本维护删除 dto
    /// </summary>
    public class DeleteShipCostMaintenanceRequestDto : IValidatableObject
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 删除日期 
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id.ToString()) || Id == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Id) });
            }

            if (string.IsNullOrWhiteSpace(Date.ToString()) || Date == DateTime.MinValue)
            {
                yield return new ValidationResult("删除日期不能为空", new string[] { nameof(Date) });
            }
        }
    }

}
