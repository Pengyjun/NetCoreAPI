using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 交建通发消息提前写入数据中间表 （针对监控日报）
    /// </summary>
    [SugarTable("t_jjtwritedatarecord", IsDisabledDelete = true)]
    public class JjtWriteDataRecord
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]//, IsOnlyIgnoreUpdate = true
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 当日公司总产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal POutputVal { get; set; }
        /// <summary>
        /// 本年项目开累产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal PAccumulateOutput { get; set; }
        /// <summary>
        /// 当日自有船舶产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal OwnShipOutputVal { get; set; }
        /// <summary>
        /// 本年自有船舶开累产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal OwnShipAccumulateOutputVal { get; set; }
        /// <summary>
        /// 截止今天计划产值累计 =昨日累计+今日日报计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal UptoNowDayPlanAccumulateOutputVal { get; set; }
        /// <summary>
        /// 日均计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal UptoDayNowOutputVal { get; set; }
        /// <summary>
        /// 船舶日均计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal ShipUptoDayNowOutputVal { get; set; }
        /// <summary>
        /// 截止今天计划产值累计 =昨日累计+今日船舶计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal ShipUptoNowDayPlanAccumulateOutputVal { get; set; }
        /// <summary>
        /// 在建项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int PInbuildNums { get; set; }
        /// <summary>
        /// 正常施工项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int PConstrucNums { get; set; }
        /// <summary>
        /// 暂停项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int pSuspendNums { get; set; }
        /// <summary>
        /// 施工船舶总数
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int OwnShipNums { get; set; }
        /// <summary>
        /// 正常施工船舶数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int OwnShipConstrucNums { get; set; }
        /// <summary>
        /// 停工船舶施工数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int OwnShipStopNums { get; set; }
        /// <summary>
        /// 交建通发送的第一版文字内容
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? FTextMsg { get; set; }
        /// <summary>
        /// 交建通发送更改后的文字信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? ModifyTextMsg { get; set; }
        /// <summary>
        /// 今日日期 20230616
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int NowDay { get; set; }
        /// <summary>
        /// 1 生产运营监控日报
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int TextType { get; set; }
        /// <summary>
        /// 发送次数
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int SendCount { get; set; }
    }
}
