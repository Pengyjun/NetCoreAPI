using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{


    /// <summary>
    /// 导出修理备件数据DTO
    /// </summary>
    public class ImportSparePartCollectionResponseDto
    {

        public List<GetRepairItemsListResponseDto> GetRepairItemsListResponseDto { get; set; }
        public List<SparePartProjectListResponseDto> SparePartProjectListResponseDto { get; set; }
        public List<SendShipSparePartListResponseDto> SendShipSparePartListResponseDto { get; set; }
        public List<SaveSparePartStorageListResponseDto> SaveSparePartStorageListResponseDto { get; set; }
    }
}
