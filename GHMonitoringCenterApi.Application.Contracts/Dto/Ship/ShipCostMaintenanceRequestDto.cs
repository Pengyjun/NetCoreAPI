
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 船舶成本维护请求dto
    /// </summary>
    public class ShipCostMaintenanceRequestDto : BaseRequestDto
    {

        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }

        /// <summary>
        /// 境内境外  
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        ///搜索开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 搜索结束
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 日期处理
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime NowTime(DateTime? date)
        {
            var now = DateTime.Now;

            if (string.IsNullOrWhiteSpace(date.ToString()) || date == DateTime.MinValue)
            {
                return new DateTime(now.Year, now.Month, 1);
            }
            else
            {
                return new DateTime(Convert.ToDateTime(date).Year, Convert.ToDateTime(date).Month, 1);
            }
        }
    }
}
