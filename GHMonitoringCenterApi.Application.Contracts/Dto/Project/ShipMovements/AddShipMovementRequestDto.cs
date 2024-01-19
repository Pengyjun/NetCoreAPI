using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using UtilsSharp;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 新增船舶动向
    /// </summary>
    public class AddShipMovementRequestDto : ShipMovementDto, IValidatableObject
    {

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
            if ( ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ProjectId) });
            }
            if (ShipType != ShipType.OwnerShip&& ShipType!= ShipType.SubShip)
            {
                yield return new ValidationResult("船舶类型不存在", new string[] { nameof(ShipType) });
            }
            if (Remarks != null && Remarks.Length>500)
            {
                yield return new ValidationResult("备注长度不能超过500", new string[] { nameof(Remarks) });
            }
            //if(EnterTime==null|| EnterTime>DateTime.Now.Date.AddDays(1))
            //{
            //    yield return new ValidationResult("船舶进场时间不能为空或大于今天日期", new string[] { nameof(EnterTime) });
            //}
        }

    }

       
}
