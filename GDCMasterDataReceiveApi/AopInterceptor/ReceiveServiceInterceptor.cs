using Castle.DynamicProxy;
using GDCMasterDataReceiveApi.Application;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public  async void Intercept(IInvocation invocation)
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
            //接收数据类型
            ReceiveDataType receiveDataType = 0;

            #region 接收类型数据判断
            if (methodName == "ProjectDataAsync")
            {
                var receiveParame = ((BaseReceiveDataRequestDto<ProjectItem>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.Person;
            } else if (methodName == "CountryContinentDataAsync") 
            {
                var receiveParame = ((BaseReceiveDataRequestDto<CountryContinentReceiveDto>)invocation.Arguments[0]).IT_DATA;
                parameCount = receiveParame.item.Count;
                requestParame = receiveParame.item.ToJson();
                receiveDataType = ReceiveDataType.CountryContinent;
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
                    ReceiveType= receiveDataType,
                };
                baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            }
            #endregion

            //目标方法
            try
            {
                invocation.Proceed();
                var res=(Task)invocation.ReturnValue;
                res.Wait();//如果任务有异常 直接报错
                #region 更新请求日志

                if (!string.IsNullOrWhiteSpace(requestParame))
                {
                    receiveRecordLog = new ReceiveRecordLog()
                    {
                        Id = receiceRecordId,
                        ReceiveNumber = parameCount,
                        RequestParame = requestParame,
                        SuccessNumber = parameCount,
                        Traceidentifier = traceIdentifier,
                        ReceiveType = receiveDataType,
                    };
                     baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Update);
                }
                #endregion
            }
            catch (Exception ex)
            {
                //这里不处理  会自动向上抛

            }



        }
    }
}
