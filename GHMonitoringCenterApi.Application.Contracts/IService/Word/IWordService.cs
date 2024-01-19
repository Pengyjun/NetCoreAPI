using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Word
{


    /// <summary>
    /// word相关操作
    /// </summary>
    public interface IWordService
    {


        /// <summary>
        /// 项目月报简报导出word
        /// </summary>
        /// <returns></returns>
        Task<Stream> MonthReportImportWordAsync(BaseConfig baseConfig , MonthtReportsRequstDto model);
        /// <summary>
        /// 项目月报简报导出word1
        /// </summary>
        /// <returns></returns>
        Task<Stream> MonthReportImportWordAsync1(BaseConfig baseConfig, MonthtReportsRequstDto model);
    }
}
