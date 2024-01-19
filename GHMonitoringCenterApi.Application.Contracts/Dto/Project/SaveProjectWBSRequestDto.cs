using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{

    /// <summary>
    /// 项目结构保存请求
    /// </summary>
    public class SaveProjectWBSRequestDto : IValidatableObject
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 节点集合
        /// </summary>
        public ReqProjectWBSNode[] Nodes { get; set; } = new ReqProjectWBSNode[0];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
            if (Nodes == null || !Nodes.Any())
            {
                yield return new ValidationResult("项目结构变更节点集合不能为空", new string[] { nameof(Nodes) });
            }
        }

        /// <summary>
        /// 项目结构变更对象
        /// </summary>
        public class ReqProjectWBSNode : ProjectWBSModelDto<ReqProjectWBSNode>, IValidatableObject, ICloneable
        {

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {

                if (string.IsNullOrWhiteSpace(Name))
                {
                    yield return new ValidationResult("项目结构名称不能为空", new string[] { nameof(Name) });
                }
                // todo 暂时不做验证，历史数据存在很多null值，前端做修改进行验证
                //if (UnitPrice == null || UnitPrice < 0)
                //{
                //    yield return new ValidationResult("单价不能为空或小于0", new string[] { nameof(UnitPrice) });
                //}
                //if (EngQuantity == null || EngQuantity < 0)
                //{
                //    yield return new ValidationResult("工程量不能为空或小于0", new string[] { nameof(EngQuantity) });
                //}
            }

            /// <summary>
            /// 克隆
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
