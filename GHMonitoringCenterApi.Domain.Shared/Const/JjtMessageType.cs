using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared.Const
{


    /// <summary>
    /// 交建通发送消息类型常量
    /// </summary>
    public class JjtMessageType
    {

        /// <summary>
        /// 文本消息类型
        /// </summary>
        public static readonly string TEXT = "text";  
        /// <summary>
        /// 文本卡片类型
        /// </summary>
        public static readonly string TEXTCARD = "textcard";  
        /// <summary>
        /// 图片消息
        /// </summary>
        public static readonly string IMAGE = "image";
        /// <summary>
        /// 群消息
        /// </summary>
        public static readonly string CHATID = "chatid";

        /// <summary>
        /// 部门消息
        /// </summary>
        public static readonly string Topartys = "toparty";


        /// <summary>
        /// 测试群消息使用类型
        /// </summary>
        public static readonly string Test = "Test";

    }
}
