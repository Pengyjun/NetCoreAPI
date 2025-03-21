using System.IO;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.Extensions.Logging;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using Newtonsoft.Json.Linq;
using Model = GHMonitoringCenterApi.Domain.Models;
using UtilsSharp;
using SqlSugar;
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Net.Security;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.Policy;
using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Service.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Domain.Models;
using NetTaste;
using NPOI.SS.Formula.Functions;
using NPOI.HPSF;
using System;
using System.Net.Http;
using System.Security.Cryptography;

namespace GHMonitoringCenterApi.Application.Service.File
{

    /// <summary>
    /// 文件导出接口层实现层
    /// </summary>
    public class FileService : IFileService
    {

        #region 依赖注入
        public ILogger<FileService> logger { get; set; }
        public IBaseRepository<Model.User> baseUserRepository { get; set; }
        public IJjtSendMessageService jjtSendMessageService { get; set; }
        public IBaseRepository<DayReportJjtPushConfi> baseRepository { get; set; }

        public ISqlSugarClient dbContext { get; set; }


        public FileService(ILogger<FileService> logger, IBaseRepository<Model.User> baseUserRepository, IJjtSendMessageService jjtSendMessageService, IBaseRepository<DayReportJjtPushConfi> baseRepository, ISqlSugarClient dbContext)
        {
            this.logger = logger;
            this.baseUserRepository = baseUserRepository;
            this.jjtSendMessageService = jjtSendMessageService;
            this.baseRepository = baseRepository;
            this.dbContext = dbContext;
        }
        #endregion

        #region 动态列导出Excel
        /// <summary>
        /// 动态列导出Excel(注意此方式需要落地文件)
        /// </summary>
        /// <param name="saveFileName">保存文件的文件名(文件名称用GUID命名)</param>
        /// <param name="ignoreColumns">忽略的列明列名集合(注意列名要和您的实体属性名称保持一致)</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public async Task<string> ExcelImportSaveAsync<T>(string saveFileName, List<string> ignoreColumns, T data)
        {
            var filePath = string.Empty;
            try
            {
                filePath = AppsettingsHelper.GetValue("Excel:SavePath") + Path.DirectorySeparatorChar + saveFileName;
                DynamicExcelColumn[] dynamicExcelColumsn = null;
                if (ignoreColumns != null && ignoreColumns.Any())
                {
                    dynamicExcelColumsn = new DynamicExcelColumn[ignoreColumns.Count];
                    for (int i = 0; i < dynamicExcelColumsn.Length; i++)
                    {
                        dynamicExcelColumsn[i] = new DynamicExcelColumn(ignoreColumns[i]) { Ignore = true };
                    }
                }
                var config = new OpenXmlConfiguration
                {
                    DynamicColumns = dynamicExcelColumsn
                };
                MiniExcel.SaveAs(filePath, data, configuration: config, overwriteFile: true);
                filePath = saveFileName;
            }
            catch (Exception ex)
            {
                filePath = string.Empty;
                logger.LogError("落地导出Excel出现错误", ex);
            }
            return filePath;
        }


        #endregion

