﻿using GDCMasterDataReceiveApi.Domain.Shared.Enums;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 增量数据相关响应dto
    /// </summary>
    public class IncrementalDataDto
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<IncrementalSearchResponse>? Item { get; set; }
    }
    /// <summary>
    /// 增量数据响应列表
    /// </summary>
    public class IncrementalSearchResponse
    {
        /// <summary>
        /// 当日变化数量
        /// </summary>
        public int ChangeNums { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime TimeValue { get; set; }
        /// <summary>
        /// 需要查询的ids
        /// </summary>
        public List<string>? DetailsIds { get; set; }
    }
    /// <summary>
    /// 增量数据请求列表
    /// </summary>
    public class IncrementalSearchRequestDto
    {
        /// <summary>
        /// 查询的日期
        /// </summary>
        public int DateDay { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public TableNameType TableName { get; set; }

    }
}
