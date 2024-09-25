using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// MDM主数据接口异步通知请求DTO
    /// </summary>
    public class MdmReturnNotificationRequestDto
    {
        public IS_REQ_HEAD_ASYNC  iS_REQ_HEAD_ASYNC { get; set; }
        public IT_RESULT_ASYNC  iT_RESULT_ASYNC { get; set; }

    }

    public class IS_REQ_HEAD_ASYNC
    {
        /// <summary>
        /// 接口请求唯一ID
        /// </summary>
        [XmlElement("ZINSTID")]
        public string? ZINSTID { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        [XmlElement("ZZREQTIME")]
        public string? ZZREQTIME { get; set; }
        /// <summary>
        /// 来源系统代码
        /// </summary>
        [XmlElement("ZZSRC_SYS")]
        public string? ZZSRC_SYS { get; set; }
        /// <summary>
        /// 主数据项
        /// </summary>
        [XmlElement("ZZOBJECT")]
        public string? ZZOBJECT { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        [XmlElement("ZZATTR1")]
        public string? ZZATTR1 { get; set; }
        /// <summary>
        /// OSB异步分发的目标系统
        /// </summary>
        [XmlElement("ZZATTR2")]
        public string? ZZATTR2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        [XmlElement("ZZATTR3")]
        public string? ZZATTR3 { get; set; }
    }

    public class IT_RESULT_ASYNC
    {
        public List<Item> Item { get; set; }
    }

    /// <summary>
    /// 业务记录
    /// </summary>
    public class Item {
        /// <summary>
        /// 发送记录ID
        /// </summary>
        [XmlElement("ZZSERIAL")] 
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 异步分发记录的状态
        /// </summary>
        [XmlElement("ZZSTAT")]
        public string? ZZSTAT { get; set; } = "E";
        /// <summary>
        ///  异步分发记录的处理结果
        /// </summary>
        [XmlElement("ZZMSG")]
        public string? ZZMSG { get; set; } = "错误";
    }
}
