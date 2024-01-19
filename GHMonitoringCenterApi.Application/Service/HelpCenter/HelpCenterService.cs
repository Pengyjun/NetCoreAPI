using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.IService.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.HelpCenter
{
    /// <summary>
    /// 帮助中心接口实现层
    /// </summary>
    public class HelpCenterService : IHelpCenterService
    {       
        #region 依赖注入
        public IBaseRepository<HelpCenterMenu> baseHelpCenterMenuRepository { get; set; }
        public ISqlSugarClient dbContext { get; set; }
        public IBaseRepository<HelpCenterDetails> baseHelpCenterDetailsRepository { get; set; }
        public ILogService logService { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        public HelpCenterService(IBaseRepository<HelpCenterMenu> baseHelpCenterMenuRepository, ISqlSugarClient dbContext, IBaseRepository<HelpCenterDetails> baseHelpCenterDetailsRepository, ILogService logService, GlobalObject globalObject)
        {
            this.baseHelpCenterMenuRepository = baseHelpCenterMenuRepository;
            this.dbContext = dbContext;
            this.baseHelpCenterDetailsRepository = baseHelpCenterDetailsRepository;
            this.logService = logService;
            _globalObject = globalObject;
        }
        #endregion
        /// <summary>
        /// 新增或修改帮助中心菜单及内容
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveHelpCenterMenuAsync(SaveHelpCenterRequsetDto saveHelpCenterRequsetDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            //新增
            if (saveHelpCenterRequsetDto.RequestType)
            {                     
            var HelpCenterMenu = await baseHelpCenterMenuRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
                //添加一级菜单
            if (saveHelpCenterRequsetDto.Id == null|| saveHelpCenterRequsetDto.Id == Guid.Empty)
            {
                    int sorts;
                    int mid;
                var sort = HelpCenterMenu.Where(x=>x.PId==0).ToList();
                    if (sort.Count<1)
                    {
                         sorts = 0;
                         mid = 0;
                    }
                    else
                    {
                         sorts = sort.Max(x=>x.PId);
                         mid = HelpCenterMenu.Max(x => x.MId);
                    }
                    HelpCenterMenu helpCenterMenu = new HelpCenterMenu()
                {
                   Id = GuidUtil.Next(),
                   Name = saveHelpCenterRequsetDto.Name,
                   PId = 0,
                   MId = mid+1,
                   Sort = sorts + 1
                };
                    #region 日志信息
                    var logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        BusinessModule = "/系统管理/帮助管理/新增",
                        BusinessRemark = "/系统管理/帮助管理/新增",
                        OperationId = _currentUser.Id,
                        OperationName = _currentUser.Name,
                    };
                    #endregion
                    await baseHelpCenterMenuRepository.AsInsertable(helpCenterMenu).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
            else
            {
                //添加下级菜单及详情
                var HelpCenterMenus = await baseHelpCenterMenuRepository.AsQueryable().Where(x => x.Id == saveHelpCenterRequsetDto.Id).SingleAsync();
                   var mid = HelpCenterMenu.Max(x => x.MId);
                    if (HelpCenterMenus == null)
                    {
                        responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_NOTEXIST);
                        responseAjaxResult.Data = false;
                        return responseAjaxResult;
                    }
                var sort = HelpCenterMenu.AsQueryable().Where(x => x.PId == HelpCenterMenus.MId).Select(x => x.Sort).ToList();
                HelpCenterMenu helpCenterMenu = new HelpCenterMenu()
                {
                    Id = GuidUtil.Next(),
                    Name = saveHelpCenterRequsetDto.Name,
                    PId = HelpCenterMenus.MId,
                    MId = mid + 1,                    
                };
                                                                               
                if (!sort.Any())
                {
                    helpCenterMenu.Sort = 1;
                }
                else
                {
                    helpCenterMenu.Sort = sort.Max() + 1;
                }
                    
                    await baseHelpCenterMenuRepository.AsInsertable(helpCenterMenu).ExecuteCommandAsync();
                    //添加详情
                    if (saveHelpCenterRequsetDto.Details.Any())
                    {
                        HelpCenterDetails helpCenterDetails = new HelpCenterDetails()
                        {
                            Id = GuidUtil.Next(),
                            HelpCenterMenuId = helpCenterMenu.Id,
                            Reutilizando = saveHelpCenterRequsetDto.Reutilizando,
                            Details = saveHelpCenterRequsetDto.Details,
                        };
                        #region 日志信息
                        var logDto = new LogInfo()
                        {
                            Id = GuidUtil.Increment(),
                            BusinessModule = "/系统管理/帮助管理/新增",
                            BusinessRemark = "/系统管理/帮助管理/新增",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                        };
                        #endregion
                        await baseHelpCenterDetailsRepository.AsInsertable(helpCenterDetails).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                    }
                }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
            return responseAjaxResult;
            }
            else
            //修改
            {
                var HelpCenterMenus = await baseHelpCenterMenuRepository.AsQueryable().Where(x => x.Id == saveHelpCenterRequsetDto.Id).SingleAsync();
                var helpCenterDetails = await baseHelpCenterDetailsRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.HelpCenterMenuId == saveHelpCenterRequsetDto.Id).SingleAsync();
                //修改父级菜单Name
                if (HelpCenterMenus.PId == 0|| HelpCenterMenus == null)
                {
                    #region 日志信息
                    var logDtos = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        BusinessModule = "/系统管理/帮助管理/修改",
                        BusinessRemark = "/系统管理/帮助管理/修改",
                        OperationId = _currentUser.Id,
                        OperationName = _currentUser.Name,
                    };
                    #endregion
                    HelpCenterMenus.Name = saveHelpCenterRequsetDto.Name;
                    await baseHelpCenterMenuRepository.AsUpdateable(HelpCenterMenus).EnableDiffLogEvent(logDtos).ExecuteCommandAsync();
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                    return responseAjaxResult;
                }
                //修改详情
               
                //没有详情时
                if (saveHelpCenterRequsetDto.Details.Any() && helpCenterDetails == null)
                {
                    HelpCenterDetails helpCenterDetai = new HelpCenterDetails()
                    {
                        Id = GuidUtil.Next(),
                        HelpCenterMenuId = saveHelpCenterRequsetDto.Id.Value,
                        Reutilizando = saveHelpCenterRequsetDto.Reutilizando,
                        Details = saveHelpCenterRequsetDto.Details,
                    };
                    await baseHelpCenterDetailsRepository.AsInsertable(helpCenterDetai).ExecuteCommandAsync();
                }
                else
                {

                    helpCenterDetails.Details = saveHelpCenterRequsetDto.Details;
                    helpCenterDetails.Reutilizando = saveHelpCenterRequsetDto.Reutilizando;
                    #region 日志信息
                    var logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        BusinessModule = "/系统管理/帮助管理/修改",
                        BusinessRemark = "/系统管理/帮助管理/修改",
                        OperationId = _currentUser.Id,
                        OperationName = _currentUser.Name,
                    };
                    #endregion
                    await baseHelpCenterDetailsRepository.AsUpdateable(helpCenterDetails).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                HelpCenterMenus.Name = saveHelpCenterRequsetDto.Name;
                await baseHelpCenterMenuRepository.AsUpdateable(HelpCenterMenus).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 获取帮助中心菜单
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchHelpCenterMenuResponseDto>>> SearchHelpCenterMenuAsync(BaseRequestDto baseRequestDto)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<SearchHelpCenterMenuResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SearchHelpCenterMenuResponseDto>>();
            List<SearchHelpCenterMenuResponseDto> searchHelpCenterMenuResponseDto = new List<SearchHelpCenterMenuResponseDto>();
            //过滤特殊字符
            baseRequestDto.KeyWords = Utils.ReplaceSQLChar(baseRequestDto.KeyWords);

            var HelpCenterMid = await baseHelpCenterMenuRepository.AsQueryable().Where(x =>SqlFunc.Contains(x.Name,baseRequestDto.KeyWords)&&x.PId == 0).Select(x => x.MId).ToListAsync();
            var HelpCenterLists = await dbContext.Queryable<HelpCenterMenu>()
                .LeftJoin<HelpCenterDetails>((x, s) => x.Id == s.HelpCenterMenuId)
                .Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, baseRequestDto.KeyWords)&& ( HelpCenterMid.Contains(x.PId) || x.PId == 0))              
                .Select((x, s) => new SearchHelpCenterMenuResponseDto { Id = x.Id, Name = x.Name, KeyId = x.MId.ToString(), Pid = x.PId.ToString(), Details = s.Details,Reutilizando = s.Reutilizando})
                .ToListAsync();

            if (HelpCenterLists.Any())
            {             
                    searchHelpCenterMenuResponseDto = ListToTreeUtil.GetTree(Guid.Empty,"0", HelpCenterLists);                
            }       
            responseAjaxResult.Data = searchHelpCenterMenuResponseDto.OrderBy(x=>x.Sort).ToList();
            responseAjaxResult.Count = searchHelpCenterMenuResponseDto.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取帮助中心详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<SearchHelpCenterDetailsResponseDto>> SearchHelpCenterDetailsAsync(Guid Id)
        {
            ResponseAjaxResult<SearchHelpCenterDetailsResponseDto> responseAjaxResult = new ResponseAjaxResult<SearchHelpCenterDetailsResponseDto>();
            var searchHelpCenterDetails =await baseHelpCenterDetailsRepository.AsQueryable().Where(x => x.HelpCenterMenuId == Id&&x.IsDelete == 1).SingleAsync();
            if (searchHelpCenterDetails == null)
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_NOTEXIST);
                responseAjaxResult.Data = null;
                return responseAjaxResult;
            }
            SearchHelpCenterDetailsResponseDto searchHelpCenterDetailsResponseDto = new SearchHelpCenterDetailsResponseDto()
            {
                Details = searchHelpCenterDetails.Details,
                Reutilizando = searchHelpCenterDetails.Reutilizando
            };
            responseAjaxResult.Data = searchHelpCenterDetailsResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 删除帮助中心菜单和内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeletehHelpCenterMenuAsync(DeletehHelpCenterMenuRequsetDto deletehHelpCenterMenuRequsetDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var HelpCenterMenu = await baseHelpCenterMenuRepository.AsQueryable()
                .Where(x => x.IsDelete == 1 && deletehHelpCenterMenuRequsetDto.Id == x.Id)
                .SingleAsync();

            if (HelpCenterMenu != null)
            {
                HelpCenterMenu.IsDelete = 0;
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST);
            }
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/帮助管理/删除",
                BusinessRemark = "/系统管理/帮助管理/删除",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
            await baseHelpCenterMenuRepository.AsUpdateable(HelpCenterMenu).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
            responseAjaxResult.Data = true;
            return responseAjaxResult;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> HelpCenterDownloadFileAsync()
        {
            //ResponseAjaxResult<string> responseAjaxResult = new ResponseAjaxResult<string>();
            var url = @"C:\Users\inhui\Desktop\发票.docx";
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpResponseMessage response = await httpClient.GetAsync(url);
            //    response.EnsureSuccessStatusCode();
            //    同样的，在此处可通过 ReadAsStreamAsync（）方法，以流的方式下载指定文件（或者将网络流通过 MemoryStream 转换为内存流，再转换为byte进行存储或保存），再通过 Image 对象从流中读取图片文件。
            //                   string retString = await response.Content.ReadAsStringAsync();
            //    System.IO.File.WriteAllText("D:\\index.html", retString);
            //    responseAjaxResult.Data = retString;
            //    return responseAjaxResult;
            //}
            //Stream stream = new Stream();
            //FileStream file = new FileStream(url);
            //            return stream;
            //var fileStream = System.IO.File.Create(url);
            //.CopyToAsync(fileStream);  //
            //打开文件
            //FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read); 
            //// 读取文件Byte[]
            //byte[] bytes = new byte[fileStream.Length];
            //fileStream.Read(bytes, 0, bytes.Length);
            //fileStream.Close();
            ////byte[]转换为Stream
            //Stream stream = new MemoryStream(bytes);


            //var actionresult = new FileStreamResult(stream, ContentType.APPLICATIONSTREAM);
            //actionresult.FileDownloadName ="Content-Disposition";
            //Response.ContentLength = res.Length;
            // return actionresult;
            // HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}.{fileSuffixName}", System.Text.Encoding.UTF8)}");

            //  return new FileStreamResult(stream,ContentType.APPLICATIONSTREAM);


            string filePath = ""; //生成的文件地址
            FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/octet-stream") { FileDownloadName = "abc.docx" };

        }
    }
}
