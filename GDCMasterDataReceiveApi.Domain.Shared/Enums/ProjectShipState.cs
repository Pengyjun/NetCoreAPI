using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{
    /// <summary>
    /// 项目-船舶状态(0:施工,1:调遣,2:厂修,3:待命)
    /// </summary>
    public enum ProjectShipState
    {
        /// <summary>
        /// 施工
        /// </summary>
        [Description("施工")]
        Construction = 0,

        /// <summary>
        /// 调遣
        /// </summary>
        [Description("调遣")]
        Dispatch = 1,

        /// <summary>
        /// 厂修
        /// </summary>
        [Description("厂修")]
        Repair = 2,

        /// <summary>
        /// 待命
        /// </summary>
        [Description("待命")]
        Standby = 3,
        /// <summary>
        /// 航修
        /// </summary>
        [Description("航修")]
        VoyageRepair = 4,

        /// <summary>
        /// 检修
        /// </summary>
        [Description("检修")]
        OverHaul = 5,

    }
}
