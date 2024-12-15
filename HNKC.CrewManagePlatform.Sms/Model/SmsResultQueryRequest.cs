using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Sms.Model
{
    /// <summary>
    /// 短信结果查询
    /// </summary>
    public class SmsResultQueryRequest
    {
        /// <summary>
        /// 系统规定参数。取值：QuerySendDetails 
        /// </summary>
        public string? Action { get; set; }
        /// <summary>
        /// 分页查看发送记录，指定发送记录的当前页码。
        /// 示例：1
        /// </summary>
        public string? CurrentPage { get; set; } = "1";
        /// <summary>
        /// 分页查看发送记录，指定每页显示的短信记录数量。取值范围为1~50。
        /// 示例：10
        /// </summary>
        public string? PageSize { get; set; } = "50";
        /// <summary>
        /// 接收短信的手机号码。格式：国内短信：11位手机号码，例如1590000****。
        /// 示例：1890000****
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 短信发送日期，支持查询最近30天的记录。格式为yyyy-MM-dd，例如2018-12-25。
        /// 示例：2022-03-28
        /// </summary>
        public string? SendDate { get; set; }
    }
}
