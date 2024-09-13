using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain
{

    /// <summary>
    /// 接收值域请求DTO
    /// </summary>
    public class ValueDomainReceiveRequestDto
    {
        public long? Id { get; set; }
        /// <summary>
        /// 值域编码
        /// </summary>

        public string? ZDOM_CODE { get; set; }


        /// <summary>
        /// 值域编码描述
        /// </summary>

        public string? ZDOM_DESC { get; set; }


        /// <summary>
        /// 域值 
        /// </summary>

        public string? ZDOM_VALUE { get; set; }


        /// <summary>
        /// 域值描述 
        /// </summary>

        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 域值层级 
        /// </summary>

        public string? ZDOM_LEVEL { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>

        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string? ZREMARKS { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>

        public string? ZCHTIME { get; set; }


        /// <summary>
        /// 版本
        /// </summary>

        public string? ZVERSION { get; set; }


        /// <summary>
        /// 状态
        /// </summary>

        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public ZLANG_LISTItem? ZLANG_LIST { get; set; }
    }

    public class ZLANG_LISTItem
    {

        public List<ValueDomainItem> Item { get; set; }
    }

    public class ValueDomainItem
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 域值描述
        /// </summary>
        public string? ZVALUE_DESC { get; set; }
    }
}
