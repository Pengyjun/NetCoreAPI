﻿using GDCMasterDataReceiveApi.Domain.Models;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy
{
    /// <summary>
    /// 国民经济行业分类 反显
    /// </summary>
    public class NationalEconomyDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 国民经济行业分类代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 国民经济行业分类类别名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 国民经济行业分类上级代码
        /// </summary>
        public string SupCode { get; set; }
        /// <summary>
        /// 国民经济行业分类说明
        /// </summary>
        public string? Descption { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string DataIdentifier { get; set; }
    }
    /// <summary>
    /// 国民经济行业分类 接收
    /// </summary>
    public class NationalEconomyReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 国民经济行业分类代码
        /// </summary>
        public string ZNEQCODE { get; set; }
        /// <summary>
        /// 国民经济行业分类类别名称
        /// </summary>
        public string ZNEQNAME { get; set; }
        /// <summary>
        /// 国民经济行业分类上级代码
        /// </summary>
        public string ZNEQCODEUP { get; set; }
        /// <summary>
        /// 国民经济行业分类说明
        /// </summary>
        public string? ZNEQDESC { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public List<ZMDGS_ZLANG5>? ZLANG_LIST { get; set; }
    }
}
