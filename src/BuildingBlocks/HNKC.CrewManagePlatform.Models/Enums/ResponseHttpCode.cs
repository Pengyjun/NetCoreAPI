﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Enums
{

    /// <summary>
    /// http状态吗响应
    /// </summary>
    public enum ResponseHttpCode
    {
        /// <summary>
        /// 添加失败
        /// </summary>
        AddFail=10000,

        /// <summary>
        /// 修改失败
        /// </summary>
        UpdateFail = 10001,

        /// <summary>
        /// 上传失败
        /// </summary>
        UploadFail = 10001,

        /// <summary>
        /// 文件大小不允许
        /// </summary>
        FileSizeNoAllow = 10002,

        /// <summary>
        /// 文件类型不允许
        /// </summary>
        FileTypeNoAllow = 10003,
    }
}
