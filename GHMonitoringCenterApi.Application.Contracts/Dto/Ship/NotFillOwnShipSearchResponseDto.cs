
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 未报自有船舶月报列表
    /// </summary>
    public class NotFillOwnShipSearchResponseDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 自有船舶id
        /// </summary>
        public Guid? OwnShipId { get; set; }
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 船舶类型id
        /// </summary>
        public Guid? ShipTypeId{ get; set; }
        /// <summary>
        /// 船舶所属单位
        /// </summary>
        public string? ShipCompanyName { get; set; }
        /// <summary>
        /// 未填报日期
        /// </summary>
        public string? NotFillDate { get; set; }

		/// <summary>
		/// 未填报日期 int类型
		/// </summary>
		public int? NotDate { get; set; }
		/// <summary>
		/// 所属公司
		/// </summary>
		public Guid? CompanyId { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }
		///// <summary>
		///// 所属项目部
		///// </summary>
		//public Guid? ProjectDept { get; set; }
		///// <summary>
		///// 所在区域
		///// </summary>
		//public Guid? ProjectRegionId { get; set; }
		///// <summary>
		///// 所在省份
		///// </summary>
		//public Guid? ProjectAreaId { get; set; }
		///// <summary>
		///// 项目类型
		///// </summary>
		//public Guid? ProjectTypeId { get; set; }
		///// <summary>
		///// 项目状态
		///// </summary>
		//public Guid? ProjectStatusId { get; set; }
		///// <summary>
		///// 境内 境外
		///// </summary>
		//public int? Category { get; set; }
		///// <summary>
		///// 传统 新兴
		///// </summary>
		//public int? Tag1 { get; set; }
		///// <summary>
		///// 现汇 投资
		///// </summary>
		//public int? Tag2 { get; set; }
		/// <summary>
		/// 进场时间
		/// </summary>
		public DateTime? enterTime { get; set; }
		/// <summary>
		/// 退场时间
		/// </summary>
		public DateTime? quitTime { get; set; }

		/// <summary>
		/// 进退场状态
		/// </summary>
		public string? InOutStatus { get; set; }
	}
}
