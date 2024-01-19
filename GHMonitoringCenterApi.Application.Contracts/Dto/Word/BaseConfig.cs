using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Word
{

    /// <summary>
    /// word基本配置
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// word图片设置
        /// </summary>
        public WordImageSetup  WordImageSetup { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 标题时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 子标题
        /// </summary>
        public string SubTitle{ get; set; }
        /// <summary>
        /// 子标题时间
        /// </summary>
        public string SubTime { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public string Foot { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int Size { get; set; }
    }

    /// <summary>
    /// word插入图片基本设置
    /// </summary>

    public class WordImageSetup {
        /// <summary>
        /// logo图片流
        /// </summary>
        public Stream LogoStream { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        public PictureType type { get; set; }
        /// <summary>
        /// 图片宽度 默认300
        /// </summary>
        public int Width { get; set; } = 500 * 9525;
        /// <summary>
        /// 图片高度 默认200
        /// </summary>
        public int Height { get; set; } = 90 * 9525;

    }
}
