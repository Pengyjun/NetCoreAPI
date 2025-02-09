﻿using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUserService;
using HNKC.CrewManagePlatform.Utils;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{

    /// <summary>
    /// 基本控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 全局对象
        /// </summary>
        public GlobalCurrentUser GlobalCurrentUser
        {
            get
            {
                var currentUser = (ICurrentUserService)Request.HttpContext.RequestServices.GetService(typeof(ICurrentUserService));
                if (currentUser != null)
                {
                    return currentUser.CurrentUserAsync().GetAwaiter().GetResult();
                }
                else
                {
                    return new GlobalCurrentUser() { };
                }

            }
        }


        #region 文件上传  在WebHelper 工具类里面也有上传文件的方法和下载文件的方法

        #region 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// <summary>
        /// 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> SingleFileUpdateAsync(IFormFile file)
        {
            var suffixName = Path.GetExtension(file.FileName);
            var fileSize = file.Length;
            var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:DefaultAllowFileType").Split(",");
            var allowFileSize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            if (allowFileTypeArray.SingleOrDefault(x => x == suffixName.Replace(".", "").Trim()) == null)
            { }
            if (fileSize > allowFileSize)
            { }
            return file.OpenReadStream();
        }
        #endregion

        #region 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// <summary>
        /// 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="defaultAllowFileType"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Result> SingleFileUpdateAsync(IFormFile file, string defaultAllowFileType = "DefaultAllowFileType")
        {
            UploadResponse responseAjaxResult = new();
            if (file.Length == 0)
            {
                return Result.Fail("找不到文件");
            }
            var suffixName = Path.GetExtension(file.FileName);
            var fileSize = file.Length;
            var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:{defaultAllowFileType}").Split(",");
            var allowFileSize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            if (allowFileTypeArray.SingleOrDefault(x => x == suffixName.Replace(".", "")) == null)
            {
                return Result.Fail("上传类型不允许");
            }
            if (fileSize > allowFileSize)
            {
                return Result.Fail("上传大小不允许");
            }

      
            #region 文件上传至前端服务器
            //var host = AppsettingsHelper.GetValue("UpdateItem:Host");
            //var port = Convert.ToInt32(AppsettingsHelper.GetValue("UpdateItem:Port"));
            //var userName = AppsettingsHelper.GetValue("UpdateItem:UserName");
            //var passWord = AppsettingsHelper.GetValue("UpdateItem:PassWord");
            //var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
            //var savePath2 = AppsettingsHelper.GetValue("UpdateItem:SavePath");
            //var uploadresponseDto = new UploadResponse();
            ////上传文件
            //using (var sftp = new SftpClient(host, port, userName, passWord))
            //{
            //    sftp.Connect();

            //    //foreach (var item in files)
            //    {
            //        var name = Path.GetExtension(file.FileName);
            //        var newFileName = GuidUtil.Generate32BitGuid();
            //        savePath = Path.Combine(savePath, $"{newFileName}{name}");
            //        sftp.BufferSize = 1024 * 100000;
            //        using (var fileStream = file.OpenReadStream())
            //        {
            //            var uploadRes = sftp.BeginUploadFile(fileStream, savePath);
            //            sftp.EndUploadFile(uploadRes);
            //        }
            //        uploadresponseDto = new UploadResponse()
            //        {
            //            Id = newFileName.ToString(),
            //            Name = newFileName + name,
            //            BId = GuidUtil.Next(),
            //            OriginName = file.FileName,
            //            SuffixName = name,
            //            FileSize = fileSize,
            //            FileType = file.ContentType,
            //            Url = AppsettingsHelper.GetValue("UpdateItem:Url") + newFileName + name
            //        };
            //        //uploadresponseDto.Add(uploadResponseDto);
            //    }
            //    sftp.Disconnect();
            //    //}

            //    //修改权限
            //    using (var client = new SshClient(host, port, userName, passWord))
            //    {
            //        client.Connect();
            //        //foreach (var item in files)
            //        //{
            //        var command = $"chmod 644 {savePath}";
            //        var result = client.RunCommand(command);
            //        //}
            //        client.Disconnect();
            //    }
            //    return Result.Success(uploadresponseDto, "上传成功");
            //}
            #endregion
            var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
            var newFileName = GuidUtil.Generate32BitGuid();
            savePath = Path.Combine(savePath, $"{newFileName}{Path.GetExtension(file.FileName)}");
            using (var stream = System.IO.File.Create(savePath))
            {
                await file.CopyToAsync(stream);
            }
            //SetFilePermissions(savePath);
            var uploadResponseDto = new UploadResponse()
            {
                Id = newFileName,
                Name = newFileName + suffixName,
                OriginName = file.FileName,
                SuffixName = suffixName,
                FileSize = fileSize,
                FileType = file.ContentType,
                Url = AppsettingsHelper.GetValue("UpdateItem:Url") + newFileName
            };
            return Result.Success(uploadResponseDto, "上传成功");
        }
        #endregion

        #endregion
    }
}
