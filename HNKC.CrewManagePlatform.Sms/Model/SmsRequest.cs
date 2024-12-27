using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Sms.Model
{
    /// <summary>
    /// 发送短信请求参数
    /// </summary>
    public class SmsRequest
    {
        /// <summary>
        /// 系统规定参数。取值：SendSms。
        /// 示例：SendSms
        /// </summary>
        public string? Action { get; set; } = "SendSms";
        /// <summary>
        /// 接收短信的手机号码。格式：国内短信：无任何前缀的11位手机号码，例如1381111****。多个手机号码使用英文","隔开，最多支持一次提交200个手机号码。
        /// 示例：1381111*****
        /// </summary>
        public string? PhoneNumber { get; set; } = "13703993516,15900967253";
        /// <summary>
        /// 短信签名名称。请在控制台的签名管理页签下签名名称一列查看。说明：必须是已添加、并通过审核的短信签名。
        /// 示例:天翼云
        /// </summary>
        public string? SignName { get; set; } = AppsettingsHelper.GetValue("CtyunSms:SignName");
        /// <summary>
        ///短信模板ID。请在控制台的模板管理页签下模板Code一列查看。说明：必须是已添加、并通过审核的短信模板。
        ///示例：SMS73419576145
        /// </summary>
        public List<SMSTemplate>? TemplateCode { get; set; }= AppsettingsHelper.GetSection<List<SMSTemplate>>("CtyunSms:TemplateCodes");
        /// <summary>
        /// 短信模板变量对应的实际值，JSON字符串格式。说明：如果JSON中需要带换行符，请参照标准的JSON协议处理。
        /// 示例：{"code":"1111"}
        /// </summary>
        public string? TemplateParam { get; set; }

        /// <summary>
        /// 临时变量  是否成功
        /// </summary>
        public bool? IsSuccess { get; set; }
    }
}
