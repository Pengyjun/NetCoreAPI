using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData
{
    /// <summary>
    /// 各个公司主数据数量
    /// </summary>
    public class EachCompanyMainDataCountResponseDto
    {
        /// <summary>
        /// 类型 1  人员  2 机构  3 项目 。。。。
        /// </summary>
        public int Type { get; set; } = 1;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? ConpanyName { get; set; }
        /// <summary>
        /// x轴数据
        /// </summary>
        public string XAxis { get; set; }
        /// <summary>
        /// Y轴数据
        /// </summary>
        public decimal? YAxis { get; set; }
    }

    /// <summary>
    /// 统计下游请求API统计
    /// </summary>
    public class EachAPIInterdaceCountResponseDto
    {
        /// <summary>
        /// x轴数据
        /// </summary>
        public string XAxis { get; set; }
        /// <summary>
        /// Y轴数据 总数量
        /// </summary>
        public decimal YAxis { get; set; }
        /// <summary>
        /// 下钻列表
        /// </summary>
        //public List<EachAPIInterdaceItem>? EachAPIInterdaceItems { get; set; }
        /// <summary>
        /// 系统APPkey
        /// </summary>
        public string? AppKey { get; set; }

    }
    /// <summary>
    /// 下钻列表
    /// </summary>
    public class EachAPIInterdaceItem
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string? AppName { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string? RequestTime { get; set; }
    }
}
