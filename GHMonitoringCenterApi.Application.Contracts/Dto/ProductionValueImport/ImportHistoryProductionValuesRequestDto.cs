using GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport
{

    /// <summary>
    /// 生产日报导出请求DTO
    /// </summary>
    public class ImportHistoryProductionValuesRequestDto
    {
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        //public DateTime? StartTime { get; set; }
        ///// <summary>
        ///// 结束日期
        ///// </summary>
        //public DateTime? EndTime { get; set; }
        public DateTime? TimeValue { get; set; }
        /// <summary>
        /// 日期转换
        /// </summary>
        public void GetYearAndMonth()
        {
            if (TimeValue == DateTime.MinValue || string.IsNullOrWhiteSpace(TimeValue.ToString()))
            {
                Year = DateTime.Now.ToDateYear();
                Month = DateTime.Now.ToDateMonth();
            }
            else
            {
                Year = TimeValue.Value.ToDateYear();
                Month = TimeValue.Value.ToDateMonth();
            }
        }
        //public int GetStartDate()
        //{
        //    if (StartTime == DateTime.MinValue || string.IsNullOrWhiteSpace(StartTime.ToString()))
        //    {
        //        return DateTime.Now.AddDays(-1).ToDateDay();
        //    }
        //    else
        //    {
        //        return Convert.ToDateTime(StartTime).ToDateDay();
        //    }

        //}
        ///// <summary>
        ///// 日期转换
        ///// </summary>
        //public int GetEndDate()
        //{
        //    if (EndTime == DateTime.MinValue || string.IsNullOrWhiteSpace(EndTime.ToString()))
        //    {
        //        return DateTime.Now.AddDays(-1).ToDateDay();
        //    }
        //    else
        //    {
        //        return Convert.ToDateTime(EndTime).ToDateDay();
        //    }
        //}
    }
    /// <summary>
    /// excel 导出数据响应调dto
    /// </summary>
    public class ImportHistoryProductionValuesResponseDto
    {
        /// <summary>
        /// 各公司项目基本情况（个数）
        /// </summary>
        public List<ExcelCompanyProjectBasePoduction>? ExcelCompanyProjectBasePoductions { get; set; }
        /// <summary>
        /// 各公司项目基本情况（产值）
        /// </summary>
        public List<ExcelCompanyBasePoductionValue>? ExcelCompanyBasePoductions { get; set; }
        /// <summary>
        /// 自有施工船舶运转情况（个数）
        /// </summary>
        public List<ExcelCompanyShipBuildInfo>? ExcelCompanyShipBuildInfos { get; set; }
        /// <summary>
        /// 自有施工船舶运转情况（产值）
        /// </summary>
        public List<ExcelCompanyShipProductionValueInfo>? ExcelCompanyShipProductionValueInfos { get; set; }
        /// <summary>
        /// 船舶产值 排名前五（产值）
        /// </summary>
        public List<ExcelShipProductionValue>? ExcelShipProductionValues { get; set; }
        /// <summary>
        /// 特殊情况
        /// </summary>
        public List<ExcelSpecialProjectInfo>? ExcelSpecialProjectInfos { get; set; }
        /// <summary>
        /// 数据质量（填报情况）
        /// </summary>
        public List<ExcelCompanyWriteReportInfo>? ExcelCompanyWriteReportInfos { get; set; }
        /// <summary>
        /// 数据质量（填报情况  ：项目未填报）
        /// </summary>
        public List<ExcelCompanyUnWriteReportInfo>? ExcelCompanyUnWriteReportInfos { get; set; }
        /// <summary>
        /// 数据质量（填报情况  ：船舶未填报）
        /// </summary>
        public List<ExcelCompanyShipUnWriteReportInfo>? ExcelCompanyShipUnWriteReportInfos { get; set; }
        /// <summary>
        /// 带班动态：已填
        /// </summary>
        public List<ExcelProjectShiftProductionInfo>? ExcelProjectShiftProductionInfos { get; set; }
        /// <summary>
        /// 带班动态：已填
        /// </summary>
        public List<ExcelUnProjectShitInfo>? ExcelUnProjectShitInfos { get; set; }
        /// <summary>
        /// titile
        /// </summary>
        public List<ExcelTitle>? ExcelTitles { get; set; }
    }
}
