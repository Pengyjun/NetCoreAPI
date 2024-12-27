using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Sms.Model
{
    public class ParameTemplate
    {

        public UserInfo user { get; set; }
        public SendTime  sendTime { get; set; }
        public SendUrl  sendUrl { get; set; }
    }

public class UserInfo 
{
    public string Name { get; set; }
}
public class SendTime
{
    public string Time { get; set; }=DateTime.Now.ToString("yyyy-MM");
}

public class SendUrl
{
    public string Url { get; set; } 
}
}
