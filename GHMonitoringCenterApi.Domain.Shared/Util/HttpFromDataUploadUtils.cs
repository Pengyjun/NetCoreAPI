using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Domain.Shared.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared.Util
{


    /// <summary>
    /// http上传文件工具类  针对交建通上传临时素材使用
    /// </summary>
    public class HttpFromDataUploadUtils
    {
        /// <summary>
        /// 交建通上传临时素材到交建通服务器
        /// </summary>
        /// <param name="token"></param>
        /// <param name="formFile"></param>
        public static bool UploadTempFileJjt(string token, IFormFile formFile) 
        {
            //var bytes = new byte[4096];
            //获取media_Id对象
            string url = "https://jjt.ccccltd.cn/cgi-bin/media/upload?access_token=" + token + "&type=image";
            var name = "media";
            var file = formFile.OpenReadStream();
            var result = JjtUtils.DoPostFile(url, name, formFile.FileName, file);
            if (!string.IsNullOrWhiteSpace(result))
            {
               var jsonData=JObject.Parse(result);
                if (jsonData["errmsg"].ToString() =="ok")
                {
                    //上传成功 返回media_id  将media_id缓存到redis  临时缓存
                    var mediaId=jsonData["media_id"].ToString();
                    RedisUtil.Instance.Set("mediaId", mediaId, 1800);
                    return  true;
                }
            }
            return false;
        }
            
    }
}
