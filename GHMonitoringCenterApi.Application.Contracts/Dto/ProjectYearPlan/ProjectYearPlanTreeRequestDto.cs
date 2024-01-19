using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan
{

    /// <summary>
    /// wbs 保存请求dto
    /// </summary>
    public class ProjectYearPlanTreeRequestDto:IValidatableObject
    {
        #region 辅助字段

        /// <summary>
        /// 请求类型  默认是true  默认是添加  false是编辑
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// 如果是true说明修改的是资源单价 如果false修改的是wbs里面的单价
        /// </summary>
        public bool IsDelete { get; set; }=false;

        #endregion
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        public Guid ProjectWbsId { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal Unitprice { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal ContractQuantity { get; set; }

        /// <summary>
        /// 合同产值
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 年初工程量
        /// </summary>
        public decimal YearInitQuantity { get; set; }

        /// <summary>
        /// 年初产值
        /// </summary>
        public decimal YearInitProductionValue { get; set; }



        /// <summary>
        /// 剩余工程量
        /// </summary>
        public decimal ResidueQuantity { get; set; }

        /// <summary>
        /// 剩余产值
        /// </summary>
        public decimal ResidueProductionValue { get; set; }


        /// <summary>
        /// 一月份工程量
        /// </summary>
        public decimal? OneQuantity { get; set; }
        /// <summary>
        /// 一月份产值
        /// </summary>
        public decimal? OneProductionValue { get; set; }
        /// <summary>
        /// 二月份工程量
        /// </summary>
        public decimal? TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        public decimal? TwoProductionValue { get; set; }


        public decimal? ThreeQuantity { get; set; }
        public decimal? ThreeProductionValue { get; set; }
        public decimal? FourQuantity { get; set; }
        public decimal? FourProductionValue { get; set; }
        public decimal? FiveQuantity { get; set; }
        public decimal? FiveProductionValue { get; set; }
        public decimal? SixQuantity { get; set; }
        public decimal? SixProductionValue { get; set; }
        public decimal? SevenQuantity { get; set; }
        public decimal? SevenProductionValue { get; set; }
        public decimal? EightQuantity { get; set; }
        public decimal? EightProductionValue { get; set; }
        public decimal? NineQuantity { get; set; }
        public decimal? NineProductionValue { get; set; }
        public decimal? TenQuantity { get; set; }
        public decimal? TenProductionValue { get; set; }
        public decimal? ElevenQuantity { get; set; }
        public decimal? ElevenProductionValue { get; set; }
        public decimal? TwelveQuantity { get; set; }
        public decimal? TwelveProductionValue { get; set; }



      

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("项目ID不能为空", new string[] { nameof(Id) });
            }
            if (Unitprice <0)
            {
                yield return new ValidationResult("单价必须大于等于0", new string[] { nameof(Unitprice) });
            }

            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("ProjectWbsId不能为空", new string[] { nameof(ProjectWbsId) });
            }
            
        }
    }
}
