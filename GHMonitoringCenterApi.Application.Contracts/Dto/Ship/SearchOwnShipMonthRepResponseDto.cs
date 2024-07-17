namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    public class SearchOwnShipMonthRepResponseDto
    {
        public List<SearchOwnShipMonthRep> searchOwnShipMonthReps { get; set; }
        public SumOwnShipMonth ownShipMonth { get; set; }
    }
    /// <summary>
    /// 自有船舶月报列表
    /// </summary>
    public class SearchOwnShipMonthRep
    {
        /// <summary>
        /// 月报id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 分包船舶id
        /// </summary>
        public Guid? OwnShipId { get; set; }
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }
        /// <summary>
        /// 船舶分类名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public string? QuitTime { get; set; }
        /// <summary>
        /// 本月运转时间
        /// </summary>
        public decimal MonthWorkHours { get; set; }
        /// <summary>
        /// 本月施工天数
        /// </summary>
        public decimal MonthWorkDays { get; set; }
        /// <summary>
        /// 合同清单类型
        /// </summary>
        public string? ContractTypeName { get; set; }
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal MonthQuantity { get; set; }
        /// <summary>
        /// 本月施工产值
        /// </summary>
        public decimal MonthOutputVal { get; set; }
        /// <summary>
        /// 年度运转时间
        /// </summary>
        public decimal YearWorkHours { get; set; }
        /// <summary>
        /// 年度运转时间
        /// </summary>
        public decimal YearWorkDays { get; set; }
        /// <summary>
        /// 年度完成工程量
        /// </summary>
        public decimal YearQuantity { get; set; }
        /// <summary>
        /// 年度完成产值
        /// </summary>
        public decimal YearOutputVal { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public string? SecUnitName { get; set; }
        /// <summary>
        /// 三级单位名称
        /// </summary>
        public string? ThiUnitName { get; set; }
        /// <summary>
        /// 负责人名称
        /// </summary>
        public string? HeadUserName { get; set; }
        /// <summary>
        /// 负责人电话
        /// </summary>
        public string? HeadUserTel { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? RepUserName { get; set; }
        /// <summary>
        /// 报表联系人电话
        /// </summary>
        public string? RepUserTel { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public int IsExamine { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 工况级别
        /// </summary>
        public Guid? GKJBId { get; set; }
        /// <summary>
        /// 工艺方式
        /// </summary>
        public Guid? GYFSId { get; set; }
        /// <summary>
        /// 疏浚吹填分类
        /// </summary>
        public Guid? SJCTId { get; set; }
        /// <summary>
        /// 合同清单类型(字典表typeno=9)
        /// </summary>
        public int QDLXId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///挖深（m）
        /// </summary>
        public decimal DigDeep { get; set; }

        /// <summary>
        /// 吹距(KM)
        /// </summary>
        public decimal BlowingDistance { get; set; }

        /// <summary>
        /// 运距(KM)
        /// </summary>
        public decimal HaulDistance { get; set; }

    }

    public class SumOwnShipMonth
    {
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal SumMonthQuantity { get; set; }
        /// <summary>
        /// 本月施工产值
        /// </summary>
        public decimal SumMonthOutputVal { get; set; }
        /// <summary>
        /// 年度完成工程量
        /// </summary>
        public decimal SumYearQuantity { get; set; }
        /// <summary>
        /// 年度完成产值
        /// </summary>
        public decimal SumYearOutputVal { get; set; }
    }
}
