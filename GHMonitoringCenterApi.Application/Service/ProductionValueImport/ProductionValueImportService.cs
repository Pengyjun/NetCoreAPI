using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.ProductionValueImport
{

    /// <summary>
    ///  生产日报每天推送和节假日日报推送历史数据导出实现层
    /// </summary>
    public class ProductionValueImportService : IProductionValueImportService
    {
        public Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> ImportProductionValuesAsync(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
