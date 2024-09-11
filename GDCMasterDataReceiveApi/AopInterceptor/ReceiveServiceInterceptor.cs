﻿using Castle.DynamicProxy;
using GDCMasterDataReceiveApi.Application;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Application.Service.ReceiveService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SqlSugar;
using System.Reflection;
using UtilsSharp;

namespace GHElectronicFileApi.AopInterceptor
{

    /// <summary>
    /// 操作记录拦截器
    /// </summary>
    public class ReceiveServiceInterceptor : IInterceptor
    {
        public async void Intercept(IInvocation invocation)
        {
            //请求唯一
            var traceIdentifier= HttpContentAccessFactory.Current.TraceIdentifier;
            //日志实例
            ReceiveRecordLog receiveRecordLog = null;
            //雪花ID
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
           //注入baseService  
           var baseService=(IBaseService)HttpContentAccessFactory.Current.RequestServices.GetService<IBaseService>();
            //接收参数
            var requestParame = string.Empty;
            //参数数量
            var parameCount = 0;
            //请求的方法名称
            var methodName = invocation.Method.Name;

            #region 接收类型数据判断
            if (methodName == "ProjectDataAsync")
            {

               var receiveParame=  ((GDCMasterDataReceiveApi.Application.Contracts.Dto.BaseReceiveDataRequestDto<GDCMasterDataReceiveApi.Application.Contracts.Dto.Project.ProjectItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
            }
            #endregion

            #region 记录接收数据的请求日志
            if (!string.IsNullOrWhiteSpace(requestParame))
            {
                receiveRecordLog = new ReceiveRecordLog()
                {
                    Id = receiceRecordId,
                    ReceiveNumber = parameCount,
                    RequestParame = requestParame,
                    Traceidentifier = traceIdentifier,
                };
               await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            }
            #endregion

            //目标方法
            invocation.Proceed();


            #region 更新请求日志
            if (!string.IsNullOrWhiteSpace(requestParame))
            {
                receiveRecordLog = new ReceiveRecordLog()
                {
                    Id = receiceRecordId,
                    ReceiveNumber = parameCount,
                    RequestParame = requestParame,
                    SuccessNumber = parameCount,
                    Traceidentifier= traceIdentifier,
                };
                await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Update);
            }
            #endregion

        }
    }
}