        #region 动态列导出Excel
        /// <summary>
        /// <summary>
        /// 动态列导出Excel(注意此方式文件不落地)
        /// </summary>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExcelImportAsync<T>(List<string> ignoreColumns, string sheetName, T data)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                DynamicExcelColumn[] dynamicExcelColumsn = null;
                if (ignoreColumns != null && ignoreColumns.Any())
                {
                    dynamicExcelColumsn = new DynamicExcelColumn[ignoreColumns.Count];
                    for (int i = 0; i < dynamicExcelColumsn.Length; i++)
                    {
                        dynamicExcelColumsn[i] = new DynamicExcelColumn(ignoreColumns[i]) { Ignore = true };
                    }
                }
                var config = new OpenXmlConfiguration
                {
                    DynamicColumns = dynamicExcelColumsn
                };
                await memoryStream.SaveAsAsync(data, sheetName: sheetName, configuration: config);
            }
            catch (Exception ex)
            {
                logger.LogError("导出Excel出现错误", ex.Message);
            }
            return memoryStream;
        }
        #endregion

        #region　导出html转换文件

        /// <summary>
        /// 导出 html转换文件
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<FileResponseDto>> ExportHtmlToFileAsync(HtmlConvertFileRequestDto model)
        {
            var result = new ResponseAjaxResult<FileResponseDto>();
            using var stream = System.IO.FileHelper.HtmlConvertStream(model.Html, model.Format);
            return await Task.FromResult(result.SuccessResult(new FileResponseDto() { Buffer = stream.ToArray() }));
        }
        #endregion

        #region 交建通上传临时素材图片
        /// <summary>
        /// 交建通上传图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> UploadImageJJT(IFormFile formFile)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 不使用交建通代理方式的情况
            //获取token
            //var accessToken = JjtUtils.GetJjtToken();
            //if (!string.IsNullOrWhiteSpace(accessToken))
            //{
            //    //上传临时图片到交建通服务器
            //    var uploadResult = HttpFromDataUploadUtils.UploadTempFileJjt(accessToken, formFile);
            //    if (uploadResult)
            //    {
            //        #region 从redis取值
            //        var redisKey = "mediaId";
            //        var mediaId = string.Empty;
            //        var redis = RedisUtil.Instance;
            //        if (await redis.ExistsAsync(redisKey))
            //        {
            //            mediaId = await redis.GetAsync(redisKey);
            //        }
            //        else
            //        {
            //            //如果redis失效了  在执行一次
            //            uploadResult = HttpFromDataUploadUtils.UploadTempFileJjt(accessToken, formFile);
            //            if (uploadResult)
            //            {
            //                mediaId = await redis.GetAsync(redisKey);
            //            }
            //        }
            //        #endregion
            //        //获取推送人员信息
            //        var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
            //   .ToListAsync();
            //        //上传成功   开始发送消息
            //        #region 交建通发送消息
            //        var currentTimeDay = DateTime.Now.Hour;
            //        if (currentTimeDay == 9)
            //        {
            //            //九点第一批人员发送
            //            var pushUsers = pushJjtUserList.Where(x => x.Type == 0).Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"九点第一批推送人员结果:{pushResult}");

            //        }
            //        else if (currentTimeDay == 10)
            //        {
            //            #region 第二批人员推送 个项目部管理人员
            //            //10 第二批人员推送 个项目部管理人员
            //            var pushUsers = pushJjtUserList.Where(x => x.Type.Value == 1).Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            logger.LogWarning($"10点第二批推送公司管理人员结果:{pushResult}");
            //            #endregion

            //            #region 部门相关人员发送消息
            //            //部门相关人员发送消息
            //            var specialOne = pushJjtUserList.Where(x => x.Type.Value == 2).SingleOrDefault();
            //            obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.Topartys,
            //                Topartys = new List<string>() { specialOne.PushAccount }
            //            };
            //            pushResult = JjtUtils.SinglePushMessage(obj);
            //            logger.LogWarning($"10点第二批推送相关部门人员结果:{pushResult}");
            //            #endregion

            //            #region 相关群组人员发消息
            //            //相关群组人员发消息
            //            var specialTwo = pushJjtUserList.Where(x => x.Type.Value == 3).SingleOrDefault();
            //            obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.CHATID,
            //                ChatId = specialTwo.GroupNumber
            //            };
            //            pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"10点第二批推送群组人员结果:{pushResult}");
            //            #endregion
            //        }
            //        else if (currentTimeDay >= 18 && currentTimeDay <= 23)
            //        {
            //            //测试使用 
            //            //测试人员陈翠
            //            var pushUsers = pushJjtUserList.Where(x => x.Type == 0 && x.PushAccount == "2016146340").Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
            //        }
            //        #endregion
            //    }
            //}
            //else
            //{
            //    logger.LogWarning("交建通发送图片获取token为空");
            //}
            #endregion


            #region 检查是否可以发送
            var today = DateTime.Now.AddDays(-1).ToDateDay();
            var approveResult= await dbContext.Queryable<DayPushApprove>().Where(x => x.IsDelete == 1 && x.DayTime == today).FirstAsync();
            if (approveResult == null&&DateTime.Now.Hour!=9)
            {
                responseAjaxResult.Data = false;
                return responseAjaxResult;
            }

            //判断Redis是否已经存在发消息的记录
            var isExist= await RedisUtil.Instance.ExistsAsync(DateTime.Now.AddDays(-1).ToDateDay() + "two");
            if (isExist && DateTime.Now.Hour != 9)
            {
                //已存在 说明已发送 不再重复发送
                responseAjaxResult.Data = false;
                return responseAjaxResult;
            }

            #endregion

            #region 使用交建通代理方式的情况
            var url = AppsettingsHelper.GetValue("JjtPushMesssage:UploadTempJjt");
            var name = "formFile";
            var fileName = formFile.FileName;
            var file = formFile.OpenReadStream();
            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(file, (int)file.Length), name, fileName);
            var _httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                 response = await _httpclient.PostAsync(url, formData);
            }
            catch (Exception ex)
            {

                logger.LogError($"生产日报推送消息发送失败:{ex}");
            }
          
            if (response!=null&&response.IsSuccessStatusCode)
            {
                var media = response.Content.ReadAsStringAsync().Result;
                #region 重试机制
                media=await RetryAsync(_httpclient, media, url, formData);
                if (string.IsNullOrWhiteSpace(media.TrimAll()))
                {
                    responseAjaxResult.Fail();
                    return responseAjaxResult;
                }
                //if (string.IsNullOrWhiteSpace(media.TrimAll()))
                //{
                //    //重试三次
                //    try
                //    {
                //        int retry = 0;
                //        while (retry<3) 
                //        {

                //            response = await _httpclient.PostAsync(url, formData);
                //            media = response.Content.ReadAsStringAsync().Result;
                //            if (!string.IsNullOrWhiteSpace(media.TrimAll()))
                //            {
                //                retry = 4; break;
                //            }
                //            else {
                //                if (retry == 2)
                //                {
                //                    //说明重试三次依然失败
                //                    logger.LogWarning("上传交建通临时素材重试三次依然失败了"); 
                //                }
                //                retry += 1;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        logger.LogError($"生产日报推送消息发送失败:{ex}");
                //    }
                //}
                #endregion
                responseAjaxResult.Data = true;
                #region 交建通发送消息
                //获取推送人员信息
                var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
           .ToListAsync();
                var currentTimeDay = DateTime.Now.Hour;
                if (currentTimeDay == 9)
                {
                    //九点第一批人员发送
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 0).Select(x => x.PushAccount).ToList();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.IMAGE,
                        UserIds = pushUsers
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, true, "one");
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"九点第一批推送人员结果:{pushResult}");

                }
                else if (currentTimeDay ==10)
                {
                    #region 第二批人员推送 个项目部管理人员
                    //10 第二批人员推送 个项目部管理人员
                    var pushUsers = pushJjtUserList.Where(x => x.Type.Value == 1).Select(x => x.PushAccount).ToList();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.IMAGE,
                        UserIds = pushUsers
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, true, "two");
                    logger.LogWarning($"10点第二批推送公司管理人员结果:{pushResult}");
                    #endregion

                    #region 部门相关人员发送消息
                    //部门相关人员发送消息
                    var specialOne = pushJjtUserList.Where(x => x.Type.Value == 2).SingleOrDefault();
                    obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.Topartys,
                        Topartys = new List<string>() { specialOne.PushAccount }
                    };
                    pushResult = JjtUtils.SinglePushMessage(obj, true, "three");
                    logger.LogWarning($"10点第二批推送相关部门人员结果:{pushResult}");
                    #endregion

                    #region 相关群组人员发消息
                    //相关群组人员发消息
                    var specialTwo = pushJjtUserList.Where(x => x.Type.Value == 3).SingleOrDefault();
                    obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = specialTwo.GroupNumber
                    };
                    pushResult = JjtUtils.SinglePushMessage(obj, true, "four");
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"10点第二批推送群组人员结果:{pushResult}");
                    #endregion
                }
                else if (currentTimeDay >= 12 && currentTimeDay <= 23)
                {
                    #region 测试使用
                    ////测试使用 
                    ////测试人员陈翠
                    //var pushUsers = pushJjtUserList.Where(x => x.Type == 0 && x.PushAccount == "2016146340").Select(x => x.PushAccount).ToList();
                    //var obj = new SingleMessageTemplateRequestDto()
                    //{
                    //    IsAll = false,
                    //    Mediaid = media,
                    //    MessageType = JjtMessageType.IMAGE,
                    //    UserIds = pushUsers
                    //};
                    //var pushResult = JjtUtils.SinglePushMessage(obj, true, "five");
                    //responseAjaxResult.Data = pushResult;
                    //logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                    #endregion
                    //测试使用 
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = pushUsers.GroupNumber
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj);
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                }
                #endregion
            }
            #endregion
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion




        #region 交建通上传临时素材图片交建公司
        /// <summary>
        /// 交建通上传临时素材图片交建公司
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> UploadImageJJByJJT(IFormFile formFile)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 不使用交建通代理方式的情况
            //获取token
            //var accessToken = JjtUtils.GetJjtToken();
            //if (!string.IsNullOrWhiteSpace(accessToken))
            //{
            //    //上传临时图片到交建通服务器
            //    var uploadResult = HttpFromDataUploadUtils.UploadTempFileJjt(accessToken, formFile);
            //    if (uploadResult)
            //    {
            //        #region 从redis取值
            //        var redisKey = "mediaId";
            //        var mediaId = string.Empty;
            //        var redis = RedisUtil.Instance;
            //        if (await redis.ExistsAsync(redisKey))
            //        {
            //            mediaId = await redis.GetAsync(redisKey);
            //        }
            //        else
            //        {
            //            //如果redis失效了  在执行一次
            //            uploadResult = HttpFromDataUploadUtils.UploadTempFileJjt(accessToken, formFile);
            //            if (uploadResult)
            //            {
            //                mediaId = await redis.GetAsync(redisKey);
            //            }
            //        }
            //        #endregion
            //        //获取推送人员信息
            //        var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
            //   .ToListAsync();
            //        //上传成功   开始发送消息
            //        #region 交建通发送消息
            //        var currentTimeDay = DateTime.Now.Hour;
            //        if (currentTimeDay == 9)
            //        {
            //            //九点第一批人员发送
            //            var pushUsers = pushJjtUserList.Where(x => x.Type == 0).Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"九点第一批推送人员结果:{pushResult}");

            //        }
            //        else if (currentTimeDay == 10)
            //        {
            //            #region 第二批人员推送 个项目部管理人员
            //            //10 第二批人员推送 个项目部管理人员
            //            var pushUsers = pushJjtUserList.Where(x => x.Type.Value == 1).Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            logger.LogWarning($"10点第二批推送公司管理人员结果:{pushResult}");
            //            #endregion

            //            #region 部门相关人员发送消息
            //            //部门相关人员发送消息
            //            var specialOne = pushJjtUserList.Where(x => x.Type.Value == 2).SingleOrDefault();
            //            obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.Topartys,
            //                Topartys = new List<string>() { specialOne.PushAccount }
            //            };
            //            pushResult = JjtUtils.SinglePushMessage(obj);
            //            logger.LogWarning($"10点第二批推送相关部门人员结果:{pushResult}");
            //            #endregion

            //            #region 相关群组人员发消息
            //            //相关群组人员发消息
            //            var specialTwo = pushJjtUserList.Where(x => x.Type.Value == 3).SingleOrDefault();
            //            obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.CHATID,
            //                ChatId = specialTwo.GroupNumber
            //            };
            //            pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"10点第二批推送群组人员结果:{pushResult}");
            //            #endregion
            //        }
            //        else if (currentTimeDay >= 18 && currentTimeDay <= 23)
            //        {
            //            //测试使用 
            //            //测试人员陈翠
            //            var pushUsers = pushJjtUserList.Where(x => x.Type == 0 && x.PushAccount == "2016146340").Select(x => x.PushAccount).ToList();
            //            var obj = new SingleMessageTemplateRequestDto()
            //            {
            //                IsAll = false,
            //                Mediaid = mediaId,
            //                MessageType = JjtMessageType.IMAGE,
            //                UserIds = pushUsers
            //            };
            //            var pushResult = JjtUtils.SinglePushMessage(obj);
            //            responseAjaxResult.Data = pushResult;
            //            logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
            //        }
            //        #endregion
            //    }
            //}
            //else
            //{
            //    logger.LogWarning("交建通发送图片获取token为空");
            //}
            #endregion

            #region 使用交建通代理方式的情况
            var url = AppsettingsHelper.GetValue("JjtPushMesssage:UploadTempJjt");
            var name = "formFile";
            var fileName = formFile.FileName;
            var file = formFile.OpenReadStream();
            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(file, (int)file.Length), name, fileName);
            var _httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpclient.PostAsync(url, formData);
            }
            catch (Exception ex)
            {

                logger.LogError($"生产日报推送消息发送失败:{ex}");
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                var media = response.Content.ReadAsStringAsync().Result;
                #region 重试机制
                media = await RetryAsync(_httpclient, media, url, formData);
                if (string.IsNullOrWhiteSpace(media.TrimAll()))
                {
                    responseAjaxResult.Fail();
                    return responseAjaxResult;
                }
                //if (string.IsNullOrWhiteSpace(media.TrimAll()))
                //{
                //    //重试三次
                //    try
                //    {
                //        int retry = 0;
                //        while (retry<3) 
                //        {

                //            response = await _httpclient.PostAsync(url, formData);
                //            media = response.Content.ReadAsStringAsync().Result;
                //            if (!string.IsNullOrWhiteSpace(media.TrimAll()))
                //            {
                //                retry = 4; break;
                //            }
                //            else {
                //                if (retry == 2)
                //                {
                //                    //说明重试三次依然失败
                //                    logger.LogWarning("上传交建通临时素材重试三次依然失败了"); 
                //                }
                //                retry += 1;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        logger.LogError($"生产日报推送消息发送失败:{ex}");
                //    }
                //}
                #endregion
                responseAjaxResult.Data = true;
                #region 交建通发送消息
                //获取推送人员信息
                var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
           .ToListAsync();
                var currentTimeDay = DateTime.Now.Hour;
                if (currentTimeDay == 10)
                {
                    #region 第二批人员推送 个项目部管理人员
                    //10 第二批人员推送 个项目部管理人员
                    var pushUsers = pushJjtUserList.Where(x => x.Type.Value == 1).Select(x => x.PushAccount).ToList();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.IMAGE,
                        UserIds = pushUsers
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, true, "jjDayReport");
                    logger.LogWarning($"10点第二批推送交建公司管理人员结果:{pushResult}");
                    #endregion
                }
                else if (currentTimeDay >= 12 && currentTimeDay <= 23)
                {
                    #region 测试使用
                    ////测试使用 
                    ////测试人员陈翠
                    //var pushUsers = pushJjtUserList.Where(x => x.Type == 0 && x.PushAccount == "2016146340").Select(x => x.PushAccount).ToList();
                    //var obj = new SingleMessageTemplateRequestDto()
                    //{
                    //    IsAll = false,
                    //    Mediaid = media,
                    //    MessageType = JjtMessageType.IMAGE,
                    //    UserIds = pushUsers
                    //};
                    //var pushResult = JjtUtils.SinglePushMessage(obj, true, "five");
                    //responseAjaxResult.Data = pushResult;
                    //logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                    #endregion
                    //测试使用 
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = pushUsers.GroupNumber
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj);
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                }
                #endregion
            }
            #endregion
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        /// <summary>
        /// 船舶日报图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UploadShipImageJJT(IFormFile formFile)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 使用交建通代理方式的情况
            var url = AppsettingsHelper.GetValue("JjtPushMesssage:UploadTempJjt");
            var name = "formFile";
            var fileName = formFile.FileName;
            var file = formFile.OpenReadStream();
            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(file, (int)file.Length), name, fileName);
            var _httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpclient.PostAsync(url, formData);
            }
            catch (Exception ex)
            {

                logger.LogError($"船舶推送消息发送失败:{ex}");
            }
            if (response!=null&&response.IsSuccessStatusCode)
            {
                var media = response.Content.ReadAsStringAsync().Result;
                #region 重试机制
                media = await RetryAsync(_httpclient, media, url, formData);
                if (string.IsNullOrWhiteSpace(media.TrimAll()))
                {
                    responseAjaxResult.Fail();
                    return responseAjaxResult;
                }
                #endregion
                responseAjaxResult.Data = true;
                #region 交建通发送消息
                //获取推送人员信息
                var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
          .ToListAsync();
                var currentTimeDay = DateTime.Now.Hour;
                if (currentTimeDay == 10)
                {
                    //十点发送
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 5).Select(x => x.PushAccount).ToList();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.IMAGE,
                        UserIds = pushUsers
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, true, "six");
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"十点推送人员结果:{pushResult}");

                }
                else if (currentTimeDay >= 11 && currentTimeDay <= 23)
                {
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type ==4).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = pushUsers.GroupNumber
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj);
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                }
                #endregion
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion
        }

		/// <summary>
		/// 项目生产动态图片
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> UploadProjectShiftImageJJT(IFormFile formFile)
        {
			ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
			#region 使用交建通代理方式的情况
			var url = AppsettingsHelper.GetValue("JjtPushMesssage:UploadTempJjt");
			var name = "formFile";
			var fileName = formFile.FileName;
			var file = formFile.OpenReadStream();
			var formData = new MultipartFormDataContent();
			formData.Add(new StreamContent(file, (int)file.Length), name, fileName);
			var _httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpclient.PostAsync(url, formData);
            }
            catch (Exception ex)
            {

                logger.LogError($"推送消息发送失败:{ex}");
            }
            if (response!=null&&response.IsSuccessStatusCode)
			{
				var media = response.Content.ReadAsStringAsync().Result;
                #region 重试机制
                media = await RetryAsync(_httpclient, media, url, formData);
                if (string.IsNullOrWhiteSpace(media.TrimAll()))
                {
                    responseAjaxResult.Fail();
                    return responseAjaxResult;
                }
                #endregion
                responseAjaxResult.Data = true;
                #region 交建通发送消息
                //获取推送人员信息
                var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
          .ToListAsync();
                var currentTimeDay = DateTime.Now.Hour;
                if (currentTimeDay == 9)
                {
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 6).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = pushUsers.GroupNumber
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj,true,"elven");
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");

                }
                else if (currentTimeDay >= 13 && currentTimeDay <= 23)
                {
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        IsAll = false,
                        Mediaid = media,
                        MessageType = JjtMessageType.CHATID,
                        ChatId = pushUsers.GroupNumber
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj);
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
                }
                #endregion
            }
			responseAjaxResult.Success();
			return responseAjaxResult;
			#endregion
		}



        /// <summary>
		/// 项目生产动态图片
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		public async Task<ResponseAjaxResult<bool>> UploadProjectShiftTextJJT( string  text)
        {
            //text= text + "【以本次推送为准】";
            //Console.WriteLine("111"+text);
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 使用交建通代理方式的情况
            var url = AppsettingsHelper.GetValue("JjtPushMesssage:UploadTempJjt");
                #region 交建通发送消息
                //获取推送人员信息
                var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1)
          .ToListAsync();
                var currentTimeDay = DateTime.Now.Hour;
                if (currentTimeDay == 9)
                {
                    //测试人员群
                    var pushUsers = pushJjtUserList.Where(x => x.Type == 6).SingleOrDefault();
                var obj = new SingleMessageTemplateRequestDto()
                {
                    IsAll = false,
                    MessageType = JjtMessageType.CHATID,
                    ChatId = pushUsers.GroupNumber,
                    TextContent = text
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, true, "nine");
                    responseAjaxResult.Data = pushResult;
                    logger.LogWarning($"测试第一批推送人员结果:{pushResult}");

                }
                else if (currentTimeDay >= 12 && currentTimeDay <= 23)
                {
                //测试人员群
                var pushUsers = pushJjtUserList.Where(x => x.Type == 4).SingleOrDefault();
                var obj = new SingleMessageTemplateRequestDto()
                {
                    IsAll = false,
                    MessageType = JjtMessageType.CHATID,
                    ChatId = pushUsers.GroupNumber,
                    TextContent = text
                };
                var pushResult = JjtUtils.SinglePushMessage(obj, true, "ten");
                responseAjaxResult.Data = pushResult;
                logger.LogWarning($"测试第一批推送人员结果:{pushResult}");
            }
                #endregion
            
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion
        }


        #region 交建通火麒麟临时素材
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public async Task<bool> GetImageJJT(string media_id)
        {
            WebHelper webHelper = new WebHelper();
            //获取token
            var corpid = AppsettingsHelper.GetValue("JjtPushMesssage:Corpid");
            var token = AppsettingsHelper.GetValue("JjtPushMesssage:GetJjtTokenUrl").Replace("@corpid", corpid);
            var responseToken = await webHelper.DoGetAsync(token);
            var bytes = new byte[4096];
            //获取media_Id对象
            string url = "https://jjt.ccccltd.cn/cgi-bin/media/get?access_token=" + responseToken.Result + "&media_id=" + media_id;
            var aa = await webHelper.DoGetAsync(url);
            return true;
        }


        #endregion


        #region 发送消息失败重试机制
        /// <summary>
        /// 发送消息失败重试机制
        /// </summary>
        /// <param name="_httpclient"></param>
        /// <param name="media"></param>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public async Task<string> RetryAsync(HttpClient _httpclient,string media, string url, MultipartFormDataContent formData ,int retryCount = 3)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(media.TrimAll()))
                {
                    //重试三次
                    int retry = 0;
                    while (retry < retryCount)
                    {

                        var response = await _httpclient.PostAsync(url, formData);
                        media = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrWhiteSpace(media.TrimAll()))
                        {
                            retry = 4; break;
                        }
                        else
                        {
                            if (retry == retryCount - 1)
                            {
                                //说明重试三次依然失败
                                logger.LogWarning("上传交建通临时素材重试三次依然失败了");
                            }
                            retry += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"重试机制出现错误{ex}");
            }
            return media;
        }
        #endregion
    }
}
