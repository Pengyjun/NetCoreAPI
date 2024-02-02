using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport
{
    public class ResultResponseDto
    {
        public string datesheetone1 { get; set; }
        public List<CompanyProjectBasePoduction> resultone1 { get; set; }
        public List<CompanyBasePoductionValue> resultone2 { get; set; }
        public List<CompanyShipBuildInfo> resultone3 { get; set; }
        public List<CompanyShipProductionValueInfo> resultone4 { get; set; }
        public List<SpecialProjectInfo> resultone6 { get; set; }
        public List<CompanyWriteReportInfo> resultone7 { get; set; }
        public List<CompanyUnWriteReportInfo> resultone8 { get; set; }
        public List<CompanyShipUnWriteReportInfo> resultone9 { get; set; }
        public List<ShipProductionValue> resultone5 { get; set; }
        public List<ProjectShiftProductionInfo> ProjectShiftProductionInfo { get; set; }
        public List<UnProjectShitInfo> UnProjectShitInfo { get; set; }
        public List<ExcelTitle> ExcelTitle { get; set; }
    }
}
