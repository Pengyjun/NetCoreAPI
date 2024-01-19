
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 交建通发消息提前写入数据中间表 （针对监控日报） 响应对象
    /// </summary>
    public class JjtWriteDataRecordResponseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 在建项目数量
        /// </summary>
        public int PInbuildNums { get; set; }
        /// <summary>
        /// 正常施工项目数量
        /// </summary>
        public int PConstrucNums { get; set; }
        /// <summary>
        /// 暂停项目数量
        /// </summary>
        public int pSuspendNums { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 当日公司总产值
        /// </summary>
        public decimal POutputVal { get; set; }
        /// <summary>
        /// 本年项目开累产值
        /// </summary>
        public decimal PAccumulateOutput { get; set; }
        /// <summary>
        /// 当日自有船舶产值
        /// </summary>
        public decimal OwnShipOutputVal { get; set; }
        /// <summary>
        /// 本年自有船舶开累产值
        /// </summary>
        public decimal OwnShipAccumulateOutputVal { get; set; }
        /// <summary>
        /// 截止今天计划产值累计 =昨日累计+今日日报计划产值
        /// </summary>
        public decimal UptoNowDayPlanAccumulateOutputVal { get; set; }
        /// <summary>
        /// 项目日均计划产值
        /// </summary>
        public decimal UptoDayNowOutputVal { get; set; }
        /// <summary>
        /// 船舶日均计划产值
        /// </summary>
        public decimal ShipUptoDayNowOutputVal { get; set; }
        /// <summary>
        /// 截止今天计划产值累计 =昨日累计+今日船舶计划产值
        /// </summary>
        public decimal ShipUptoNowDayPlanAccumulateOutputVal { get; set; }
        /// <summary>
        /// 施工船舶总数
        /// </summary>
        public int OwnShipNums { get; set; }
        /// <summary>
        /// 正常施工船舶数量
        /// </summary>
        public int OwnShipConstrucNums { get; set; }
        /// <summary>
        /// 停工船舶施工数量
        /// </summary>
        public int OwnShipStopNums { get; set; }
        /// <summary>
        /// 今日日期 20230616
        /// </summary>
        public int NowDay { get; set; }
        /// <summary>
        /// 1 生产运营监控日报
        /// </summary>
        public int TextType { get; set; }

    }
}
