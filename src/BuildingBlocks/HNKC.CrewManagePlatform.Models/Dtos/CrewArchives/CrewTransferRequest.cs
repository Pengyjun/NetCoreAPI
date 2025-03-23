using HNKC.CrewManagePlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 船员调任
    /// </summary>
    public class CrewTransferRequest : BaseRequest
    {
        /// <summary>
        /// 改变船员计划工作时间 true是 修改原计划
        /// </summary>
        public bool IsChangePlan { get; set; }
        /// <summary>
        /// 原船舶id
        /// </summary>
        public string? OldShipId { get; set; }
        /// <summary>
        /// 船舶
        /// </summary>
        public string? OnShip { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string? Postition { get; set; }
        /// <summary>
        /// 上船日期
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
      //  public DateTime WorkShipEndTime { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (WorkShipStartTime > WorkShipEndTime) yield return new ValidationResult("上船日期大于下船日期", new string[] { nameof(WorkShipStartTime) });
        //}
    }
}
