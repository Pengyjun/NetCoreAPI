using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Sms.Model
{

    /// <summary>
    /// 短信发送模板
    /// </summary>
    public class SMSTemplate
    {
        public int Type { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
    }
}
