namespace GHMonitoringCenterApi.Application.Contracts.Dto.External
{
    /// <summary>
    /// 对外接口请求dto
    /// </summary>
    public class ExternalRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 开始结果赋值
        /// </summary>
        public DateTime StartTimeValue { get; set; }
        /// <summary>
        /// 结束时间赋值
        /// </summary>
        public DateTime EndTimeValue { get; set; }
        /// <summary>
        /// 日期赋值
        /// </summary>
        public void TimeValidatableObject()
        {
            if (string.IsNullOrEmpty(StartTime.ToString()) || DateTime.MinValue == StartTime)
            {
                StartTimeValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                StartTimeValue = StartTime.Value;
            }
            if (string.IsNullOrEmpty(EndTime.ToString()) || DateTime.MinValue == EndTime)
            {
                EndTimeValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                EndTimeValue = EndTime.Value;
            }
        }
    }

}
