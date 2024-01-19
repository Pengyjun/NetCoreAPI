using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements
{
    /// <summary>
    /// 船舶进出场结果
    /// <para>备注：if Status！=1 显示进场按钮，else 显示退场按钮</para>
    /// </summary>
    public class ShipMovementResponseDto : ShipMovementDto
    {
        /// <summary>
        /// 船舶进出场Id
        /// </summary>
        public Guid ShipMovementId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ShipMovementStatus Status { get; set; }

        /// <summary>
        /// 船舶来源类型名称(注：前端界面（船舶来源）绑定字段)
        /// </summary>
        public string? ShipTypeName { get { return EnumExtension.GetEnumDescription(ShipType); } }

        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipKindTypeName { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? QuitTime { get; set; }

        /// <summary>
        /// 进场/退场时间
        /// </summary>
        public DateTime? EnterOrQuitTime
        {
            get
            {
                if (Status == ShipMovementStatus.Enter)
                {
                    return EnterTime;
                }
                else if (Status == ShipMovementStatus.Quit)
                {
                    return QuitTime;
                }
                return null;
            }
        }
    }
}
