using HNKC.CrewManagePlatform.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.ShipDuty
{
    /// <summary>
    /// 离船申请dto
    /// </summary>
    public class DepartureResponseDto
    {
        public Guid? UserId { get; set; }
        public DateTime? DisembarkDate { get; set; }
        public DateTime? ReturnShipDate { get; set; }
        public RotaEnum RotaType { get; set; }
    }
}
