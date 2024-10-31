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
        /// Y轴数据
        /// </summary>
        public List<EachAPIInterdaceItem>? YAxis { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class EachAPIInterdaceItem
    {
        public string? Name { get; set; }
        public decimal Value { get; set; }
    }
}
